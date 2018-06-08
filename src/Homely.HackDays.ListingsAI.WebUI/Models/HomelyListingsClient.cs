using Homely.HackDays.ListingsAI.WebUI.Models.HomelyListing;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Models
{
    public class HomelyListingsClient
    {
        private readonly HttpClient _httpClient;

        public HomelyListingsClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));   
        
            _httpClient.BaseAddress = new Uri("https://api.homely.com.au");
        }

        public async Task<ListingDetails> GetListingDetailsAsync(int listingId)
        {
            if (listingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(listingId));
            }

            var uri = new Uri($"listing/{listingId}", UriKind.Relative);
            var result = await _httpClient.GetAsync(uri);
            result.EnsureSuccessStatusCode();

            return await result.Content.ReadAsAsync<ListingDetails>();
        }
    }
}