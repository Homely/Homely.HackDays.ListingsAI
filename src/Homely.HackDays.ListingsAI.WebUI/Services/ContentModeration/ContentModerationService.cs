using Homely.HackDays.ListingsAI.WebUI.Models;
using Microsoft.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator.Models;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text;
using System.Threading;

namespace Homely.HackDays.ListingsAI.WebUI.Services.ContentModeration
{
    public class ContentModerationService : IContentModerationService
    {
        private readonly IContentModeratorClient _contentModeratorClient;

        public ContentModerationService(IContentModeratorClient contentModeratorClient)
        {
            _contentModeratorClient = contentModeratorClient ?? throw new System.ArgumentNullException(nameof(contentModeratorClient));
        }

        public Screen ValidateText(string text)
        {
            var jsonString = JObject.Parse(text);

            byte[] byteArray = Encoding.ASCII.GetBytes(jsonString["listing"]["info"]["description"].ToString());
            MemoryStream stream = new MemoryStream(byteArray);

            // TODO: research on lists for text moderation
            return _contentModeratorClient.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);
        }

        public EvaluationData ValidateImage(string url)
        {
            var url1 = new BodyModel("URL", url);
            var imageData = new EvaluationData();

            imageData.ImageUrl = url1.Value;

            // Evaluate for adult and racy content.
            imageData.ImageModeration =
                _contentModeratorClient.ImageModeration.EvaluateUrlInput("application/json", url1, true);
            Thread.Sleep(1000);
            return imageData;
        }
    }
}
