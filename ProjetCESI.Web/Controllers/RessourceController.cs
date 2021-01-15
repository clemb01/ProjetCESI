using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class RessourceController : BaseController
    {
        [HttpGet]
        [Route("Ressource/{id}")]
        public async Task<IActionResult> Ressource(int id = 0)
        {
            var model = PrepareModel<RessourceViewModel>();

            model.Ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadTest(IFormFile file)
        {
            var uploads = Path.Combine(HostingEnvironnement.WebRootPath, "uploads");
            bool exists = Directory.Exists(uploads);
            if (!exists)
                Directory.CreateDirectory(uploads);

            var fileName = Path.GetFileName(file.FileName);
            string mimeType = file.ContentType;

            byte[] fileData = null;
            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
            {
                fileData = new byte[file.Length];
                file.OpenReadStream().Read(fileData, 0, (int)file.Length);
                fileStream.Write(fileData);
                fileStream.Close();
            }

            BlobClient objBlobService = new BlobClient(Configuration.GetConnectionString("BlobStorage"), "stockage", fileName);

            var test = await objBlobService.UploadAsync(Path.Combine(uploads, file.FileName));

            return RedirectToAction("Accueil", "Accueil");
        }
    }
}
