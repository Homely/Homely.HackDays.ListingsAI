using Homely.HackDays.ListingsAI.WebUI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CognitiveServices.ContentModerator;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Homely.HackDays.ListingsAI.WebUI.Controllers
{
    public class TextContentModeratorController : Controller
    {
        private static readonly string OutputFile = @"C:\temp\TextModerationOutput.txt";

        public IActionResult Index()
        {
            return View();
        }

        public void ValidateDetails()
        {
            var text = System.IO.File.ReadAllText(@"C:\temp\Hackdays\SampleText.txt");
            text = text.Replace(System.Environment.NewLine, " ");

            byte[] byteArray = Encoding.ASCII.GetBytes(text);
            MemoryStream stream = new MemoryStream(byteArray);

            // Save the moderation results to a file.
            using (StreamWriter outputWriter = new StreamWriter(OutputFile, false))
            {
                // Create a Content Moderator client and evaluate the text.
                using (var client = ContentModeratorService.NewClient())
                {
                    // Screen the input text: check for profanity, classify the text into three
                    // categories do autocorrect text, and check for personally identifying
                    // information (PII)
                    outputWriter.WriteLine("Autocorrect typos, check for matching terms, PII, and classify.");
                    var screenResult =
                        client.TextModeration.ScreenText("text/plain", stream, "eng", true, true, null, true);
                    outputWriter.WriteLine(
                        JsonConvert.SerializeObject(screenResult, Formatting.Indented));
                }
                outputWriter.Flush();
                outputWriter.Close();
            }
        }
    }
}