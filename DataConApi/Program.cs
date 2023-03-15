﻿using DataCon.IRepositories;
using DataConApplication;
using DataConCore;
using DataConCore.Handels.HandelDto;
using DataConCore.TableEntitys;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddCommandLine(args)
    .Build();

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStartupConfigServices(typeof(IAppServers));
builder.Services.AddStartupConfigServices(typeof(IAppCore));
builder.Services.AddRepositories();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.Configuration. ti(args);
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/healthz");
app.Run();

