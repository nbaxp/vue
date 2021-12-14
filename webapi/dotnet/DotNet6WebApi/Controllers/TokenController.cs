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
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtBearerOptions _options;
        private readonly SigningCredentials _credentials;
        private readonly string _cookieKey = "refresh_token";
        private readonly CookieOptions _cookieOptions = new CookieOptions { HttpOnly = true, Path = "/refresh_token" };

        public TokenController(IConfiguration configuration,
                JwtSecurityTokenHandler jwtSecurityTokenHandler,
                TokenValidationParameters tokenValidationParameters,
                IOptionsSnapshot<JwtBearerOptions> options,
                SigningCredentials credentials)
        {
            _configuration = configuration;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
            _tokenValidationParameters = tokenValidationParameters;
            _options = options.Get(JwtBearerDefaults.AuthenticationScheme);
            _credentials = credentials;
        }

        [ResponseCache(NoStore = true)]
        [HttpPost("/token")]
        public IActionResult GetToken(string username)
        {
            var claims = new Claim[] {
                new Claim(_options.TokenValidationParameters.NameClaimType, username),
                new Claim("NickName","管理員")
            };
            var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

            var now = DateTime.UtcNow;
            var accessTokenTimeout = _configuration.GetValue("Jwt:AccessTokenExpires", 3600);
            var accessToken = CreateToken(claimsIdentity, now, accessTokenTimeout);
            var refreshTokenTimeout = _configuration.GetValue("Jwt:AccessTokenExpires", 1209600);//3600 * 24 * 7 * 2
            var refreshToken = CreateToken(claimsIdentity, now, refreshTokenTimeout);

            Response.Cookies.Delete(_cookieKey, _cookieOptions);
            Response.Cookies.Append(_cookieKey, refreshToken, _cookieOptions);

            return this.Ok(new
            {
                access_token = accessToken,
                token_type = JwtBearerDefaults.AuthenticationScheme,
                expires_in = accessTokenTimeout,
            });
        }

        [ResponseCache(NoStore = true)]
        [HttpPost("/refresh_token")]
        public IActionResult RefreshToken()
        {
            try
            {
                var refresh_token = Request.Cookies[_cookieKey];
                var claimsPrincipal = _jwtSecurityTokenHandler.ValidateToken(refresh_token, _tokenValidationParameters, out SecurityToken validatedToken);
                return this.GetToken(claimsPrincipal?.Identity?.Name!);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }
        }

        [HttpPost("/logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(_cookieKey, _cookieOptions);
            return Ok();
        }

        private string CreateToken(ClaimsIdentity subject, DateTime now, int timeout)
        {
            var handler = _options.SecurityTokenValidators.OfType<JwtSecurityTokenHandler>().First();
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _options.TokenValidationParameters.ValidIssuer,
                Audience = _options.TokenValidationParameters.ValidAudience,
                SigningCredentials = _credentials,
                Subject = subject,
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddSeconds(timeout),
            };
            var securityToken = handler.CreateJwtSecurityToken(tokenDescriptor);
            var token = handler.WriteToken(securityToken);
            return token;
        }
    }
}
