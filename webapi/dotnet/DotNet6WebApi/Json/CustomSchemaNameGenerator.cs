using System.ComponentModel.DataAnnotations;
using Namotion.Reflection;
using NJsonSchema.Generation;

namespace DotNet6WebApi.Json;

public class CustomSchemaNameGenerator : DefaultSchemaNameGenerator
{
    public override string Generate(Type type)
    {
        CachedType cachedType = type.ToCachedType();
        DisplayAttribute inheritedAttribute = cachedType.GetInheritedAttribute<DisplayAttribute>();
        if (!string.IsNullOrEmpty(inheritedAttribute?.Name))
        {
            return inheritedAttribute.Name;
        }
        return base.Generate(type);
    }
}
