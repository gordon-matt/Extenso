using Extenso.AspNetCore.Mvc.Rendering;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Extenso.AspNetCore.Mvc.Tests.Rendering
{
    public class FluentTagBuilderExtensions
    {
        [Fact]
        public void Test1()
        {
            string expected = @"<button class=""btn btn-primary"" data-id=""Foo"" onclick=""foo_onClick();"" type=""button"">Click Me</button>";

            string actual = new FluentTagBuilder("button")
                .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new { data_id = "Foo" }))
                .MergeAttribute("type", "button")
                .MergeAttribute("onclick", "foo_onClick();")
                .AddCssClass("btn btn-primary")
                .SetInnerHtml("Click Me")
                .ToString();

            actual.Should().Be(expected);
        }

        [Fact]
        public void Test2()
        {
            string expected =
@"<table class=""table table-striped"" id=""my-table"">
<thead>
<tr><th>First Name</th><th>Last Name</th></tr>
</thead>
<tbody>
<tr class=""odd-row""><td>John</td><td>Doe</td></tr>
<tr class=""even-row""><td>George</td><td>Smith</td></tr>
<tr class=""odd-row""><td>Michael</td><td>Berry</td></tr>
</tbody>
</table>";

            string actual = new FluentTagBuilder("table")
                .AddCssClass("table table-striped")
                .MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new { id = "my-table" }))
                    .StartTag("thead")
                        .StartTag("tr")
                            .StartTag("th")
                                .SetInnerHtml("First Name")
                            .EndTag()
                            .StartTag("th")
                                .SetInnerHtml("Last Name")
                            .EndTag()
                        .EndTag() // </tr>
                    .EndTag() // </thead>
                    .StartTag("tbody")
                        .StartTag("tr")
                            .AddCssClass("odd-row")
                            .StartTag("td")
                                .SetInnerHtml("John")
                            .EndTag()
                            .StartTag("td")
                                .SetInnerHtml("Doe")
                            .EndTag()
                        .EndTag() // </tr>
                        .StartTag("tr")
                            .AddCssClass("even-row")
                            .StartTag("td")
                                .SetInnerHtml("George")
                            .EndTag()
                            .StartTag("td")
                                .SetInnerHtml("Smith")
                            .EndTag()
                        .EndTag() // </tr>
                        .StartTag("tr")
                            .AddCssClass("odd-row")
                            .StartTag("td")
                                .SetInnerHtml("Michael")
                            .EndTag()
                            .StartTag("td")
                                .SetInnerHtml("Berry")
                            .EndTag()
                        .EndTag() // </tr>
                    .EndTag() // </tbody>
                .ToString();

            actual.Should().Be(expected.Replace(Environment.NewLine, string.Empty));
        }
    }
}