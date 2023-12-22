﻿using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using TopicSenderConsoleApp;


string connectionString = "Endpoint=sb://day3servicebus.servicebus.windows.net/;SharedAccessKeyName=senderPolicy;SharedAccessKey=Wu238+FBXl4lMnoJCdTRPUhrmsFAfaZGy+ASbM2o5rg=;EntityPath=stocks";
string topicName = "stocks";

List<Order> orders = new List<Order>()
{
        new Order(){OrderID="01",Quantity=100,UnitPrice=9.99F},
        new Order(){OrderID="02",Quantity=200,UnitPrice=10.99F},
        new Order(){OrderID="03",Quantity=300,UnitPrice=8.99F}

};

await SendMessage(orders);
async Task SendMessage(List<Order> orders)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(topicName);

    ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    int messageId = 1;
    foreach (Order order in orders)
    {

        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(order));
        serviceBusMessage.ContentType = "application/json";
        serviceBusMessage.MessageId = messageId.ToString();
        messageId++;

        if (!serviceBusMessageBatch.TryAddMessage(
            serviceBusMessage))
        {
            throw new Exception("Error occured");
        }

    }
    Console.WriteLine("Sending messages");
    await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);

    await serviceBusSender.DisposeAsync();
    await serviceBusClient.DisposeAsync();

}
