using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Homely.HackDays.ListingsAI.WebUI.Models;
using Homely.HackDays.ListingsAI.WebUI.Services;

namespace Homely.HackDays.ListingsAI.WebUI.Controllers
{
    public class KeyPhraseExtractionController : Controller
    {
        private readonly IKeyPhraseExtractionService _keyPhraseExtractionService;

        public KeyPhraseExtractionController(IKeyPhraseExtractionService keyPhraseExtractionService)
        {
            _keyPhraseExtractionService = keyPhraseExtractionService ?? throw new ArgumentNullException(nameof(keyPhraseExtractionService));
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int listingId)
        {
            if (listingId <= 0)
            {
                return BadRequest("No listingId was provided or it was not a positive number.");
            }

            var results = await _keyPhraseExtractionService.ExtractTagsFromListingAsync(listingId);
//results.PopularTags.Any();
            return View(results);
        }
    }
}
