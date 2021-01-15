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
        public async Task<IActionResult> CreateArticle(CreateRessourceViewModel model)
        {
            var ressource = new Ressource();
            ressource.Titre = model.Titre;
            ressource.Contenu = model.Contenu;
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

            return View("Create", model);
        }
    }
}
