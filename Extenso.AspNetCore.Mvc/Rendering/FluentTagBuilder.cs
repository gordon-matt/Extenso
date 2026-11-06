using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.Rendering;

/// <summary>
/// Represents the fluent tag builder.
/// </summary>
public class FluentTagBuilder
{
    private readonly TagBuilder tagBuilder;
    private readonly FluentTagBuilder parent;
    private readonly StringBuilder stringBuilder;

    /// <summary>
    /// Initializes a new instance of the FluentTagBuilder class.
    /// </summary>
    /// <param name="tagName">The tag name.</param>
    /// <param name="renderMode">The render mode.</param>
    /// <param name="parent">The parent.</param>
    public FluentTagBuilder(string tagName, TagRenderMode renderMode = TagRenderMode.Normal, FluentTagBuilder parent = null)
    {
        tagBuilder = new TagBuilder(tagName)
        {
            TagRenderMode = renderMode
        };
        this.parent = parent;
        stringBuilder = new StringBuilder();
    }

    /// <summary>
    /// Starts the tag.
    /// </summary>
    /// <param name="tagName">The tag name.</param>
    /// <param name="renderMode">The render mode.</param>
    /// <returns>The tag.</returns>
    public FluentTagBuilder StartTag(string tagName, TagRenderMode renderMode = TagRenderMode.Normal) => new(tagName, renderMode, this);

    /// <summary>
    /// Gets the end tag.
    /// </summary>
    /// <returns>The end tag.</returns>
    public FluentTagBuilder EndTag()
    {
        tagBuilder.InnerHtml.AppendHtml(stringBuilder.ToString());
        stringBuilder.Clear();
        parent.AppendContent(this.ToString());
        return parent;
    }

    /// <summary>
    /// Appends the content.
    /// </summary>
    /// <param name="content">The content.</param>
    /// <returns>The content.</returns>
    public FluentTagBuilder AppendContent(string content)
    {
        stringBuilder.Append(content);
        //tagBuilder.InnerHtml += content;
        return this;
    }

    /// <summary>
    /// Appends the content format.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="args">The args.</param>
    /// <returns>The content format.</returns>
    public FluentTagBuilder AppendContentFormat(string format, params object[] args)
    {
        stringBuilder.AppendFormat(format, args);
        //tagBuilder.InnerHtml += string.Format(format, args);
        return this;
    }

    /// <summary>
    /// Appends the content format.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="args">The args.</param>
    /// <returns>The content format.</returns>
    public FluentTagBuilder AppendContentFormat(string format, ReadOnlySpan<object> args)
    {
        stringBuilder.AppendFormat(format, args);
        //tagBuilder.InnerHtml += string.Format(format, args);
        return this;
    }

    /// <summary>
    /// Adds the CSS class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The CSS class.</returns>
    public FluentTagBuilder AddCssClass(string value)
    {
        tagBuilder.AddCssClass(value);
        return this;
    }

    /// <summary>
    /// Creates the sanitized identifier.
    /// </summary>
    /// <param name="originalId">The original identifier.</param>
    /// <param name="invalidCharReplacement">The invalid char replacement.</param>
    /// <returns>The sanitized identifier.</returns>
    public static string CreateSanitizedId(string originalId, string invalidCharReplacement = "_") => TagBuilder.CreateSanitizedId(originalId, invalidCharReplacement);

    /// <summary>
    /// Generates the identifier.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="invalidCharReplacement">The invalid char replacement.</param>
    /// <returns>The identifier.</returns>
    public FluentTagBuilder GenerateId(string name, string invalidCharReplacement = "_")
    {
        tagBuilder.GenerateId(name, invalidCharReplacement);
        return this;
    }

    /// <summary>
    /// Merges the attributes.
    /// </summary>
    /// <param name="attributes">The attributes.</param>
    /// <returns>The attributes.</returns>
    public FluentTagBuilder MergeAttributes(object attributes)
    {
        var htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
        tagBuilder.MergeAttributes(htmlAttributes);
        return this;
    }

    /// <summary>
    /// Merges the attributes.
    /// </summary>
    /// <param name="attributes">The attributes.</param>
    /// <param name="replaceExisting">The replace existing.</param>
    /// <returns>The attributes.</returns>
    public FluentTagBuilder MergeAttributes(object attributes, bool replaceExisting)
    {
        var htmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
        tagBuilder.MergeAttributes(htmlAttributes, replaceExisting);
        return this;
    }

    /// <summary>
    /// Merges the attribute.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The attribute.</returns>
    public FluentTagBuilder MergeAttribute(string key, string value)
    {
        tagBuilder.MergeAttribute(key, value);
        return this;
    }

    /// <summary>
    /// Merges the attribute.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>The attribute.</returns>
    public FluentTagBuilder MergeAttribute(string key, object value)
    {
        tagBuilder.MergeAttribute(key, value.ToString());
        return this;
    }

    /// <summary>
    /// Merges the attribute.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="replaceExisting">The replace existing.</param>
    /// <returns>The attribute.</returns>
    public FluentTagBuilder MergeAttribute(string key, string value, bool replaceExisting)
    {
        tagBuilder.MergeAttribute(key, value, replaceExisting);
        return this;
    }

    /// <summary>
    /// Merges the attributes.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="attributes">The attributes.</param>
    /// <returns>The attributes.</returns>
    public FluentTagBuilder MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
    {
        tagBuilder.MergeAttributes(attributes);
        return this;
    }

    /// <summary>
    /// Merges the attributes.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="attributes">The attributes.</param>
    /// <param name="replaceExisting">The replace existing.</param>
    /// <returns>The attributes.</returns>
    public FluentTagBuilder MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes, bool replaceExisting)
    {
        tagBuilder.MergeAttributes(attributes, replaceExisting);
        return this;
    }

    /// <summary>
    /// Sets the inner HTML.
    /// </summary>
    /// <param name="innerHtml">The inner HTML.</param>
    /// <returns>The inner HTML.</returns>
    public FluentTagBuilder SetInnerHtml(string innerHtml)
    {
        tagBuilder.InnerHtml.AppendHtml(innerHtml);
        return this;
    }

    /// <inheritdoc/>
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