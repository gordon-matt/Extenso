using Bogus;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Person = Demo.Extenso.AspNetCore.Mvc.OData.Data.Entities.Person;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Controllers;

[Route("people")]
public class PersonController : Controller
{
    public PersonController(IRepository<Person> personRepository)
    {
        if (personRepository.Count() == 0)
        {
            // Populate for testing purposes

            var faker = new Faker<Person>()
                .RuleFor(x => x.GivenNames, x => x.Name.FirstName())
                .RuleFor(x => x.FamilyName, x => x.Name.LastName())
                .RuleFor(x => x.DateOfBirth, x => x.Date.Between(new DateTime(1900, 1, 1), DateTime.Today.Date));

            personRepository.Insert(faker.Generate(1000));
        }
    }

    [Route("")]
    public IActionResult Index()
    {
        ViewBag.UseMapped = false;
        return View();
    }

    [Route("mapped")]
    public IActionResult Mapped()
    {
        ViewBag.UseMapped = true;
        return View("Index");
    }

    [Route("stream")]
    public IActionResult Stream()
    {
        ViewBag.UseMapped = false;
        return View();
    }

    [Route("stream/mapped")]
    public IActionResult StreamMapped()
    {
        ViewBag.UseMapped = true;
        return View("Stream");
    }
}