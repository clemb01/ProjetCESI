using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
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
using Newtonsoft.Json;


namespace ProjetCESI.Web.Controllers
{
    [Authorize]
    public class RessourceController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("Ressource/{id}")]
        public async Task<IActionResult> Ressource(int id)
        {
            var model = PrepareModel<RessourceViewModel>();

            var ressourceMetier = MetierFactory.CreateRessourceMetier();

            if(model.Ressource.TypeRessourceId == (int)TypeRessources.PDF)
            {
                string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"100%\" height=\"650px\">";
                embed += "Si vous ne pouvez pas visualiez le fichier, vous pouvez le télécharger <a href = \"{0}\">ici</a>";
                embed += " ou vous pouvez télécharger <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> pour visualiser le PDF.";
                embed += "</object>";

                model.Ressource.Contenu = string.Format(embed, @"/uploads/attestation.pdf");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AjouterFavoris(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().AjouterFavoris(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public async Task<IActionResult> SupprimerFavoris(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().SupprimerFavoris(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public async Task<IActionResult> MettreDeCote(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().MettreDeCote(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public async Task<IActionResult> DeMettreDeCote(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().DeMettreDeCote(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public async Task<IActionResult> EstExploite(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().EstExploite(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public async Task<IActionResult> PasExploite(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().PasExploite(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
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
