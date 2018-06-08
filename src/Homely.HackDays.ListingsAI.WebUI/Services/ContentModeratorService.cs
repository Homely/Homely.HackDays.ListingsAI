using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.CognitiveServices.ContentModerator;

namespace Homely.HackDays.ListingsAI.WebUI.Services
{
    public static class ContentModeratorService
    {
        private static readonly string AzureRegion = "australiaEast";
        private static readonly string AzureBaseURL =
        $"{AzureRegion}.api.cognitive.microsoft.com";

        // valid for 7 days
        private static readonly string CMSubscriptionKey = "662691e053c146e380b00c5691b13efb";

        public static ContentModeratorClient NewClient()
        {
            // Create and initialize an instance of the Content Moderator API wrapper.
            ContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(CMSubscriptionKey));

            client.BaseUrl = AzureBaseURL;
            return client;
        }

    }
}
