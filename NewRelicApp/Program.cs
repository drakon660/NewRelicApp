// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NewRelic.Api.Agent;
using NewRelic.LogEnrichers.Serilog;
using Serilog;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithNewRelicLogsInContext()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting host");

    var hostbuilder = new HostBuilder();
    hostbuilder.ConfigureServices(((context, collection) =>
    {
        collection.AddSerilog();
    }));
    var sp = hostbuilder.Build();
    var logger = sp.Services.GetRequiredService<ILogger<Program>>();
    
    LogDelay(logger);
    await LogALot(logger);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}

return 0;
//await LogALot(logger);

static async Task LogALot(ILogger logger)
{
    var data = Enumerable.Range(1, 10000);
    foreach (var item in data)
    {
        await Task.Delay(2000);
        
        logger.LogCritical("cos tam");
    }
}

static async Task LogDelay(ILogger logger)
{
     var data = Enumerable.Range(1, 10000);
     foreach (var item in data)
     {  
         await Task.Delay(15000);
         logger.LogCritical("TEMPLATENOTFOUND TEST1");
     }
}