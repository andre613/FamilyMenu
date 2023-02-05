using FamilyPlanner.Common.Entities;
using FamilyPlanner.api.Repositories;
using FamilyPlanner.api.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextFactory<FamilyPlannerDataContext>(
    options =>
    {
        var connectionStringBuilder = new DbConnectionStringBuilder();
        var familyPlannerDataConnectionString = builder.Configuration.GetConnectionString("FamilyPlannerData");

        if (string.IsNullOrWhiteSpace(familyPlannerDataConnectionString))
        {
            throw new ArgumentNullException(nameof(familyPlannerDataConnectionString), $"Family Planner Data connection string is missing. Check configuration.");
        }

        try
        {
            connectionStringBuilder.ConnectionString = familyPlannerDataConnectionString;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException($"\"{familyPlannerDataConnectionString}\" Is not a valid connection string. Check configuration.", ex);
        }

        options
            .UseMySql(
                connectionStringBuilder.ToString(),
                new MySqlServerVersion(new Version(8, 0, 29)))

            // The following 3 options help with debugging, but should
            // be changed or removed for production.
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    });

builder.Services.AddTransient<IRepository<Meal>, BaseRepository<Meal>>();

var app = builder.Build();

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
