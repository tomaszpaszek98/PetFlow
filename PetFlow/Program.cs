using Application;
using Microsoft.AspNetCore.Mvc;
using PetFlow.ExceptionHandlers;
using PetFlow.Infrastructure;
using PetFlow.Persistence;

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
});
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