using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Extenso.AspNetCore.Blazor.OData.Services;
using Extenso;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Pages
{
    public abstract class BaseDataPage<TEntity, TKey> : ComponentBase
        where TEntity : class, IEntity, new()
    {
        protected TEntity Model { get; set; } = new();

        protected RadzenDataGrid<TEntity> DataGrid { get; set; }

        protected IEnumerable<TEntity> Records { get; set; }

        protected int RecordCount { get; set; }

        protected bool IsLoading { get; set; }

        protected bool ShowEditMode { get; set; }

        #region Dependencies

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected IGenericODataService<TEntity, TKey> ODataService { get; set; }

        #endregion Dependencies

        #region CRUD Operations

        protected virtual async Task LoadGridAsync(LoadDataArgs args)
        {
            IsLoading = true;

            string odataFilter = GetODataFilter(args);

            var response = await ODataService.FindAsync(
                filter: odataFilter,
                top: args.Top,
                skip: args.Skip,
                orderby: args.OrderBy,
                count: true);

            if (response.Succeeded)
            {
                Records = response.Data.Value.AsODataEnumerable();
                RecordCount = response.Data.Count;
            }
            else
            {
                Records = Enumerable.Empty<TEntity>();
                RecordCount = 0;
                NotificationService.Notify(NotificationSeverity.Error, "Error", "Unable to retrieve records!");
                // TODO: Log Error
            }

            IsLoading = false;
        }

        protected virtual string GetODataFilter(LoadDataArgs args) => args.Filter;

        protected virtual void Create()
        {
            Model = new TEntity();
            ShowEditMode = true;
        }

        protected virtual async Task EditAsync(TKey id)
        {
            var response = await ODataService.FindOneAsync(id);
            if (response.Succeeded)
            {
                Model = response.Data;
                ShowEditMode = true;
            }
            else
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", "Unable to retrieve record!");
                // TODO: Log Error
            }
        }

        protected virtual async Task OnValidSumbitAsync()
        {
            var key = (TKey)Model.KeyValues.First();
            bool isInsert = key.IsDefault();

            var response = isInsert
                ? await ODataService.InsertAsync(Model)
                : await ODataService.UpdateAsync(key, Model);

            if (response.Succeeded)
            {
                await DataGrid.Reload();
                ShowEditMode = false;

                NotificationService.Notify(
                    NotificationSeverity.Info,
                    "Info",
                    isInsert ? "Record created!" : "Record updated!");
            }
            else
            {
                NotificationService.Notify(
                    NotificationSeverity.Error,
                    "Error",
                    isInsert ? "Unable to insert record!" : "Unable to update record!");

                // TODO: Log Error
            }
        }

        protected virtual async Task DeleteAsync(TKey id)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var response = await ODataService.DeleteAsync(id);
                    if (response.Succeeded)
                    {
                        await DataGrid.Reload();
                        NotificationService.Notify(NotificationSeverity.Info, "Info", "Record deleted!");
                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Error, "Error", "Unable to delete record!");
                        // TODO: Log Error
                    }
                }
            }
            catch
            {
                NotificationService.Notify(NotificationSeverity.Error, "Error", "Unable to delete record!");
                // TODO: Log Error
            }
        }

        protected virtual void Cancel()
        {
            Model = new TEntity();
            ShowEditMode = false;
        }

        #endregion CRUD Operations
    }
}