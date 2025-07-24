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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.FilterObjectWrapper, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.Logic, Is.EqualTo("and"));
            Assert.That(gridRequest.FilterObjectWrapper.LogicToken, Is.EqualTo("&&"));
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects.Count(), Is.EqualTo(1));

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];

            Assert.That(filter1.IsConjugate, Is.False);
            Assert.That(filter1.Field1, Is.EqualTo("CompanyName"));
            Assert.That(filter1.Operator1, Is.EqualTo("eq"));
            Assert.That(filter1.Value1, Is.EqualTo("A"));
            Assert.That(filter1.Logic, Is.Null);
            Assert.That(filter1.LogicToken, Is.Null);
        }
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.FilterObjectWrapper, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.Logic, Is.EqualTo("and"));
            Assert.That(gridRequest.FilterObjectWrapper.LogicToken, Is.EqualTo("&&"));
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects.Count(), Is.EqualTo(1));

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];

            Assert.That(filter1.IsConjugate, Is.False);
            Assert.That(filter1.Field1, Is.EqualTo("CompanyName"));
            Assert.That(filter1.Operator1, Is.EqualTo("eq"));
            Assert.That(filter1.Value1, Is.EqualTo("A"));
            Assert.That(filter1.Logic, Is.Null);
            Assert.That(filter1.LogicToken, Is.Null);
        }
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.FilterObjectWrapper, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.Logic, Is.EqualTo("and"));
            Assert.That(gridRequest.FilterObjectWrapper.LogicToken, Is.EqualTo("&&"));
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects.Count(), Is.EqualTo(2));

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];
            Assert.That(filter1.IsConjugate, Is.True);
            Assert.That(filter1.Field1, Is.EqualTo("CompanyName"));
            Assert.That(filter1.Operator1, Is.EqualTo("eq"));
            Assert.That(filter1.Value1, Is.EqualTo("A"));
            Assert.That(filter1.Field2, Is.EqualTo("CompanyName"));
            Assert.That(filter1.Operator2, Is.EqualTo("contains"));
            Assert.That(filter1.Value2, Is.EqualTo("B"));
            Assert.That(filter1.Logic, Is.EqualTo("or"));
            Assert.That(filter1.LogicToken, Is.EqualTo("||"));

            var filter2 = filterObjects[1];
            Assert.That(filter2.IsConjugate, Is.False);
            Assert.That(filter2.Field1, Is.EqualTo("Last"));
            Assert.That(filter2.Operator1, Is.EqualTo("contains"));
            Assert.That(filter2.Value1, Is.EqualTo("s"));

            Assert.That(gridRequest.SortObjects, Is.Not.Null);
            Assert.That(gridRequest.SortObjects.Count(), Is.EqualTo(2));
            var sortObjects = gridRequest.SortObjects.ToList();

            var sort1 = sortObjects[0];
            Assert.That(sort1.Field, Is.EqualTo("First"));
            Assert.That(sort1.Direction, Is.EqualTo("asc"));

            var sort2 = sortObjects[1];
            Assert.That(sort2.Field, Is.EqualTo("Email"));
            Assert.That(sort2.Direction, Is.EqualTo("desc"));
        }
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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.FilterObjectWrapper, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.Logic, Is.EqualTo("and"));
            Assert.That(gridRequest.FilterObjectWrapper.LogicToken, Is.EqualTo("&&"));
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects.Count(), Is.EqualTo(1));

            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();
            var filter1 = filterObjects[0];

            Assert.That(filter1.IsConjugate, Is.False);
            Assert.That(filter1.Field1, Is.EqualTo("CompanyName"));
            Assert.That(filter1.Operator1, Is.EqualTo("eq"));
            Assert.That(filter1.Value1, Is.EqualTo("A"));
            Assert.That(filter1.Logic, Is.Null);
            Assert.That(filter1.LogicToken, Is.Null);
        }
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

        using (Assert.EnterMultipleScope())
        {
            // FilterObjectWrapper assertions
            Assert.That(gridRequest.FilterObjectWrapper, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.Logic, Is.EqualTo("and"));
            Assert.That(gridRequest.FilterObjectWrapper.LogicToken, Is.EqualTo("&&"));

            // FilterObjects assertions
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects, Is.Not.Null);
            Assert.That(gridRequest.FilterObjectWrapper.FilterObjects.Count(), Is.EqualTo(2));
            var filterObjects = gridRequest.FilterObjectWrapper.FilterObjects.ToList();

            // First filter assertions
            var filter1 = filterObjects[0];
            Assert.That(filter1.IsConjugate, Is.True);
            Assert.That(filter1.Field1, Is.EqualTo("LastName"));
            Assert.That(filter1.Operator1, Is.EqualTo("contains"));
            Assert.That(filter1.Value1, Is.EqualTo("s"));
            Assert.That(filter1.Field2, Is.EqualTo("LastName"));
            Assert.That(filter1.Operator2, Is.EqualTo("endswith"));
            Assert.That(filter1.Value2, Is.EqualTo("ll"));

            // Second filter assertions
            var filter2 = filterObjects[1];
            Assert.That(filter2.IsConjugate, Is.False);
            Assert.That(filter2.Field1, Is.EqualTo("FirstName"));
            Assert.That(filter2.Operator1, Is.EqualTo("startswith"));
            Assert.That(filter2.Value1, Is.EqualTo("n"));
            Assert.That(filter2.Logic, Is.Null);
            Assert.That(filter2.LogicToken, Is.Null);

            // SortObjects assertions
            var sortObjects = gridRequest.SortObjects;
            Assert.That(sortObjects, Is.Not.Null);

            var sortList = sortObjects.ToList();
            Assert.That(sortList.First().Field, Is.EqualTo("FirstName"));
            Assert.That(sortList.First().Direction, Is.EqualTo("asc"));
            Assert.That(sortList.Last().Field, Is.EqualTo("LastName"));
            Assert.That(sortList.Last().Direction, Is.EqualTo("desc"));
        }
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

        using (Assert.EnterMultipleScope())
        {
            // Null checks
            Assert.That(gridRequest.FilterObjectWrapper, Is.Null);
            Assert.That(gridRequest.SortObjects, Is.Null);
            Assert.That(gridRequest.GroupObjects, Is.Null);

            // Aggregate objects assertions
            var aggregateObjects = gridRequest.AggregateObjects.ToList();
            Assert.That(aggregateObjects, Has.Count.EqualTo(2));

            // First aggregate object
            var aggregate0 = aggregateObjects[0];

            Assert.That(aggregate0.Field, Is.EqualTo("id"));
            Assert.That(aggregate0.Aggregate, Is.EqualTo("count"));

            // Second aggregate object
            var aggregate1 = aggregateObjects[1];

            Assert.That(aggregate1.Field, Is.EqualTo("id"));
            Assert.That(aggregate1.Aggregate, Is.EqualTo("sum"));
        }
    }

    #region Check helper methods

    private static void CheckTake(KendoGridBaseRequest gridRequest, int take)
    {
        Assert.That(gridRequest.Take, Is.Not.Null);
        Assert.That(gridRequest.Take.Value, Is.EqualTo(take));
    }

    private static void CheckSkip(KendoGridBaseRequest gridRequest, int skip)
    {
        Assert.That(gridRequest.Skip, Is.Not.Null);
        Assert.That(gridRequest.Skip.Value, Is.EqualTo(skip));
    }

    private static void CheckPage(KendoGridBaseRequest gridRequest, int page)
    {
        Assert.That(gridRequest.Page, Is.Not.Null);
        Assert.That(gridRequest.Page.Value, Is.EqualTo(page));
    }

    private static void CheckPageSize(KendoGridBaseRequest gridRequest, int pagesize)
    {
        Assert.That(gridRequest.PageSize, Is.Not.Null);
        Assert.That(gridRequest.PageSize.Value, Is.EqualTo(pagesize));
    }

    #endregion Check helper methods
}