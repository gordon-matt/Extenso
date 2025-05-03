using Demo.Extenso.AspNetCore.Mvc.OData.Data.Entities;
using Extenso.Data.Entity;

namespace Demo.Extenso.AspNetCore.OData.Models;

public class PersonModel : BaseEntity<int>
{
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