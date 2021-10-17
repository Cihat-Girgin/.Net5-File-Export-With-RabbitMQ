using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqFileExport.Model;
using RabbitMqFileExport.WorkerService.RabbitMqServices;

namespace RabbitMqFileExport.WorkerService
{
    public class Worker : BackgroundService
    {
         private readonly ILogger<Worker> _logger;

        private readonly RabbitMqClientService _rabbitMQClientService;

        private readonly IServiceProvider _serviceProvider;

        private IModel _channel;
        public Worker(ILogger<Worker> logger,RabbitMqClientService rabbitMQClientService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _rabbitMQClientService = rabbitMQClientService;
            _serviceProvider = serviceProvider;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

         _channel=_rabbitMQClientService.Connect();
            _channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }

        protected override  Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var consumer = new AsyncEventingBasicConsumer(_channel);

            _channel.BasicConsume(RabbitMqClientService.QueueName, false, consumer);
          
            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            await Task.Delay(5000);


            var createExcelMessage = JsonSerializer.Deserialize<FileMessage>(Encoding.UTF8.GetString(@event.Body.ToArray()));


           using var ms = new MemoryStream();


            var wb = new XLWorkbook();
            var ds = new DataSet();
            ds.Tables.Add(GetTable("companies"));


            wb.Worksheets.Add(ds);
            wb.SaveAs(ms);

            MultipartFormDataContent multipartFormDataContent = new();

            multipartFormDataContent.Add(new ByteArrayContent(ms.ToArray()), "file", Guid.NewGuid() + ".xlsx");

            var baseUrl = "https://localhost:5001/api/upload";

            using (var httpClient=  new HttpClient())
            {

                var response = await httpClient.PostAsync($"{baseUrl}?fileId={createExcelMessage.FileId}", multipartFormDataContent);

                if(response.IsSuccessStatusCode)
                {

                    _logger.LogInformation($"File ( Id : {createExcelMessage.FileId}) was created by successful");
                    _channel.BasicAck(@event.DeliveryTag, false);
                }
            }

        }

        private DataTable GetTable(string tableName)
        {
            List<Company> companies;

            using (var scope= _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                companies = context.Companies.ToList();
            }

            DataTable table = new DataTable { TableName = tableName };

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Title", typeof(string));
            table.Columns.Add("City", typeof(string));
            table.Columns.Add("Country", typeof(string));
            table.Columns.Add("TaxNo", typeof(string));

            companies.ForEach(x =>
            {
                table.Rows.Add(x.Id, x.Title, x.City, x.Country,x.TaxNo);

            });

            return table;


        }
    }
}