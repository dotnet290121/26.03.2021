using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitProducer260321
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "flight_center_requests",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                
                string message = $"{{ timestamp: '{DateTime.Now.ToString()}', resultqueue: '{Guid.NewGuid().ToString()}'"+
                    $", methodid: 1, params: {{}} }}";

                var body = Encoding.UTF8.GetBytes(message);
                for (int i = 0; i < 1000; i++)
                {
                    channel.BasicPublish(exchange: "",
                                         routingKey: "flight_center_requests",
                                         basicProperties: null,
                                         body: body);

                }
                Console.WriteLine(" [x] Sent {0}", message);
            }

            Console.WriteLine(" Press [enter] to exit.");

        }
    }
}
