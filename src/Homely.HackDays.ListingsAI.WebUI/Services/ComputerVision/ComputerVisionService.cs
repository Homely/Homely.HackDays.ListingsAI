using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Services.ComputerVision
{
    public class ComputerVisionService : IComputerVisionService
    {
        private readonly IEnumerable<string> _endpoints;
        private readonly IEnumerable<string> _keys;
        private readonly IVisionServiceClient _api;

        public ComputerVisionService()
        {
            _keys = new[]
            {
                "fe075680dc554bf7a9403b7a1d096d22",
                "dcffa92067a44f95ab0f0d8b26c6ce83"
            };

            _api = new VisionServiceClient(_keys.First(), "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0");
        }

        public Task<AnalysisResult> AnalyzeImageAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("message", nameof(url));
            }

            return _api.AnalyzeImageAsync(url, visualFeatures: null);
        }

        public Task<AnalysisResult> DescribeImageAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("message", nameof(url));
            }

            return _api.DescribeAsync(url);
        }
    }
}
