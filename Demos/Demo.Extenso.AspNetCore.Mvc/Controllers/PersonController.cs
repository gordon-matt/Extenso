using Demo.Extenso.AspNetCore.Mvc.Data.Entities;
using Demo.Extenso.AspNetCore.Mvc.Models;
using Extenso.Data.Entity;
using KendoGridBinder;
using KendoGridBinder.ModelBinder.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Extenso.AspNetCore.Mvc.Controllers;

[Route("people")]
public class PersonController : Controller
{
    private readonly IMappedRepository<PersonModel, Person> personRepository;

    public PersonController(IMappedRepository<PersonModel, Person> personRepository)
    {
        this.personRepository = personRepository;
    }

    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {
        if (personRepository.Count() == 0)
        {
            // Populate for testing purposes

            var people = new List<PersonModel>
            {
                new() { FamilyName = "Jordan", GivenNames = "Michael", DateOfBirth = new DateTime(1963, 2, 17) },
                new() { FamilyName = "Johnson", GivenNames = "Dwayne", DateOfBirth = new DateTime(1972, 5, 2) },
                new() { FamilyName = "Froning", GivenNames = "Rich", DateOfBirth = new DateTime(1987, 7, 21) }
            };

            personRepository.Insert(people);
        }

        return View();
    }

    [HttpPost]
    [Route("grid")]
    public IActionResult Grid([FromBody] KendoGridMvcRequest request)
    {
        using var connection = personRepository.OpenConnection();
        var query = connection.Query(x => true);

        var grid = new KendoGrid<PersonModel>(request, query);
        return Json(grid);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var person = await personRepository.FindOneAsync(id);
        return person is null ? NotFound() : Json(person);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var person = await personRepository.FindOneAsync(id);
        if (person is null)
        {
            return NotFound();
        }

        await personRepository.DeleteAsync(person);
        return Json(person);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Post([FromBody] PersonModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await personRepository.InsertAsync(model);
        return Json(model);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PersonModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var person = await personRepository.FindOneAsync(id);
        if (person is null)
        {
            return NotFound();
        }

        await personRepository.UpdateAsync(model);
        return Json(model);
    }
}