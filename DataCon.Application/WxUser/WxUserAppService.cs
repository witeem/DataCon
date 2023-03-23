using DataCon.Application;
using DataCon.IApplication;
using DataCon.IRepositories.WxUser;
using DataConCore.Handels.RabbitMQ;
using DataConCore.TableEntitys;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace DataConApplication;

public class WxUserAppService : BaseApplication, IWxUserAppService
{
    private readonly ILogger<WxUserAppService> _logger;
    private readonly IWxUserRepositories _wxUserRepositories;
    private readonly IGoodsAppService _goodsApp;
    private readonly IRabbitMqProvider _mqProvider;

    public WxUserAppService(ILogger<WxUserAppService> logger,
        IWxUserRepositories wxUserRepositories,
        IGoodsAppService goodsApp,
        IRabbitMqProvider mqProvider)
    {
        _logger = logger;
        _wxUserRepositories = wxUserRepositories;
        _goodsApp = goodsApp;
        _mqProvider = mqProvider;
    }

    public async Task<List<UserEntity>> AppServiceTest()
    {
        _logger.LogInformation("Application服务");
        var query = _wxUserRepositories.QueryList(m => m.IsDel == 0);
        var list = await query.ToListAsync();
        return list;

        /*
        await Task.CompletedTask;
        return new List<UserEntity>();*/
    }

    public async Task<object> MyTestAsync()
    {
        _logger.LogInformation(nameof(MyTestAsync));
        var list = new { Code = 1, Name = nameof(MyTestAsync) };
        await _goodsApp.GetGoods(default);
        return list;
    }

    public string SendMqAsync()
    {
        var mqProducer = _mqProvider.RegisterProducer(queue => { queue.Name = "queue2"; });
        mqProducer.BasicPublish("queueMsg", false);
        return "success";
    }

    public string ExpendMqAsync()
    {
        var mqConsume = _mqProvider.RegisterConsumer(queue => { queue.Name = "queue2"; }, "consumer1");
        mqConsume.AMQPC((channel, msg) =>
        {
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            channel.BasicConsume(msg.Name, false, consumer);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine(" Worker Queue Received => {0}", message);
                channel.BasicAck(ea.DeliveryTag, false);
            };
        });

        return "success";
    }

    public string CloseExpendMqAsync()
    {
        var mqConsume = _mqProvider.RegisterConsumer(queue => { queue.Name = "queue2"; }, "consumer1");
        mqConsume.Disponse();
        return "success";
    }
}
