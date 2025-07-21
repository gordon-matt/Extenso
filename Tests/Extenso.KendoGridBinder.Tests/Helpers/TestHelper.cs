using System.Globalization;
using Extenso.KendoGridBinder.ModelBinder.Mvc;
using Extenso.KendoGridBinder.Tests.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace Extenso.KendoGridBinder.Tests.Helpers;

public class TestHelper
{
    #region InitEmployees

    protected static IEnumerable<Employee> InitEmployeesWithData()
    {
        var countryNL = new Country { Id = 1, Code = "NL", Name = "The Netherlands" };
        var countryBE = new Country { Id = 2, Code = "BE", Name = "Belgium" };

        var mainCompany1 = new MainCompany { Id = 1, Name = "m-1" };
        var mainCompany2 = new MainCompany { Id = 2, Name = "m-2" };

        var companyA = new Company { Id = 1, Name = "A", MainCompany = mainCompany1 };
        var companyB = new Company { Id = 2, Name = "B", MainCompany = mainCompany1 };
        var companyC = new Company { Id = 3, Name = "C", MainCompany = mainCompany2 };

        return InitEmployees(countryNL, countryBE, companyA, companyB, companyC);
    }

    protected static IEnumerable<Employee> InitEmployees(Country countryNL = null, Country countryBE = null, Company companyA = null, Company companyB = null, Company companyC = null) => new List<Employee>
        {
            new() { Id = 1, Country = countryNL, Company = companyA, FirstName = "Bill", LastName = "Smith", Email = "bsmith@email.com", EmployeeNumber = 1001, HireDate = Convert.ToDateTime("01/12/1990")},
            new() { Id = 2, Country = countryNL, Company = companyB, FirstName = "Jack", LastName = "Smith", Email = "jsmith@email.com", EmployeeNumber = 1002, HireDate = Convert.ToDateTime("12/12/1997")},
            new() { Id = 3, Country = countryNL, Company = companyC, FirstName = "Mary", LastName = "Gates", Email = "mgates@email.com", EmployeeNumber = 1003, HireDate = Convert.ToDateTime("03/03/2000")},
            new() { Id = 4, Country = countryNL, Company = companyA, FirstName = "John", LastName = "Doe", Email = "jd@email.com", EmployeeNumber = 1004, HireDate = Convert.ToDateTime("11/11/2011")},
            new() { Id = 5, Country = countryBE, Company = companyB, FirstName = "Chris", LastName = "Cross", Email = "cc@email.com", EmployeeNumber = 1005, HireDate = Convert.ToDateTime("05/05/1995")},
            new() { Id = 6, Country = countryBE, Company = companyC, FirstName = "Niki", LastName = "Myers", Email = "nm@email.com", EmployeeNumber = 1006, HireDate = Convert.ToDateTime("06/05/1995")},
            new() { Id = 7, Country = countryBE, Company = companyA, FirstName = "Joseph", LastName = "Hall", Email = "jh@email.com", EmployeeNumber = 1007, HireDate = Convert.ToDateTime("07/05/1995")},
            new() { Id = 8, Country = countryBE, Company = companyB, FirstName = "Daniel", LastName = "Wells", Email = "cc@email.com", EmployeeNumber = 1008, HireDate = Convert.ToDateTime("08/05/1995")},
            new() { Id = 9, Country = countryNL, Company = companyC, FirstName = "Robert", LastName = "Lawrence", Email = "cc@email.com", EmployeeNumber = 1009, HireDate = Convert.ToDateTime("09/05/1995")},
            new() { Id = 10, Country = countryNL, Company = companyA, FirstName = "Reginald", LastName = "Quinn", Email = "cc@email.com", EmployeeNumber = 1010, HireDate = Convert.ToDateTime("10/05/1995")},
            new() { Id = 11, Country = countryNL, Company = companyB, FirstName = "Quinn", LastName = "Quick", Email = "cc@email.com", EmployeeNumber = 1011, HireDate = Convert.ToDateTime("11/05/1995")},
            new() { Id = 12, Country = countryNL, Company = companyC, FirstName = "Test", LastName = "User", Email = "tu@email.com", EmployeeNumber = 1012, HireDate = Convert.ToDateTime("11/05/2012")},
        };

    #endregion InitEmployees

    protected static KendoGridBaseRequest SetupBinder(Dictionary<string, StringValues> form, Dictionary<string, StringValues> queryString)
    {
        // Create a mock HTTP context using Microsoft.AspNetCore.Http
        var httpContext = new DefaultHttpContext();

        // Setup the request
        httpContext.Request.Method = "POST";
        httpContext.Request.ContentType = "application/x-www-form-urlencoded";

        // Add form data if provided
        if (form?.Any() == true)
        {
            httpContext.Request.Form = new FormCollection(form);
        }

        // Add query string if provided
        if (queryString?.Any() == true)
        {
            httpContext.Request.Query = new QueryCollection(queryString);
        }

        // Create controller context
        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor()
        );

        // Setup model binding context
        var modelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(KendoGridMvcRequest));
        var valueProvider = new CompositeValueProvider(
        [
            new FormValueProvider(BindingSource.Form, httpContext.Request.Form, CultureInfo.InvariantCulture),
            new QueryStringValueProvider(BindingSource.Query, httpContext.Request.Query, CultureInfo.InvariantCulture)
        ]);

        var modelBindingContext = new DefaultModelBindingContext
        {
            ActionContext = actionContext,
            ModelMetadata = modelMetadata,
            ModelName = string.Empty,
            ValueProvider = valueProvider,
            ModelState = actionContext.ModelState,
            BindingSource = BindingSource.Custom
        };

        // Create and use the model binder
        var binder = new KendoGridMvcModelBinder();
        binder.BindModelAsync(modelBindingContext).GetAwaiter().GetResult();

        // Assertions using NUnit (or xUnit/MSTest)
        Assert.That(modelBindingContext.Result.IsModelSet, Is.True, "Model binding should succeed");
        Assert.That(modelBindingContext.Result.Model, Is.Not.Null, "Bound model should not be null");

        var gridRequest = modelBindingContext.Result.Model as KendoGridMvcRequest;
        Assert.That(gridRequest, Is.Not.Null, "Model should be of type KendoGridMvcRequest");

        return gridRequest;
    }
}