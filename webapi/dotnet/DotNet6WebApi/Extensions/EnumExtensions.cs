using System.ComponentModel.DataAnnotations;

namespace DotNet6WebApi.Extensions;
public static class EnumExtensions
{
    public static int GetValue(this Enum value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        return (int)Enum.ToObject(value.GetType(), value);
    }

    public static string GetDisplayName(this Enum value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        if (name == null)
        {
            return null;
        }
        var field = type.GetField(name);
        var attribute = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
        return attribute?.Name ?? name;
    }
}
