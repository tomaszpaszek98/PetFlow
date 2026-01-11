using Application;
using Microsoft.AspNetCore.Mvc;
using PetFlow.ExceptionHandlers;
using PetFlow.Filters;
using PetFlow.Infrastructure;
using PetFlow.Persistence;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddExceptionHandlers();
builder.Services.AddProblemDetails();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ConsumesAttribute("application/json"));
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new AcceptHeaderValidationFilter());
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
});
builder.Services.AddInvalidJsonFieldsValidation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin()); //TODO to change!
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
//app.UseCors("AllowAll");
app.UseExceptionHandler();
app.MapControllers();

app.Run();

public partial class Program { }