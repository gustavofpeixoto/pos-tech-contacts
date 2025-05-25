using Microsoft.AspNetCore.Mvc.ApplicationModels;
using PosTech.Contacts.ApplicationCore;
using PosTech.Contacts.Infrastructure;
using PosTech.Contacts.Worker;
using PosTech.Contacts.Worker.Settings;
using Prometheus;
using Prometheus.DotNetRuntime;
using Quartz;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFileByName("sharedsettings");

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var prometheusBuilder = DotNetRuntimeStatsBuilder.Default();
prometheusBuilder = DotNetRuntimeStatsBuilder.Customize()
    .WithContentionStats(CaptureLevel.Informational)
    .WithGcStats(CaptureLevel.Verbose)
    .WithThreadPoolStats(CaptureLevel.Informational)
    .WithExceptionStats(CaptureLevel.Errors)
    .WithJitStats();

prometheusBuilder.RecycleCollectorsEvery(new TimeSpan(0, 20, 0));
prometheusBuilder.StartCollecting();

builder.Services
    .AddControllers(options
         => options.Conventions.Add(new RouteTokenTransformerConvention(new OutboundParameterTransformerSetting())))
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddSerilog();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
builder.Services.AddApplicationCoreServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });

var app = builder.Build();

await app.AddQuartzJobsAsync(app.Configuration);

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseHttpMetrics();
app.UseMetricServer();
app.MapControllers();
app.Run();
