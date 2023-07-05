using Application;
using Application.Core;
using Application.DTOs;
using Application.Validations.Club;
using Application.Validations.Player;
using Domain;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);
builder.Services.AddScoped<IPlayersService, PlayersService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IValidator<CreatePlayerDTO>, CreatePlayerValidator>();
builder.Services.AddScoped<IValidator<EditPlayerDTO>, EditPlayerValidation>();
builder.Services.AddScoped<IValidator<CreateClubDTO>, CreateClubValidator>();
builder.Services.AddScoped<IValidator<EditClubDTO>, EditClubValidator>();

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
