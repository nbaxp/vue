using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DotNet6WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly JwtBearerOptions _options;
        private readonly SigningCredentials _credentials;

        public TokenController(
                IConfiguration configuration,
               IOptionsSnapshot<JwtBearerOptions> options,
               SigningCredentials credentials)
        {
            _configuration = configuration;
            _options = options.Get(JwtBearerDefaults.AuthenticationScheme);
            _credentials = credentials;
        }

        [HttpGet()]
        public object GetToken(string username)
        {
            var timeout = _configuration.GetValue("Jwt:Expires", 30);
            var claims = new Claim[] {
                new Claim(_options.TokenValidationParameters.NameClaimType, username),
                new Claim("NickName","管理員")
            };
            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            var handler = _options.SecurityTokenValidators.OfType<JwtSecurityTokenHandler>().First();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _options.TokenValidationParameters.ValidIssuer,
                Audience = _options.TokenValidationParameters.ValidAudience,
                SigningCredentials = _credentials,
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(timeout),
            };
            var securityToken = handler.CreateJwtSecurityToken(tokenDescriptor);
            var token = handler.WriteToken(securityToken);
            return new {
                token_type= JwtBearerDefaults.AuthenticationScheme,
                expires_in= timeout,
                refresh_token=token,
                access_token = token
            };
        }
    }
}
