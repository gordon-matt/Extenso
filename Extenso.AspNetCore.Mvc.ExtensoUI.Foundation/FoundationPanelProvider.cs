using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.Foundation
{
    public class FoundationPanelProvider : IPanelProvider
    {
        #region IPanelProvider Members

        public void BeginPanel(Panel panel, TextWriter writer)
        {
            switch (panel.State)
            {
                case State.Danger: panel.EnsureClass("callout alert"); break;
                case State.Info: panel.EnsureClass("callout secondary"); break;
                case State.Inverse: panel.EnsureClass("callout secondary"); break;
                case State.Primary: panel.EnsureClass("callout primary"); break;
                case State.Success: panel.EnsureClass("callout success"); break;
                case State.Warning: panel.EnsureClass("callout warning"); break;
                case State.Default:
                default: panel.EnsureClass("callout"); break;
            }

            var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
                .MergeAttributes(panel.HtmlAttributes);

            string tag = builder.ToString();

            writer.Write(tag);
        }

        public void BeginPanelSection(PanelSectionType sectionType, TextWriter writer, string title = null)
        {
            switch (sectionType)
            {
                case PanelSectionType.Heading:
                    {
                        writer.Write($@"<div class=""panel-heading""><h5 class=""panel-title"">{title}</h5>");
                    }
                    break;

                case PanelSectionType.Body:
                    writer.Write(@"<div class=""panel-body"">");
                    break;

                case PanelSectionType.Footer:
                    writer.Write(@"<div class=""panel-footer"">");
                    break;
            }
        }

        public void EndPanel(Panel panel, TextWriter writer)
        {
            writer.Write("</div>");
        }

        public void EndPanelSection(PanelSectionType sectionType, TextWriter writer)
        {
            writer.Write("</div>");
        }

        #endregion IPanelProvider Members
    }
}