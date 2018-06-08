using System.Collections.Generic;

namespace Homely.HackDays.ListingsAI.WebUI.Models
{
    public class KeyPhraseResults
    {
        public string Description { get; set; }
        public ICollection<string> PopularTags { get; set; }
        public ICollection<string> UnpopularTags { get; set; }
    }
}