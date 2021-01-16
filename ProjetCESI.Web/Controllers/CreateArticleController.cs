using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using ProjetCESI.Metier;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ProjetCESI.Web.Controllers
{
    public class CreateArticleController : BaseController
    {
        private SelectList ToSelectList<T>(List<T> liste)
        {
            var selectList = new SelectList(liste, "Id", "Nom");

            return selectList;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(CreateRessourceViewModel model)
        {
            var ressource = new Ressource();

            if (model.SelectedTypeRessources == (int)TypeRessources.PDF)
            {
                var uploads = Path.Combine(HostingEnvironnement.WebRootPath, "uploads");
                bool exists = Directory.Exists(uploads);
                if (!exists)
                    Directory.CreateDirectory(uploads);

                var fileName = Path.GetFileName(model.File.FileName);
                string mimeType = model.File.ContentType;

                byte[] fileData = null;
                using (var fileStream = new FileStream(Path.Combine(uploads, model.File.FileName), FileMode.Create))
                {
                    fileData = new byte[model.File.Length];
                    model.File.OpenReadStream().Read(fileData, 0, (int)model.File.Length);
                    fileStream.Write(fileData);
                    fileStream.Close();
                }

                string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"100%\" height=\"650px\">";
                embed += "Si vous ne pouvez pas visualiez le fichier, vous pouvez le télécharger <a href = \"{0}\">ici</a>";
                embed += " ou vous pouvez télécharger <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> pour visualiser le PDF.";
                embed += "</object>";

                ressource.Contenu = string.Format(embed, @"/uploads/" + model.File.FileName);
            }
            else if(model.SelectedTypeRessources == (int)TypeRessources.Video)
            {
                if(model.urlVideo.Contains("youtube") == true)
                {
                    ressource.Contenu = "https://www.youtube.com/embed/" + model.urlVideo.Substring(model.urlVideo.IndexOf("v=") + 2, 11) + " ? rel = 0";
                }
                else if(model.urlVideo.Contains("dailymotion") == true)
                {
                    ressource.Contenu = "https://www.dailymotion.com/embed/video/" + model.urlVideo.Substring(model.urlVideo.IndexOf("video/") + 6, 7);
                }
                


            }
            else
            {
                ressource.Contenu = model.Contenu;
            }

            ressource.Titre = model.Titre;
            ressource.EstValide = false;
            //ressource.CategorieId = model.Ressource.Categorie.Id;
            ressource.Commentaires = null;
            ressource.DateCreation = DateTimeOffset.Now;
            ressource.DateModification = DateTimeOffset.Now;
            ressource.NombreConsultation = 0;
            //ressource.UtilisateurCreateur = Utilisateur;
            ressource.UtilisateurCreateurId = Utilisateur.Id;
            ressource.CategorieId = model.SelectedCategories;
            ressource.TypeRessourceId = model.SelectedTypeRessources;

            await MetierFactory.CreateRessourceMetier().SaveRessource(ressource);

            await MetierFactory.CreateTypeRelationRessourceMetier().AjouterRelationsToRessource(model.SelectedTypeRelation, ressource.Id);

            return RedirectToAction("Consultation", "Consultation");
        }
    }
}
