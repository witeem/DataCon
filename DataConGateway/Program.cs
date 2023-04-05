using CSRedis;
using DataConGateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Ocelot.Configuration.File;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
#region 初始化Redis
RedisHelper.Initialization(new CSRedisClient(builder.Configuration["CsRedis"]));
#endregion

// Ocelot 配置信息从数据库读取
/*builder.Services.AddOcelot().AddCustomOcelot(o => 
{ 
    o.MySqlConnection = builder.Configuration["MySqlConnection"];
    o.EnableTimer = true;
    o.TimerDelay = 30 * 1000;
}).AddConsul();*/

// Ocelot 配置信息从缓存读取
builder.Services.AddOcelot().AddCustomOcelot(o => 
{
    o.GlobalConfiguration = builder.Configuration.GetSection("GlobalConfiguration").Get<FileGlobalConfiguration>();
    o.EnableTimer = false;
    o.TimerDelay = 10 * 1000;
}).AddConsul();

// builder.Services.AddOcelot().AddConsul();
var app = builder.Build();

app.UseOcelot().Wait();
app.Run();