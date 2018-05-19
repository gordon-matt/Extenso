
# Extenso Code Library

## Intro
This project is partly based on a much older project of mine called, MBG Extensions Library, which I wrote back in 2010. There is an article on [CodeProject](https://www.codeproject.com/Articles/116940/MBG-Extensions-Library) of which large portions are still applicable to Extenso.Core and parts of Extenso.Data. In any case, Extenso aims to be more than just an extensions library - various other helper classes will be added in separate packages, so you can pick and choose what you like.

## Documentation
Proper documentation will be added in future, but for now, here is a quick rundown of the different packages:

#### Extenso.Core
> NuGet: https://www.nuget.org/packages/Extenso.Core/

This is the main library and mostly contains various extension methods.

#### Extenso.Data
> NuGet: https://www.nuget.org/packages/Extenso.Data/

Extension methods and other data-related helper classes.

#### Extenso.Data.Entity
> NuGet: https://www.nuget.org/packages/Extenso.Data.Entity/

This package contains a generic `IRepository<TEntity>` interface and base class implementation for Entity Framework. 

#### Extenso.Data.QueryBuilder
> NuGet: https://www.nuget.org/packages/Extenso.Data.QueryBuilder/

This package consists of a Query Builder which lets you build T-SQL statements through a fluent interface. 

#### Extenso.AspNetCore.Mvc
> NuGet: https://www.nuget.org/packages/Extenso.AspNetCore.Mvc/

MVC-related extension methods and other helper classes. Includes a FluentTagBuilder, an IRazorViewRenderService (to render Razor views to a string), various HTML Helpers and more.

#### Extenso.AspNetCore.Mvc.ExtensoUI
> NuGet: https://www.nuget.org/packages/Extenso.AspNetCore.Mvc.ExtensoUI/

HTML Helpers for creating common UI components (Accordions, Tabs, Panels, Modal Dialogs, etc). This package includes a default provider for Bootstrap 3.

#### Extenso.AspNetCore.Mvc.ExtensoUI.KendoUI
> NuGet: https://www.nuget.org/packages/Extenso.AspNetCore.Mvc.ExtensoUI.KendoUI/

Kendo UI Provider for ExtensoUI.

#### Extenso.AspNetCore.OData
> NuGet: https://www.nuget.org/packages/Extenso.AspNetCore.OData/

This library contains a `GenericODataController` based on [Microsoft.AspNetCore.OData](https://github.com/OData/Webapi/tree/feature/netcore). To get started, see the Demo project in this repo, which has a fully working CRUD demo using OData, KendoGrid and KnockoutJS.

## License

This project is licensed under the [MIT license](LICENSE.txt).
