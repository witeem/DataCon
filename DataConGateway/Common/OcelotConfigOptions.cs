using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocelot.Configuration.File;

namespace DataConGateway.Common
{
    public class OcelotConfigOptions
    {
        public string MySqlConnection { get; set; }

        public FileGlobalConfiguration GlobalConfiguration { get; set; }

        /// <summary>
        /// 是否启用定时器， 默认不启动
        /// </summary>
        public bool EnableTimer { get; set; } = false;

        /// <summary>
        /// 定时器周期，单位（ms），默认10分钟自动更新一次
        /// </summary>
        public int TimerDelay { get; set; } = 10 * 60 * 1000;
    }
}
