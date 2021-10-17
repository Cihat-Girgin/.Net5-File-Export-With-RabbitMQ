using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMqFileExport.Model;
using RabbitMqFileExport.Web.RabbitMqServices;

namespace RabbitMqFileExport.Web.Controllers
{
    public class FilesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly RabbitMqPublisher _publisher;

        public FilesController(DatabaseContext context, RabbitMqPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

     

        // GET
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExcelFiles.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(string fileName)
        {
            var excelFile = new ExcelFile {Name = $"{fileName}_{Guid.NewGuid().ToString().Substring(1, 15)}.xlsx", status = EFileStatus.Creating};
            await _context.ExcelFiles.AddAsync(excelFile);
            await _context.SaveChangesAsync();
            _publisher.Publish(new FileMessage{FileId = excelFile.Id});
            return RedirectToAction(nameof(Index));
        }
    }
}