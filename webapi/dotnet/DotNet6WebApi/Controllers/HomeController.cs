// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using DotNet6WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DotNet6WebApi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(TestViewModel model)
        {
            if(!ModelState.IsValid)
            {

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Valid(string userName)
        {
            await Task.Delay(10 * 1000);
            return Ok(userName == "admin");
        }
    }
}
