using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using MyPracticeWebSite.Services;
using Shared.Models;

namespace MyPracticeWebSite.Controllers
{
    [Authorize]
    public class ConferenceController : Controller
    {
        public readonly IConferenceService _service;

        public ConferenceController(IConferenceService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Conference Overview";
            return View(await _service.GetAll());
        }

        public IActionResult Add()
        {
            ViewBag.Title = "Add Conference";
            return View(new ConferenceModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ConferenceModel model)
        {
            if (ModelState.IsValid)
               await _service.Add(model);

            return RedirectToAction("Index");
        }


    }
}