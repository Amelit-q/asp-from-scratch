using System.Reflection;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);


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
