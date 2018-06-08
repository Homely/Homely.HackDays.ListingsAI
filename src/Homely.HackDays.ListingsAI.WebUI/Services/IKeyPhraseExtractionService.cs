
using System.Threading.Tasks;
using Homely.HackDays.ListingsAI.WebUI.Models;

namespace Homely.HackDays.ListingsAI.WebUI.Services
{
    public interface IKeyPhraseExtractionService
    {
        Task<KeyPhraseResults> ExtractTagsFromListingAsync(int listingId);
        Task<KeyPhraseResults> ExtractTagsAsync(string text);
    }
}