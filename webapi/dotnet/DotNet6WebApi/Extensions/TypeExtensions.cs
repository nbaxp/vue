using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DotNet6WebApi.Extensions;
public static class TypeExtensions
{
    public static string GetDisplayName(this Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }
        return type.GetCustomAttribute<DisplayAttribute>()?.Name ?? type.Name;
    }
}
