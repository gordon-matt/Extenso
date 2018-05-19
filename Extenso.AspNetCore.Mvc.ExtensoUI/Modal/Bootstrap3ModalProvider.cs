﻿using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public class Bootstrap3ModalProvider : IModalProvider
    {
        #region IModalProvider Members

        public void BeginModal(Modal modal, TextWriter writer)
        {
            modal.EnsureClass("modal fade");
            modal.EnsureHtmlAttribute("tabindex", "-1");
            modal.EnsureHtmlAttribute("role", "dialog");
            modal.EnsureHtmlAttribute("aria-labelledby", $"{modal.Id}-label");
            modal.EnsureHtmlAttribute("aria-hidden", "true");

            var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
                .MergeAttributes(modal.HtmlAttributes);

            string tag = builder.ToString();

            writer.Write(tag);
        }

        public void BeginModalSectionPanel(ModalSection section, TextWriter writer, string title = null)
        {
            switch (section)
            {
                case ModalSection.Header:
                    {
                        writer.Write(@"<div class=""modal-dialog""><div class=""modal-content"">");

                        //var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
                        //    .AddCssClass("modal-dialog")
                        //    .StartTag("div", TagRenderMode.StartTag)
                        //        .AddCssClass("modal-content")
                        //    .EndTag();

                        //writer.Write(builder.ToString());

                        writer.Write(@"<div class=""modal-header"">");

                        writer.Write(
$@"<button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
    <span aria-hidden=""true"">&times;</span>
</button>
<h4 class=""modal-title"">{title}</h4>");
                    }
                    break;

                case ModalSection.Body: writer.Write(@"<div class=""modal-body"">"); break;
                case ModalSection.Footer: writer.Write(@"<div class=""modal-footer"">"); break;
            }
        }

        public void EndModalSectionPanel(ModalSection section, TextWriter writer)
        {
            //if (section == ModalSection.Footer)
            //{
            //    writer.Write("</div></div></div>");
            //}
            //else
            //{
            //    writer.Write("</div>");
            //}
            writer.Write("</div>");
        }

        public void EndModal(Modal modal, TextWriter writer)
        {
            //writer.Write("</div>");
            writer.Write("</div></div></div>");
        }

        public IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("button")
                .AddCssClass("btn btn-primary")
                .MergeAttribute("type", "button")
                .MergeAttribute("data-toggle", "modal")
                .MergeAttribute("data-target", $"#{modalId}")
                .SetInnerHtml(text);

            return new HtmlString(builder.ToString());
        }

        public IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("button")
                .AddCssClass("btn btn-default")
                .MergeAttribute("type", "button")
                .MergeAttribute("data-dismiss", "modal")
                .SetInnerHtml(text);

            return new HtmlString(builder.ToString());
        }

        #endregion IModalProvider Members
    }
}