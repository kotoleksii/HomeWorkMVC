using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace StoreApp.TagHelpers
{
    public class YearTagHelper: TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Content.SetContent(DateTime.Now.Year.ToString());
        }
    }
}
