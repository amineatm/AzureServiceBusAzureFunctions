using Microsoft.Azure.ServiceBus;
using ServiceBusShared.Models;
using ServiceBusShared.Static;
using System.Text;
using System.Text.Json;

namespace ServiceBusReceiver
{
    class Program
    {
        static IQueueClient queueClient;

        static async Task Main(string[] args)
        {
            queueClient = new QueueClient(ServiceBusConfig.PrimaryConnectionString, ServiceBusConfig.Queue);

            queueClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var product = JsonSerializer.Deserialize<Product>(Encoding.UTF8.GetString(message.Body)) ?? new Product();
                    Console.WriteLine($"Received product: {product.Name} {product.Reference} {product.Price}$ {product.Description}");
                    await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                },
                new MessageHandlerOptions(args => { Console.WriteLine($"Error: {args.Exception}"); return Task.CompletedTask; })
                { MaxConcurrentCalls = 1, AutoComplete = false });

            Console.ReadLine();
            await queueClient.CloseAsync();
        }
    }

}