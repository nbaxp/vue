using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace DotNet6WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(new
            {
                Version = "0.0.6",
                Server = Request.HttpContext.Features.Get<IHttpConnectionFeature>()?.LocalIpAddress?.ToString(),
                Request = Request.GetDisplayUrl(),
                HeaderKeys = string.Join(';', Request.Headers.Keys),
                HostValue = Request.Host.Value,
                RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString()
            });
        }
    }
}
