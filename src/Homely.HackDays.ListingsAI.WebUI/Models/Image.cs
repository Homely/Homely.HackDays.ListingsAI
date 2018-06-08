using Newtonsoft.Json;

namespace Homely.HackDays.ListingsAI.WebUI.Models
{
    public class Image
    {
        public string Identifier { get; set; }

        [JsonIgnore]
        public string Url => $"https://res-2.cloudinary.com/hd1n2hd4y/image/upload/{Identifier}.jpg";
    }
}
