using System.Collections.Generic;

namespace Homely.HackDays.ListingsAI.WebUI.Models.Vision
{
    public class DescribeResultModel
    {
        public string Url { get; set; }
        public IEnumerable<TagModel> Tags { get; set; }
        public string Caption { get; set; }
    }

    public class TagModel
    {
        public string Name { get; set; }
        public string CssClass { get; set; }

        public override string ToString()
        {
            return $"{Name} - {CssClass}";
        }
    }
}
