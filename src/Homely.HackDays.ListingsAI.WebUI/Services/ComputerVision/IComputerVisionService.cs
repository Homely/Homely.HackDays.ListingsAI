using Homely.HackDays.ListingsAI.WebUI.Models.Vision;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Services.ComputerVision
{
    public interface IComputerVisionService
    {
        Task<List<DescribeResultModel>> AnalyzeImagesAsync(string tagFilter = "");
    }
}