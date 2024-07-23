// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(x=>x.SetMinimumLevel(LogLevel.Trace).AddConsole());
var sp = serviceCollection.BuildServiceProvider();
var logger = sp.GetRequiredService<ILogger<Program>>();

await LogALot(logger);



static async Task LogALot(ILogger<Program> logger)
{
    var data = Enumerable.Range(1, 10000);
    foreach (var item in data)
    {
        await Task.Delay(2000);
        logger.LogInformation("loguje {datetime}", DateTime.Now);
    }
}