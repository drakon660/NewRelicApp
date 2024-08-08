using Correlate.AspNetCore;
using Correlate.DependencyInjection;
using NewRelicWebAppClient;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using TracingGrpcService;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog((services, lc) => lc
   .ReadFrom.Configuration(builder.Configuration)
       .ReadFrom.Services(services)
       .Enrich.FromLogContext());
    //.WriteTo.Console(outputTemplate:"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}[{CorrelationId}] [{Level:u3}] {Message:lj}{NewLine}{Exception}"));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCorrelate();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddGrpcClient<Greeter.GreeterClient>(options => options.Address = new Uri("http://newrelicwebapp:8080"))
    .AddGrpcCorrelationIdForwarding();

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

app.Run();