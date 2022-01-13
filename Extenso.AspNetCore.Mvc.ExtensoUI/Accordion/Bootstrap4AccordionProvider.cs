using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public class Bootstrap4AccordionProvider : IAccordionProvider
    {
        #region IAccordionProvider Members

        public void BeginAccordion(Accordion accordion, TextWriter writer)
        {
            accordion.EnsureClass("accordion");

            var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
                .MergeAttributes(accordion.HtmlAttributes);

            string tag = builder.ToString();

            writer.Write(tag);
        }

        public void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded)
        {
            writer.Write(@"<div class=""card"">");

            var panel = new FluentTagBuilder("div")
                .AddCssClass("card-header")
                .MergeAttribute("id", $"{panelId}-heading")
                .StartTag("h5")
                    .AddCssClass("mb-0")
                    .StartTag("button")
                        .AddCssClass($"btn btn-link{(expanded ? string.Empty : " collapsed")}")
                        .MergeAttribute("type", "button")
                        .MergeAttribute("data-toggle", "collapse")
                        .MergeAttribute("data-target", $"#{panelId}")
                        .MergeAttribute("aria-controls", panelId)
                        .MergeAttribute("aria-expanded", expanded ? "true" : "false")
                        .SetInnerHtml(title)
                    .EndTag()
                .EndTag();

            writer.Write(panel.ToString());

            var collapse = new FluentTagBuilder("div", TagRenderMode.StartTag)
                .AddCssClass($"collapse{(expanded ? " show" : string.Empty)}")
                .MergeAttribute("id", panelId)
                .MergeAttribute("aria-labelledby", $"{panelId}-heading")
                .MergeAttribute("data-parent", $"#{parentAccordionId}");

            writer.Write(collapse.ToString());
            writer.Write(@"<div class=""card-body"">");
        }

        public void EndAccordion(Accordion accordion, TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void EndAccordionPanel(TextWriter writer)
        {
            writer.Write("</div></div></div>");
        }

        #endregion IAccordionProvider Members
    }
}