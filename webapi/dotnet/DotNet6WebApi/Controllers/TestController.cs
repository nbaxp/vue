using Microsoft.AspNetCore.Mvc;

namespace DotNet6WebApi.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class TestController : ControllerBase {

        [HttpGet]
        public JsonResult Get () {
            return new JsonResult(new
            {
                Version="0.0.3",
                HeaderKeys = string.Join(';',Request.Headers.Keys),
                HostValue = Request.Host.Value,
                RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString ()
            });
        }
    }
}