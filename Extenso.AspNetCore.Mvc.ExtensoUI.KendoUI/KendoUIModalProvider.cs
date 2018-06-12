using System.IO;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.KendoUI
{
    public class KendoUIModalProvider : IModalProvider
    {
        private string title;

        private readonly BaseUIProvider uiProvider;

        public KendoUIModalProvider(BaseUIProvider uiProvider)
        {
            this.uiProvider = uiProvider;
        }

        #region IModalProvider Members

        public void BeginModal(Modal modal, TextWriter writer)
        {
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
                        this.title = title;
                        writer.Write(@"<div class=""modal-header"">");
                    }
                    break;

                case ModalSection.Body: writer.Write(@"<div class=""modal-body"">"); break;
                case ModalSection.Footer: writer.Write(@"<div class=""modal-footer"">"); break;
            }
        }

        public void EndModal(Modal modal, TextWriter writer)
        {
            uiProvider.Scripts.Add(
$@"$('#{modal.Id}').kendoWindow({{
    actions: ['Maximize', 'Minimize', 'Close'],
    modal: true,
    draggable: true,
    resizable: true,
    title: '{title}',
    width: '500px',
    height: '300px',
    visible: false
}}).data('kendoWindow').center();");

            writer.Write("</div>");
        }

        public void EndModalSectionPanel(ModalSection section, TextWriter writer)
        {
            writer.Write("</div>");
        }

        public IHtmlContent ModalLaunchButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("span")
                .GenerateId($"{modalId}-launch-button")
                .AddCssClass("k-button")
                .SetInnerHtml(text);

            uiProvider.Scripts.Add(
$@"$('#{modalId}-launch-button')
.bind('click', function() {{
    $('#{modalId}').data('kendoWindow').open();
}});");

            return new HtmlString(builder.ToString());
        }

        public IHtmlContent ModalCloseButton(string modalId, string text, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("span")
                .GenerateId($"{modalId}-close-button")
                .AddCssClass("k-button")
                .SetInnerHtml(text);

            uiProvider.Scripts.Add(
$@"$('#{modalId}-close-button')
.bind('click', function() {{
    $('#{modalId}').data('kendoWindow').close();
}});");

            return new HtmlString(builder.ToString());
        }

        #endregion IModalProvider Members
    }
}