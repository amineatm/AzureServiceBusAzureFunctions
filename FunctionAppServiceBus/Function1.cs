using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.ServiceBus;
using ServiceBusShared.Models;
using ServiceBusShared.Static;
using System.Text;
using System.Text.Json;

namespace FunctionAppServiceBus
{
    public class Function1
    {
        [Function("Function1")]
        public async Task Run([TimerTrigger("*/3 * * * * *")] TimerInfo myTimer)
        {
            Console.WriteLine($"Timer trigger executed at: {DateTime.Now}");

            var productMessage = new Product
            {
                Name = "Sample Product",
                Price = 10000,
                Reference = "ABC123",
                Description = "This is a sample product"
            };

            var messageBody = JsonSerializer.Serialize(productMessage);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            var connectionString = ServiceBusConfig.PrimaryConnectionString;
            var queueName = ServiceBusConfig.Queue;

            var queueClient = new QueueClient(connectionString, queueName);

            try
            {
                await queueClient.SendAsync(message);
                Console.WriteLine($"Sent message: {productMessage.Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
            finally
            {
                // Close the connection manually
                await queueClient.CloseAsync();
            }
        }
    }
}
