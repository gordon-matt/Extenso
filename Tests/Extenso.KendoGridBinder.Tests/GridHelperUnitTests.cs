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

        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(null, result.AggregateObjects);
        ClassicAssert.AreEqual(null, result.FilterObjectWrapper);
        ClassicAssert.AreEqual(null, result.GroupObjects);
        ClassicAssert.AreEqual(null, result.Logic);
        ClassicAssert.AreEqual(1, result.Page);
        ClassicAssert.AreEqual(11, result.PageSize);
        ClassicAssert.AreEqual(3, result.Skip);
        ClassicAssert.AreEqual(null, result.SortObjects);
        ClassicAssert.AreEqual(10, result.Take);
    }

    [Test]
    public void GridHelper_ParseGroup()
    {
        const string jsonString = "{\"group\":[]}";
        var result = GridHelper.Parse(jsonString);

        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(null, result.AggregateObjects);
        ClassicAssert.AreEqual(null, result.FilterObjectWrapper);
        ClassicAssert.AreEqual(null, result.GroupObjects);
        ClassicAssert.AreEqual(null, result.Logic);
        ClassicAssert.AreEqual(null, result.Page);
        ClassicAssert.AreEqual(null, result.PageSize);
        ClassicAssert.AreEqual(null, result.Skip);
        ClassicAssert.AreEqual(null, result.SortObjects);
        ClassicAssert.AreEqual(null, result.Take);
    }

    [Test]
    public void GridHelper_ParseAggregates()
    {
        const string jsonString = "{\"aggregate\":[]}";
        var result = GridHelper.Parse(jsonString);

        ClassicAssert.IsNotNull(result);
        ClassicAssert.AreEqual(null, result.AggregateObjects);
        ClassicAssert.AreEqual(null, result.FilterObjectWrapper);
        ClassicAssert.AreEqual(null, result.GroupObjects);
        ClassicAssert.AreEqual(null, result.Logic);
        ClassicAssert.AreEqual(null, result.Page);
        ClassicAssert.AreEqual(null, result.PageSize);
        ClassicAssert.AreEqual(null, result.Skip);
        ClassicAssert.AreEqual(null, result.SortObjects);
        ClassicAssert.AreEqual(null, result.Take);
    }
}