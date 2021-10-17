using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RabbitMqFileExport.Model;

namespace RabbitMqFileExport.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UploadController(DatabaseContext context)
        {
            _context = context;
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
            
            //SignalR notification oluşturulacak
//            await _hubContext.Clients.User(userFile.UserId).SendAsync("CompletedFile");



            return Ok();
        }
    }
}
