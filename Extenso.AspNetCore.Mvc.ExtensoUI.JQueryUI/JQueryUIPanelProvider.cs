using System.IO;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.JQueryUI;

public class JQueryUIPanelProvider : IPanelProvider
{
    #region IPanelProvider Members

    public void BeginPanel(Panel panel, TextWriter writer)
    {
        panel.EnsureClass("controlgroup");

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
                    writer.Write($@"<div class=""panel-heading""><h3 class=""panel-title"">{title}</h3>");
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

    public void EndPanel(Panel panel, TextWriter writer) => writer.Write("</div>");

    public void EndPanelSection(PanelSectionType sectionType, TextWriter writer) => writer.Write("</div>");

    #endregion IPanelProvider Members
}