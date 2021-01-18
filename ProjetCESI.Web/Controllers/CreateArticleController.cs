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
using System.Text.RegularExpressions;

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
            CreateRessourceViewModel model = PrepareModel<CreateRessourceViewModel>();

            ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRessourceViewModel model)
        {
            if (model.SelectedCategories == 0)
                ModelState.AddModelError("SelectedCategories", "Vous devez selectionner une catégorie");

            if (model.SelectedTypeRessources == 0)
                ModelState.AddModelError("SelectedTypeRessources", "Vous devez selectionner un type de ressource");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
                ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
                ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

                return View(model);
            }

            var ressource = new Ressource();

            if (model.SelectedTypeRessources == (int)TypeRessources.PDF)
            {
                if(model.File == null || model.File.ContentType != "application/pdf" || model.File.Length == 0)
                {
                    ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
                    ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
                    ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

                    ModelState.AddModelError("File", "Veuillez selectionner un fichier valide !");

                    return View(model);
                }

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
                if(string.IsNullOrEmpty(model.urlVideo))
                {
                    ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
                    ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
                    ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

                    ModelState.AddModelError("urlVideo", "Veuillez saisir une url");

                    return View(model);
                }

                if (Regex.IsMatch(model.urlVideo, @"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$")/* && model.urlVideo.Contains("youtube") == true*/)
                {
                    ressource.Contenu = "https://www.youtube.com/embed/" + model.urlVideo.Substring(model.urlVideo.IndexOf("v=") + 2, 11) + " ?rel=0";
                }
                else if(Regex.IsMatch(model.urlVideo, @"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:dailymotion\.com))(\/(?:[\w\-]+\/video//\/)?)([\w\-]+)(\S+)?$")/*model.urlVideo.Contains("dailymotion") == true*/)
                {
                    ressource.Contenu = "https://www.dailymotion.com/embed/video/" + model.urlVideo.Substring(model.urlVideo.IndexOf("video/") + 6, 7);
                }
                else
                {
                    ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
                    ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
                    ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

                    ModelState.AddModelError("urlVideo", "L'url saisie n'est pas valide");

                    return View(model);
                }
            }
            else
            {
                ressource.Contenu = model.Contenu;
            }

            ressource.Titre = model.Titre;
            ressource.EstValide = false;
            ressource.Commentaires = null;
            ressource.DateCreation = DateTimeOffset.Now;
            ressource.DateModification = DateTimeOffset.Now;
            ressource.NombreConsultation = 0;
            ressource.UtilisateurCreateurId = Utilisateur.Id;
            ressource.CategorieId = model.SelectedCategories;
            ressource.TypeRessourceId = model.SelectedTypeRessources;
            ressource.TypePartage = model.TypePartage;

            await MetierFactory.CreateRessourceMetier().SaveRessource(ressource);

            await MetierFactory.CreateTypeRelationRessourceMetier().AjouterRelationsToRessource(model.SelectedTypeRelation, ressource.Id);

            return RedirectToAction("Consultation", "Consultation");
        }
    }
}
