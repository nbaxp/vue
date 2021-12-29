﻿using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace DotNet6WebApi.Web;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class GenericControllerNameConventionAttribute : Attribute, IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (controller.ControllerType.GetGenericTypeDefinition() != typeof(GenericController<>))
        {
            return;
        }

        var entityType = controller.ControllerType.GenericTypeArguments[0];
        controller.ControllerName = entityType.Name;
    }
}
