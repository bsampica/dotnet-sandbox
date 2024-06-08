using System.Runtime.CompilerServices;
using dotnet_sandbox.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace dotnet_sandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HangfireJobController(ILogger<HangfireJobController> _logger,
    IBackgroundJobClient _backgroundClient,
    AppShutdownService _shutdownService) : ControllerBase
{

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> QueueJob()
    {
        _logger.LogInformation("QueueJob Called");
        await Task.Run(() => _backgroundClient.Enqueue(() =>
            BackgroundWork(1, CancellationToken.None)));
        return Ok();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> QueueMultipleJobs(int numOfJobs)
    {
        _logger.LogInformation("QueueMultipleJobs Called");
        await foreach (int index in RangeAsync(1, numOfJobs, _shutdownService.Token))
        {
            Console.WriteLine($"Adding Background Jobs To Queue: {index}");
            await Task.Run(() => _backgroundClient.Enqueue(() =>
                BackgroundWork(index, CancellationToken.None)))
                    .ContinueWith((context) =>
                        {
                            Console.WriteLine($"Adding to Queue complete - Added : {numOfJobs}");
                        });
        }

        return Ok();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task BackgroundWork(int jobParameter, CancellationToken token)
    {
        // Simulate some work with a delay
        await Task.Delay(TimeSpan.FromMinutes(1), token);
        Console.WriteLine($"Running Job: {jobParameter}");
    }

    static async IAsyncEnumerable<int> RangeAsync(int start, int count, [EnumeratorCancellation] CancellationToken token)
    {

        for (int i = 0; i < count; i++)
        {
            if (token.IsCancellationRequested) break;
            await Task.Delay(1);
            yield return start + i;
        }
    }
}