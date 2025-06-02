[![NuGet](https://img.shields.io/nuget/v/Extenso.Mapping?style=flat-square&logo=nuget&label=Version)](https://www.nuget.org/packages/Extenso.Mapping)
[![NuGet](https://img.shields.io/nuget/dt/Extenso.Mapping?style=flat-square&logo=nuget&label=Downloads)](https://www.nuget.org/packages/Extenso.Mapping)

<img src="https://github.com/gordon-matt/Extenso/blob/master/_Misc/ExtensoLogo.png" alt="Logo" width="250" />

# Extenso.Mapping

## Intro
A lightweight, simpler alternative to AutoMapper, complete with expression mapping for predicates, includes, projections, etc.

## Examples:

### Registration

```csharp
using Extenso.Mapping;

// Basic registration of a mapping between two types
ExtensoMapper.Register<Person, PersonModel>(x => new()
{
    Id = x.Id,
    FamilyName = x.FamilyName,
    GivenNames = x.GivenNames,
    DateOfBirth = x.DateOfBirth
});

// Recommended: use an extension method, as it's cleaner and you can use the extension method directly in most cases
// Only use ExtensoMapper where needed. For example, generic classes, where you don't know the types at compile time.
ExtensoMapper.Register<Person, PersonModel>(x => x.ToModel());
```

### Mapping
```csharp
using Extenso.Mapping;

var model = personEntity.MapTo<PersonModel>();

// or:
var model = ExtensoMapper.Map<Person, PersonModel>(personEntity);
```

You can map expressions as well. See the `ExtensoMapperEntityFrameworkRepository` in the `Extenso.Data.Entity` project for an example