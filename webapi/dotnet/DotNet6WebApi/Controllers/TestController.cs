using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotNet6WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public object Get()
        {
            return new
            {
                Test="测试",
                Version = "0.0.6",
                Server = Request.HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalIpAddress?.ToString(),
                Request = Request.GetDisplayUrl(),
                HeaderKeys = string.Join(';', Request.Headers.Keys),
                HostValue = Request.Host.Value,
                RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
            };
        }

        [HttpGet("authentication"), Authorize(Roles = "admin,user")]
        public object Authentication()
        {
            var test = User.IsInRole("test");
            return "ok";
        }

        [HttpGet("un-authentication"), Authorize(Roles = "test")]
        public object UnAuthentication()
        {
            return "ok";
        }

        [HttpGet("login"), AllowAnonymous]
        public object Login(string userName)
        {
            var cfg = Request.HttpContext.RequestServices.GetService<IConfiguration>();
            var issuer = cfg.GetValue("Jwt:Issuer", "value");
            var audience = cfg.GetValue("Jwt:Audience", "value");
            var key = cfg.GetValue("Jwt:Audience", "0123456789abcdef");
            var expires = DateTime.Now.AddMinutes(cfg.GetValue("Jwt:Expires", 30));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(issuer, audience, new Claim[] { new Claim(ClaimTypes.Name, userName) }, null, expires, credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
