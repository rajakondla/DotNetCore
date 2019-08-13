using System;
using System.Collections.Generic;
using System.Linq;
using MyPracticeWebSite.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyPracticeWebSite.ViewComponents
{
    public class StatisticsViewComponent:ViewComponent
    {
        public IConferenceService _service;

        public StatisticsViewComponent(IConferenceService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(string caption)
        {
            ViewBag.Caption = caption;
            return View("StatisticsDisplay", await _service.GetStatistics());
        }
    }
}
