# dotnet-sandbox

This is a personal playground, IE: Sandbox, to test and prototype code in an operating environment.  This is not meant to be production level code, but if some of the ideas in here, examples, or techniques; then use them as you wish!  

# Setup
#### Clone the repository:
``` 
git clone git@github.com:bsampica/dotnet-sandbox.git     

or

git clone https://github.com/bsampica/dotnet-sandbox.git

```

## Install the dependencies

``` 
dotnet restore
```

``` 
dotnet run
```

## URL Access
```
http://localhost:5000/swagger
```
```
http://localhost:5000/hangfire
```


This project depends on:
- [Microsoft WebAPI](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0)
- [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [C# 12.0 with Advanced Features enabled](https://github.com/dotnet/docs/blob/main/docs/csharp/whats-new/csharp-12.md)
- [git for windows (command line)](https://git-scm.com/downloads)
- [HangFire](https://www.hangfire.io/)
- [Hangfire.InMemoryStorage](https://github.com/HangfireIO/Hangfire.InMemory)

##### Note, these dependencies should install with dotnet restore.


## Development Environment

Its being developed in **Visual Studio Code**, and I've tested it with **Visual Studio 2022**, and **JetBrains Rider**.


## Special Notation
### Method Decorators
In the HangfireJonController you'll notice that the class is annotated.

```[ApiExplorerSettings(IgnoreApi = true)]```

```
[ApiExplorerSettings(IgnoreApi = true)]
public async Task BackgroundWork(int jobParameter CancellationToken token)
{
    // Simulate some work with a delay
    await Task.Delay(TimeSpan.FromMinutes(1), token);
    Console.WriteLine($"Running Job: {jobParameter}");
}
``` 
This prevents <span style="color:#00a2ed"> Swagger</span> and <span style="color:#00a2ed">Swashbuckle</span> from attempting to <span style="color:orange"> discover </span> this method.  It will attempt to <span style="color:orange"> ***map*** </span> any public method on the controller, but in this case will fail because it's not decorated with <span style="color:orange"> [HttpPost] </span> or <span style="color:orange">[HttpGet] </span>..etc.  

It's simply a helper function inside the controller.  In a production product you'd probably have a helper class located somewhere else, or do this work in the next layer down.