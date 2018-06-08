using Homely.HackDays.ListingsAI.WebUI.Models.Vision;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Services.ComputerVision
{
    public interface IComputerVisionService
    {
        Task<DescribeResultModel> DescribeImageAsync(string url);
    }
}