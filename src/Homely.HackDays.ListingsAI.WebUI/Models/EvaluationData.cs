using Microsoft.CognitiveServices.ContentModerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homely.HackDays.ListingsAI.WebUI.Models
{
    public class EvaluationData
    {
        // <summary>
        // The URL of the evaluated image.
        // </summary>
        public string ImageUrl;

        /// <summary>
        /// The image moderation results.
        /// </summary>
        public Evaluate ImageModeration;

        /// <summary>
        /// The text detection results.
        /// </summary>
        public OCR TextDetection;

        /// <summary>
        /// The face detection results;
        /// </summary>
        public FoundFaces FaceDetection;
    }
}
