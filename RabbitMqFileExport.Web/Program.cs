using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMqFileExport.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMqFileExport.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            if (!context.Companies.Any())
            {
                List<Company> companies = new();

                Enumerable.Range(1, 500).ToList().ForEach(x =>
                {
                    companies.Add(new Company
                    {
                        Title = Faker.Company.Name(),
                        City = Faker.Address.City(),
                        Country = Faker.Address.Country(),
                        TaxNo = Faker.RandomNumber.Next(1000000000, 9999999999)
                    });
                });
                context.Companies.AddRange(companies);
                context.SaveChanges();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
