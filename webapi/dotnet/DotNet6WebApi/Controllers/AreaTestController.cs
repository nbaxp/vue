using Microsoft.AspNetCore.Mvc;

namespace DotNet6WebApi.Controllers;
[Area("Area")]
public class AreaTestController : Controller
{
    public IActionResult Index()
    {
        return Content(Url.Action());
    }
}
