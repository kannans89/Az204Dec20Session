using Azure.Messaging.ServiceBus;

const string connectionString = "Endpoint=sb://day3servicebus.servicebus.windows.net/;SharedAccessKeyName=senderPolicy;SharedAccessKey=4Wqi72RdTwfDdVra7qy8WsZZtMSAQdb5b+ASbKsZkeU=;EntityPath=ordersq";
const string queueName = "ordersq";
ServiceBusClient client = default!;
ServiceBusSender sender = default!;
const int numOfMessages = 3;


client = new ServiceBusClient(connectionString);
sender = client.CreateSender(queueName);

using ServiceBusMessageBatch messageBatch = await 
    sender.CreateMessageBatchAsync();

for (int i = 1; i <= numOfMessages; i++)
{
    if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Message {i}")))
    {
        throw new Exception($"The message {i} is too large to fit in the batch.");
    }
}

try
{
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
}
finally
{
    await sender.DisposeAsync();
    await client.DisposeAsync();
}