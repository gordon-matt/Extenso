[![NuGet](https://img.shields.io/nuget/v/Extenso.AspNetCore.OData?style=flat-square&logo=nuget&label=Version)](https://www.nuget.org/packages/Extenso.AspNetCore.OData)
[![NuGet](https://img.shields.io/nuget/dt/Extenso.AspNetCore.OData?style=flat-square&logo=nuget&label=Downloads)](https://www.nuget.org/packages/Extenso.AspNetCore.OData)

<img src="https://github.com/gordon-matt/Extenso/blob/master/_Misc/ExtensoLogo.png" alt="Logo" width="250" />

# Extenso.AspNetCore.OData

## Intro
This package contains a `GenericODataController`, as well as a `GenericMappedODataController` if working from DAOs instead of entities is required.
To get started, see the Demo project in this repo, which has a fully working CRUD demo using OData, KendoGrid and KnockoutJS.

## Getting Started

This package is dependant on the "Extenso.Data.Entity" package, which provides the `IRepository<T>` and `IMappedRepository<T, TModel>` interfaces required.
Follow the instructions in the [Extenso.Data.Entity](https://github.com/gordon-matt/Extenso) package to get your `IDbContextFactory` and repositories registered for dependency injection.
Once that is done, you can use the `GenericODataController` or `GenericMappedODataController` in your ASP.NET Core application as follows:

Setup an `IODataRegistrar` to register your OData controllers and models:

```csharp
public class ODataRegistrar : IODataRegistrar
{
    public void Register(ODataOptions options)
    {
        ODataModelBuilder builder = new ODataConventionModelBuilder();
        builder.EntitySet<Person>("PersonApi");
        options.AddRouteComponents("odata", builder.GetEdmModel());
    }
}
```

NOTE: Using an `IODataRegistrar` is not required, but it is a good way to keep your OData configuration clean and organized. Moreover, it's
helpful if you have multiple projects each with their own OData controllers that need to be registered.

```csharp
services.AddSingleton<IODataRegistrar, ODataRegistrar>();

services.AddOData((options, serviceProvider) =>
{
    options.Select().Expand().Filter().OrderBy().SetMaxTop(null).Count();

    // You may have many IODataRegistrar implementations in Razor Class Libraries or other projects.
    var registrars = serviceProvider.GetRequiredService<IEnumerable<IODataRegistrar>>();
    foreach (var registrar in registrars)
    {
        registrar.Register(options);
    }
});

```

And now all you need to do is create your OData controllers. Example:

```csharp
public class PersonApiController : BaseODataController<Person, int>
{
    public PersonApiController(IAuthorizationService authorizationService, IRepository<Person> repository)
        : base(authorizationService, repository)
    {
    }

    protected override int GetId(Person entity) => entity.Id;

    protected override void SetNewId(Person entity)
    {
        // Don't try to set the ID here unless it's not auto incremented, such as a GUID, for example.
    }
}
```

As simple as that.. you now have a fully functional OData API for your `Person` entity. If you have a grid that supports OData out of the box, such as KendoGrid, you can now bind it to this API very easily:

```javascript
self.apiUrl = "/odata/PersonApi";

$("#grid").kendoGrid({
    data: null,
    dataSource: {
        type: 'odata-v4',
        transport: {
            read: self.apiUrl
        },
        // etc..
```

Take a look at the demo projects for more details.