using System;
using System.Threading.Tasks;
using Homely.HackDays.ListingsAI.WebUI.Models;
using Homely.HackDays.ListingsAI.WebUI.Services.ComputerVision;
using Microsoft.AspNetCore.Mvc;

namespace Homely.HackDays.ListingsAI.WebUI.Controllers
{
    public class VisionController : Controller
    {
        private readonly IComputerVisionService _computerVisionService;
       
        public VisionController(IComputerVisionService computerVisionService)
        {
            _computerVisionService = computerVisionService ?? throw new ArgumentNullException(nameof(computerVisionService));
        }

        // GET: /vision?tag=pool
        public async Task<IActionResult> Index(string tag)
        {
            try
            {
                var analysisResults = await _computerVisionService.AnalyzeImagesAsync(tag);
                return View(analysisResults);
            }
            catch
            {
                return View("Error", new ErrorViewModel { Message = "Oops! Error occured. Most likely you went over the rate limit. Try again in 1 minute!" });
            }
        }
    }
}
