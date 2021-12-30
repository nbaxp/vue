using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNet6WebApi.Web;

/// <summary>
/// https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/mvc/advanced/app-parts/sample2
/// </summary>
/// <typeparam name="T"></typeparam>
[GenericControllerNameConvention]
[Route("[controller]/[action]")]
[Route("{culture}/[controller]/[action]")]
[ApiController]
public class GenericController<T> : Controller where T : BaseEntity
{
    private readonly DbContext _dbContext;

    public GenericController(DbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Index([FromQuery]PaginationViewModel<T> model)
    {
        var query = this._dbContext.Set<T>().AsNoTracking();
        if(!string.IsNullOrWhiteSpace(model.Query))
        {
            query = query.Where(model.Query);
        }
        model.TotalCount = query.Count();
        if (!string.IsNullOrWhiteSpace(model.OrderBy))
        {
            query = query.OrderBy(model.OrderBy);
        }
        model.Items = query.Skip(model.PageSize*(model.PageIndex-1)).Take(model.PageSize).ToList();
        return Ok(model);
    }
}
