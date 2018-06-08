using Homely.HackDays.ListingsAI.WebUI.Models;
using Homely.HackDays.ListingsAI.WebUI.Models.Vision;
using Microsoft.ProjectOxford.Vision;
using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Services.ComputerVision
{
    public class ComputerVisionService : IComputerVisionService
    {
        private readonly IVisionServiceClient _api;
        private readonly IEnumerable<Image> _images;

        public ComputerVisionService(string listingsJson, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(listingsJson))
            {
                throw new ArgumentException("message", nameof(listingsJson));
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("message", nameof(apiKey));
            }

            _images = GetListingImages(listingsJson).OrderBy(image => Guid.NewGuid()).Take(20); // rate limit: 20 per min.
            _api = new VisionServiceClient(apiKey, "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0");
        }

        private static IEnumerable<Image> GetListingImages(string file)
        {
            var json = System.IO.File.ReadAllText(file);
            return JsonConvert.DeserializeObject<SearchResults>(json)
                              .Items
                              .SelectMany(item => item.Images);
        }

        public async Task<List<DescribeResultModel>> AnalyzeImagesAsync(string tagFilter = "")
        {
            var analysisResults = new List<DescribeResultModel>();

            foreach (var batch in _images.Batch(10))
            {
                analysisResults.AddRange(await Task.WhenAll(batch.Select(image => DescribeImageAsync(image.Url))));
            }

            if (!string.IsNullOrWhiteSpace(tagFilter))
            {
                analysisResults = analysisResults.Where(result => result.Tags.Any(t => string.Compare(t.Name, tagFilter, true) == 0))
                                                 .ToList();

                foreach (var result in analysisResults)
                {
                    foreach (var t in result.Tags)
                    {
                        if (string.Compare(t.Name, tagFilter, true) == 0)
                        {
                            t.CssClass = "selected";
                        }
                    }
                }
            }

            return analysisResults;
        }

        private async Task<DescribeResultModel> DescribeImageAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("message", nameof(url));
            }

            var result = await _api.DescribeAsync(url)
                                   .ConfigureAwait(false);
            if (result == null)
            {
                return null;
            }

            return new DescribeResultModel
            {
                Url = url,
                Tags = result.Description.Tags.Select(tag => new TagModel
                {
                    Name = tag
                }).ToList(),
                Caption = string.Join(',', result.Description.Captions.Select(caption => $"{caption.Text} (confidence: {caption.Confidence:N2}"))
            };
        }
    }
}