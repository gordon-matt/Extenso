using System;

namespace Demo.Extenso.AspNetCore.Mvc.Models;

public class PersonModel
{
    public int Id { get; set; }

    public string FamilyName { get; set; }

    public string GivenNames { get; set; }

    public DateTime DateOfBirth { get; set; }
}