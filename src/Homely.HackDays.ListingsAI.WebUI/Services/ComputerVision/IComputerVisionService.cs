using Microsoft.ProjectOxford.Vision.Contract;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Services.ComputerVision
{
    public interface IComputerVisionService
    {
        Task<AnalysisResult> AnalyzeImageAsync(string url);
        Task<AnalysisResult> DescribeImageAsync(string url);
    }
}