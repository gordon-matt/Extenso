using Extenso.KendoGridBinder.ModelBinder;
using NUnit.Framework.Legacy;

namespace Extenso.KendoGridBinder.Tests;

[TestFixture]
public class GridHelperUnitTests
{
    [Test]
    public void GridHelper_ParseTest()
    {
        const string jsonString = "{\"take\":10,\"skip\":3,\"page\":1,\"pageSize\":11,\"group\":[],\"aggregate\":[]}";
        var result = GridHelper.Parse(jsonString);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AggregateObjects, Is.Null);
            Assert.That(result.FilterObjectWrapper, Is.Null);
            Assert.That(result.GroupObjects, Is.Null);
            Assert.That(result.Logic, Is.Null);
            Assert.That(result.Page, Is.EqualTo(1));
            Assert.That(result.PageSize, Is.EqualTo(11));
            Assert.That(result.Skip, Is.EqualTo(3));
            Assert.That(result.SortObjects, Is.Null);
            Assert.That(result.Take, Is.EqualTo(10));
        }
    }

    [Test]
    public void GridHelper_ParseGroup()
    {
        const string jsonString = "{\"group\":[]}";
        var result = GridHelper.Parse(jsonString);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AggregateObjects, Is.Null);
            Assert.That(result.FilterObjectWrapper, Is.Null);
            Assert.That(result.GroupObjects, Is.Null);
            Assert.That(result.Logic, Is.Null);
            Assert.That(result.Page, Is.Null);
            Assert.That(result.PageSize, Is.Null);
            Assert.That(result.Skip, Is.Null);
            Assert.That(result.SortObjects, Is.Null);
            Assert.That(result.Take, Is.Null);
        }
    }

    [Test]
    public void GridHelper_ParseAggregates()
    {
        const string jsonString = "{\"aggregate\":[]}";
        var result = GridHelper.Parse(jsonString);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.AggregateObjects, Is.Null);
            Assert.That(result.FilterObjectWrapper, Is.Null);
            Assert.That(result.GroupObjects, Is.Null);
            Assert.That(result.Logic, Is.Null);
            Assert.That(result.Page, Is.Null);
            Assert.That(result.PageSize, Is.Null);
            Assert.That(result.Skip, Is.Null);
            Assert.That(result.SortObjects, Is.Null);
            Assert.That(result.Take, Is.Null);
        }
    }
}