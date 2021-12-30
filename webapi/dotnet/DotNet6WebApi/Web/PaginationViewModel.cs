namespace DotNet6WebApi.Web;

public class PaginationViewModel<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public string? Query { get; set; }
    public string? OrderBy { get; set; }
    public int TotalCount { get; set; }
    public List<T> Items { get; set; } = new List<T>();
}
