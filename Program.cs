
using System.Runtime.CompilerServices;
using dotnet_sandbox.Services;
using Hangfire;
using Hangfire.MemoryStorage;


namespace dotnet_sandbox;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHangfire(config =>
        {
            config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
            config.UseSimpleAssemblyNameTypeSerializer();
            config.UseRecommendedSerializerSettings();
            config.UseMemoryStorage();
        });

        builder.Services.AddHangfireServer(options =>
            options.WorkerCount = Environment.ProcessorCount * 5);
        builder.Services.AddSingleton<AppShutdownService>();

        var app = builder.Build();



        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Lifetime.ApplicationStopping.Register(() =>
        {
            app.Logger.LogInformation("Application is shutting down, trigger Cancellation Token!");

            var appShutdownService = app.Services.GetRequiredService<AppShutdownService>();
            appShutdownService.TriggerShutdown();
        });

        app.UseHangfireDashboard();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
