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
using ProjetCESI.Web.Outils;

namespace ProjetCESI.Web.Controllers
{
    [Authorize]
    public class RessourceController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        [StatistiqueFilter]
        [Route("Ressource/{id}")]
        public async Task<IActionResult> Ressource(int id)
        {
            var model = PrepareModel<RessourceViewModel>();

            var ressourceMetier = MetierFactory.CreateRessourceMetier();

            Ressource ressource = await ressourceMetier.GetRessourceComplete(id);

            model.RessourceId = id;
            model.Titre = ressource.Titre;
            model.UtilisateurCreateur = ressource.UtilisateurCreateur;
            model.TypeRessource = ressource.TypeRessource;
            model.TypeRelations = ressource.TypeRelationsRessources.Select(c => c.TypeRelation).ToList();
            model.Categorie = ressource.Categorie;
            model.Commentaires = ressource.Commentaires;
            model.DateCreation = ressource.DateCreation;
            model.DateModification = ressource.DateModification;
            model.Contenu = ressource.Contenu;
            model.EstValide = ressource.EstValide;
            model.NombreConsultation = ++ressource.NombreConsultation;

            if(User.Identity.IsAuthenticated)
            {
                UtilisateurRessource utilisateurRessource = await MetierFactory.CreateUtilisateurRessourceMetier().GetByUtilisateurAndRessourceId(Utilisateur.Id, id);

                model.EstExploite = utilisateurRessource.EstExploite;
                model.EstFavoris = utilisateurRessource.EstFavoris;
                model.EstMisDeCote = utilisateurRessource.EstMisDeCote;
            }

            ressource.TypeRelationsRessources = null;
            ressource.TypeRessource = null;
            ressource.Categorie = null;
            ressource.Commentaires = null;
            ressource.UtilisateurCreateur = null;
            ressource.UtilisateurRessources = null;

            await ressourceMetier.InsertOrUpdate(ressource);

            return View(model);
        }

        [HttpPost]
        [StatistiqueFilter]
        public async Task<IActionResult> AjouterFavoris(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().AjouterFavoris(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [StatistiqueFilter]
        public async Task<IActionResult> SupprimerFavoris(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().SupprimerFavoris(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [StatistiqueFilter]
        public async Task<IActionResult> AjouterMettreDeCote(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().MettreDeCote(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [StatistiqueFilter]
        public async Task<IActionResult> SupprimerMettreDeCote(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().DeMettreDeCote(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [StatistiqueFilter]
        public async Task<IActionResult> AjouterExploite(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().EstExploite(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        [StatistiqueFilter]
        public async Task<IActionResult> SupprimerExploite(int ressourceId)
        {
            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().PasExploite(Utilisateur.Id, ressourceId);

            if (result)
                return StatusCode(StatusCodes.Status200OK);
            else
                return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
