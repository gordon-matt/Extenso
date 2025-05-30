[![NuGet](https://img.shields.io/nuget/v/Extenso.Mapping?style=flat-square&logo=nuget&label=Version)](https://www.nuget.org/packages/Extenso.Mapping)
[![NuGet](https://img.shields.io/nuget/dt/Extenso.Mapping?style=flat-square&logo=nuget&label=Downloads)](https://www.nuget.org/packages/Extenso.Mapping)
[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=gordon_matt%40live%2ecom&lc=AU&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted)

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

## Donate
If you find this project helpful, consider buying me a cup of coffee.  :-)

[![PayPal](https://img.shields.io/badge/PayPal-003087?logo=paypal&logoColor=fff)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=gordon_matt%40live%2ecom&lc=AU&currency_code=AUD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted)
[![Bitcoin](https://img.shields.io/badge/Bitcoin-FF9900?logo=bitcoin&logoColor=white)](bitcoin:1EeDfbcqoEaz6bbcWsymwPbYv4uyEaZ3Lp)
[![Ethereum](https://img.shields.io/badge/Ethereum-3C3C3D?logo=ethereum&logoColor=white)](ethereum:0x277552efd6ea9ca9052a249e781abf1719ea9414)
[![Litecoin](https://img.shields.io/badge/Litecoin-A6A9AA?logo=litecoin&logoColor=white)](litecoin:LRUP8hukWGXRrcPK6Tm7iUp9vPvnNNt3uz)
