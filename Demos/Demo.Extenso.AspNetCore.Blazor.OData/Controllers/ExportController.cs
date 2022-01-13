using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using Demo.Extenso.AspNetCore.Blazor.OData.Extensions;
using Demo.Extenso.AspNetCore.Blazor.OData.Helpers;
using Extenso.Collections;
using Extenso.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Controllers
{
    public partial class ExportController<T> : Controller
         where T : class, IEntity
    {
        public IQueryable<T> ApplyQuery(IQueryable<T> items, IQueryCollection query = null)
        {
            if (query != null)
            {
                if (query.ContainsKey("$filter"))
                {
                    items = items.Where(query["$filter"].ToString());
                }

                if (query.ContainsKey("$orderBy"))
                {
                    items = items.OrderBy(query["$orderBy"].ToString());
                }

                if (query.ContainsKey("$expand"))
                {
                    string[] propertiesToExpand = query["$orderBy"].ToString().Split(',');
                    foreach (string p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (query.ContainsKey("$skip"))
                {
                    items = items.Skip(int.Parse(query["$skip"].ToString()));
                }

                if (query.ContainsKey("$top"))
                {
                    items = items.Take(int.Parse(query["$top"].ToString()));
                }
            }

            return items;
        }

        public FileResult Download(IQueryable<T> query, DownloadOptions options)
        {
            switch (options.FileFormat)
            {
                case DownloadFileFormat.Delimited:
                    {
                        string separator;
                        string contentType;
                        string fileExtension;

                        switch (options.Delimiter)
                        {
                            case DownloadFileDelimiter.Tab:
                                separator = "\t";
                                contentType = "text/tab-separated-values";
                                fileExtension = "tsv";
                                break;

                            case DownloadFileDelimiter.VerticalBar:
                                separator = "|";
                                contentType = "text/plain";
                                fileExtension = "txt";
                                break;

                            case DownloadFileDelimiter.Semicolon:
                                separator = ";";
                                contentType = "text/plain";
                                fileExtension = "txt";
                                break;

                            case DownloadFileDelimiter.Comma:
                            default:
                                separator = ",";
                                contentType = "text/csv";
                                fileExtension = "csv";
                                break;
                        }

                        string delimited = query.ToDelimited(
                            delimiter: separator,
                            outputColumnNames: options.OutputColumnNames,
                            alwaysEnquote: options.AlwaysEnquote);

                        return File(Encoding.UTF8.GetBytes(delimited), contentType, $"{query.ElementType}_{DateTime.Now:yyyy-MM-dd HH_mm_ss}.{fileExtension}");
                    }
                case DownloadFileFormat.XLSX:
                    {
                        byte[] bytes = query.ToDataTable().ToXlsx();
                        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{query.ElementType}_{DateTime.Now:yyyy-MM-dd HH_mm_ss}.xlsx");
                    }
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}