using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public class Bootstrap4PanelProvider : IPanelProvider
    {
        private Panel panel;

        #region IPanelProvider Members

        public void BeginPanel(Panel panel, TextWriter writer)
        {
            this.panel = panel;

            switch (panel.State)
            {
                case State.Default: panel.EnsureClass("card"); break;
                default: panel.EnsureClass("card text-white"); break;
            }

            var builder = new FluentTagBuilder("div", TagRenderMode.StartTag)
                .MergeAttributes(panel.HtmlAttributes);

            string tag = builder.ToString();

            writer.Write(tag);
        }

        public void BeginPanelSection(PanelSectionType sectionType, TextWriter writer, string title = null)
        {
            string headerClass = string.Empty;

            switch (panel.State)
            {
                case State.Default: headerClass = "bg-light"; break;
                case State.Danger: headerClass = "bg-danger"; break;
                case State.Info: headerClass = "bg-info"; break;
                case State.Inverse: headerClass = "bg-dark"; break;
                case State.Success: headerClass = "bg-success"; break;
                case State.Warning: headerClass = "bg-warning"; break;
                case State.Primary:
                default: headerClass = "bg-primary"; break;
            }

            switch (sectionType)
            {
                case PanelSectionType.Heading:
                    {
                        writer.Write($@"<div class=""card-header {headerClass}"">{title}");
                    }
                    break;

                case PanelSectionType.Body:
                    writer.Write(@"<div class=""card-body"">");
                    break;

                case PanelSectionType.Footer:
                    writer.Write(@"<div class=""card-footer"">");
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