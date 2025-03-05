using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OrderService.Service
{
    public class RabbitMQProducer
    {
      public async Task StartConsumerAsync()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "orderQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Received Order: {message}");

                // Burada ödəniş işlənəcək
            };

            channel.BasicConsume(queue: "orderQueue", autoAck: true, consumer: consumer);

            Console.ReadLine();
        }
    }
}