using Demo.Extenso.AspNetCore.Mvc.OData.Data.Entities;
using Extenso.AspNetCore.OData;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Controllers.Api;

public class PersonApiController : BaseODataController<Person, int>
{
    public PersonApiController(IAuthorizationService authorizationService, IRepository<Person> repository)
        : base(authorizationService, repository)
    {
    }

    protected override int GetId(Person entity) => entity.Id;

    protected override void SetNewId(Person entity)
    {
    }
}