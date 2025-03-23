using System.IO;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

public interface IPanelProvider
{
    void BeginPanel(Panel panel, TextWriter writer);

    void BeginPanelSection(PanelSectionType sectionType, TextWriter writer, string title = null);

    void EndPanel(Panel panel, TextWriter writer);

    void EndPanelSection(PanelSectionType sectionType, TextWriter writer);
}