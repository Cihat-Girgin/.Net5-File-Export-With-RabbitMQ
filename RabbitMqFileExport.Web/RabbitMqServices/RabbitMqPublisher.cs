using System.Text;
using System.Text.Json;
using RabbitMqFileExport.Model;

namespace RabbitMqFileExport.Web.RabbitMqServices
{
    public class RabbitMqPublisher
    {
        private readonly RabbitMqClientService _rabbitMQClientService;

        public RabbitMqPublisher(RabbitMqClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        public void Publish(FileMessage fileMessage)
        {
            var channel = _rabbitMQClientService.Connect();

            var bodyString = JsonSerializer.Serialize(fileMessage);

            var bodyByte = Encoding.UTF8.GetBytes(bodyString);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(RabbitMqClientService.ExchangeName,RabbitMqClientService.RoutingFile,false,properties,bodyByte);

        }
    }
}