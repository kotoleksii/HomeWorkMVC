using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StoreApp.TagHelpers
{
    public class ItemTagHelper : TagHelper
    {
        public string Count { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "input";
            output.Attributes.SetAttribute("value", "Кількість товарів: " + Count);
            output.Attributes.Add("readonly", "readonly");
            output.Attributes.SetAttribute("style", "border:none; pointer-events: none;");
        }
    }
}