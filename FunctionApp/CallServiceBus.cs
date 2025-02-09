using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ServiceBusShared.Models;
using ServiceBusShared.Static;
using System.Text;
using System.Text.Json;

public static class CallServiceBus
{
    [FunctionName("CallServiceBus")]
    public static async Task Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer, ILogger log)
    {
        log.LogInformation($"Timer trigger executed at: {DateTime.Now}");

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
            log.LogInformation($"Sent message: {productMessage.Name}");
        }
        catch (Exception ex)
        {
            log.LogError($"Error sending message: {ex.Message}");
        }
        finally
        {
            await queueClient.CloseAsync();
        }
    }
}
