using Demo.Extenso.AspNetCore.Mvc.OData.Data.Entities;
using Extenso.AspNetCore.OData;
using Extenso.Data.Entity;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Controllers.Api
{
    public class PersonApiController : BaseODataController<Person, int>
    {
        public PersonApiController(IRepository<Person> repository)
            : base(repository)
        {
        }

        protected override int GetId(Person entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Person entity)
        {
        }
    }
}