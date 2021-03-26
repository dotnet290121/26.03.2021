using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitConsumer260321
{
    class Program
    {
        static void HandleRequest(object model, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            //Console.WriteLine(" [x] Received {0}", message);

            Task.Run(() =>
            {
                Console.WriteLine(" Worker executing " + message);
            });
        }
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            factory.Port = 5677;
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "flight_center_requests",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += HandleRequest;

                    channel.BasicConsume(queue: "flight_center_requests",
                                         autoAck: true,
                                         consumer: consumer);
                    // Before Dispose!!!
                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }

        }
    }
}
