using System;
using Demo.Extenso.AspNetCore.Mvc.Data.Entities;

namespace Demo.Extenso.AspNetCore.Mvc.Models;

public class PersonModel
{
    public int Id { get; set; }

    public string FamilyName { get; set; }

    public string GivenNames { get; set; }

    public DateTime DateOfBirth { get; set; }
}

public static class PersonMappingExtensions
{
    public static PersonModel ToModel(this Person person) => new()
    {
        Id = person.Id,
        FamilyName = person.FamilyName,
        GivenNames = person.GivenNames,
        DateOfBirth = person.DateOfBirth
    };

    public static Person ToEntity(this PersonModel personModel) => new()
    {
        Id = personModel.Id,
        FamilyName = personModel.FamilyName,
        GivenNames = personModel.GivenNames,
        DateOfBirth = personModel.DateOfBirth
    };
}