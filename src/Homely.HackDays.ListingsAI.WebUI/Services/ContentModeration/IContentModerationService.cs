using Homely.HackDays.ListingsAI.WebUI.Models;
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Models;

namespace Homely.HackDays.ListingsAI.WebUI.Services.ContentModeration
{
    public interface IContentModerationService
    {
        Screen ValidateText(string text);
        EvaluationData ValidateImage(string url);
    }
}
