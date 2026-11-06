using Extenso.AspNetCore.Mvc.ExtensoUI.Providers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Extenso.AspNetCore.Mvc.ExtensoUI;

/// <summary>
/// Provides extension methods for HTML helpers.
/// </summary>
public static class HtmlHelperExtensions
{
    extension<TModel>(IHtmlHelper<TModel> html)
    {
        /// <summary>
        /// Gets the extenso UI.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>The extenso UI.</returns>
        public ExtensoUI<TModel> ExtensoUI(IExtensoUIProvider provider = null)
        {
            if (provider != null)
            {
                return new ExtensoUI<TModel>(html, provider);
            }

            string areaName = (string)html.ViewContext.RouteData.DataTokens["area"];
            return !string.IsNullOrEmpty(areaName) && ExtensoUISettings.AreaUIProviders.ContainsKey(areaName)
                ? new ExtensoUI<TModel>(html, ExtensoUISettings.AreaUIProviders[areaName])
                : new ExtensoUI<TModel>(html);
        }
    }

    extension(IDictionary<string, object> htmlAttributes)
    {
        internal void EnsureCssClass(string className) => htmlAttributes.EnsureHtmlAttribute("class", className, false);

        internal void EnsureHtmlAttribute(string key, string value, bool replaceExisting = true)
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