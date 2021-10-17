using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMqFileExport.WorkerService.RabbitMqServices;

namespace RabbitMqFileExport.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var Configuration = hostContext.Configuration;

                    services.AddDbContext<DatabaseContext>(options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
                    });


                    services.AddSingleton<RabbitMqClientService>();
                    services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(Configuration.GetSection("RabbitMQ").Value), DispatchConsumersAsync = true });
                    services.AddHostedService<Worker>();
                });
    }
}