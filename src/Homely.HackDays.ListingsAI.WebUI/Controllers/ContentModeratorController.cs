using Homely.HackDays.ListingsAI.WebUI.Services.ContentModeration;
using Microsoft.AspNetCore.Mvc;

namespace Homely.HackDays.ListingsAI.WebUI.Controllers
{
    public class ContentModeratorController : Controller
    {
        private readonly IContentModerationService _contentModerationService;

        public ContentModeratorController(IContentModerationService contentModerationService)
        {
            _contentModerationService = contentModerationService ?? throw new System.ArgumentNullException(nameof(contentModerationService));
        }

        public IActionResult ValidateText()
        {
            var text = System.IO.File.ReadAllText(@"App_Data\listingDetailsSample.txt");
            text = text.Replace(System.Environment.NewLine, " ");

            var result = _contentModerationService.ValidateText(text);
            return View(result);
        }

        public IActionResult ValidateImage()
        {
            var result = _contentModerationService.ValidateImage("https://i.imgflip.com/wo06b.jpg");
            return View(result);
        }
    }
}