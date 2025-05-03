using Demo.Extenso.AspNetCore.Mvc.OData.Data.Entities;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Controllers;

[Route("people")]
public class PersonController : Controller
{
    public PersonController(IRepository<Person> personRepository)
    {
        if (personRepository.Count() == 0)
        {
            // Populate for testing purposes

            var people = new List<Person>
            {
                new() { FamilyName = "Jordan", GivenNames = "Michael", DateOfBirth = new DateTime(1963, 2, 17) },
                new() { FamilyName = "Johnson", GivenNames = "Dwayne", DateOfBirth = new DateTime(1972, 5, 2) },
                new() { FamilyName = "Froning", GivenNames = "Rich", DateOfBirth = new DateTime(1987, 7, 21) }
            };

            personRepository.Insert(people);
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
}