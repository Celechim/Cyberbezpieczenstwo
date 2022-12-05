using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Cyberbezpieczenstwo.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Cyberbezpieczenstwo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<FileController> logger;

        public FileController(IWebHostEnvironment env,
            ILogger<FileController> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        /*[HttpPost]
        public async Task<ActionResult<IList<UploadResult>>> PostFile(
            [FromForm] IEnumerable<IFormFile> files)
        {
            string? path;
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
            List<UploadResult> uploadResults = new();


            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay =
                    WebUtility.HtmlEncode(untrustedFileName);

                path = Path.Combine(Directory.GetCurrentDirectory(),
                    "Development", "unsafe_uploads", file.FileName);

                await using FileStream fs = new(path, FileMode.Create);
                await file.CopyToAsync(fs);

                uploadResult.Uploaded = true;
                uploadResult.StoredFileName = file.Name;
            }
            return new CreatedResult(resourcePath, uploadResults);

        }*/
        //[Route("Pdf/{name}")]
        [HttpGet("/Download/{name}")]
        public ActionResult GetPDF(string name)
        {
            var randomBinaryData = new byte[50 * 1024];
            var fileStream = new MemoryStream(randomBinaryData);

            using var streamRef = new DotNetStreamReference(stream: fileStream);
            var stream = new MemoryStream();
            return File(stream, "application/pdf", name);
        }

        [HttpGet("[controller]/{name}")]
        public ActionResult GetFile(string name)
        {
            //string physicalPath = path;
            byte[] pdfBytes = System.IO.File.ReadAllBytes(new String(Directory.GetCurrentDirectory()+"/Development/unsafe_uploads/"+name));
            MemoryStream ms = new MemoryStream(pdfBytes);
            return new FileStreamResult(ms, "application/pdf");
        }
        
    }
}
