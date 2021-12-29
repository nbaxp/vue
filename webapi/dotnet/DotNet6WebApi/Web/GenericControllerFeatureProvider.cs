using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace DotNet6WebApi.Web;

public class GenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var types = Assembly.GetEntryAssembly()!.GetTypes().Where(o => o.IsAssignableTo(typeof(BaseEntity))).Select(o=>o.GetTypeInfo()).ToList();
        foreach (var entityType in types)
        {
            var typeName = entityType.Name + "Controller";
            if (!feature.Controllers.Any(t => t.Name == typeName))
            {
                var controllerType = typeof(GenericController<>).MakeGenericType(entityType.AsType()).GetTypeInfo();
                feature.Controllers.Add(controllerType);
            }
        }
    }
}
