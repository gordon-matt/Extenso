using Demo.Extenso.AspNetCore.Mvc.OData.Data.Entities;
using Demo.Extenso.AspNetCore.OData.Models;
using Extenso.AspNetCore.OData;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Infrastructure;

public class ODataRegistrar : IODataRegistrar
{
    public void Register(ODataOptions options)
    {
        ODataModelBuilder builder = new ODataConventionModelBuilder();
        builder.EntitySet<Person>("PersonApi");
        builder.EntitySet<PersonModel>("MappedPersonApi");
        options.AddRouteComponents("odata", builder.GetEdmModel());
    }
}