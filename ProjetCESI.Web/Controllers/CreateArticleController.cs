using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ProjetCESI.Core;
using ProjetCESI.Web.Models.Ressource;
using ProjetCESI.Metier;

namespace ProjetCESI.Web.Controllers
{
    public class CreateArticleController : BaseController
    {
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateArticle(RessourceViewModel model)
        {
            var ressource = new Ressource();
            ressource.Titre = model.Titre;
            ressource.Contenu = model.Contenu;
            ressource.EstValide = model.EstValide;
            //ressource.CategorieId = model.Categorie.Id;
            ressource.Commentaires = model.Commentaires;
            ressource.DateCreation = model.DateCreation;
            ressource.DateModification = model.DateModification;
            ressource.NombreConsultation = model.NombreConsultation;
            //ressource.UtilisateurCreateur = model.UtilisateurCreateur;
            //ressource.UtilisateurCreateurId = ;

            MetierFactory.CreateRessourceMetier().SaveRessource(ressource);

            return View("Visualisation", model.Contenu);
        }


    }
}
