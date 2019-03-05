using System;
using System.Text;
using RabbitMQ.Client;

namespace AMQP_PublisherQueue_WorkQueue
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "myQueue",
                                            durable: false,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: null);
                    Console.WriteLine("please enter the first message to send");
                    string message;
                    do
                    {
                        message = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(message);

                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;

                        channel.BasicPublish(exchange: "",
                                                routingKey: "myQueue",
                                                basicProperties: properties,
                                                body: body);

                        Console.WriteLine("[x] message sent: {0}", message);
                        Console.WriteLine("Enter another message...");
                    } while (!message.Contains("END"));

                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "Hello World!");
        }
    }
}
