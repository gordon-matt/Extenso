using System.Runtime.CompilerServices;
using System.Text.Json;
using Demo.Extenso.AspNetCore.Blazor.OData.Data.Entities;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace Demo.Extenso.AspNetCore.Blazor.OData.Pages;

public partial class PeopleStream : ComponentBase, IAsyncDisposable
{
    private readonly List<Person> records = [];
    private CancellationTokenSource cancellationTokenSource;
    private Task streamingTask;

    protected RadzenDataGrid<Person> DataGrid { get; set; }

    protected IReadOnlyList<Person> Records => records;

    protected bool IsStreaming { get; private set; }

    protected string StatusMessage { get; private set; } = "Click Start Streaming to begin.";

    private string statusVariant = "info";

    protected string StatusCssClass => $"alert-{statusVariant}";

    [Inject]
    protected HttpClient HttpClient { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    [Inject]
    protected ILogger<PeopleStream> Logger { get; set; }

    protected async Task StartStreaming()
    {
        if (IsStreaming)
        {
            return;
        }

        ResetRecords();

        cancellationTokenSource = new CancellationTokenSource();
        IsStreaming = true;
        statusVariant = "info";
        StatusMessage = "Streaming people ...";

        var streamUri = new Uri(new Uri(NavigationManager.BaseUri), "odata/PersonApi/stream");

        streamingTask = Task.Run(async () =>
        {
            try
            {
                await foreach (var person in ReadPeopleStreamAsync(streamUri, cancellationTokenSource.Token))
                {
                    records.Add(person);
                    await InvokeAsync(StateHasChanged);
                }

                statusVariant = "success";
                StatusMessage = $"Streaming completed. {records.Count} records loaded.";
            }
            catch (OperationCanceledException)
            {
                statusVariant = "warning";
                StatusMessage = $"Streaming cancelled after {records.Count} records.";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Streaming failed.");
                statusVariant = "danger";
                StatusMessage = $"Streaming failed: {ex.Message}";
            }
            finally
            {
                IsStreaming = false;
                await InvokeAsync(StateHasChanged);
            }
        });
    }

    protected void CancelStreaming()
    {
        if (!IsStreaming)
        {
            return;
        }

        cancellationTokenSource?.Cancel();
    }

    protected void ResetRecords()
    {
        if (IsStreaming)
        {
            return;
        }

        records.Clear();
        statusVariant = "secondary";
        StatusMessage = "Ready to stream.";
    }

    private async IAsyncEnumerable<Person> ReadPeopleStreamAsync(Uri streamUri, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, streamUri);
        using var response = await HttpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(responseStream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var person = JsonSerializer.Deserialize<Person>(
                line,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (person != null)
            {
                yield return person;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (IsStreaming)
        {
            cancellationTokenSource?.Cancel();
        }

        if (streamingTask is not null)
        {
            await streamingTask;
        }

        cancellationTokenSource?.Dispose();
    }
}

