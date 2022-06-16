using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StoreApp.TagHelpers
{
    [HtmlTargetElement("article-header")]
    public class HeaderTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "h2";
            output.Attributes.RemoveAll("article-header");
        }
    }
}