using Microsoft.AspNetCore.Mvc;

namespace DotNet6WebApi.Web;

/// <summary>
/// https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/mvc/advanced/app-parts/sample2
/// </summary>
/// <typeparam name="T"></typeparam>
[GenericControllerNameConvention]
[Route("[controller]/[action]")]
[Route("{culture}/[controller]/[action]")]
[ApiController]
public class GenericController<T> : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return Content($"Hello from a generic {typeof(T).Name} controller.");
    }
}
