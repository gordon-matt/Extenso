using Demo.Extenso.AspNetCore.Blazor.OData.Data.Entities;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Services;

public class PersonODataService : GenericODataService<Person>
{
    public PersonODataService() : base("PersonApi")
    {
    }
}