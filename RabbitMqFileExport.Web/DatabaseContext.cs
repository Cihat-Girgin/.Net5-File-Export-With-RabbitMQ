using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RabbitMqFileExport.Model;

namespace RabbitMqFileExport.Web
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options):base(options)
        {
            
        }
        public DbSet<Company> Companies { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            List<Company> companies = new();
            
            Enumerable.Range(1,500).ToList().ForEach(x =>
            {
                companies.Add(new Company
                {
                    Id = x,
                    Title = Faker.Company.Name(),
                    City = Faker.Address.City(),
                    Country =   Faker.Address.Country(),
                    TaxNo = Faker.RandomNumber.Next(1000000000,9999999999)
                });
            });
            modelBuilder.Entity<Company>().HasData(companies);
        }
    }
}
