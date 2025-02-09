using Microsoft.Azure.ServiceBus;
using ServiceBusShared.Models;
using ServiceBusShared.Static;
using System.Text;
using System.Text.Json;

public class ServiceBusReceiverService : IServiceBusReceiverService
{
    private readonly IQueueClient _queueClient;
    private bool _handlerRegistered;

    public List<Product> Products { get; set; } = new List<Product>();

    public ServiceBusReceiverService()
    {
        _queueClient = new QueueClient(ServiceBusConfig.PrimaryConnectionString, ServiceBusConfig.Queue);
        _handlerRegistered = false;
    }

    public async Task StartReceivingMessagesAsync()
    {
        if (!_handlerRegistered)
        {
            _queueClient.RegisterMessageHandler(async (message, token) =>
            {
                var product = JsonSerializer.Deserialize<Product>(Encoding.UTF8.GetString(message.Body));
                if (product != null)
                {
                    Products.Add(product);
                }

                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
            },
            new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            });

            _handlerRegistered = true;
        }
    }

    private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
    {
        Console.WriteLine($"Message handler exception: {args.Exception}");
        return Task.CompletedTask;
    }
}
