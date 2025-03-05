using RabbitMQ.Client;
using System.Text;

namespace OrderService.Service
{
    public class RabbitMQProducer
    {
        public  void SendOrderCreatedMessageAsync(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            // Asinxron olaraq bağlantı qurulmasını gözləyirik
             var connection =  factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Queye parametrlərini müəyyən edirik
            channel.QueueDeclare(queue: "orderQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            // Mesaj göndəririk
            channel.BasicPublish(exchange: "", routingKey: "orderQueue", basicProperties: null, body: body);
        }
    }
}