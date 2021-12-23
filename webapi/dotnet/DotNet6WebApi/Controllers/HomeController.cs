// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using DotNet6WebApi.Extensions;
using DotNet6WebApi.Resources;
using DotNet6WebApi.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;

namespace DotNet6WebApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<Resource> _localizer;

        public HomeController(IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string header = Request.Headers.AcceptLanguage.ToString();
            var languages = header.Split(',')
                .Select(StringWithQualityHeaderValue.Parse)
                .OrderByDescending(s => s.Quality.GetValueOrDefault(1)).FirstOrDefault();
            ViewBag.Test = $"{CultureInfo.CurrentCulture.Name}/{CultureInfo.CurrentCulture.NativeName}:{_localizer["RequiredAttribute"]}";
            var rc = HttpContext.Features.Get<IRequestCultureFeature>();
            return View();
        }

        public IActionResult Locale(string returnUrl)
        {
            return View(model:returnUrl);
        }

        [HttpPost]
        public IActionResult Index(TestViewModel model)
        {
            var errors = ModelState.Where(o => o.Value.ValidationState == ModelValidationState.Invalid);
            var result = new
            {
                schema = this.GetJsonSchema<TestViewModel>(),
                model,
                errors,
                data = ViewData
            };
            var json = JsonSerializer.Serialize(result);
            Response.Cookies.Append(".AspNetCore.Culture", "c=en-US|uic=en-US");
            return View(model);
        }

        [HttpGet]
        public IActionResult Type()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Type(TypeViewModel model)
        {
            var errors = ModelState.Where(o => o.Value.ValidationState == ModelValidationState.Invalid);
            var result = new
            {
                schema = this.GetJsonSchema<TestViewModel>(),
                model,
                errors,
                data = ViewData
            };
            var json = JsonSerializer.Serialize(result);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Valid(string userName)
        {
            await Task.Delay(10 * 1000);
            return Ok(userName == "admin");
        }

        protected IActionResult Result<TEditModel>(object model)
        {
            return Json(new
            {
                schema = this.GetJsonSchema<TEditModel>(),
                model,
                errors = ModelState.Where(o => o.Value.ValidationState == ModelValidationState.Invalid),
                data = ViewData
            });
        }

        private bool IsJsonRequest(ControllerBase controller)
        {
            if (controller is null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            return controller.Request.Headers["accept"].ToString().Contains("json", StringComparison.OrdinalIgnoreCase);
        }
    }
}
