using DRY.MailJetClient.Library.Extensions;
using Hangfire;
using LocationsMarker.API.Configurations;
using LocationsMarker.API.Extensions;
using LocationsMarker.API.Filters;
using Microsoft.AspNetCore.Mvc;
using Mongo.Common.MongoDB;
using RedisCache.Common.Repository.Extensions;

var builder = WebApplication.CreateBuilder(args);
AppConfigurations.ConfigureLogging(builder.Configuration["ELK:Connection"]!);

// Add services to the container.
var config = builder.Configuration;
builder.Services.ConfigureController();
builder.Services.ConfigureMongoSettings(config["MongoConnection:ConnString"]!, config["MongoConnection:Database"]!);
var conn = builder.Configuration["Redis:Connection"];
builder.Services.ConfigureHttpClient(config);
builder.Services.ConfigureRedis(config["Redis:Connection"]!)
    .ConfigureCacheRepository();
builder.Services.ConfigureMailJet(config["MailJet:ApiKey"]!, config["MailJet:ApiSecret"]!, 
    config["MailJet:Email"]!, "Location Marker API");
builder.Services.AddApiVersioning(opt =>
{
    opt.ReportApiVersions = true;
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.ConfigureServices();
builder.Services.ConfigureHangfireClient();
builder.Services.ConfigureHangfireServer();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization = [ new HangfireAuthorizationFilter(builder.Configuration) ],
});
await app.SeedSystemData(logger);
await app.SetConstants();
app.MapControllers();

app.Run();
