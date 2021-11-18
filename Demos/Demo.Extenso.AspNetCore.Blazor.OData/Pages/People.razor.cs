using Demo.Extenso.AspNetCore.Blazor.OData.Data.Domain;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Pages
{
    public partial class People
    {
        private RadzenTextBox FamilyNameTextBox;
        private RadzenTextBox GivenNamesTextBox;

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public void SearchAsync()
        {
            // ??
        }

        public void Export(string type)
        {
            var query = new Query { OrderBy = DataGrid.Query.OrderBy, Filter = DataGrid.Query.Filter };
            NavigationManager.NavigateTo(query.ToUrl($"/person/export/{type}"), forceLoad: true);
        }
    }
}