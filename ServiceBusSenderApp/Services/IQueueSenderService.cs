
namespace ServiceBusSenderApp.Services
{
    public interface IQueueSenderService
    {
        Task SendMessageAsync<T>(T serviceBusMessage, string queueName);
    }
}