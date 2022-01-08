using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.Producer
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
                    //SetFanoutExchange(channel);
                    //SetFanoutExchange2(channel);
                    SetTopicExchange2(channel);
                    //SetTopicExchange(channel);
                    //SetDirectExchange(channel);
                }
            }
        }

        private static void SetFanoutExchange(IModel channel)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = false;
            byte[] messagebuffer = Encoding.Default.GetBytes("Message is of fanout Exchange type");
            channel.BasicPublish("fanout.exchange", "", properties, messagebuffer);
            Console.WriteLine("Message Sent From : fanout.exchange");
            Console.WriteLine("Routing Key :  Routing key is not required for fanout exchange");
            Console.WriteLine("Message Sent");
        }
        private static void SetFanoutExchange2(IModel channel)
        {
            channel.ExchangeDeclare("newFanout", ExchangeType.Fanout, false, false, null);
            channel.QueueDeclare("newQueue", true, false, false, null);
            channel.QueueBind("newQueue", "newFanout", "", null);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = false;
            byte[] messagebuffer = Encoding.Default.GetBytes("Message is of fanout Exchange type");
            channel.BasicPublish("newFanout", "", properties, messagebuffer);
            Console.WriteLine("Message Sent From : newFanout");
            Console.WriteLine("Routing Key :  Routing key is not required for fanout exchange");
            Console.WriteLine("Message Sent");
        }


        private static void SetTopicExchange(IModel channel)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = false;
            byte[] messagebuffer = Encoding.Default.GetBytes("Message from Topic Exchange 'Bombay' ");
            channel.BasicPublish("topic.exchange", "Message.Bombay.Email", properties, messagebuffer);
            Console.WriteLine("Message Sent From: topic.exchange ");
            Console.WriteLine("Routing Key: Message.Bombay.Email");
            Console.WriteLine("Message Sent");
        }
        private static void SetTopicExchange2(IModel channel)
        {
            channel.ExchangeDeclare("newTopic", ExchangeType.Topic, false, false,null);
            //channel.ExchangeBind("newTopic", "newTopic", "*.yasin.*", null); //exchange'e bind eklenmek istenirse
            channel.QueueDeclare("newTopicQueue", true, false, false, null);
            channel.QueueBind("newTopicQueue", "newTopic", "*.yasin.*", null);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = false;
            byte[] messagebuffer = Encoding.Default.GetBytes("Message from Topic Exchange 'yasin' ");
            channel.BasicPublish("newTopic", "deneme.yasin.deneme12121", properties, messagebuffer);
            Console.WriteLine("Message Sent From: newTopic ");
            Console.WriteLine("Routing Key: deneme.yasin.deneme12121");
            Console.WriteLine("Message Sent");
        }

        private static void SetDirectExchange(IModel channel)
        {
            channel.QueueDeclare("testqueue", true, false, false, null);
            var message = new { Name = "Procuder", Message = "Hello" };
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish("", "testqueue", null, body);
        }
    }
}
