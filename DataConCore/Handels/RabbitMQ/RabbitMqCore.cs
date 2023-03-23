using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace DataConCore.Handels.RabbitMQ
{
    public abstract class RabbitMqCore
    {
        private static readonly IDictionary<string, IConnection> _connectionCache = new ConcurrentDictionary<string, IConnection>();
        private readonly SemaphoreSlim _connectLock = new SemaphoreSlim(1, 1);
        public RabbitMqQueue RabbitMqueue { get; set; }

        public RabbitMqCore()
        {
        }

        /// <summary>
        /// 生产端 -- 每次使用释放对象
        /// </summary>
        /// <param name="mg">队列名称</param>
        /// <param name="action">执行动作</param>
        public virtual void AMQP(Action<IModel, RabbitMqQueue> action) { }

        /// <summary>
        /// 消费端--不释放对象
        /// </summary> 
        /// <param name="action">执行动作</param>
        public virtual void AMQPC(Action<IModel, RabbitMqQueue> action) { }

        /// <summary>
        /// 获取RabbitMq链接
        /// </summary>
        /// <param name="optionStr"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual IConnection Connect(string optionStr, string cacheN)
        {
            var options = GetOptions(optionStr);
            if (options == null || options.Connector == null)
                throw new ArgumentNullException("请先配置RabbitMq连接参数");

            return GetConnect(options, cacheN);
        }

        /// <summary>
        /// 根据缓存key清空缓存
        /// </summary>
        /// <param name="cacheN"></param>
        public void Disponse(string cacheN)
        {
            if (_connectionCache.ContainsKey(cacheN)) _connectionCache.Remove(cacheN);
        }

        /// <summary>
        /// 构建Model
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="mg"></param>
        /// <returns></returns>
        protected abstract IModel Build(IModel channel, RabbitMqQueue mg);

        #region 私有方法

        private IConnection GetConnect(RabbitMqOptions options, string cachaN)
        {
            _connectLock.Wait();
            try
            {
                if (_connectionCache.TryGetValue(cachaN, out IConnection connection))
                {
                    return connection;
                }

                connection = RabbitMqConnFactory.GetConnector(options);
                _connectionCache[cachaN] = connection;
                return connection;
            }
            finally
            {
                _connectLock.Release();
            }
        }

        private RabbitMqOptions GetOptions(string optionStr)
        {
            RabbitMqOptions options = new RabbitMqOptions();
            return JsonConvert.DeserializeObject<RabbitMqOptions>(optionStr);
            //return AppSettingsReader.GetInstance("Neter:RabbitMq", options);
        }
        #endregion
    }
}
