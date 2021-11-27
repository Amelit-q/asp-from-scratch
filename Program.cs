using System.Reflection;
using AspFromScratch.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appSettings.{builder.Environment.EnvironmentName}.json")
    .Build();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    var connectionString = configuration.GetConnectionString("PostgresSql");
    options.UseNpgsql(connectionString);
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    var projectDirectory = AppContext.BaseDirectory;

    var projectName = Assembly.GetExecutingAssembly().GetName().Name;
    var xmlFileName = $"{projectName}.xml";

    options.IncludeXmlComments(Path.Combine(projectDirectory, xmlFileName));
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();


app.Run();
