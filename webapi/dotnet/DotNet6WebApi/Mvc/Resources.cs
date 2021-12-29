using System.Linq.Expressions;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures;

public class Resources
{
    public static string TemplateHelpers_TemplateLimitations { get; set; } = nameof(TemplateHelpers_TemplateLimitations);

    public static string FormatPropertyOfTypeCannotBeNull(string arg1, string arg2) => nameof(TemplateHelpers_TemplateLimitations);

    public static string FormatExpressionHelper_InvalidIndexerExpression(Expression arg1, string arg2) => nameof(FormatExpressionHelper_InvalidIndexerExpression);
}
