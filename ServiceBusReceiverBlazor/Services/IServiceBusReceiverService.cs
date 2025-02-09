using ServiceBusShared.Models;

public interface IServiceBusReceiverService
{
    List<Product> Products { get; set; }

    Task StartReceivingMessagesAsync();
}
