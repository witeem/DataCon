using DataConGateway.Common;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using SqlSugar;

namespace DataConGateway.OcelotConfigExtends
{
    /// <summary>
    /// 使用mysql 实现配置文件仓储接口
    /// </summary>
    public class MysqlFileConfigurationRepository : IFileConfigurationRepository
    {
        private readonly OcelotConfigOptions _options;
        public MysqlFileConfigurationRepository(OcelotConfigOptions options)
        {
            _options = options;
        }

        public async Task<Response<FileConfiguration>> Get()
        {
            var sqlSugar = SqlSugarHandel.GetMySqlDb(_options.MySqlConnection);
            var file = new FileConfiguration();
            // 查询数据库配置信息
            var result = await sqlSugar.Queryable<OcelotEntity>().Where(m => m.IsDel == 0).FirstAsync();
            if (result != null)
            {
                var glb = new FileGlobalConfiguration();
                glb.BaseUrl = result.BaseUrl;
                glb.DownstreamScheme = result.DownstreamScheme;
                glb.RequestIdKey = result.RequestIdKey;
            }
            else 
            {
                throw new Exception("未监测到任何可用的配置信息");
            }
            // 将获取的结果赋值到 file

            return new OkResponse<FileConfiguration>(file);
        }

        /// <summary>
        /// 使用了数据库存储，可以不实现接口直接返回
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Response> Set(FileConfiguration file)
        {
            await Task.CompletedTask;
            return new OkResponse();
        }
    }
}
