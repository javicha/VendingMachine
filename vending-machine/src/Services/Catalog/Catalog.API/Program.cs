using Application.Middleware;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Vending.API.Extensions;
using Vending.Application;
using Vending.Infrastructure;
using Vending.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //API information and description
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Vending Machine API",
        Description = "Insert coins and buy your product",
        Contact = new OpenApiContact
        {
            Name = "Javier Val",
            Url = new Uri("https://www.linkedin.com/in/javier-val-lista-ing-software/")
        }
    });

    //We configure Swagger to use the documentation XML file
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//Register all the Application and Infrastructure layers services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.PopulateDatabase<VendingContext>((context, services) =>
{
    var logger = services.GetService<ILogger<VendingContextSeed>>();
    VendingContextSeed.SeedAsync(context, logger).Wait();
});

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>(); //Global handling exceptions

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "VendingMachine API - v1");
});


app.UseAuthorization();

app.MapControllers();

app.Run();
