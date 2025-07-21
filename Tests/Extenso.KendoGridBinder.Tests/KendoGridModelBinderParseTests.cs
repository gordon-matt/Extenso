using Extenso.KendoGridBinder.Tests.Helpers;
using Microsoft.Extensions.Primitives;
using NUnit.Framework.Legacy;

namespace Extenso.KendoGridBinder.Tests;

[TestFixture]
public class KendoGridModelBinderParseTests : TestHelper
{
    [Test]
    public void TestParse_KendoGridModelBinder_Page()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"}
        };

        var gridRequest = SetupBinder(form, null);
        CheckTake(gridRequest, 5);
        CheckSkip(gridRequest, 0);
        CheckPage(gridRequest, 1);
        CheckPageSize(gridRequest, 5);
    }

    [Test]
    public void TestParse_KendoGridModelBinder_Page_Filter()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"filter[filters][0][field]", "CompanyName"},
            {"filter[filters][0][operator]", "eq"},
            {"filter[filters][0][value]", "A"},
            {"filter[logic]", "and"},
        };

        var gridRequest = SetupBinder(form, null);
        CheckTake(gridRequest, 5);
        CheckSkip(gridRequest, 0);
        CheckPage(gridRequest, 1);
        CheckPageSize(gridRequest, 5);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper);
        ClassicAssert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
        ClassicAssert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
        ClassicAssert.AreEqual(1, gridRequest.FilterObjectWrapper.FilterObjects.Count());

        var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
        var filter1 = filterObjects[0];
        ClassicAssert.AreEqual(false, filter1.IsConjugate);
        ClassicAssert.AreEqual("CompanyName", filter1.Field1);
        ClassicAssert.AreEqual("eq", filter1.Operator1);
        ClassicAssert.AreEqual("A", filter1.Value1);
        ClassicAssert.AreEqual(null, filter1.Logic);
        ClassicAssert.AreEqual(null, filter1.LogicToken);
    }

    [Test]
    public void TestParse_KendoGridModelBinder_Page_Filter_DifferentOrder()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"filter[filters][0][operator]", "eq"}, // Different order
            {"filter[filters][0][field]", "CompanyName"}, // Different order
            {"filter[filters][0][value]", "A"}, // Different order
            {"filter[logic]", "and"},
        };

        var gridRequest = SetupBinder(form, null);
        CheckTake(gridRequest, 5);
        CheckSkip(gridRequest, 0);
        CheckPage(gridRequest, 1);
        CheckPageSize(gridRequest, 5);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper);
        ClassicAssert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
        ClassicAssert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
        ClassicAssert.AreEqual(1, gridRequest.FilterObjectWrapper.FilterObjects.Count());

        var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
        var filter1 = filterObjects[0];
        ClassicAssert.AreEqual(false, filter1.IsConjugate);
        ClassicAssert.AreEqual("CompanyName", filter1.Field1);
        ClassicAssert.AreEqual("eq", filter1.Operator1);
        ClassicAssert.AreEqual("A", filter1.Value1);
        ClassicAssert.AreEqual(null, filter1.Logic);
        ClassicAssert.AreEqual(null, filter1.LogicToken);
    }

    [Test]
    public void TestParse_KendoGridModelBinder_Page_Filter_Sort()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"sort[0][field]", "First"},
            {"sort[0][dir]", "asc"},
            {"sort[1][field]", "Email"},
            {"sort[1][dir]", "desc"},

            {"filter[filters][0][logic]", "or"},
            {"filter[filters][0][filters][0][field]", "CompanyName"},
            {"filter[filters][0][filters][0][operator]", "eq"},
            {"filter[filters][0][filters][0][value]", "A"},
            {"filter[filters][0][filters][1][field]", "CompanyName"},
            {"filter[filters][0][filters][1][operator]", "contains"},
            {"filter[filters][0][filters][1][value]", "B"},

            {"filter[filters][1][field]", "Last"},
            {"filter[filters][1][operator]", "contains"},
            {"filter[filters][1][value]", "s"},
            {"filter[logic]", "and"}
        };

        var gridRequest = SetupBinder(form, null);
        CheckTake(gridRequest, 5);
        CheckSkip(gridRequest, 0);
        CheckPage(gridRequest, 1);
        CheckPageSize(gridRequest, 5);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper);
        ClassicAssert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
        ClassicAssert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
        ClassicAssert.AreEqual(2, gridRequest.FilterObjectWrapper.FilterObjects.Count());

        var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
        var filter1 = filterObjects[0];
        ClassicAssert.AreEqual(true, filter1.IsConjugate);
        ClassicAssert.AreEqual("CompanyName", filter1.Field1);
        ClassicAssert.AreEqual("eq", filter1.Operator1);
        ClassicAssert.AreEqual("A", filter1.Value1);
        ClassicAssert.AreEqual("CompanyName", filter1.Field2);
        ClassicAssert.AreEqual("contains", filter1.Operator2);
        ClassicAssert.AreEqual("B", filter1.Value2);
        ClassicAssert.AreEqual("or", filter1.Logic);
        ClassicAssert.AreEqual("||", filter1.LogicToken);

        var filter2 = filterObjects[1];
        ClassicAssert.AreEqual(false, filter2.IsConjugate);
        ClassicAssert.AreEqual("Last", filter2.Field1);
        ClassicAssert.AreEqual("contains", filter2.Operator1);
        ClassicAssert.AreEqual("s", filter2.Value1);

        ClassicAssert.IsNotNull(gridRequest.SortObjects);
        ClassicAssert.AreEqual(2, gridRequest.SortObjects.Count());
        var sortObjects = gridRequest.SortObjects.ToList();

        var sort1 = sortObjects[0];
        ClassicAssert.AreEqual("First", sort1.Field);
        ClassicAssert.AreEqual("asc", sort1.Direction);

        var sort2 = sortObjects[1];
        ClassicAssert.AreEqual("Email", sort2.Field);
        ClassicAssert.AreEqual("desc", sort2.Direction);
    }

    //{"take":5,"skip":0,"page":1,"pageSize":5,"filter":{"logic":"and","filters":[{"field":"CompanyName","operator":"eq","value":"A"}]},"group":[]}
    [Test]
    public void TestParseJson_KendoGridModelBinder_Page_Filter()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"group", "[]"},
            {"filter", "{\"logic\":\"and\",\"filters\":[{\"field\":\"CompanyName\",\"operator\":\"eq\",\"value\":\"A\"}]}"}
        };

        var gridRequest = SetupBinder(form, null);
        CheckTake(gridRequest, 5);
        CheckSkip(gridRequest, 0);
        CheckPage(gridRequest, 1);
        CheckPageSize(gridRequest, 5);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper);
        ClassicAssert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
        ClassicAssert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
        ClassicAssert.AreEqual(1, gridRequest.FilterObjectWrapper.FilterObjects.Count());

        var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
        var filter1 = filterObjects[0];
        ClassicAssert.AreEqual(false, filter1.IsConjugate);
        ClassicAssert.AreEqual("CompanyName", filter1.Field1);
        ClassicAssert.AreEqual("eq", filter1.Operator1);
        ClassicAssert.AreEqual("A", filter1.Value1);
        ClassicAssert.AreEqual(null, filter1.Logic);
        ClassicAssert.AreEqual(null, filter1.LogicToken);
    }

    //{"take":5,"skip":0,"page":1,"pageSize":5,"sort":[{"field":"FirstName","dir":"asc","compare":null}],"filter":{"logic":"and","filters":[{"field":"CompanyName","operator":"eq","value":"A"}]},"group":[]}
    [Test]
    public void TestParseJson_KendoGridModelBinder_Page_Filter_Sort()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"group", "[]"},
            {"filter", "{\"logic\":\"and\",\"filters\":[{\"logic\":\"or\",\"filters\":[{\"field\":\"LastName\",\"operator\":\"contains\",\"value\":\"s\"},{\"field\":\"LastName\",\"operator\":\"endswith\",\"value\":\"ll\"}]},{\"field\":\"FirstName\",\"operator\":\"startswith\",\"value\":\"n\"}]}"},
            {"sort", "[{\"field\":\"FirstName\",\"dir\":\"asc\",\"compare\":null},{\"field\":\"LastName\",\"dir\":\"desc\",\"compare\":null}]"}
        };

        var gridRequest = SetupBinder(form, null);
        CheckTake(gridRequest, 5);
        CheckSkip(gridRequest, 0);
        CheckPage(gridRequest, 1);
        CheckPageSize(gridRequest, 5);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper);
        ClassicAssert.AreEqual("and", gridRequest.FilterObjectWrapper.Logic);
        ClassicAssert.AreEqual("&&", gridRequest.FilterObjectWrapper.LogicToken);

        ClassicAssert.IsNotNull(gridRequest.FilterObjectWrapper.FilterObjects);
        ClassicAssert.AreEqual(2, gridRequest.FilterObjectWrapper.FilterObjects.Count());

        var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
        var filter1 = filterObjects[0];
        ClassicAssert.AreEqual(true, filter1.IsConjugate);
        ClassicAssert.AreEqual("LastName", filter1.Field1);
        ClassicAssert.AreEqual("contains", filter1.Operator1);
        ClassicAssert.AreEqual("s", filter1.Value1);
        ClassicAssert.AreEqual("LastName", filter1.Field2);
        ClassicAssert.AreEqual("endswith", filter1.Operator2);
        ClassicAssert.AreEqual("ll", filter1.Value2);

        var filter2 = filterObjects[1];
        ClassicAssert.AreEqual(false, filter2.IsConjugate);
        ClassicAssert.AreEqual("FirstName", filter2.Field1);
        ClassicAssert.AreEqual("startswith", filter2.Operator1);
        ClassicAssert.AreEqual("n", filter2.Value1);
        ClassicAssert.AreEqual(null, filter2.Logic);
        ClassicAssert.AreEqual(null, filter2.LogicToken);

        var sortObjects = gridRequest.SortObjects;
        ClassicAssert.IsNotNull(sortObjects);

        var sortList = sortObjects.ToList();
        ClassicAssert.AreEqual("FirstName", sortList.First().Field);
        ClassicAssert.AreEqual("asc", sortList.First().Direction);
        ClassicAssert.AreEqual("LastName", sortList.Last().Field);
        ClassicAssert.AreEqual("desc", sortList.Last().Direction);
    }

    [Test]
    public void TestParse_KendoGridModelBinder_Page_Agggregates()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"aggregate[0][field]", "id"},
            {"aggregate[0][aggregate]", "count"},
            {"aggregate[1][field]", "id"},
            {"aggregate[1][aggregate]", "sum"}
        };

        var gridRequest = SetupBinder(form, null);
        CheckTake(gridRequest, 5);
        CheckSkip(gridRequest, 0);
        CheckPage(gridRequest, 1);
        CheckPageSize(gridRequest, 5);

        ClassicAssert.IsNull(gridRequest.FilterObjectWrapper);
        ClassicAssert.IsNull(gridRequest.SortObjects);
        ClassicAssert.IsNull(gridRequest.GroupObjects);

        var aggregateObjects = gridRequest.AggregateObjects.ToList();
        ClassicAssert.AreEqual(2, aggregateObjects.Count);

        var aggregate0 = aggregateObjects[0];
        ClassicAssert.AreEqual("id", aggregate0.Field);
        ClassicAssert.AreEqual("count", aggregate0.Aggregate);

        var aggregate1 = aggregateObjects[1];
        ClassicAssert.AreEqual("id", aggregate1.Field);
        ClassicAssert.AreEqual("sum", aggregate1.Aggregate);
    }

    #region Check helper methods

    private static void CheckTake(KendoGridBaseRequest gridRequest, int take)
    {
        ClassicAssert.IsNotNull(gridRequest.Take);
        ClassicAssert.AreEqual(take, gridRequest.Take.Value);
    }

    private static void CheckSkip(KendoGridBaseRequest gridRequest, int skip)
    {
        ClassicAssert.IsNotNull(gridRequest.Skip);
        ClassicAssert.AreEqual(skip, gridRequest.Skip.Value);
    }

    private static void CheckPage(KendoGridBaseRequest gridRequest, int page)
    {
        ClassicAssert.IsNotNull(gridRequest.Page);
        ClassicAssert.AreEqual(page, gridRequest.Page.Value);
    }

    private static void CheckPageSize(KendoGridBaseRequest gridRequest, int pagesize)
    {
        ClassicAssert.IsNotNull(gridRequest.PageSize);
        ClassicAssert.AreEqual(pagesize, gridRequest.PageSize.Value);
    }

    #endregion Check helper methods
}