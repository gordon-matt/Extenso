using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

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

            return Image(helper, urlHelper, string.Concat("data:image/jpg;base64,", base64), alt, htmlAttributes);
        }

        public static IHtmlContent Image(this IHtmlHelper helper, IUrlHelper urlHelper, string src, string alt, object htmlAttributes = null)
        {
            var builder = new TagBuilder("img");
            builder.TagRenderMode = TagRenderMode.SelfClosing;
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

        public static IHtmlContent FileUpload(this IHtmlHelper html, string name, object htmlAttributes = null)
        {
            var builder = new TagBuilder("input");
            builder.TagRenderMode = TagRenderMode.SelfClosing;
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
        public static IHtmlContent JsonObject<TEntity>(this IHtmlHelper html, string name, TEntity item)
        {
            return new HtmlString(string.Format("var {0} = {1};", name, item.JsonSerialize()));
        }

        #endregion Other

        public static IHtmlContent CheckBoxList(
            this IHtmlHelper html,
            string name,
            IEnumerable<SelectListItem> selectList,
            IEnumerable<string> selectedValues,
            byte numberOfColumns = 1,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null)
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
                    sb.AppendFormat(@"<label class=""checkbox-list-group-label"">{0}</label>", group.Key);

                    foreach (var item in group)
                    {
                        var isChecked = values.Contains(item.Value);

                        var tagBuilder = new FluentTagBuilder("label")
                            .MergeAttribute("for", fullHtmlFieldName)
                            .MergeAttributes(labelHtmlAttributes)
                            .StartTag("input", TagRenderMode.SelfClosing)
                                .MergeAttribute("type", "checkbox")
                                .MergeAttribute("name", fullHtmlFieldName)
                                .MergeAttribute("id", fullHtmlFieldId + "_" + index)
                                .MergeAttribute("value", item.Value);

                        if (isChecked)
                        {
                            tagBuilder = tagBuilder.MergeAttribute("checked", "checked");
                        }

                        if (checkboxHtmlAttributes != null)
                        {
                            tagBuilder = tagBuilder.MergeAttributes(checkboxHtmlAttributes);
                        }

                        tagBuilder = tagBuilder.EndTag(); //end checkbox
                        tagBuilder = tagBuilder
                            .StartTag("span")
                                .SetInnerHtml(item.Text)
                            .EndTag();

                        sb.Append(tagBuilder.ToString());
                        index++;
                    }
                }
            }
            else
            {
                var rows = (int)Math.Ceiling((selectList.Count() * 1d) / numberOfColumns);
                var columnWidth = (int)Math.Ceiling(12d / numberOfColumns);

                for (var i = 0; i < numberOfColumns; i++)
                {
                    var items = selectList.Skip(i * rows).Take(rows);
                    sb.AppendFormat("<div class=\"col-md-{0}\">", columnWidth);

                    foreach (var item in items)
                    {
                        var isChecked = values.Contains(item.Value);

                        var tagBuilder = new FluentTagBuilder("label")
                            .MergeAttribute("for", fullHtmlFieldName)
                            .MergeAttributes(labelHtmlAttributes)
                            .StartTag("input", TagRenderMode.SelfClosing)
                                .MergeAttribute("type", "checkbox")
                                .MergeAttribute("name", fullHtmlFieldName)
                                .MergeAttribute("id", fullHtmlFieldId + "_" + index)
                                .MergeAttribute("value", item.Value);

                        if (isChecked)
                        {
                            tagBuilder = tagBuilder.MergeAttribute("checked", "checked");
                        }

                        if (checkboxHtmlAttributes != null)
                        {
                            tagBuilder = tagBuilder.MergeAttributes(checkboxHtmlAttributes);
                        }

                        tagBuilder = tagBuilder.EndTag(); //end checkbox
                        tagBuilder = tagBuilder
                            .StartTag("span")
                                .SetInnerHtml(item.Text)
                            .EndTag();

                        sb.Append(tagBuilder.ToString());
                        index++;
                    }

                    sb.Append("</div>");
                }
            }

            return new HtmlString(sb.ToString());
        }

        public static IHtmlContent CheckBoxListFor<TModel, TProperty>(
            this IHtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TProperty>>> expression,
            IEnumerable<SelectListItem> selectList,
            byte numberOfColumns = 1,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null) where TModel : class
        {
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
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
                    sb.AppendFormat("<strong>{0}</strong>", group.Key);

                    foreach (var item in group)
                    {
                        var isChecked = values.Contains(item.Value);

                        var tagBuilder = new FluentTagBuilder("label")
                            .MergeAttribute("for", fullHtmlFieldName)
                            .MergeAttributes(labelHtmlAttributes)
                            .StartTag("input", TagRenderMode.SelfClosing)
                                .MergeAttribute("type", "checkbox")
                                .MergeAttribute("name", fullHtmlFieldName)
                                .MergeAttribute("id", fullHtmlFieldId + "_" + index)
                                .MergeAttribute("value", item.Value);

                        if (isChecked)
                        {
                            tagBuilder = tagBuilder.MergeAttribute("checked", "checked");
                        }

                        if (checkboxHtmlAttributes != null)
                        {
                            tagBuilder = tagBuilder.MergeAttributes(checkboxHtmlAttributes);
                        }

                        tagBuilder = tagBuilder.EndTag(); //end checkbox
                        tagBuilder = tagBuilder
                            .StartTag("span")
                                .SetInnerHtml(item.Text)
                            .EndTag();

                        sb.Append(tagBuilder.ToString());
                        index++;
                    }
                }
            }
            else
            {
                var rows = (int)Math.Ceiling((selectList.Count() * 1d) / numberOfColumns);
                var columnWidth = (int)Math.Ceiling(12d / numberOfColumns);

                for (var i = 0; i < numberOfColumns; i++)
                {
                    var items = selectList.Skip(i * rows).Take(rows);
                    sb.AppendFormat("<div class=\"col-md-{0}\">", columnWidth);

                    foreach (var item in items)
                    {
                        var isChecked = values.Contains(item.Value);

                        var tagBuilder = new FluentTagBuilder("label")
                            .MergeAttribute("for", fullHtmlFieldName)
                            .MergeAttributes(labelHtmlAttributes)
                            .StartTag("input", TagRenderMode.SelfClosing)
                                .MergeAttribute("type", "checkbox")
                                .MergeAttribute("name", fullHtmlFieldName)
                                .MergeAttribute("id", fullHtmlFieldId + "_" + index)
                                .MergeAttribute("value", item.Value);

                        if (isChecked)
                        {
                            tagBuilder = tagBuilder.MergeAttribute("checked", "checked");
                        }

                        if (checkboxHtmlAttributes != null)
                        {
                            tagBuilder = tagBuilder.MergeAttributes(checkboxHtmlAttributes);
                        }

                        tagBuilder = tagBuilder.EndTag(); //end checkbox
                        tagBuilder = tagBuilder
                            .StartTag("span")
                                .SetInnerHtml(item.Text)
                            .EndTag();

                        sb.Append(tagBuilder.ToString());
                        index++;
                    }

                    sb.Append("</div>");
                }
            }

            return new HtmlString(sb.ToString());
        }

        public static IHtmlContent EnumMultiDropDownListFor<TModel, TEnum>(this IHtmlHelper<TModel> html, Expression<Func<TModel, IEnumerable<TEnum>>> expression, string emptyText = null, object htmlAttributes = null) where TEnum : struct
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

        //TODO: Test
        public static IHtmlContent Table<T>(this IHtmlHelper html, IEnumerable<T> items, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("table")
                .MergeAttributes(htmlAttributes)
                .StartTag("thead")
                    .StartTag("tr");

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                builder = builder.StartTag("th").SetInnerHtml(property.Name).EndTag();
            }

            builder
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
    }
}