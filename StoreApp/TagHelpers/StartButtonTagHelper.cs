using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace StoreApp.TagHelpers
{
    public class StartButtonTagHelper : TagHelper
    {
        public bool Condition { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!Condition)
            {
                output.SuppressOutput();
            }
            else
            {
                output.TagName = "div";
                output.TagMode = TagMode.StartTagAndEndTag;
                output.Content.SetHtmlContent(@"<a href=""/Home/Goods"" class=""btn btn-primary"">Go To Goods</a>");
            }
        }
    }
}
