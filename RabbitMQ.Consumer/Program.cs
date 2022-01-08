using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //GetFanout(channel,"Bangalore");
                    //GetFanout(channel,"newQueue");
                    //GetTopicQueue(channel, "topic.bombay.queue");
                    GetTopicQueue(channel, "newTopicQueue");
                    //GetDirectQueue(channel);
                    Console.ReadLine();
                }
            }
        }

        private static void GetFanout(IModel channel, string queue)
        {
            channel.BasicQos(0, 1, false);
            MessageReceiver messageReceiver = new MessageReceiver(channel);
            channel.BasicConsume(queue, false, messageReceiver);
        }

        private static void GetTopicQueue(IModel channel, string queue)
        {
            channel.BasicQos(0, 1, false);
            MessageReceiver messageReceiver = new MessageReceiver(channel);
            channel.BasicConsume(queue, false, messageReceiver);
        }

        private static void GetDirectQueue(IModel channel)
        {
            channel.QueueDeclare("testqueue", true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume("testqueue", true, consumer);
        }
    }
}
