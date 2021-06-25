using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShoutOut.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty] public string Message { get; set; }
        [BindProperty] public IFormFile Upload { get; set; }
        public string ImagePath { get; set; }
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnGet()
        {
            var fileAbsolutePath = Directory.EnumerateFiles(Path.Combine(_environment.ContentRootPath, "wwwroot\\uploads")).FirstOrDefault();
            var fileExtension = Path.GetExtension(fileAbsolutePath);
            var finalPath = "/uploads/image" + fileExtension;
            ImagePath = finalPath;
        }

        public IActionResult OnPost()
        {
            if (Upload != null)
            {
                var ext = Path.GetExtension(Upload.FileName);
                var file = Path.Combine(_environment.ContentRootPath, "wwwroot\\uploads", "image" + ext);
                _logger.LogInformation("Writing user-submitted file to " + file);
                var existingFiles = Directory.GetFiles(Path.Combine(_environment.ContentRootPath, "wwwroot\\uploads"));
                existingFiles.ToList().ForEach(x => { var theFile = new FileInfo(x); theFile.Delete(); });
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    Upload.CopyTo(fileStream);
                }
            }
            _logger.LogInformation("A user submitted " + Message);
            OnGet();
            return Page();
        }
    }
}
