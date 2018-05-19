using System.Collections.Generic;
using System.IO;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public abstract class HtmlElement
    {
        public IDictionary<string, object> HtmlAttributes { get; private set; }

        public HtmlElement(object htmlAttributes)
        {
            HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }

        internal IExtensoUIProvider Provider { get; set; }

        protected internal abstract void StartTag(TextWriter textWriter);

        protected internal abstract void EndTag(TextWriter textWriter);

        public void EnsureClass(string className)
        {
            HtmlAttributes.EnsureCssClass(className);
        }

        public void EnsureHtmlAttribute(string key, string value, bool replaceExisting = true)
        {
            HtmlAttributes.EnsureHtmlAttribute(key, value, replaceExisting);
        }
    }
}