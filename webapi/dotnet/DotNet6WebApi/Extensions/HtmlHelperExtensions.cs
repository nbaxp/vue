using System.Linq.Expressions;
using DotNet6WebApi.Resources;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;

namespace Microsoft.AspNetCore.Mvc.Rendering;

public static class HtmlHelperExtensions
{
    public static IHtmlContent DescriptionFor<TModel, TResult>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TResult>> expression)
    {
        var serviceProvider = htmlHelper.ViewContext.HttpContext.RequestServices;
        var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression,
            htmlHelper.ViewData,
            serviceProvider.GetRequiredService<IModelMetadataProvider>()).Metadata;
        var factory = serviceProvider.GetRequiredService<IStringLocalizerFactory>();
        var localizer = factory.Create(typeof(Resource));
        return new HtmlString(localizer[metadata.Description ?? string.Empty]);
    }
}
