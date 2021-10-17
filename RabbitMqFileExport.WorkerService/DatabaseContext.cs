using Microsoft.EntityFrameworkCore;
using RabbitMqFileExport.Model;

namespace RabbitMqFileExport.WorkerService
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