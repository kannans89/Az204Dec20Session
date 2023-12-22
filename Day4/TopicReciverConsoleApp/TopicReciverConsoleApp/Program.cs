using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

//local
using TopicReciverConsoleApp;



string connectionString = "Endpoint=sb://day3servicebus.servicebus.windows.net/;SharedAccessKeyName=receiverPolicy;SharedAccessKey=+01EMy3edFOgd2JtCMTbhsL9pyaFzSjeZ+ASbBsou6g=;EntityPath=stocks";
string topicName = "stocks";
string subscriptionName = "ConsumerB";//change to ConsumerA,ConsumerB,ConsumerC
await ReceiveMessages();

async Task ReceiveMessages()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(topicName, subscriptionName,
        new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

    IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver.ReceiveMessagesAsync();

    await foreach (ServiceBusReceivedMessage message in messages)
    {

        Order order = JsonConvert.DeserializeObject<Order>(message.Body.ToString());
        Console.WriteLine("Order Id {0}", order.OrderID);
        Console.WriteLine("Quantity {0}", order.Quantity);
        Console.WriteLine("Unit Price {0}", order.UnitPrice);
        Console.WriteLine();
        //await Console.Out.WriteLineAsync();

    }
}

