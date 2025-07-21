using System.Collections.Specialized;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;

// ReSharper disable once CheckNamespace
namespace Extenso.KendoGridBinder.ModelBinder.Api;

public class KendoGridApiModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var reader = new StreamReader(bindingContext.HttpContext.Request.Body);
        string content = await reader.ReadToEndAsync();

        try
        {
            // Try to parse as Json
            bindingContext.Model = GridHelper.Parse(content);
        }
        catch (Exception)
        {
            // Parse the QueryString
            var queryString = new NameValueCollection();
            foreach (var entry in QueryHelpers.ParseQuery(content))
            {
                string key = entry.Key;
                string value = entry.Value.ToArray().FirstOrDefault();
                queryString.Add(key, value);
            }
            bindingContext.Model = GridHelper.Parse<KendoGridApiRequest>(queryString);
        }
    }
}