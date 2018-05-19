using System.Collections.Generic;
using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI
{
    public static class HtmlHelperExtensions
    {
        public static ExtensoUI<TModel> ExtensoUI<TModel>(this IHtmlHelper<TModel> htmlHelper, IExtensoUIProvider provider = null)
        {
            if (provider != null)
            {
                return new ExtensoUI<TModel>(htmlHelper, provider);
            }

            string areaName = (string)htmlHelper.ViewContext.RouteData.DataTokens["area"];
            if (!string.IsNullOrEmpty(areaName) && ExtensoUISettings.AreaUIProviders.ContainsKey(areaName))
            {
                return new ExtensoUI<TModel>(htmlHelper, ExtensoUISettings.AreaUIProviders[areaName]);
            }

            return new ExtensoUI<TModel>(htmlHelper);
        }

        internal static void EnsureCssClass(this IDictionary<string, object> htmlAttributes, string className)
        {
            htmlAttributes.EnsureHtmlAttribute("class", className, false);
        }

        internal static void EnsureHtmlAttribute(this IDictionary<string, object> htmlAttributes, string key, string value, bool replaceExisting = true)
        {
            if (htmlAttributes.ContainsKey(key))
            {
                if (replaceExisting)
                {
                    htmlAttributes[key] = value;
                }
                else
                {
                    htmlAttributes[key] += $" {value}";
                }
            }
            else
            {
                htmlAttributes.Add(key, value);
            }
        }
    }
}