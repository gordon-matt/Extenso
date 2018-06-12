using System;
using System.Collections.Generic;
using Demo.Extenso.AspNetCore.Mvc.OData.Data.Domain;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Extenso.AspNetCore.Mvc.OData.Controllers
{
    [Route("people")]
    public class PersonController : Controller
    {
        private readonly IRepository<Person> personRepository;

        public PersonController(IRepository<Person> personRepository)
        {
            this.personRepository = personRepository;
        }

        [Route("")]
        public IActionResult Index()
        {
            if (personRepository.Count() == 0)
            {
                // Populate for testing purposes

                var people = new List<Person>();

                people.Add(new Person { FamilyName = "Jordan", GivenNames = "Michael", DateOfBirth = new DateTime(1963, 2, 17) });
                people.Add(new Person { FamilyName = "Johnson", GivenNames = "Dwayne", DateOfBirth = new DateTime(1972, 5, 2) });
                people.Add(new Person { FamilyName = "Froning", GivenNames = "Rich", DateOfBirth = new DateTime(1987, 7, 21) });

                personRepository.Insert(people);
            }

            return View();
        }
    }
}