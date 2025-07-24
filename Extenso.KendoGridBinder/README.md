[![NuGet](https://img.shields.io/nuget/v/Extenso.KendoGridBinder?style=flat-square&logo=nuget&label=Version)](https://www.nuget.org/packages/Extenso.KendoGridBinder)
[![NuGet](https://img.shields.io/nuget/dt/Extenso.KendoGridBinder?style=flat-square&logo=nuget&label=Downloads)](https://www.nuget.org/packages/Extenso.KendoGridBinder)

<img src="https://github.com/gordon-matt/Extenso/blob/master/_Misc/ExtensoLogo.png" alt="Logo" width="250" />

# Extenso.KendoGridBinder

## Intro
This library provides a simple way to bind Kendo UI grids to data in ASP.NET MVC applications.

## Getting Started

### Action Method
```csharp
[HttpPost]
public JsonResult Grid(KendoGridMvcRequest request)
{
    var employees = new List<Employee>
    {
        new Employee { EmployeeId = 1, FirstName = "Bill", LastName = "Jones", Email = "bill@email.com" },
        new Employee { EmployeeId = 2, FirstName = "Rob", LastName = "Johnson", Email = "rob@email.com" },
        new Employee { EmployeeId = 3, FirstName = "Jane", LastName = "Smith", Email = "jane@email.com" }
    };

    var grid = new KendoGridEx<Employee, EmployeeVM>(request, employees.AsQueryable());
    return Json(grid);
}
```

### HTML
```html
<div id="grid"></div>
```

### Script
```javascript
<script>
    var url = '@Url.Action("Grid")';

    var dataSource = {
        serverPaging: true,
        serverSorting: true,
        serverFiltering: true,
        pageSize: 20,
        transport: {
            read: {
                type: 'post',
                dataType: 'json',
                url: url
            }
        },
        schema: {
            data: 'Data',
            total: 'Total',
            model: {
                id: 'Id',
                fields: {
                    FirstName: { type: 'string' },
                    LastName: { type: 'string' },
                    Email: { type: 'string' }
                }
            }
        }
    };

    $('#grid').kendoGrid({
        dataSource: dataSource,
        height: 400,
        columns: [
            { field: 'FirstName', title: 'First Name' },
            { field: 'LastName', title: 'Last Name' },
            { field: 'Email' }
        ],        
        filterable: true,
        sortable: true,
        pageable: true
    });
</script>
```

## Credits

The code was originally written by [Stef Heyenrath](https://github.com/StefH/KendoGridBinderEx) and licensed under the MIT License.