using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace DotNet6WebApi.Resources;

public class LocalizedValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly ValidationAttributeAdapterProvider _originalProvider = new ValidationAttributeAdapterProvider();

    public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
    {
        if(string.IsNullOrEmpty(attribute.ErrorMessage))
        {
            attribute.ErrorMessage = attribute.GetType().Name;
            System.Diagnostics.Debug.WriteLine(attribute.ErrorMessage);
        }
        return _originalProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }
}
