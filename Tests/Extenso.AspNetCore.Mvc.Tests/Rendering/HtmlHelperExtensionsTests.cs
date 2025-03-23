using Extenso.AspNetCore.Mvc.Html;
using Extenso.AspNetCore.Mvc.Rendering;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Extenso.AspNetCore.Mvc.Tests.Rendering;

public class HtmlHelperExtensionsTests
{
    private readonly IHtmlHelper<TestObject> html;
    private readonly IUrlHelper url;
    private readonly IServiceProvider serviceProvider;

    public HtmlHelperExtensionsTests()
    {
        var webHost = Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .Build();

        serviceProvider = webHost.Services;

        var actionContext = new ActionContext(
            new DefaultHttpContext { RequestServices = serviceProvider },
            new RouteData(),
            new ActionDescriptor());

        var viewContext = new ViewContext(
            actionContext,
            Mock.Of<IView>(), //view
            new ViewDataDictionary<TestObject>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = new TestObject
                {
                    Quantity = 2
                }
            },
            new TempDataDictionary(actionContext.HttpContext, serviceProvider.GetService<ITempDataProvider>()),
            new StringWriter(),
            new HtmlHelperOptions()
        );

        html = serviceProvider.GetService<IHtmlHelper<TestObject>>();
        if (html is IViewContextAware)
        {
            ((IViewContextAware)html).Contextualize(viewContext);
        }

        //url = serviceProvider.GetService<IUrlHelper>();
        url = new UrlHelper(new ActionContext { RouteData = new RouteData() });
    }

    [Fact]
    public void Image()
    {
        string src = "/path/to/img.png";
        string alt = "Foo";
        string expected = $@"<img alt=""{alt}"" src=""{src}"" />";

        var actual = html.Image(url, src, alt);

        actual.ToString().Should().Be(expected);
    }

    [Fact]
    public void ImageLink()
    {
        string href = "/some/page";
        string src = "/path/to/img.png";
        string alt = "Foo";
        string expected = $@"<a class=""my-class"" href=""{href}"" target=""_blank""><img alt=""{alt}"" class=""thumb"" src=""{src}"" /></a>";

        var actual = html.ImageLink(
            url,
            src,
            alt,
            href,
            aHtmlAttributes: new { @class = "my-class" },
            imgHtmlAttributes: new { @class = "thumb" },
            target: PageTarget.Blank);

        actual.ToString().Should().Be(expected);
    }

    [Fact]
    public void NumbersDropDown()
    {
        string expected =
$@"<select class=""form-control"" data-val=""true"" data-val-required=""The Quantity field is required."" id=""Quantity"" name=""Quantity""><option value=""1"">1</option>
<option selected=""selected"" value=""2"">2</option>
<option value=""3"">3</option>
</select>";

        var actual = html.NumbersDropDown("Quantity", 1, 3, selected: 2, htmlAttributes: new { @class = "form-control" });

        actual.GetString().Should().Be(expected);
    }

    [Fact]
    public void NumbersDropDownFor()
    {
        string expected =
$@"<select class=""form-control"" data-val=""true"" data-val-required=""The Quantity field is required."" id=""Quantity"" name=""Quantity""><option value=""1"">1</option>
<option selected=""selected"" value=""2"">2</option>
<option value=""3"">3</option>
</select>";

        var actual = html.NumbersDropDownFor(m => m.Quantity, 1, 3, htmlAttributes: new { @class = "form-control" });

        actual.GetString().Should().Be(expected);
    }

    [Fact]
    public void FileUpload()
    {
        string expected = $@"<input class=""form-control"" id=""Foo"" name=""Foo"" type=""file"" />";

        var actual = html.FileUpload("Foo", htmlAttributes: new { @class = "form-control" });

        actual.ToString().Should().Be(expected);
    }

    [Fact]
    public void JsonObject()
    {
        string expected = @"const Foo = {""Bar"":""Baz""};";

        var actual = html.JsonObject("Foo", new { Bar = "Baz" });

        actual.ToString().Should().Be(expected);
    }

    [Fact]
    public void CheckBoxList()
    {
        string expected =
@"<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_0"" name=""DaysOfWeek"" type=""checkbox"" value=""0"" /><span>Sunday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_1"" name=""DaysOfWeek"" type=""checkbox"" value=""1"" /><span>Monday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_2"" name=""DaysOfWeek"" type=""checkbox"" value=""2"" /><span>Tuesday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_3"" name=""DaysOfWeek"" type=""checkbox"" value=""3"" /><span>Wednesday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_4"" name=""DaysOfWeek"" type=""checkbox"" value=""4"" /><span>Thursday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_5"" name=""DaysOfWeek"" type=""checkbox"" value=""5"" /><span>Friday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_6"" name=""DaysOfWeek"" type=""checkbox"" value=""6"" /><span>Saturday</span></label></div>";

        var actual = html.CheckBoxList(
            "DaysOfWeek",
            html.GetEnumSelectList<DayOfWeek>(),
            null,
            new { @class = "form-check-label" },
            new { @class = "form-check-input" },
            inputInsideLabel: true,
            wrapInDiv: true,
            wrapperHtmlAttributes: new { @class = "form-check" });

        actual.ToString().Should().Be(expected.Replace(Environment.NewLine, string.Empty));
    }

    [Fact]
    public void CheckBoxListFor()
    {
        string expected =
@"<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_0"" name=""DaysOfWeek"" type=""checkbox"" value=""0"" /><span>Sunday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_1"" name=""DaysOfWeek"" type=""checkbox"" value=""1"" /><span>Monday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_2"" name=""DaysOfWeek"" type=""checkbox"" value=""2"" /><span>Tuesday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_3"" name=""DaysOfWeek"" type=""checkbox"" value=""3"" /><span>Wednesday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_4"" name=""DaysOfWeek"" type=""checkbox"" value=""4"" /><span>Thursday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_5"" name=""DaysOfWeek"" type=""checkbox"" value=""5"" /><span>Friday</span></label></div>
<div class=""form-check""><label class=""form-check-label"" for=""DaysOfWeek""><input class=""form-check-input"" id=""DaysOfWeek_6"" name=""DaysOfWeek"" type=""checkbox"" value=""6"" /><span>Saturday</span></label></div>";

        var actual = html.CheckBoxListFor(
            m => m.DaysOfWeek,
            html.GetEnumSelectList<DayOfWeek>(),
            labelHtmlAttributes: new { @class = "form-check-label" },
            checkboxHtmlAttributes: new { @class = "form-check-input" },
            inputInsideLabel: true,
            wrapInDiv: true,
            wrapperHtmlAttributes: new { @class = "form-check" });

        actual.ToString().Should().Be(expected.Replace(Environment.NewLine, string.Empty));
    }

    [Fact]
    public void EnumMultiDropDownListFor()
    {
        string expected =
@"<select class=""form-control"" id=""DaysOfWeek"" multiple=""multiple"" name=""DaysOfWeek""><option value=""0"">Sunday</option>
<option value=""1"">Monday</option>
<option value=""2"">Tuesday</option>
<option value=""3"">Wednesday</option>
<option value=""4"">Thursday</option>
<option value=""5"">Friday</option>
<option value=""6"">Saturday</option>
</select>";

        var actual = html.EnumMultiDropDownListFor(m => m.DaysOfWeek, htmlAttributes: new { @class = "form-control" });

        actual.GetString().Should().Be(expected);
    }

    [Fact]
    public void Table()
    {
        var people = new List<Person>
        {
            new() { FirstName = "John", LastName = "Doe" },
            new() { FirstName = "George", LastName = "Smith" },
            new() { FirstName = "Michael", LastName = "Berry" }
        };

        string expected =
@"<table class=""table table-striped"">
<thead>
<tr><th>First Name</th><th>Last Name</th></tr>
</thead>
<tbody>
<tr><td>John</td><td>Doe</td></tr>
<tr><td>George</td><td>Smith</td></tr>
<tr><td>Michael</td><td>Berry</td></tr>
</tbody>
</table>";

        var actual = html.Table(people, new { @class = "table table-striped" });

        actual.ToString().Should().Be(expected.Replace(Environment.NewLine, string.Empty));
    }

    private class TestObject
    {
        public int Quantity { get; set; }

        public IEnumerable<DayOfWeek> DaysOfWeek { get; set; }
    }

    private class Person
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}