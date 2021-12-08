using System.ComponentModel.DataAnnotations;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Helpers
{
    public class DownloadOptions
    {
        public DownloadFileFormat FileFormat { get; set; }

        public DownloadFileDelimiter Delimiter { get; set; } = DownloadFileDelimiter.Comma;

        public bool AlwaysEnquote { get; set; } = true;

        public bool OutputColumnNames { get; set; } = true;
    }

    public enum DownloadFileFormat : byte
    {
        Delimited = 0,
        XLSX = 1
    }

    public enum DownloadFileDelimiter : byte
    {
        [Display(Name = "Comma (,)")]
        Comma = 0,

        [Display(Name = "Tab")]
        Tab = 1,

        [Display(Name = "Vertical Bar (|)")]
        VerticalBar = 2,

        [Display(Name = "Semicolon (;)")]
        Semicolon = 3
    }
}