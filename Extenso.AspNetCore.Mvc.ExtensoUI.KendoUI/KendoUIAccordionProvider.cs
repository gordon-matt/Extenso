using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Extenso.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI.KendoUI;

public class KendoUIAccordionProvider : IAccordionProvider
{
    private readonly BaseUIProvider uiProvider;

    public KendoUIAccordionProvider(BaseUIProvider uiProvider)
    {
        this.uiProvider = uiProvider;
    }

    #region IAccordionProvider Members

    public void BeginAccordion(Accordion accordion, TextWriter writer)
    {
        uiProvider.Scripts.Add(
$@"$('#{accordion.Id}').kendoPanelBar({{
    expandMode: 'single'
}});");

        var builder = new FluentTagBuilder("ul", TagRenderMode.StartTag)
            .MergeAttributes(accordion.HtmlAttributes);

        string tag = builder.ToString();

        writer.Write(tag);
    }

    public void BeginAccordionPanel(TextWriter writer, string title, string panelId, string parentAccordionId, bool expanded)
    {
        var li = new FluentTagBuilder("li", TagRenderMode.StartTag);

        if (expanded)
        {
            li.AddCssClass("k-state-active");
        }

        writer.Write(li.ToString());
        writer.Write(title);
        writer.Write(@"<div class=""k-content"">");
    }

    public void EndAccordion(Accordion accordion, TextWriter writer) => writer.Write("</ul>");

    public void EndAccordionPanel(TextWriter writer) => writer.Write("</div></li>");

    #endregion IAccordionProvider Members
}