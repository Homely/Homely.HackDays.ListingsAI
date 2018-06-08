using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homely.HackDays.ListingsAI.WebUI.Services.ComputerVision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ProjectOxford.Vision.Contract;
using MoreLinq;
using Newtonsoft.Json;

namespace Homely.HackDays.ListingsAI.WebUI.Controllers
{
    public class VisionController : Controller
    {
        private readonly IComputerVisionService _computerVisionService;
        private static IEnumerable<Image> _images;

        public VisionController()
        {
            _computerVisionService = new ComputerVisionService();
            _images = GetListingImages().OrderBy(image => Guid.NewGuid()).Take(20); // rate limit: 20 per min.
        }

        // GET: /vision/analyze
        public async Task<IActionResult> Analyze()
        {
            var analysisResults = new List<AnalysisResult>();

            foreach (var batch in _images.Batch(10))
            {
                analysisResults.AddRange(await Task.WhenAll(batch.Select(image => _computerVisionService.AnalyzeImageAsync(image.Url))));
            }

            return Json(analysisResults);
        }

        // GET: /vision/describe
        public async Task<IActionResult> Describe()
        {
            var analysisResults = new List<AnalysisResult>();

            foreach (var batch in _images.Batch(10))
            {
                analysisResults.AddRange(await Task.WhenAll(batch.Select(image => _computerVisionService.DescribeImageAsync(image.Url))));
            }

            return Json(analysisResults);
        }

        private static IEnumerable<Image> GetListingImages()
        {
            var json = System.IO.File.ReadAllText("App_Data/mosman-list.json");
            return JsonConvert.DeserializeObject<SearchResults>(json)
                              .Items
                              .SelectMany(item => item.Images);
        }
    }

    public class SearchResults
    {
        public IEnumerable<Listing> Items { get; set; }
    }

    public class Listing
    {
        public IEnumerable<Image> Images { get; set; }
    }

    public class Image
    {
        public string Identifier { get; set; }

        [JsonIgnore]
        public string Url => $"https://res-2.cloudinary.com/hd1n2hd4y/image/upload/{Identifier}.jpg";
    }
}
