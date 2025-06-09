[![NuGet](https://img.shields.io/nuget/v/Extenso.WebAssets.KendoUI?style=flat-square&logo=nuget&label=Version)](https://www.nuget.org/packages/Extenso.WebAssets.KendoUI)
[![NuGet](https://img.shields.io/nuget/dt/Extenso.WebAssets.KendoUI?style=flat-square&logo=nuget&label=Downloads)](https://www.nuget.org/packages/Extenso.WebAssets.KendoUI)

<img src="https://github.com/gordon-matt/Extenso/blob/master/_Misc/ExtensoLogo.png" alt="Logo" width="250" />

# Extenso.WebAssets.KendoUI

## Intro
Kendo UI Web 2014.1.318 was the final version released under the GPLv3 license, making it suitable for non-commercial use.

## Usage:

Make sure to use static assets:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseStaticFiles();
```

Then simply reference the Kendo UI assets in your HTML files:
```html
<link rel="stylesheet" type="text/css" href="~/_content/Extenso.WebAssets.KendoUI/css/kendo/2014.1.318/kendo.common.min.css" />
<link rel="stylesheet" type="text/css" href="~/_content/Extenso.WebAssets.KendoUI/css/kendo/2014.1.318/kendo.bootstrap.min.css" />

<script type="text/javascript" src="~/_content/Extenso.WebAssets.KendoUI/js/kendo/2014.1.318/kendo.web.min.js"></script>
```