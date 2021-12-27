using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace DotNet6WebApi.Resources;

public class LocalizedValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
{
    private readonly ValidationAttributeAdapterProvider _originalProvider = new ValidationAttributeAdapterProvider();

    public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
    {
        attribute.ErrorMessage = attribute.GetType().Name;
        return _originalProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }
}
