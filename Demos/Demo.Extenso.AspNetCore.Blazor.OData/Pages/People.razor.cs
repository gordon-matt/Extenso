using Demo.Extenso.AspNetCore.Blazor.OData.Extensions;
using Extenso.Collections;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Pages;

public partial class People
{
    private RadzenTextBox FamilyNameTextBox;
    private RadzenTextBox GivenNamesTextBox;

    [Inject]
    private NavigationManager NavigationManager { get; set; }

    protected override string GetODataFilter(LoadDataArgs args)
    {
        var filters = new List<FilterDescriptor>();

        if (!string.IsNullOrEmpty(FamilyNameTextBox.Value))
        {
            filters.Add(new FilterDescriptor
            {
                FilterOperator = FilterOperator.Contains,
                FilterValue = FamilyNameTextBox.Value,
                LogicalFilterOperator = LogicalFilterOperator.And,
                Property = "FamilyName"
            });
        }

        if (!string.IsNullOrEmpty(GivenNamesTextBox.Value))
        {
            filters.Add(new FilterDescriptor
            {
                FilterOperator = FilterOperator.Contains,
                FilterValue = GivenNamesTextBox.Value,
                LogicalFilterOperator = LogicalFilterOperator.And,
                Property = "GivenNames"
            });
        }

        return filters.IsNullOrEmpty() ? args.Filter : filters.ToODataFilterString(DataGrid);
    }

    public async Task SearchAsync()
    {
        DataGrid.Reset();
        await DataGrid.Reload();
    }

    public void Export(string type)
    {
        var query = new Query { OrderBy = DataGrid.Query.OrderBy, Filter = DataGrid.Query.Filter };
        NavigationManager.NavigateTo(query.ToUrl($"/person/export/{type}"), forceLoad: true);
    }
}