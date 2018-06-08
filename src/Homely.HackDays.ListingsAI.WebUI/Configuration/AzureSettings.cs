namespace Homely.HackDays.ListingsAI.WebUI.Configuration
{
    public class AzureSettings
    {
        public const string ConfigurationKey = "Azure";

        public string TextAnalyticsApiKey { get; set; }
        public string ComputerVisionApiKey { get; set; }
    }
}
