using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homely.HackDays.ListingsAI.WebUI.Configuration;
using Homely.HackDays.ListingsAI.WebUI.Models;
using Homely.HackDays.ListingsAI.WebUI.Models.Vision;
using Homely.HackDays.ListingsAI.WebUI.Services.ComputerVision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MoreLinq;
using Newtonsoft.Json;

namespace Homely.HackDays.ListingsAI.WebUI.Controllers
{
    public class VisionController : Controller
    {
        private readonly IComputerVisionService _computerVisionService;
        private readonly IEnumerable<Image> _images;

        public VisionController(IOptions<AzureSettings> azureSettings)
        {
            _computerVisionService = new ComputerVisionService(azureSettings.Value.ComputerVisionApiKey);
            _images = GetListingImages().OrderBy(image => Guid.NewGuid()).Take(20); // rate limit: 20 per min.
        }

        // GET: /vision?tag=pool
        public async Task<IActionResult> Index(string tag)
        {
            var analysisResults = new List<DescribeResultModel>();

            try
            {
                foreach (var batch in _images.Batch(10))
                {
                    analysisResults.AddRange(await Task.WhenAll(batch.Select(image => _computerVisionService.DescribeImageAsync(image.Url))));
                }

                if (!string.IsNullOrWhiteSpace(tag))
                {
                    analysisResults = analysisResults.Where(result => result.Tags.Any(t => string.Compare(t.Name, tag, true) == 0))
                                                     .ToList();

                    foreach (var result in analysisResults)
                    {
                        foreach (var t in result.Tags)
                        {
                            if (string.Compare(t.Name, tag, true) == 0)
                            {
                                t.CssClass = "selected";
                            }
                        }
                    }
                }
            }
            catch
            {
                return View("Error", new ErrorViewModel { Message = "Oops! Error occured. Most likely you went over the rate limit. Try again in 1 minute!" });
            }

            return View(analysisResults);
        }
     
        private static IEnumerable<Image> GetListingImages()
        {
            var json = System.IO.File.ReadAllText("App_Data/mosman-list.json");
            return JsonConvert.DeserializeObject<SearchResults>(json)
                              .Items
                              .SelectMany(item => item.Images);
        }
    }
}
