using Demo.Extenso.AspNetCore.Mvc.OData.Data.Entities;
using Demo.Extenso.AspNetCore.OData.Models;
using Extenso.AspNetCore.OData;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Controllers.Api;

public class MappedPersonApiController : BaseMappedODataController<PersonModel, Person, int>
{
    public MappedPersonApiController(IAuthorizationService authorizationService, IMappedRepository<PersonModel, Person> repository)
        : base(authorizationService, repository)
    {
    }

    protected override int GetId(PersonModel entity) => entity.Id;

    protected override void SetNewId(PersonModel entity)
    {
    }
}