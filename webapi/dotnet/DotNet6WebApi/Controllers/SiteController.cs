using Microsoft.AspNetCore.Mvc;

namespace DotNet6WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SiteController : ControllerBase
{
    [HttpGet("/site")]
    public IActionResult Layout()
    {
        return Ok(new
        {
            title = "Html Title",
            name = "网站名称",
            logo = "logo.svg",
            copyright = $"© {DateTime.Now.Year} 版权示例"
        });
    }
}
