using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Extenso.Net;

public static class HttpClientExtensions
{
    /// <summary>
    /// Returns a data URL for the specified requestUri. This can be used to download and embed images, for example.
    /// </summary>
    /// <param name="httpClient">The HttpClient</param>
    /// <param name="requestUri">The URI</param>
    /// <returns>A data URL. Example: "data:[<mediatype>];base64,&lt;data&gt;"</returns>
    public static async Task<string> GetBase64DataUrlAsync(this HttpClient httpClient, string requestUri)
    {
        using var response = await httpClient.GetAsync(requestUri);
        _ = response.EnsureSuccessStatusCode();
        byte[] bytes = await response.Content.ReadAsByteArrayAsync();
        string base64Data = Convert.ToBase64String(bytes);
        string contentType = response.Content.Headers.ContentType.MediaType;
        return $"data:{contentType};base64,{base64Data}";
    }
}