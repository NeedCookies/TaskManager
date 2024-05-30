using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class FileManagerController : Controller
    {
        private readonly ILogger<FileManagerController> _logger;

        public FileManagerController(ILogger<FileManagerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("download")]
        public IActionResult DownloadFile(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogInformation("User tried to download non-existed file");
                return NotFound("No such file!");
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return File(fileStream, fileName);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var filesFolder = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            
            if (!Directory.Exists(filesFolder))
            {
                Directory.CreateDirectory(filesFolder);
            }

            var filePath = Path.Combine(filesFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _logger.LogInformation($"Upload file {file.FileName}");
            return Ok("File uploaded");
        }
    }
}
