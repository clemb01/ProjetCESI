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
using ProjetCESI.Web.Outils;
using Microsoft.AspNetCore.Authorization;
using ProjetCESI.Metier.Outils;

namespace ProjetCESI.Web.Controllers
{
    [Authorize]
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

            model.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            model.TypeRelations = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            model.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            ViewBag.Preview = null;

            model.RessourceId = await MetierFactory.CreateRessourceMetier().InitNewRessource(UserId.GetValueOrDefault());
            
            return View("CreateOrUpdate", model);
        }

        [HttpPost]
        [StatistiqueFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdateRessource(CreateRessourceViewModel model)
        {
            PrepareModel(model);

            model.Statut = Statut.EnPause;

            await UpdateRessource(model);

            return View("CreateOrUpdate", model);
        }

        [HttpGet]
        [Route("CreateArticle/Edit/{ressourceId}")]
        public async Task<IActionResult> Edit(int ressourceId)
        {
            CreateRessourceViewModel model = PrepareModel<CreateRessourceViewModel>();

            var ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(ressourceId);

            if(ressource != null)
            {
                List<string> roles = new List<string>() { Enum.GetName(TypeUtilisateur.Admin), Enum.GetName(TypeUtilisateur.SuperAdmin) };

                if (ressource.UtilisateurCreateurId != UserId || !UtilisateurRoles.Any(c => roles.Contains(c)))
                {
                    return RedirectToAction("Accueil", "Accueil");
                }

                model.Titre = ressource.Titre;
                model.SelectedCategories = ressource.CategorieId ?? 0;
                model.SelectedTypeRessources = ressource.TypeRessourceId ?? 0;
                model.SelectedTypeRelation = ressource.TypeRelationsRessources.Select(c => c.TypeRelationId).ToList();
                model.TypePartage = ressource.TypePartage;
                model.Statut = ressource.Statut;
                model.IsEdit = true;
                                
                if(ressource.TypeRessource != null)
                {
                    if (ressource.TypeRessource.Id == (int)TypeRessources.Video)
                    {
                        model.urlVideo = ressource.ContenuOriginal;
                    }
                    else if (ressource.TypeRessource.Id != (int)TypeRessources.PDF)
                    {
                        model.Contenu = ressource.Contenu;
                    }
                    else
                    {   
                        model.NomPdf = ressource.ContenuOriginal;
                    }
                }

                model.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
                model.TypeRelations = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
                model.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());
            }
            else
            {
                return RedirectToAction("Accueil", "Accueil");
            }

            return View("CreateOrUpdate", model);
        }

        [HttpPost]
        [StatistiqueFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinirPlusTard(CreateRessourceViewModel model)
        {
            PrepareModel(model);

            model.Statut = Statut.EnPause;

            await UpdateRessource(model, true);

            return RedirectToAction("TableauDeBord", "TableauDeBord", new { nomVue = "crees" });
        }

        [HttpPost]
        [StatistiqueFilter]
        [ValidateAntiForgeryToken]
        [Route("CreateArticle/Edit/{ressourceId}")]
        public async Task<IActionResult> Edit(CreateRessourceViewModel model)
        {
            PrepareModel(model);

            model.Statut = Statut.EnPause;

            await UpdateRessource(model);

            return View("CreateOrUpdate", model);
        }

        [HttpPost]
        [StatistiqueFilter]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinaliserRessource(int ressourceId)
        {
            var ressourceMetier = MetierFactory.CreateRessourceMetier();

            Ressource ressource = await ressourceMetier.GetById(ressourceId);

            if(User.IsInRole(Enum.GetName(TypeUtilisateur.Admin)) || User.IsInRole(Enum.GetName(TypeUtilisateur.SuperAdmin)))
                ressource.Statut = Statut.Accepter;
            else
                ressource.Statut = Statut.AttenteValidation;

            await ressourceMetier.InsertOrUpdate(ressource);

            return RedirectToAction("Ressource", "Ressource", new { area = "", id = ressourceId });
        }

        async Task UpdateRessource(CreateRessourceViewModel model, bool ignoreModelState = false)
        {
            model.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            model.TypeRelations = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            model.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            if (model.SelectedCategories == 0)
                ModelState.AddModelError("SelectedCategories", "Vous devez selectionner une catégorie");

            if (model.SelectedTypeRessources == 0)
                ModelState.AddModelError("SelectedTypeRessources", "Vous devez selectionner un type de ressource");

            Ressource ressource;
            string content = string.Empty;
            string originalContent = null;

            if (ModelState.IsValid || ignoreModelState)
            {                
                if (model.SelectedTypeRessources == (int)TypeRessources.PDF)
                {
                    if (!model.IsEdit && (model.File == null || model.File.ContentType != "application/pdf" || model.File.Length == 0))
                    {
                        ModelState.AddModelError("File", "Veuillez selectionner un fichier valide !");
                    }
                    else
                    {
                        if(!model.IsEdit || (model.File != null && model.File.ContentType == "application/pdf" && model.File.Length > 0))
                        {
                            var uploads = Path.Combine(HostingEnvironnement.WebRootPath, "uploads");
                            bool exists = Directory.Exists(uploads);
                            if (!exists)
                                Directory.CreateDirectory(uploads);

                            var fileName = Path.GetFileName(model.File.FileName);
                            string mimeType = model.File.ContentType;

                            byte[] fileData = null;
                            fileData = new byte[model.File.Length];
                            model.File.OpenReadStream().Read(fileData, 0, (int)model.File.Length);

                            var blobFile = BlobStorage.UploadFileToBlob(fileName, fileData, mimeType);
                            await BlobStorage.GetBlobData(blobFile.Substring(blobFile.LastIndexOf("stockage/") + 9), Path.Combine(uploads, blobFile.Substring(blobFile.LastIndexOf("/") + 1)));

                            originalContent = $"{model.File.FileName}||{blobFile}";
                        }
                        else
                        {
                            originalContent = model.NomPdf;
                        }

                        string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"100%\" height=\"650px\">";
                        embed += "Si vous ne pouvez pas visualiez le fichier, vous pouvez le télécharger <a href = \"{0}\">ici</a>";
                        embed += " ou vous pouvez télécharger <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> pour visualiser le PDF.";
                        embed += "</object>";

                        content = string.Format(embed, @"/uploads/" + originalContent.Split("||").Last()[(originalContent.Split("||").Last().LastIndexOf("/") + 1)..]);
                    }
                }
                else if (model.SelectedTypeRessources == (int)TypeRessources.Video)
                {
                    string embed = "<div class=\"embed-responsive embed-responsive-16by9\">";
                    embed += "<iframe class=\"embed-responsive-item\" src=\"{0}\" allowfullscreen></iframe>";
                    embed += "</div>";

                    if (string.IsNullOrEmpty(model.urlVideo))
                    {
                        ModelState.AddModelError("urlVideo", "Veuillez saisir une url");
                    }

                    if (Regex.IsMatch(model.urlVideo, @"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$"))
                    {
                        content = "https://www.youtube.com/embed/" + model.urlVideo.Substring(model.urlVideo.IndexOf("v=") + 2, 11) + "?rel=0";
                        content = string.Format(embed, content);
                        originalContent = model.urlVideo;
                    }
                    else if (Regex.IsMatch(model.urlVideo, @"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:dailymotion\.com))(\/(?:[\w\-]+\/video//\/)?)([\w\-]+)(\S+)?$"))
                    {
                        content = "https://www.dailymotion.com/embed/video/" + model.urlVideo.Substring(model.urlVideo.IndexOf("video/") + 6, 7);
                        content = string.Format(embed, content);
                        originalContent = model.urlVideo;
                    }
                    else
                    {
                        ModelState.AddModelError("urlVideo", "L'url saisie n'est pas valide");
                    }
                }
                else
                {
                    content = model.Contenu;
                }

                if (ModelState.IsValid || ignoreModelState)
                {
                    ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(model.RessourceId);

                    IEnumerable<int> diff1 = new List<int>(), diff2 = new List<int>();                    
                    
                    if(model.SelectedTypeRelation != null && ressource.TypeRelationsRessources.Any())
                    {
                        diff1 = model.SelectedTypeRelation.Except(ressource.TypeRelationsRessources.Select(c => c.TypeRelationId));
                        diff2 = ressource.TypeRelationsRessources.Select(c => c.TypeRelationId).Except(model.SelectedTypeRelation);
                    }

                    if ((diff1.Any() || diff2.Any()) || ressource.Titre != model.Titre || ressource.Contenu != content ||
                        ressource.CategorieId != model.SelectedCategories || ressource.TypeRessourceId != model.SelectedTypeRessources ||
                        ressource.TypePartage != model.TypePartage)
                    {
                        if (ressource.Statut != Statut.Empty && ressource.Statut != Statut.EnPause && ressource.Statut != Statut.AttenteValidation)
                        {
                            var ressourceHistorique = ressource.Clone();
                            ressourceHistorique.Id = default;
                            ressourceHistorique.RessourceParentId = ressource.Id;

                            await MetierFactory.CreateRessourceMetier().InsertOrUpdate(ressourceHistorique);

                            await MetierFactory.CreateTypeRelationRessourceMetier().AjouterRelationsToRessource(model.SelectedTypeRelation, ressourceHistorique.Id);
                        }
                        else
                        {
                            if (ressource.Statut == Statut.Empty)
                                ressource.DateCreation = DateTimeOffset.Now;
                        }

                        ressource.ContenuOriginal = originalContent;
                        ressource.Titre = model.Titre;
                        ressource.Statut = model.Statut;
                        ressource.DateModification = DateTimeOffset.Now;
                        ressource.CategorieId = model.SelectedCategories == 0 ? null : model.SelectedCategories;
                        ressource.TypeRessourceId = model.SelectedTypeRessources == 0 ? null : model.SelectedTypeRessources;
                        ressource.TypePartage = model.TypePartage;
                        ressource.Contenu = content;

                        await MetierFactory.CreateRessourceMetier().InsertOrUpdate(ressource);

                        await MetierFactory.CreateTypeRelationRessourceMetier().AjouterRelationsToRessource(model.SelectedTypeRelation, ressource.Id);
                    }

                    ViewBag.Preview = "show";
                    ViewBag.Content = ressource.Contenu;
                }
            }
        }
    }
}
