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
        public DbSet<ExcelFile> ExcelFiles { get; set; }

    }
}
