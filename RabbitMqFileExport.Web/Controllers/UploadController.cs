using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RabbitMqFileExport.Model;
using RabbitMqFileExport.Web.Hubs;

namespace RabbitMqFileExport.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public UploadController(DatabaseContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int fileId)
        {
            if (file is not { Length: > 0 }) return BadRequest();


            var fileEntity = await _context.ExcelFiles.FindAsync(fileId);


            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files", fileEntity.Name);


            try
            {
                await using FileStream stream = new(path, FileMode.Create);

                await file.CopyToAsync(stream);


                fileEntity.status = EFileStatus.Created;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            await _hubContext.Clients.All.SendAsync("CompletedFile");
            return Ok();
        }
    }
}
