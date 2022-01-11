using Demo.Extenso.AspNetCore.Blazor.OData.Data;
using Demo.Extenso.AspNetCore.Blazor.OData.Data.Entities;
using Demo.Extenso.AspNetCore.Blazor.OData.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Controllers
{
    [Route("person")]
    public class PersonController : ExportController<Person>
    {
        private readonly ApplicationDbContext context;

        public PersonController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("export/csv")]
        public FileResult ExportToCsv()
        {
            return Download(ApplyQuery(context.People, Request.Query), new DownloadOptions
            {
                FileFormat = DownloadFileFormat.Delimited
            });
        }

        [HttpGet("export/excel")]
        public FileResult ExportToExcel()
        {
            return Download(ApplyQuery(context.People, Request.Query), new DownloadOptions
            {
                FileFormat = DownloadFileFormat.XLSX
            });
        }
    }
}