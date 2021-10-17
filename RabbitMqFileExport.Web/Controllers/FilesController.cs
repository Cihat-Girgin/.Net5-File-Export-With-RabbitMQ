using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMqFileExport.Model;

namespace RabbitMqFileExport.Web.Controllers
{
    public class FilesController : Controller
    {
        private readonly DatabaseContext _context;

        public FilesController(DatabaseContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExcelFiles.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(string fileName)
        {
            await _context.ExcelFiles.AddAsync(new ExcelFile {Name = $"{fileName}_{Guid.NewGuid().ToString().Substring(1, 15)}.xlsx"});
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}