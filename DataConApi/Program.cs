using DataCon.Application;
using DataCon.IApplication;
using DataConCore;

var builder = WebApplication.CreateBuilder(args);
new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddCommandLine(args)
    .Build();

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.MqProducerRegist(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStartupConfigServices(new List<Type> { typeof(BaseApplication), typeof(IAppCore) });
builder.Services.AddRepositories();
builder.Services.AddLazyResolution();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.Configuration.ConsulRegist(args);
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/healthz");
app.Run();

