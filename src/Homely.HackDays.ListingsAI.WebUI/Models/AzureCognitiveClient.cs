using Homely.HackDays.ListingsAI.WebUI.Models.AzureCognitive;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Models
{
    public class AzureCognitiveClient
    {
        private readonly HttpClient _httpClient;

        public AzureCognitiveClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _httpClient.BaseAddress = new Uri("https://australiaeast.api.cognitive.microsoft.com");

            // NOTE: I don't know how to register a new Typed HttpClient with a ctor requirement.s
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "d3d7325d1d784b2c84444eb208105423");
        }

        public async Task<string[]> GetKeyPhrasesAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(nameof(text));
            }

            const string uri = "text/analytics/v2.0/keyPhrases";
            var requestBody = new
            {
                documents = new []
                {
                    new
                    {
                        id = 1,
                        language = "en",
                        text
                    }
                }
            };

            var result = await _httpClient.PostAsJsonAsync(uri, requestBody);
            result.EnsureSuccessStatusCode();

            var response = await result.Content.ReadAsAsync<KeyPhrasesResponse>();

            return response != null &&
                response.Documents != null &&
                response.Documents.Any()
                ? response.Documents.First().KeyPhrases
                : null;
        }
    }
}
