using Homely.HackDays.ListingsAI.WebUI.Models.Vision;
using Microsoft.ProjectOxford.Vision;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Services.ComputerVision
{
    public class ComputerVisionService : IComputerVisionService
    {
        private readonly IVisionServiceClient _api;

        public ComputerVisionService(string key)
        {
            _api = new VisionServiceClient(key, "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0");
        }
        
        public async Task<DescribeResultModel> DescribeImageAsync(string url)
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