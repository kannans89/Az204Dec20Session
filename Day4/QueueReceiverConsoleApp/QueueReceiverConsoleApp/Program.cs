
using Azure.Messaging.ServiceBus;

class Program
{
    static string connectionString = "Endpoint=sb://day3servicebus.servicebus.windows.net/;SharedAccessKeyName=receiverPolicy;SharedAccessKey=vjvx/WRGpf3UxtH31JSj6qEBbXh9xQQnt+ASbAvDU3Q=;EntityPath=ordersq";

    static string queueName = "ordersq";
    static ServiceBusClient client = default!;
    static ServiceBusProcessor processor = default!;

    static async Task MessageHandler(ProcessMessageEventArgs args)
    {
        string body = args.Message.Body.ToString();
        Console.WriteLine($"Received: {body}");
        await args.CompleteMessageAsync(args.Message);
    }

    static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }

    static async Task Main()
    {
        client = new ServiceBusClient(connectionString);
        processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

        try
        {
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();
            Console.WriteLine("Wait for a minute and then press any key to end the processing");
            Console.ReadKey();

            Console.WriteLine("\nStopping the receiver...");
            await processor.StopProcessingAsync();
            Console.WriteLine("Stopped receiving messages");
        }
        finally
        {
            await processor.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}
