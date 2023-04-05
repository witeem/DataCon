using DataConGateway.Common;
using DataConGateway.OcelotConfigExtends;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;

namespace DataConGateway
{
    public static class StartupConfigServices
    {
        public static IOcelotBuilder AddCustomOcelot(this IOcelotBuilder builder, Action<OcelotConfigOptions> options)
        {

            builder.Services.Configure(options);

            //配置信息
            builder.Services.AddSingleton(
                resolver => resolver.GetRequiredService<IOptions<OcelotConfigOptions>>().Value);

            // 删除默认的 IFileConfigurationRepository
            builder.Services.RemoveAll<IFileConfigurationRepository>();

            //配置文件仓储注入
            // builder.Services.AddSingleton<IFileConfigurationRepository, MysqlFileConfigurationRepository>();
            builder.Services.AddSingleton<IFileConfigurationRepository, RedisFileConfigurationRepository>();

            //注册后端服务
            builder.Services.AddHostedService<DbConfigurationPoller>();
            return builder;
        }
    }
}
