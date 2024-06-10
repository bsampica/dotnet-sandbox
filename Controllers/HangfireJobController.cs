using System.Runtime.CompilerServices;
using dotnet_sandbox.Services;
using Hangfire;
using Hangfire.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace dotnet_sandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HangfireJobController(ILogger<HangfireJobController> _logger,
    IBackgroundJobClient _backgroundClient) : ControllerBase
{

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> QueueJob(int jobDelaySeconds)
    {
        _logger.LogInformation("QueueJob Called");
        var backgroundJob = _backgroundClient.Enqueue(() =>
            BackgroundWork(1, jobDelaySeconds, CancellationToken.None));

        _backgroundClient.ContinueJobWith(backgroundJob, () => JobContinueWith(null));
        return Ok();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> QueueMultipleJobs(int numOfJobs, int jobDelaySeconds)
    {
        _logger.LogInformation("QueueMultipleJobs Called");
        await Task.Run(() =>
        {
            foreach (int index in Enumerable.Range(1, numOfJobs))
            {
                Console.WriteLine($"Adding Background Jobs To Queue: {index}");
                var backgroundJob = _backgroundClient.Enqueue(() =>
                   BackgroundWork(index, jobDelaySeconds, CancellationToken.None));

                _backgroundClient.ContinueJobWith(backgroundJob, () => JobContinueWith(null));
            }
        });

        //await jobAddingTask.ContinueWith((context) => Console.WriteLine("All jobs have been added"));

        return Ok();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [ContinuationsSupport(pushResults: true)]
    public async Task<string> BackgroundWork(int jobParameter, int jobDelaySeconds, CancellationToken token)
    {
        // Simulate some work with a delay
        await Task.Delay(TimeSpan.FromSeconds(jobDelaySeconds), token);
        Console.WriteLine($"Running Job: {jobParameter}");

        return jobParameter.ToString();
    }

    private static async IAsyncEnumerable<int> RangeAsync(int start, int count, [EnumeratorCancellation] CancellationToken token)
    {
        for (int i = 0; i < count; i++)
        {
            if (token.IsCancellationRequested) break;

            // slow it down a bit so we can see it in the console.
            // there's really no other reason to do this.
            await Task.Delay(1, token);
            yield return start + i;
        }
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public void JobContinueWith(PerformContext? context)
    {
        // NOTE:  No idea why you have to use the AntecedentResult parameter, What the heck HANGFIRE?
        var jobParameter = context?.GetJobParameter<string>("AntecedentResult");
        Console.WriteLine($"Job ID: {context?.BackgroundJob.Id} Completed: {jobParameter}");
    }
}