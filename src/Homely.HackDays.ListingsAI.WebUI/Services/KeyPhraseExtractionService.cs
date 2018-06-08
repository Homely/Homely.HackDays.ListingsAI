using Homely.HackDays.ListingsAI.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Services
{
    public class KeyPhraseExtractionService : IKeyPhraseExtractionService
    {
        private Lazy<ICollection<string>> _tags;
        private readonly HomelyListingsClient _homelyListingsClient;
        private readonly AzureCognitiveClient _azureCognitiveClient;

        public KeyPhraseExtractionService(string tagsFilePath,
                                          HomelyListingsClient homelyListingsClient,
                                          AzureCognitiveClient azureCognitiveClient)
        {
            if (string.IsNullOrWhiteSpace(tagsFilePath))
            {
                throw new ArgumentException(nameof(tagsFilePath));
            }

            _homelyListingsClient = homelyListingsClient ?? throw new ArgumentNullException(nameof(homelyListingsClient));
            _azureCognitiveClient = azureCognitiveClient ?? throw new ArgumentNullException(nameof(azureCognitiveClient));

            _tags = new Lazy<ICollection<string>>(() =>
            {
                // Load the tags file.
                if (!File.Exists(tagsFilePath))
                {
                    throw new Exception($"Tags file [{tagsFilePath}] doesn't exist.");
                }

                return File.ReadAllLines(tagsFilePath);
            });
        }
        
        private ICollection<string> Tags => _tags.Value;

        public async Task<KeyPhraseResults> ExtractTagsFromListingAsync(int listingId)
        {
            if (listingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(listingId), "ListingId must be greater than 0.");
            }

            // Grab the contents of this listing details.
            var listingDetails = await _homelyListingsClient.GetListingDetailsAsync(listingId);
            if (string.IsNullOrWhiteSpace(listingDetails?.Listing?.Info?.Description))
            {
                // No listing / listing data found.
                return new KeyPhraseResults();
            }

            // Extract some key phrases given the listing description.
            return await ExtractTagsAsync(listingDetails.Listing.Info.Description);
        }

        public async Task<KeyPhraseResults> ExtractTagsAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(nameof(text));
            }

            // Grabs the list of tags from the service.
            var keyPhrases = await _azureCognitiveClient.GetKeyPhrasesAsync(text);

            // Split the list of extracted tags into "Popular" and "Unpopular".
            if (keyPhrases == null ||
                !keyPhrases.Any())
            {
                // Nothing returned.
                return new KeyPhraseResults
                {
                    Description = text
                };
            }

            var keyPhraseResult = ToKeyPhraseResult(keyPhrases);
            keyPhraseResult.Description = text;

            return keyPhraseResult;
        }

        private KeyPhraseResults ToKeyPhraseResult(ICollection<string> keyPhrases)
        {
            if (keyPhrases == null)
            {
                throw new ArgumentNullException(nameof(keyPhrases));
            }

            var results = new KeyPhraseResults
            {
                PopularTags = Tags.Intersect(keyPhrases).ToList(),
                UnpopularTags = keyPhrases.Except(Tags).ToList()
            };

            return results;
        }
    }
}