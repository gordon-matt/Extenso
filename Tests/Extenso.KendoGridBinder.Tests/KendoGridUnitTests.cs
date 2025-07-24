using AutoMapper;
using Extenso.KendoGridBinder.AutoMapperExtensions;
using Extenso.KendoGridBinder.Tests.Entities;
using Extenso.KendoGridBinder.Tests.Extensions;
using Extenso.KendoGridBinder.Tests.Helpers;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Extenso.KendoGridBinder.Tests;

[TestFixture]
public class KendoGridUnitTests : TestHelper
{
    private MapperConfiguration _mapperConfiguration;
    private KendoGridQueryableHelper _instanceUnderTest;
    private IMapper _mapper;

    [Test]
    public void AutomapperUtilsTest()
    {
        var employees = InitEmployeesWithData().AsQueryable();
        var mappings = new Dictionary<string, MapExpression<Employee>>
        {
            { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
            { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } }
        };

        var companyIds = employees.Select(mappings.First().Value.Expression).ToList();
        Assert.That(companyIds, Has.Count.EqualTo(12));
        Assert.That(companyIds.First(), Is.EqualTo(1));
    }

    [Test]
    public void Test_KendoGrid_WithNullConversoin_And_OneGroupByOneAggregateCount_Should_ApplyGrouping()
    {
        // Assign
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"sort[0][field]", "Full"},
            {"sort[0][dir]", "asc"},

            {"group[0][field]", "First"},
            {"group[0][dir]", "asc"},
            {"group[0][aggregates][0][field]", "First"},
            {"group[0][aggregates][0][aggregate]", "count"}
        };

        var request = SetupBinder(form, null);
        var mappings = new Dictionary<string, MapExpression<Employee>>
        {
            { "First", new MapExpression<Employee> { Path = "FirstName", Expression = m => m.FirstName } },
            { "Full", new MapExpression<Employee> { Path = "FullName", Expression = m => m.FullName } }
        };
        var employees = InitEmployeesWithData().AsQueryable();

        // Act
        var kendoGrid = new KendoGrid<Employee, EmployeeVM>(request, employees, mappings);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(kendoGrid.Data, Is.Null);
            Assert.That(kendoGrid.Groups, Is.Not.Null);

            string json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.That(groups, Is.Not.Null);

            Assert.That(groups.Count(), Is.EqualTo(5));
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_Grid_Page()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"}
        };

        var gridRequest = SetupBinder(form, null);

        InitAutoMapper();
        var employees = InitEmployeesWithData().AsQueryable();

        _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
        var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(kendoGrid, Is.Not.Null);
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));
            Assert.That(kendoGrid.Data, Is.Not.Null);
            Assert.That(kendoGrid.Data.Count(), Is.EqualTo(5));
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_Grid_Sort_Filter_EntitiesWithNullValues()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"sort[0][field]", "Id"},
            {"sort[0][dir]", "asc"},

            {"filter[filters][0][field]", "LastName"},
            {"filter[filters][0][operator]", "contains"},
            {"filter[filters][0][value]", "s"},
            {"filter[filters][1][field]", "Email"},
            {"filter[filters][1][operator]", "contains"},
            {"filter[filters][1][value]", "r"},
            {"filter[logic]", "or"}
        };

        var gridRequest = SetupBinder(form, null);

        var employeeList = InitEmployeesWithData().ToList();
        foreach (var employee in employeeList.Where(e => e.LastName.Contains("e")))
        {
            employee.LastName = null;
            employee.FirstName = null;
        }

        var employees = employeeList.AsQueryable();

        _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
        var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(kendoGrid, Is.Not.Null);
            Assert.That(kendoGrid.Total, Is.EqualTo(3));
            Assert.That(kendoGrid.Data, Is.Not.Null);
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_Grid_Page_Filter_Sort()
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

        InitAutoMapper();
        var employees = InitEmployeesWithData().AsQueryable();
        var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } }
            };

        _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
        var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(kendoGrid, Is.Not.Null);
            Assert.That(kendoGrid.Total, Is.EqualTo(4));
            Assert.That(kendoGrid.Data, Is.Not.Null);

            Assert.That(kendoGrid.Data.Count(), Is.EqualTo(4));
            Assert.That(kendoGrid.Data.First().Full, Is.EqualTo("Bill Smith"));
            Assert.That(kendoGrid.Data.Last().Full, Is.EqualTo("Jack Smith"));

            var query = kendoGrid.AsQueryable();

            Assert.That(query.First().FullName, Is.EqualTo("Bill Smith"));
            Assert.That(query.Last().FullName, Is.EqualTo("Jack Smith"));
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_One_GroupBy_WithIncludes()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"group[0][field]", "CountryName"},
            {"group[0][dir]", "asc"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects.Count(), Is.EqualTo(1));
            Assert.That(gridRequest.GroupObjects.First().AggregateObjects.Count(), Is.EqualTo(0));
        }

        InitAutoMapper();
        var employees = InitEmployeesWithData().AsQueryable();
        var mappings = new Dictionary<string, MapExpression<Employee>>
        {
            { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
            { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } },
            { "CountryName", new MapExpression<Employee> { Path = "Country.Name", Expression = m => m.Country.Name } }
        };

        _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
        var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, ["Company", "Company.MainCompany", "Country"], mappings);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(kendoGrid.Data, Is.Null);
            Assert.That(kendoGrid.Groups, Is.Not.Null);

            string json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.That(groups, Is.Not.Null);

            Assert.That(groups.Count(), Is.EqualTo(2));
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));

            var employeesFromFirstGroup = groups.First().items as IEnumerable<EmployeeVM>;
            Assert.That(employeesFromFirstGroup, Is.Not.Null);

            var employeesFromFirstGroupList = employeesFromFirstGroup.ToList();
            Assert.That(employeesFromFirstGroupList.Count, Is.EqualTo(4));

            var testEmployee = employeesFromFirstGroupList.First();
            Assert.That(testEmployee.CountryName, Is.EqualTo("Belgium"));
            Assert.That(testEmployee.CompanyName, Is.EqualTo("B"));
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_Aggregates_WithIncludes()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"aggregate[0][field]", "Id"},
            {"aggregate[0][aggregate]", "sum"},
            {"aggregate[1][field]", "Id"},
            {"aggregate[1][aggregate]", "min"},
            {"aggregate[2][field]", "Id"},
            {"aggregate[2][aggregate]", "max"},
            {"aggregate[3][field]", "Id"},
            {"aggregate[3][aggregate]", "count"},
            {"aggregate[4][field]", "Id"},
            {"aggregate[4][aggregate]", "average"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects, Is.Null);
            Assert.That(gridRequest.AggregateObjects.Count(), Is.EqualTo(5));
        }

        InitAutoMapper();
        var employees = InitEmployeesWithData().AsQueryable();
        var mappings = new Dictionary<string, MapExpression<Employee>>
        {
            { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
            { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } }
        };

        _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
        var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, new[] { "Company", "Company.MainCompany", "Country" }, mappings);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(kendoGrid.Groups, Is.Null);
            Assert.That(kendoGrid.Data, Is.Not.Null);

            Assert.That(kendoGrid.Data.Count(), Is.EqualTo(5));

            Assert.That(kendoGrid.Aggregates, Is.Not.Null);
            string json = JsonConvert.SerializeObject(kendoGrid.Aggregates, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var aggregatesAsDictionary = kendoGrid.Aggregates as Dictionary<string, Dictionary<string, object>>;
            Assert.That(aggregatesAsDictionary, Is.Not.Null);
            Assert.That(aggregatesAsDictionary.Keys.Count, Is.EqualTo(1));
            Assert.That(aggregatesAsDictionary.Keys.First(), Is.EqualTo("Id"));

            var aggregatesForId = aggregatesAsDictionary["Id"];
            Assert.That(aggregatesForId.Keys.Count, Is.EqualTo(5));
            Assert.That(aggregatesForId["sum"], Is.EqualTo(78));
            Assert.That(aggregatesForId["min"], Is.EqualTo(1));
            Assert.That(aggregatesForId["max"], Is.EqualTo(12));
            Assert.That(aggregatesForId["count"], Is.EqualTo(12));
            Assert.That(aggregatesForId["average"], Is.EqualTo(6.5d));
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_Aggregates_WithIncludes_NoResults()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"filter[filters][0][field]", "LastName"},
            {"filter[filters][0][operator]", "equals"},
            {"filter[filters][0][value]", "xxx"},
            {"filter[filters][1][field]", "Email"},
            {"filter[filters][1][operator]", "contains"},
            {"filter[filters][1][value]", "r"},
            {"filter[logic]", "or"},

            {"aggregate[0][field]", "Id"},
            {"aggregate[0][aggregate]", "sum"},
            {"aggregate[1][field]", "Id"},
            {"aggregate[1][aggregate]", "min"},
            {"aggregate[2][field]", "Id"},
            {"aggregate[2][aggregate]", "max"},
            {"aggregate[3][field]", "Id"},
            {"aggregate[3][aggregate]", "count"},
            {"aggregate[4][field]", "Id"},
            {"aggregate[4][aggregate]", "average"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects, Is.Null);
            Assert.That(gridRequest.AggregateObjects.Count(), Is.EqualTo(5));

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } }
            };

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, new[] { "Company", "Company.MainCompany", "Country" }, mappings);

            Assert.That(kendoGrid.Groups, Is.Null);
            Assert.That(kendoGrid.Data, Is.Not.Null);
            Assert.That(kendoGrid.Data.Count(), Is.Zero);

            Assert.That(kendoGrid.Aggregates, Is.Not.Null);
            string json = JsonConvert.SerializeObject(kendoGrid.Aggregates, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var aggregatesAsDictionary = kendoGrid.Aggregates as Dictionary<string, Dictionary<string, object>>;
            Assert.That(aggregatesAsDictionary, Is.Not.Null);
            Assert.That(aggregatesAsDictionary.Keys.Count, Is.Zero);
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_One_GroupBy_WithoutIncludes()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"group[0][field]", "LastName"},
            {"group[0][dir]", "asc"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects.Count(), Is.EqualTo(1));
            Assert.That(gridRequest.GroupObjects.First().AggregateObjects.Count(), Is.EqualTo(0));

            InitAutoMapper();
            var employees = InitEmployees().AsQueryable();
            var employeeVMs = _mapper.Map<List<EmployeeVM>>(employees.ToList());
            Assert.That(employeeVMs, Is.Not.Null);

            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } }
            };

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.That(kendoGrid.Data, Is.Null);
            Assert.That(kendoGrid.Groups, Is.Not.Null);
            string json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.That(groups, Is.Not.Null);

            Assert.That(groups.Count(), Is.EqualTo(5));
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));

            var employeesFromFirstGroup = groups.First().items as IEnumerable<EmployeeVM>;
            Assert.That(employeesFromFirstGroup, Is.Not.Null);

            var employeesFromFirstGroupList = employeesFromFirstGroup.ToList();
            Assert.That(employeesFromFirstGroupList.Count, Is.EqualTo(1));

            var testEmployee = employeesFromFirstGroupList.First();
            Assert.That(testEmployee.CountryName, Is.Null);
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_One_GroupBy_One_Aggregate_Count()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"sort[0][field]", "Full"},
            {"sort[0][dir]", "asc"},

            {"group[0][field]", "First"},
            {"group[0][dir]", "asc"},
            {"group[0][aggregates][0][field]", "First"},
            {"group[0][aggregates][0][aggregate]", "count"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects.Count(), Is.EqualTo(1));
            Assert.That(gridRequest.GroupObjects.First().AggregateObjects.Count(), Is.EqualTo(1));

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);

            Assert.That(kendoGrid.Data, Is.Null);
            Assert.That(kendoGrid.Groups, Is.Not.Null);
            string json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.That(groups, Is.Not.Null);

            Assert.That(groups.Count(), Is.EqualTo(5));
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_One_GroupBy_One_Aggregate_Sum()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "10"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "10"},

            {"group[0][field]", "Last"},
            {"group[0][dir]", "asc"},
            {"group[0][aggregates][0][field]", "Number"},
            {"group[0][aggregates][0][aggregate]", "sum"},
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            // Group objects count assertions
            Assert.That(gridRequest.GroupObjects.Count(), Is.EqualTo(1));
            Assert.That(gridRequest.GroupObjects.First().AggregateObjects.Count(), Is.EqualTo(1));

            // Setup
            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);

            Assert.That(kendoGrid.Data, Is.Null);
            Assert.That(kendoGrid.Groups, Is.Not.Null);

            string json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            // Groups assertions
            var groups = kendoGrid.Groups as List<KendoGroup>;

            Assert.That(groups, Is.Not.Null);
            Assert.That(groups, Has.Count.EqualTo(9));
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));

            // Group by Smith assertions
            var groupBySmith = groups.FirstOrDefault(g => g.value.ToString() == "Smith");
            Assert.That(groupBySmith, Is.Not.Null);

            var items = groupBySmith.items as List<EmployeeVM>;

            Assert.That(items, Is.Not.Null);
            Assert.That(items, Has.Count.EqualTo(2));
            Assert.That(items.Count(e => e.Last == "Smith"), Is.EqualTo(2));

            // Aggregates assertions
            var aggregates = groupBySmith.aggregates as Dictionary<string, Dictionary<string, object>>;
            Assert.That(aggregates, Is.Not.Null);

            Assert.That(aggregates, Contains.Key("Number"));
            var aggregatesNumber = aggregates["Number"];
            Assert.That(aggregatesNumber, Is.Not.Null);
            Assert.That(aggregatesNumber, Has.Count.EqualTo(1));

            var aggregateSum = aggregatesNumber.First();
            Assert.That(aggregateSum.Key, Is.EqualTo("sum"));
            Assert.That(aggregateSum.Value, Is.EqualTo(2003));
        }
    }

    /*
    take=10&skip=0&page=1&pageSize=10&
    group[0][field]=CompanyName&
    group[0][dir]=asc&
    group[0][aggregates][0][field]=Number&
    group[0][aggregates][0][aggregate]=min&
    group[0][aggregates][1][field]=Number&
    group[0][aggregates][1][aggregate]=max&
    group[0][aggregates][2][field]=Number&
    group[0][aggregates][2][aggregate]=average&
    group[0][aggregates][3][field]=Number&
    group[0][aggregates][3][aggregate]=count&

    group[1][field]=LastName&
    group[1][dir]=asc&
    group[1][aggregates][0][field]=Number&
    group[1][aggregates][0][aggregate]=min&
    group[1][aggregates][1][field]=Number&
    group[1][aggregates][1][aggregate]=max&
    group[1][aggregates][2][field]=Number&
    group[1][aggregates][2][aggregate]=average&
    group[1][aggregates][3][field]=Number&
    group[1][aggregates][3][aggregate]=count
     * */

    [Test]
    public void Test_KendoGridModelBinder_Two_GroupBy_One_Aggregate_Min()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "10"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "10"},

            {"group[0][field]", "CompanyName"},
            {"group[0][dir]", "asc"},
            {"group[0][aggregates][0][field]", "Number"},
            {"group[0][aggregates][0][aggregate]", "min"},

            {"group[1][field]", "LastName"},
            {"group[1][dir]", "asc"},
            {"group[1][aggregates][0][field]", "Number"},
            {"group[1][aggregates][0][aggregate]", "min"},
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects.Count(), Is.EqualTo(2));
            Assert.That(gridRequest.GroupObjects.First().AggregateObjects.Count(), Is.EqualTo(1));
            Assert.That(gridRequest.GroupObjects.Last().AggregateObjects.Count(), Is.EqualTo(1));

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } },
                { "CountryName", new MapExpression<Employee> { Path = "Country.Name", Expression = m => m.Country.Name } }
            };

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.That(kendoGrid.Data, Is.Null);
            Assert.That(kendoGrid.Groups, Is.Not.Null);
            string json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.That(groups, Is.Not.Null);

            Assert.That(groups.Count(), Is.EqualTo(10));
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));
        }

        /*
        var groupBySmith = groups.FirstOrDefault(g => g.value.ToString() == "Smith");
        ClassicAssert.IsNotNull(groupBySmith);

        var items = groupBySmith.items as List<EmployeeVM>;
        ClassicAssert.IsNotNull(items);
        ClassicAssert.AreEqual(2, items.Count);
        ClassicAssert.AreEqual(2, items.Count(e => e.Last == "Smith"));

        var aggregates = groupBySmith.aggregates as Dictionary<string, Dictionary<string, object>>;
        ClassicAssert.IsNotNull(aggregates);

        ClassicAssert.IsTrue(aggregates.ContainsKey("Number"));
        var aggregatesNumber = aggregates["Number"];
        ClassicAssert.IsNotNull(aggregatesNumber);
        ClassicAssert.AreEqual(1, aggregatesNumber.Count);

        var aggregateSum = aggregatesNumber.First();
        ClassicAssert.IsNotNull(aggregateSum);
        ClassicAssert.AreEqual("sum", aggregateSum.Key);
        ClassicAssert.AreEqual(2003, aggregateSum.Value);
        */
    }

    //take=10&
    //skip=0&
    //page=1&
    //pageSize=10&
    //group[0][field]=Id&
    //group[0][dir]=asc&
    //group[0][aggregates][0][field]=Id&
    //group[0][aggregates][0][aggregate]=count
    [Test]
    public void Test_KendoGridModelBinder_A()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "10"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "10"},

            {"group[0][field]", "Id"},
            {"group[0][dir]", "asc"},
            {"group[0][aggregates][0][field]", "Id"},
            {"group[0][aggregates][0][aggregate]", "count"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects.Count(), Is.EqualTo(1));
            Assert.That(gridRequest.GroupObjects.First().AggregateObjects.Count(), Is.EqualTo(1));
            Assert.That(gridRequest.GroupObjects.Last().AggregateObjects.Count(), Is.EqualTo(1));

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } },
                { "CompanyName", new MapExpression<Employee> { Path = "Company.Name", Expression = m => m.Company.Name } },
                { "CountryName", new MapExpression<Employee> { Path = "Country.Name", Expression = m => m.Country.Name } }
            };

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.That(kendoGrid.Data, Is.Null);
            Assert.That(kendoGrid.Groups, Is.Not.Null);
            string json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.That(groups, Is.Not.Null);

            Assert.That(groups.Count(), Is.EqualTo(10));
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));
        }
    }

    [Test]
    //{"take":5,"skip":0,"page":1,"pageSize":5,"group":[]}
    public void Test_KendoGridModelBinder_Json_WithoutIncludes()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"group", "[]"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects, Is.Null);

            InitAutoMapper();
            var employees = InitEmployees().AsQueryable();
            var employeeVMs = _mapper.Map<List<EmployeeVM>>(employees.ToList());
            Assert.That(employeeVMs, Is.Not.Null);

            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } }
            };

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.That(kendoGrid, Is.Not.Null);

            Assert.That(kendoGrid.Groups, Is.Null);
            Assert.That(kendoGrid.Data, Is.Not.Null);

            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));

            Assert.That(kendoGrid.Data, Is.Not.Null);
            Assert.That(kendoGrid.Data.Count(), Is.EqualTo(5));
        }
    }

    [Test]
    public void Test_KendoGridModelBinder_Json_Filter()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"group", "[]"},
            //{"filter", "{\"logic\":\"and\",\"filters\":[{\"field\":\"CompanyName\",\"operator\":\"eq\",\"value\":\"A\"}]}"},
            {"filter", "{\"logic\":\"and\",\"filters\":[{\"logic\":\"or\",\"filters\":[{\"field\":\"LastName\",\"operator\":\"contains\",\"value\":\"s\"},{\"field\":\"LastName\",\"operator\":\"endswith\",\"value\":\"ll\"}]},{\"field\":\"FirstName\",\"operator\":\"startswith\",\"value\":\"n\"}]}"},
            {"sort", "[{\"field\":\"FirstName\",\"dir\":\"asc\",\"compare\":null},{\"field\":\"LastName\",\"dir\":\"desc\",\"compare\":null}]"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects, Is.Null);

            InitAutoMapper();
            var employees = InitEmployees().AsQueryable();
            var employeeVMs = _mapper.Map<List<EmployeeVM>>(employees.ToList());
            Assert.That(employeeVMs, Is.Not.Null);

            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } }
            };

            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.That(kendoGrid, Is.Not.Null);
            Assert.That(kendoGrid.Groups, Is.Null);
            Assert.That(kendoGrid.Data, Is.Not.Null);

            Assert.That(kendoGrid.Total, Is.EqualTo(1));
            Assert.That(kendoGrid.Data, Is.Not.Null);
            Assert.That(kendoGrid.Data.Count(), Is.EqualTo(1));
        }
    }

    [Test]
    //{"take":5,"skip":0,"page":1,"pageSize":5,"group":[{"field":"LastName","dir":"asc","aggregates":[]}]}
    public void Test_KendoGridModelBinder_Json_One_GroupBy_WithoutIncludes()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "5"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "5"},

            {"group", "[{\"field\":\"LastName\",\"dir\":\"asc\",\"aggregates\":[]}]"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects.Count(), Is.EqualTo(1));
            Assert.That(gridRequest.GroupObjects.First().AggregateObjects.Count(), Is.EqualTo(0));

            InitAutoMapper();
            var employees = InitEmployees().AsQueryable();
            var employeeVMs = _mapper.Map<List<EmployeeVM>>(employees.ToList());
            Assert.That(employeeVMs, Is.Not.Null);

            var mappings = new Dictionary<string, MapExpression<Employee>>
            {
                { "CompanyId", new MapExpression<Employee> { Path = "Company.Id", Expression = m => m.Company.Id } }
            };
            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest, null, mappings);

            Assert.That(kendoGrid.Data, Is.Null);
            Assert.That(kendoGrid.Groups, Is.Not.Null);
            string json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.That(groups, Is.Not.Null);

            Assert.That(groups.Count(), Is.EqualTo(5));
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));

            var employeesFromFirstGroup = groups.First().items as IEnumerable<EmployeeVM>;
            Assert.That(employeesFromFirstGroup, Is.Not.Null);

            var employeesFromFirstGroupList = employeesFromFirstGroup.ToList();
            Assert.That(employeesFromFirstGroupList.Count, Is.EqualTo(1));

            var testEmployee = employeesFromFirstGroupList.First();
            Assert.That(testEmployee.CountryName, Is.Null);
        }
    }

    [Test]
    //{"take":5,"skip":0,"page":1,"pageSize":5,"group":[{"field":"LastName","dir":"asc","aggregates":["field":"Number","aggregate":"Sum"]}]}
    public void Test_KendoGridModelBinder_Json_One_GroupBy_One_Aggregate_Sum()
    {
        var form = new Dictionary<string, StringValues>
        {
            {"take", "10"},
            {"skip", "0"},
            {"page", "1"},
            {"pagesize", "10"},

            {"group", "[{\"field\":\"LastName\",\"dir\":\"asc\",\"aggregates\":[{\"field\":\"Number\",\"aggregate\":\"sum\"}]}]"}
        };

        var gridRequest = SetupBinder(form, null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(gridRequest.GroupObjects.Count(), Is.EqualTo(1));
            Assert.That(gridRequest.GroupObjects.First().AggregateObjects.Count(), Is.EqualTo(1));

            InitAutoMapper();
            var employees = InitEmployeesWithData().AsQueryable();
            _instanceUnderTest = new KendoGridQueryableHelper(_mapperConfiguration);
            var kendoGrid = _instanceUnderTest.ToKendoGridEx<Employee, EmployeeVM>(employees, gridRequest);

            Assert.That(kendoGrid.Data, Is.Null);
            Assert.That(kendoGrid.Groups, Is.Not.Null);
            string json = JsonConvert.SerializeObject(kendoGrid.Groups, Formatting.Indented);
            Assert.That(json, Is.Not.Null);

            var groups = kendoGrid.Groups as List<KendoGroup>;
            Assert.That(groups, Is.Not.Null);

            Assert.That(groups.Count(), Is.EqualTo(9));
            Assert.That(kendoGrid.Total, Is.EqualTo(employees.Count()));

            var groupBySmith = groups.FirstOrDefault(g => g.value.ToString() == "Smith");
            Assert.That(groupBySmith, Is.Not.Null);

            var items = groupBySmith.items as List<EmployeeVM>;
            Assert.That(items, Is.Not.Null);
            Assert.That(items, Has.Count.EqualTo(2));
            Assert.That(items.Count(e => e.Last == "Smith"), Is.EqualTo(2));

            var aggregates = groupBySmith.aggregates as Dictionary<string, Dictionary<string, object>>;
            Assert.That(aggregates, Is.Not.Null);

            Assert.That(aggregates.ContainsKey("Number"), Is.True);
            var aggregatesNumber = aggregates["Number"];
            Assert.That(aggregatesNumber, Is.Not.Null);
            Assert.That(aggregatesNumber.Count, Is.EqualTo(1));

            var aggregateSum = aggregatesNumber.First();
            Assert.That(aggregateSum.Key, Is.EqualTo("sum"));
            Assert.That(aggregateSum.Value, Is.EqualTo(2003));
        }
    }

    #region InitAutoMapper

    private void InitAutoMapper()
    {
        _mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<EmployeeProfile>());

        _mapperConfiguration.AssertConfigurationIsValid();

        _mapper = _mapperConfiguration.CreateMapper();
    }

    #endregion InitAutoMapper

    public interface IKendoResolver
    {
        string GetM();
    }

    public class IdResolver : IValueResolver<Company, object, long>
    {
        public long Resolve(Company source, object destination, long destMember, ResolutionContext context) => source?.Id ?? 0;
    }

    public class IdResolver2 : IValueResolver<IEntity, object, long>, IKendoResolver
    {
        public string GetM() => "xxx";

        public long Resolve(IEntity source, object destination, long destMember, ResolutionContext context) => source?.Id ?? 0;
    }

    public class CompanyNameResolver : IValueResolver<Company, object, string>
    {
        public string Resolve(Company source, object destination, string destMember, ResolutionContext context) => source != null ? source.Name : string.Empty;
    }

    public class MainCompanyNameResolver : IValueResolver<Company, object, string>
    {
        public string Resolve(Company source, object destination, string destMember, ResolutionContext context) => source.NullSafeGetValue(x => x.MainCompany.Name, null);
    }

    public class CountryCodeResolver : IValueResolver<Country, object, string>
    {
        public string Resolve(Country source, object destination, string destMember, ResolutionContext context) => source?.Code;
    }

    public class CountryNameResolver : IValueResolver<Country, object, string>
    {
        public string Resolve(Country source, object destination, string destMember, ResolutionContext context) => source?.Name;
    }
}