using Microsoft.Azure.ServiceBus;
using ServiceBusShared.Static;
using System.Text;
using System.Text.Json;

namespace ServiceBusSenderApp.Services
{
    public class QueueSenderService : IQueueSenderService
    {
        public QueueSenderService() { }

        public async Task SendMessageAsync<T>(T serviceBusMessage, string queueName)
        {
            try
            {
                QueueClient queueClient = new QueueClient(ServiceBusConfig.PrimaryConnectionString, queueName);
                string messageBody = JsonSerializer.Serialize(serviceBusMessage);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                await queueClient.SendAsync(message);
                await queueClient.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
                throw;
            }
        }
    }
}
