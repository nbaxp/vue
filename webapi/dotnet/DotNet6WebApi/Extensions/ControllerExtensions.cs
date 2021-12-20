using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;

namespace DotNet6WebApi.Extensions;
public static class ControllerExtensions
{
    public static ModelMetadata GetModelMetadata<T>(this ControllerBase controller)
    {
        if (controller is null)
        {
            throw new ArgumentNullException(nameof(controller));
        }
        return controller.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>().GetMetadataForType(typeof(T));
    }
    public static object GetJsonSchema<T>(this ControllerBase controller)
    {
        if (controller is null)
        {
            throw new ArgumentNullException(nameof(controller));
        }
        var metadata = controller.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>().GetMetadataForType(typeof(T));

        return CreateJson(controller, metadata as DefaultModelMetadata);
    }

    public static object GetJsonSchema(this ControllerBase controller, Type type)
    {
        if (controller is null)
        {
            throw new ArgumentNullException(nameof(controller));
        }
        var metadata = controller.HttpContext.RequestServices.GetRequiredService<IModelMetadataProvider>().GetMetadataForType(type);

        return CreateJson(controller, metadata as DefaultModelMetadata);
    }
    public static object CreateJson(this ControllerBase controller, DefaultModelMetadata metadata, DefaultModelMetadata parent = null)
    {
        if (controller is null)
        {
            throw new ArgumentNullException(nameof(controller));
        }

        if (metadata is null)
        {
            return null;
        }
        try
        {
            var factory = controller.HttpContext.RequestServices.GetService<IStringLocalizerFactory>();
            var localizer = factory.Create("Resources.Resource", Assembly.GetEntryAssembly().GetName().Name);
            dynamic json = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)json;
            //type
            if (metadata.IsNullableValueType)
            {
                json.nullable = true;
            }
            if (metadata.IsComplexType && metadata.IsCollectionType)
            {
                json.type = "array";
                json.items = controller.GetJsonSchema(metadata.ModelType.GenericTypeArguments[0]);
            }
            else if (metadata.IsEnum)
            {
                json.type = "string";
                var type = metadata.IsNullableValueType ? metadata.ModelType.GenericTypeArguments[0] : metadata.ModelType;
                dictionary["enum"] = from Enum e in Enum.GetValues(type) select new { value = e.GetValue(), text = e.ToString(), title = e.GetDisplayName() };
            }
            else
            {
                var modelType = metadata.ModelType;
                var underlyingType = Nullable.GetUnderlyingType(modelType);
                if (underlyingType != null)
                {
                    modelType = underlyingType;
                }
                if (modelType == typeof(string))
                {
                    json.type = "string";
                }
                else if (modelType == typeof(int) || modelType == typeof(long))
                {
                    json.type = "integer";
                    json.ui = "string";
                    json.digits = new
                    {
                        message = "整数格式错误"
                    };
                }
                else if (modelType == typeof(float) || modelType == typeof(double) || modelType == typeof(decimal))
                {
                    json.type = "number";
                    json.ui = "string";
                    json.number = new
                    {
                        message = "数值格式错误"
                    };
                }
                else if (modelType == typeof(bool))
                {
                    json.type = "boolean";
                }
                else if (modelType == typeof(Guid))
                {
                    json.type = "guid";
                }
                else if (modelType == typeof(DateTime))
                {
                    json.type = "string";
                }
                else
                {
                    json.type = "object";
                }
            }
            //title
            if (metadata.DisplayName != null)
            {
                json.title = metadata.DisplayName;
            }
            foreach (var attribute in metadata.Attributes.Attributes)
            {
                if (attribute is DescriptionAttribute descriptionAttribute)
                {//description
                    json.description = descriptionAttribute.Description;
                }
                else if (attribute is RequiredAttribute requiredAttribute)
                {
                    json.required = new
                    {
                        message = requiredAttribute.GetErrorMessage(localizer, metadata.DisplayName)
                    };
                }
                else if (attribute is RegularExpressionAttribute regularExpressionAttribute)
                {//pattern
                    json.pattern = new
                    {
                        regex = regularExpressionAttribute.Pattern,
                        message = regularExpressionAttribute.GetErrorMessage(localizer, metadata.DisplayName, regularExpressionAttribute.Pattern)
                    };
                }
                else if (attribute is DataTypeAttribute dataTypeAttribute)
                {//format
                    if (dataTypeAttribute.DataType == DataType.EmailAddress)
                    {
                        json.format = "email";
                        json.ui = "string";
                        json.email = new
                        {
                            message = "email格式错误"
                        };
                    }
                    else if (dataTypeAttribute.DataType == DataType.Url)
                    {
                        json.format = "url";
                        json.ui = "string";
                        json.url = new
                        {
                            message = "url格式错误"
                        };
                    }
                    else if (dataTypeAttribute.DataType == DataType.DateTime)
                    {
                        json.format = "date";
                        json.ui = "string";
                        json.date = new
                        {
                            message = "日期格式错误"
                        };
                    }
                    else if (dataTypeAttribute.DataType == DataType.CreditCard)
                    {
                        json.format = "creditcard";
                        json.ui = "string";
                        json.url = new
                        {
                            message = "信用卡格式错误"
                        };
                    }
                    else if (dataTypeAttribute.DataType == DataType.Custom)
                    {//自定义
                        json.format = dataTypeAttribute.GetDataTypeName().ToLower(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        json.format = dataTypeAttribute.DataType.ToString().ToLower(CultureInfo.InvariantCulture);
                    }
                }
                else if (attribute is ReadOnlyAttribute readOnlyAttribute)
                {//readOnly
                    json.readOnly = readOnlyAttribute.IsReadOnly;
                }
                else if (attribute is MinLengthAttribute minLengthAttribute)
                {//minLength
                    json.minLength = new
                    {
                        min = minLengthAttribute.Length,
                        message = minLengthAttribute.GetErrorMessage(localizer, metadata.DisplayName, minLengthAttribute.Length.ToString(CultureInfo.InvariantCulture))
                    };
                }
                else if (attribute is MaxLengthAttribute maxLengthAttribute)
                {//maxLength
                    json.maxLength = new
                    {
                        max = maxLengthAttribute.Length,
                        message = maxLengthAttribute.GetErrorMessage(localizer, metadata.DisplayName, maxLengthAttribute.Length.ToString(CultureInfo.InvariantCulture))
                    };
                }
                else if (attribute is StringLengthAttribute stringLengthAttribute)
                {//minLength,maxLength
                    json.rangeLength = new
                    {
                        min = stringLengthAttribute.MinimumLength,
                        max = stringLengthAttribute.MaximumLength,
                        message = stringLengthAttribute.GetErrorMessage(localizer, metadata.DisplayName,
                        stringLengthAttribute.MinimumLength.ToString(CultureInfo.InvariantCulture),
                        stringLengthAttribute.MaximumLength.ToString(CultureInfo.InvariantCulture))
                    };
                }
                else if (attribute is RangeAttribute rangeAttribute)
                {//minimum,maximum
                    json.range = new
                    {
                        min = rangeAttribute.Minimum,
                        max = rangeAttribute.Maximum,
                        message = rangeAttribute.GetErrorMessage(localizer, metadata.DisplayName,
                        rangeAttribute.Minimum.ToString(),
                        rangeAttribute.Maximum.ToString())
                    };
                }
                else if (attribute is CompareAttribute compareAttribute)
                {//compare 自定义
#pragma warning disable CA1062 // 验证公共方法的参数
                    var compareName = parent.Properties.FirstOrDefault(o => o.PropertyName == compareAttribute.OtherProperty)?.DisplayName;
#pragma warning restore CA1062 // 验证公共方法的参数
                    json.equalTo = new
                    {
                        other = compareAttribute.OtherProperty,
                        message = compareAttribute.GetErrorMessage(localizer, metadata.DisplayName, compareName)
                    };
                }
                else if (attribute is RemoteAttribute remoteAttribute)
                {//remote 自定义
                    var routeData = attribute.GetType().GetProperty("RouteData", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(remoteAttribute) as RouteValueDictionary;
                    json.remote = new
                    {
                        url = controller.Url.Action(routeData["action"].ToString(), routeData["controller"].ToString(), new { Area = routeData["area"]?.ToString() }),
                        remoteAttribute.AdditionalFields,
                        message = remoteAttribute.GetErrorMessage(localizer, metadata.DisplayName)
                    };
                }
                else if (attribute is ScaffoldColumnAttribute scaffoldColumnAttribute)
                {//scaffold 自定义
                    json.scaffold = scaffoldColumnAttribute.Scaffold;
                }
                else if (attribute is HiddenInputAttribute hiddenInputAttribute)
                {//hidden 自定义
                    json.hidden = hiddenInputAttribute.DisplayValue;
                }
                else if (attribute is UIHintAttribute uIHintAttribute)
                {//ui 自定义
                    json.ui = uIHintAttribute.UIHint.ToLower(CultureInfo.InvariantCulture);
                }
            }
            dynamic properties = new ExpandoObject();
            var propertiesDictionary = (IDictionary<string, object>)properties;
            if (metadata.IsComplexType && !metadata.IsCollectionType && metadata.Properties.Any())
            {
                foreach (var item in metadata.Properties)
                {
                    var modelMetadataItem = item as DefaultModelMetadata;
                    propertiesDictionary[item.PropertyName] = CreateJson(controller, modelMetadataItem, metadata);
                }
                json.properties = properties;
            }
            return json;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null;
        }
    }
    public static string GetErrorMessage(this ValidationAttribute attribute, IStringLocalizer localizer, params string[] args)
    {
        if (attribute is null)
        {
            throw new ArgumentNullException(nameof(attribute));
        }

        var localizedString = localizer.GetString(attribute.GetType().Name);
        //return localizedString.ResourceNotFound ? attribute.FormatErrorMessage(args[0]) : string.Format(localizedString.Value, args);
        var format = attribute.ErrorMessage;
        if (localizedString.ResourceNotFound)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = attribute.GetType().GetProperty("ErrorMessageString", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(attribute) as string;
            }
        }
        else
        {
            format = localizedString.Value;
        }
        try
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return format;
    }
}
