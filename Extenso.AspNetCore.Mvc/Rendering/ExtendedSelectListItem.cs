using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.Rendering
{
    internal class ExtendedSelectListItem : SelectListItem
    {
        public object HtmlAttributes { get; set; }

        public string Category { get; set; }
    }
}