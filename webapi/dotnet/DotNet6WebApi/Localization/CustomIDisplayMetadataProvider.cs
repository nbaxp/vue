using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace DotNet6WebApi.Localization;

public class CustomIDisplayMetadataProvider : IDisplayMetadataProvider
{
    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        var attributes = context.Attributes;
        var displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();
        if (displayAttribute != null && string.IsNullOrEmpty(displayAttribute.Name))
        {
            displayAttribute.Name = context.Key.Name;
        }
    }
}
