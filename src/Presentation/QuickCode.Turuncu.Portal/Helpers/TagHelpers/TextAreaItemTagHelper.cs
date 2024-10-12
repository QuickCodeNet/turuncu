using System;
using Microsoft.AspNetCore.Razor.TagHelpers;
using QuickCode.Turuncu.Portal.Models;

namespace QuickCode.Turuncu.Portal.TagHelpers
{
    [HtmlTargetElement("sg-text", TagStructure = TagStructure.WithoutEndTag)]
    public class TextAreaItemTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            TextAreaData textArea = new TextAreaData(Text);
            output.Content.SetContent(textArea.Data);
            output.TagName = "";

        }


        [HtmlAttributeName("text")]
        public string Text { get; set; }
    }
}
