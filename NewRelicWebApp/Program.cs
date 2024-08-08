using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using NewRelic.LogEnrichers;
using NewRelic.LogEnrichers.Serilog;
using NewRelicWebApp.Services;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCorrelate(options => 
    options.RequestHeaders =
    [
        // List of incoming headers possible. First that is set on given request is used and also returned in the response.
        "X-Correlation-ID"
    ]
);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console().CreateBootstrapLogger();


builder.Services.AddSerilog((services, lc) => lc
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithNewRelicLogsInContext().WriteTo.Console(
        formatter: new NewRelicFormatter().WithPropertyMapping("CorrelationId", NewRelicLoggingProperty.CorrelationId)));
       //.WriteTo.Console(outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}[{CorrelationId}] [{Level:u3}] {Message:lj}{NewLine}{Exception}"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddGrpc();

var app = builder.Build();
app.UseCorrelate();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GreeterService>();

//LogALot(app.Logger);
app.Run();

static async Task LogALot(ILogger logger)
{
    var data = Enumerable.Range(1, 10000);
    foreach (var item in data)
    {
        await Task.Delay(2000);
        logger.LogInformation("loguje {datetime}", DateTime.Now);
    }
}