namespace Demo.Extenso.AspNetCore.Blazor.OData.Models;

public class ApiResponse
{
    public ApiResponse()
    {
    }

    internal ApiResponse(bool succeeded, string error)
    {
        Succeeded = succeeded;
        Errors = new[] { error };
    }

    internal ApiResponse(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    public bool Succeeded { get; set; }

    public IEnumerable<string> Errors { get; set; }

    public static ApiResponse Failure(IEnumerable<string> errors) => new(false, errors);

    public static ApiResponse Failure(string error) => new(false, error);

    public static ApiResponse Success() => new(true, Enumerable.Empty<string>());
}

public class ApiResponse<T>
{
    internal ApiResponse(bool succeeded, string error)
    {
        Succeeded = succeeded;
        Errors = new[] { error };
    }

    internal ApiResponse(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors;
    }

    internal ApiResponse(bool succeeded, T data, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors;
        Data = data;
    }

    public bool Succeeded { get; set; }

    public T Data { get; set; }

    public IEnumerable<string> Errors { get; set; }

    public static ApiResponse<T> Failure(string error) => new(false, error);

    public static ApiResponse<T> Failure(IEnumerable<string> errors) => new(false, errors);

    public static ApiResponse<T> Success(T data) => new(true, data, Enumerable.Empty<string>());
}