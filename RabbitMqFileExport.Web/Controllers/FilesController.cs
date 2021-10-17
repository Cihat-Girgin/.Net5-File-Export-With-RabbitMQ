using Microsoft.AspNetCore.Mvc;

namespace RabbitMqFileExport.Web.Controllers
{
    public class FilesController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}