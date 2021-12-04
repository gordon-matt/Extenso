using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.Rendering
{
    public class FluentTagBuilder
    {
        private readonly TagBuilder tagBuilder;
        private readonly FluentTagBuilder parent;
        private readonly StringBuilder stringBuilder;

        public FluentTagBuilder(string tagName, TagRenderMode renderMode = TagRenderMode.Normal, FluentTagBuilder parent = null)
        {
            tagBuilder = new TagBuilder(tagName)
            {
                TagRenderMode = renderMode
            };
            this.parent = parent;
            stringBuilder = new StringBuilder();
        }

        public FluentTagBuilder StartTag(string tagName, TagRenderMode renderMode = TagRenderMode.Normal)
        {
            return new FluentTagBuilder(tagName, renderMode, this);
        }

        public FluentTagBuilder EndTag()
        {
            tagBuilder.InnerHtml.AppendHtml(stringBuilder.ToString());
            stringBuilder.Clear();
            parent.AppendContent(this.ToString());
            return parent;
        }

        public FluentTagBuilder AppendContent(string content)
        {
            stringBuilder.Append(content);
            //tagBuilder.InnerHtml += content;
            return this;
        }

        public FluentTagBuilder AppendContentFormat(string format, params object[] args)
        {
            stringBuilder.AppendFormat(format, args);
            //tagBuilder.InnerHtml += string.Format(format, args);
            return this;
        }

        public FluentTagBuilder AddCssClass(string value)
        {
            tagBuilder.AddCssClass(value);
            return this;
        }

        public static string CreateSanitizedId(string originalId, string invalidCharReplacement = "_")
        {
            return TagBuilder.CreateSanitizedId(originalId, invalidCharReplacement);
        }

        public FluentTagBuilder GenerateId(string name, string invalidCharReplacement = "_")
        {
            tagBuilder.GenerateId(name, invalidCharReplacement);
            return this;
        }

        public FluentTagBuilder MergeAttributes(object attributes)
        {
            IDictionary<string, object> htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            tagBuilder.MergeAttributes(htmlAttributes);
            return this;
        }

        public FluentTagBuilder MergeAttributes(object attributes, bool replaceExisting)
        {
            IDictionary<string, object> htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            tagBuilder.MergeAttributes(htmlAttributes, replaceExisting);
            return this;
        }

        public FluentTagBuilder MergeAttribute(string key, string value)
        {
            tagBuilder.MergeAttribute(key, value);
            return this;
        }

        public FluentTagBuilder MergeAttribute(string key, object value)
        {
            tagBuilder.MergeAttribute(key, value.ToString());
            return this;
        }

        public FluentTagBuilder MergeAttribute(string key, string value, bool replaceExisting)
        {
            tagBuilder.MergeAttribute(key, value, replaceExisting);
            return this;
        }

        public FluentTagBuilder MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
        {
            tagBuilder.MergeAttributes(attributes);
            return this;
        }

        public FluentTagBuilder MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes, bool replaceExisting)
        {
            tagBuilder.MergeAttributes(attributes, replaceExisting);
            return this;
        }

        public FluentTagBuilder SetInnerHtml(string innerHtml)
        {
            tagBuilder.InnerHtml.AppendHtml(innerHtml);
            return this;
        }

        public override string ToString()
        {
            string content = stringBuilder.ToString();
            if (!string.IsNullOrEmpty(content))
            {
                tagBuilder.InnerHtml.AppendHtml(content);
            }
            return tagBuilder.Build();
        }
    }
}