using System.IO;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.JQueryUI
{
    public class JQueryUIModalProvider : IModalProvider
    {
        private string modalId;
        private readonly BaseUIProvider uiProvider;

        public JQueryUIModalProvider(BaseUIProvider uiProvider)
        {
            this.uiProvider = uiProvider;
        }

        #region IModalProvider Members

        public void BeginModal(Modal modal, TextWriter writer)
        {
            modalId = modal.Id;

            uiProvider.Scripts.Add(
$@"$(""#{modalId}"").dialog({{
    modal: true
}});");

            modal.EnsureHtmlAttribute("title", "Modal Dialog"); // this will be replaced in script...

            var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
                .MergeAttributes(modal.HtmlAttributes);

            string tag = builder.ToString();

            writer.Write(tag);
        }

        public void BeginModalSectionPanel(ModalSection section, TextWriter writer, string title = null)
        {
            if (!string.IsNullOrEmpty(title))
            {
                uiProvider.Scripts.Add($"$('#{modalId}').attr('title', '{title}')");
            }

            switch (section)
            {
                case ModalSection.Header:
                    {
                        writer.Write(@"<div class=""modal-header"">");
                    }
                    break;

                case ModalSection.Body: writer.Write(@"<div class=""modal-body"">"); break;
                case ModalSection.Footer: writer.Write(@"<div class=""modal-footer"">"); break;
            }
        }

        public void EndModal(Modal modal, TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void EndModalSectionPanel(ModalSection section, TextWriter writer)
        {
            writer.Write("</div>");
        }

        public IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("button")
                .MergeAttribute("type", "button")
                .GenerateId($"{modalId}-launch-button")
                .AddCssClass("ui-button ui-widget ui-corner-all")
                .SetInnerHtml(text);

            uiProvider.Scripts.Add(
$@"$('#{modalId}-launch-button')
.bind('click', function() {{
    $('#{modalId}').dialog('open');
}});");

            return new HtmlString(builder.ToString());
        }

        public IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("button")
                .MergeAttribute("type", "button")
                .GenerateId($"{modalId}-close-button")
                .AddCssClass("ui-button ui-widget ui-corner-all")
                .SetInnerHtml(text);

            uiProvider.Scripts.Add(
$@"$('#{modalId}-close-button')
.bind('click', function() {{
    $('#{modalId}').dialog('close');
}});");
            
            return new HtmlString(builder.ToString());
        }

        #endregion IModalProvider Members
    }
}