using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Extenso.AspNetCore.Mvc.ViewFeatures;
using Extenso.Collections;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.Rendering
{
    public enum PageTarget : byte
    {
        Default = 0,
        Blank = 1,
        Parent = 2,
        Self = 3,
        Top = 4
    }

    public static class HtmlHelperExtensions
    {
        #region Images

        public static IHtmlContent EmbeddedImage(this IHtmlHelper helper, IUrlHelper urlHelper, Assembly assembly, string resourceName, string alt, object htmlAttributes = null)
        {
            string base64 = string.Empty;
            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream())
            {
                resourceStream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                base64 = Convert.ToBase64String(memoryStream.ToArray());
            }

            return Image(helper, urlHelper, $"data:image/jpg;base64,{base64}", alt, htmlAttributes);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method.")]
        public static IHtmlContent Image(this IHtmlHelper helper, IUrlHelper urlHelper, string src, string alt, object htmlAttributes = null)
        {
            var builder = new TagBuilder("img")
            {
                TagRenderMode = TagRenderMode.SelfClosing
            };
            builder.MergeAttribute("src", urlHelper.Content(src));

            if (!string.IsNullOrEmpty(alt))
            {
                builder.MergeAttribute("alt", alt);
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return new HtmlString(builder.Build());
        }

        public static IHtmlContent ImageLink(this IHtmlHelper helper, IUrlHelper urlHelper, string src, string alt, string href, object aHtmlAttributes = null, object imgHtmlAttributes = null, PageTarget target = PageTarget.Default)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", href);

            switch (target)
            {
                case PageTarget.Blank: builder.MergeAttribute("target", "_blank"); break;
                case PageTarget.Parent: builder.MergeAttribute("target", "_parent"); break;
                case PageTarget.Self: builder.MergeAttribute("target", "_self"); break;
                case PageTarget.Top: builder.MergeAttribute("target", "_top"); break;
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(aHtmlAttributes));

            var img = helper.Image(urlHelper, src, alt, imgHtmlAttributes);

            builder.InnerHtml.AppendHtml(img.ToString());

            return new HtmlString(builder.Build());
        }

        #endregion Images

        #region NumbersDropDown

        public static IHtmlContent NumbersDropDown(this IHtmlHelper html, string name, int min, int max, int? selected = null, string emptyText = null, object htmlAttributes = null)
        {
            var numbers = new List<int>();
            for (int i = min; i <= max; i++)
            {
                numbers.Add(i);
            }

            var selectList = numbers.ToSelectList(value => value, text => text.ToString(), selected, emptyText);

            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public static IHtmlContent NumbersDropDownFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int min, int max, string emptyText = null, object htmlAttributes = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var numbers = new List<int>();
            for (int i = min; i <= max; i++)
            {
                numbers.Add(i);
            }

            var selectList = numbers.ToSelectList(value => value, text => text.ToString(), selectedValue, emptyText);

            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        #endregion NumbersDropDown

        #region Other

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method.")]
        public static IHtmlContent FileUpload(this IHtmlHelper html, string name, object htmlAttributes = null)
        {
            var builder = new TagBuilder("input")
            {
                TagRenderMode = TagRenderMode.SelfClosing
            };
            builder.MergeAttribute("type", "file");
            builder.GenerateId(name, "_");

            if (!string.IsNullOrEmpty(name))
            {
                builder.MergeAttribute("name", name);
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return new HtmlString(builder.Build());
        }

        /// <summary>
        /// <para>Serializes the given item as JSON and returns a JavaScript statement assigning the JSON to a variable with given name.</para>
        /// <para>Use inside &lt;script&gt; tag</para>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="html"></param>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method.")]
        public static IHtmlContent JsonObject<TEntity>(this IHtmlHelper html, string name, TEntity item)
        {
            return new HtmlString($"var {name} = {item.JsonSerialize()};");
        }

        #endregion Other

        //public static IHtmlContent CheckBoxList(
        //    this IHtmlHelper html,
        //    string name,
        //    IEnumerable<SelectListItem> selectList,
        //    IEnumerable<string> selectedValues,
        //    byte numberOfColumns = 1,
        //    object labelHtmlAttributes = null,
        //    object checkboxHtmlAttributes = null)
        //{
        //    string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
        //    string fullHtmlFieldId = html.ViewData.TemplateInfo.GetFullHtmlFieldId(name);

        //    var values = new List<string>();
        //    if (selectedValues != null)
        //    {
        //        values.AddRange(selectedValues);
        //    }

        //    if (selectList.IsNullOrEmpty())
        //    {
        //        return HtmlString.Empty;
        //    }

        //    int index = 0;

        //    var sb = new StringBuilder();

        //    bool groupByCategory = (selectList.First() is ExtendedSelectListItem);

        //    if (groupByCategory)
        //    {
        //        var items = selectList.Cast<ExtendedSelectListItem>().ToList();
        //        var groups = items.GroupBy(x => x.Category);

        //        foreach (var @group in groups)
        //        {
        //            sb.Append($@"<label class=""checkbox-list-group-label"">{group.Key}</label>");

        //            foreach (var item in group)
        //            {
        //                string checkbox = CreateCheckBox(item, values, fullHtmlFieldId, fullHtmlFieldName, index, labelHtmlAttributes, checkboxHtmlAttributes);
        //                sb.Append(checkbox);
        //                index++;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var rows = (int)Math.Ceiling((selectList.Count() * 1d) / numberOfColumns);
        //        var columnWidth = (int)Math.Ceiling(12d / numberOfColumns);

        //        for (var i = 0; i < numberOfColumns; i++)
        //        {
        //            var items = selectList.Skip(i * rows).Take(rows);
        //            sb.Append($"<div class=\"col-md-{columnWidth}\">");

        //            foreach (var item in items)
        //            {
        //                string checkbox = CreateCheckBox(item, values, fullHtmlFieldId, fullHtmlFieldName, index, labelHtmlAttributes, checkboxHtmlAttributes);
        //                sb.Append(checkbox);
        //                index++;
        //            }

        //            sb.Append("</div>");
        //        }
        //    }

        //    return new HtmlString(sb.ToString());
        //}

        //public static IHtmlContent CheckBoxListFor<TModel, TProperty>(
        //    this IHtmlHelper<TModel> html,
        //    Expression<Func<TModel, IEnumerable<TProperty>>> expression,
        //    IEnumerable<SelectListItem> selectList,
        //    byte numberOfColumns = 1,
        //    object labelHtmlAttributes = null,
        //    object checkboxHtmlAttributes = null) where TModel : class
        //{
        //    string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        //    string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
        //    string fullHtmlFieldId = html.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);

        //    var func = expression.Compile();
        //    var selectedValues = func(html.ViewData.Model);

        //    var values = new List<string>();
        //    if (selectedValues != null)
        //    {
        //        values.AddRange(selectedValues.Select(x => Convert.ToString(x)));
        //    }

        //    if (selectList == null)
        //    {
        //        throw new ArgumentNullException(nameof(selectList));
        //    }

        //    int index = 0;

        //    var sb = new StringBuilder();

        //    bool groupByCategory = (selectList.First() is ExtendedSelectListItem);

        //    if (groupByCategory)
        //    {
        //        var items = selectList.Cast<ExtendedSelectListItem>().ToList();
        //        var groups = items.GroupBy(x => x.Category);

        //        foreach (var @group in groups)
        //        {
        //            sb.Append($"<strong>{group.Key}</strong>");

        //            foreach (var item in group)
        //            {
        //                string checkbox = CreateCheckBox(item, values, fullHtmlFieldId, fullHtmlFieldName, index, labelHtmlAttributes, checkboxHtmlAttributes);
        //                sb.Append(checkbox);
        //                index++;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var rows = (int)Math.Ceiling((selectList.Count() * 1d) / numberOfColumns);
        //        var columnWidth = (int)Math.Ceiling(12d / numberOfColumns);

        //        for (var i = 0; i < numberOfColumns; i++)
        //        {
        //            var items = selectList.Skip(i * rows).Take(rows);
        //            sb.Append($"<div class=\"col-md-{columnWidth}\">");

        //            foreach (var item in items)
        //            {
        //                string checkbox = CreateCheckBox(item, values, fullHtmlFieldId, fullHtmlFieldName, index, labelHtmlAttributes, checkboxHtmlAttributes);
        //                sb.Append(checkbox);
        //                index++;
        //            }

        //            sb.Append("</div>");
        //        }
        //    }

        //    return new HtmlString(sb.ToString());
        //}

        public static IHtmlContent CheckBoxList(
            this IHtmlHelper html,
            string name,
            IEnumerable<SelectListItem> selectList,
            IEnumerable<string> selectedValues,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null,
            bool inputInsideLabel = true,
            bool wrapInDiv = true,
            object wrapperHtmlAttributes = null)
        {
            string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            string fullHtmlFieldId = html.ViewData.TemplateInfo.GetFullHtmlFieldId(name);

            var values = new List<string>();
            if (selectedValues != null)
            {
                values.AddRange(selectedValues);
            }

            if (selectList.IsNullOrEmpty())
            {
                return HtmlString.Empty;
            }

            int index = 0;

            var sb = new StringBuilder();

            bool groupByCategory = (selectList.First() is ExtendedSelectListItem);

            if (groupByCategory)
            {
                var items = selectList.Cast<ExtendedSelectListItem>().ToList();
                var groups = items.GroupBy(x => x.Category);

                foreach (var @group in groups)
                {
                    sb.Append($@"<label class=""checkbox-list-group-label"">{group.Key}</label>");

                    foreach (var item in group)
                    {
                        string checkbox = CreateCheckBox(
                            item,
                            values,
                            fullHtmlFieldId,
                            fullHtmlFieldName,
                            index,
                            labelHtmlAttributes,
                            checkboxHtmlAttributes,
                            inputInsideLabel,
                            wrapInDiv,
                            wrapperHtmlAttributes);

                        sb.Append(checkbox);
                        index++;
                    }
                }
            }
            else
            {
                foreach (var item in selectList)
                {
                    string checkbox = CreateCheckBox(
                        item,
                        values,
                        fullHtmlFieldId,
                        fullHtmlFieldName,
                        index,
                        labelHtmlAttributes,
                        checkboxHtmlAttributes,
                        inputInsideLabel,
                        wrapInDiv,
                        wrapperHtmlAttributes);

                    sb.Append(checkbox);
                    index++;
                }
            }

            return new HtmlString(sb.ToString());
        }

        public static IHtmlContent CheckBoxListFor<TModel, TProperty>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TProperty>>> expression,
            IEnumerable<SelectListItem> selectList,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null,
            bool inputInsideLabel = true,
            bool wrapInDiv = true,
            object wrapperHtmlAttributes = null) where TModel : class
        {
            var expresionProvider = html.ViewContext.HttpContext.RequestServices
                .GetService(typeof(ModelExpressionProvider)) as ModelExpressionProvider;

            string htmlFieldName = expresionProvider.GetExpressionText(expression);
            string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            string fullHtmlFieldId = html.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);

            var func = expression.Compile();
            var selectedValues = func(html.ViewData.Model);

            var values = new List<string>();
            if (selectedValues != null)
            {
                values.AddRange(selectedValues.Select(x => Convert.ToString(x)));
            }

            if (selectList == null)
            {
                throw new ArgumentNullException(nameof(selectList));
            }

            int index = 0;

            var sb = new StringBuilder();

            bool groupByCategory = (selectList.First() is ExtendedSelectListItem);

            if (groupByCategory)
            {
                var items = selectList.Cast<ExtendedSelectListItem>().ToList();
                var groups = items.GroupBy(x => x.Category);

                foreach (var @group in groups)
                {
                    sb.Append($"<strong>{group.Key}</strong>");

                    foreach (var item in group)
                    {
                        string checkbox = CreateCheckBox(
                            item,
                            values,
                            fullHtmlFieldId,
                            fullHtmlFieldName,
                            index,
                            labelHtmlAttributes,
                            checkboxHtmlAttributes,
                            inputInsideLabel,
                            wrapInDiv,
                            wrapperHtmlAttributes);

                        sb.Append(checkbox);
                        index++;
                    }
                }
            }
            else
            {
                foreach (var item in selectList)
                {
                    string checkbox = CreateCheckBox(
                        item,
                        values,
                        fullHtmlFieldId,
                        fullHtmlFieldName,
                        index,
                        labelHtmlAttributes,
                        checkboxHtmlAttributes,
                        inputInsideLabel,
                        wrapInDiv,
                        wrapperHtmlAttributes);

                    sb.Append(checkbox);
                    index++;
                }
            }

            return new HtmlString(sb.ToString());
        }

        private static string CreateCheckBox(
            SelectListItem item,
            IEnumerable<string> values,
            string fullHtmlFieldId,
            string fullHtmlFieldName,
            int index,
            object labelHtmlAttributes,
            object checkboxHtmlAttributes,
            bool inputInsideLabel,
            bool wrapInDiv,
            object wrapperHtmlAttributes)
        {
            FluentTagBuilder tagBuilder;

            if (wrapInDiv)
            {
                tagBuilder = new FluentTagBuilder("div")
                    .MergeAttributes(wrapperHtmlAttributes ?? new { @class = "extenso-checkbox" })
                    .StartTag("label");
            }
            else
            {
                tagBuilder = new FluentTagBuilder("label");
            }

            tagBuilder = tagBuilder
                .MergeAttribute("for", fullHtmlFieldName)
                .MergeAttributes(labelHtmlAttributes);

            if (!inputInsideLabel)
            {
                tagBuilder = tagBuilder.EndTag(); // end label
            }

            #region CheckBox Input

            tagBuilder = tagBuilder
                .StartTag("input", TagRenderMode.SelfClosing)
                    .MergeAttribute("type", "checkbox")
                    .MergeAttribute("name", fullHtmlFieldName)
                    .MergeAttribute("id", $"{fullHtmlFieldId}_{index}")
                    .MergeAttribute("value", item.Value);

            bool isChecked = values.Contains(item.Value);
            if (isChecked)
            {
                tagBuilder = tagBuilder.MergeAttribute("checked", "checked");
            }

            if (checkboxHtmlAttributes != null)
            {
                tagBuilder = tagBuilder.MergeAttributes(checkboxHtmlAttributes);
            }

            tagBuilder = tagBuilder.EndTag(); //end checkbox

            #endregion CheckBox Input

            // CheckBox Texts
            tagBuilder = tagBuilder
                .StartTag("span")
                    .SetInnerHtml(item.Text)
                .EndTag(); // end span

            if (inputInsideLabel)
            {
                tagBuilder = tagBuilder.EndTag(); // end label
            }

            return tagBuilder.ToString(); // final div or label ended automatically
        }

        #region Cultures

        public static IHtmlContent CulturesDropDownList<TModel>(
            this IHtmlHelper<TModel> html, string name, string selectedValue = null, object htmlAttributes = null, string emptyText = null)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var selectList = cultures
                .OrderBy(x => x.DisplayName)
                .ToSelectList(
                    value => value.Name,
                    text => text.DisplayName,
                    selectedValue,
                    emptyText);

            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public static IHtmlContent CulturesDropDownListFor<TModel>(
            this IHtmlHelper<TModel> html, Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var func = expression.Compile();
            string selectedValue = func(html.ViewData.Model);

            var selectList = cultures
                .OrderBy(x => x.DisplayName)
                .ToSelectList(
                    value => value.Name,
                    text => text.DisplayName,
                    selectedValue,
                    emptyText);

            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        #endregion Cultures

        public static IHtmlContent EnumMultiDropDownListFor<TModel, TEnum>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TEnum>>> expression,
            string emptyText = null,
            object htmlAttributes = null) where TEnum : struct
        {
            var func = expression.Compile();
            var selectedValues = func(html.ViewData.Model);

            var parsedSelectedValues = Enumerable.Empty<long>();

            if (selectedValues != null)
            {
                parsedSelectedValues = selectedValues.Select(x => Convert.ToInt64(x));
            }

            var multiSelectList = EnumExtensions.ToMultiSelectList<TEnum>(parsedSelectedValues, emptyText);

            return html.ListBoxFor(expression, multiSelectList, htmlAttributes);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method.")]
        public static IHtmlContent Table<T>(this IHtmlHelper html, IEnumerable<T> items, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("table")
                .MergeAttributes(htmlAttributes)
                .StartTag("thead")
                    .StartTag("tr");

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                builder = builder
                    .StartTag("th")
                        .SetInnerHtml(property.Name.SplitPascal())
                    .EndTag();
            }

            builder = builder
                .EndTag() // </tr>
                .EndTag() // </thead>
                .StartTag("tbody");

            foreach (var item in items)
            {
                builder = builder.StartTag("tr");
                foreach (var property in properties)
                {
                    string value = property.GetValue(item).ToString();
                    builder = builder.StartTag("td").SetInnerHtml(value).EndTag();
                }
                builder = builder.EndTag(); // </tr>
            }
            builder = builder.EndTag(); // </tbody>

            return new HtmlString(builder.ToString());
        }

        #region Time Zones

        public static IHtmlContent TimeZonesDropDownList<TModel>(
            this IHtmlHelper<TModel> html, string name, string selectedValue = null, object htmlAttributes = null, string emptyText = null)
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones();

            var selectList = timeZones
                .OrderBy(x => x.DisplayName)
                .ToSelectList(
                    value => value.Id,
                    text => text.DisplayName,
                    selectedValue,
                    emptyText);

            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public static IHtmlContent TimeZonesDropDownListFor<TModel>(
            this IHtmlHelper<TModel> html, Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones();

            var func = expression.Compile();
            string selectedValue = func(html.ViewData.Model);

            var selectList = timeZones
                .OrderBy(x => x.DisplayName)
                .ToSelectList(
                    value => value.Id,
                    text => text.DisplayName,
                    selectedValue,
                    emptyText);

            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        #endregion Time Zones
    }
}