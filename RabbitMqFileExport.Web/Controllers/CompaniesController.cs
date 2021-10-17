using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RabbitMqFileExport.Web.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly DatabaseContext _context;

        public CompaniesController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(_context.Companies.ToListAsync());
        }
    }
}