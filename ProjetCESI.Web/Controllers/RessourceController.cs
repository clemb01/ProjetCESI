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

            if(ressource.UtilisateurCreateurId != UserId)
                if (ressource.Statut != Statut.Accepter)
                    return RedirectToAction("Accueil", "Accueil");

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
            model.Statut = ressource.Statut;
            model.NombreConsultation = ressource.Statut == Statut.Accepter ? ++ressource.NombreConsultation : ressource.NombreConsultation;
            model.DateSuppression = ressource.DateSuppression;
            model.RessourceSupprime = ressource.RessourceSupprime;

            if (User.Identity.IsAuthenticated)
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

        [HttpGet]
        [Route("ValidateRessource/{id}")]
        public async Task<IActionResult> ValidateRessource(int id)
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
            model.Statut = ressource.Statut;

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

        [HttpPost]
        public async Task<IActionResult> ValiderRessource(int ressourceId)
        {
            var Ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(ressourceId);
            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            string UserId = Ressource.UtilisateurCreateurId.ToString();
            var User = await UserManager.FindByIdAsync(UserId);
            string message = "Votre ressource :" + Ressource.Titre + ", a été validé !";
            Ressource.Statut = Statut.Accepter;
            
            var result = await ressourceMetier.InsertOrUpdate(Ressource);
            var model = new GestionViewModel();
            model.Ressources = (await MetierFactory.CreateRessourceMetier().GetRessourcesNonValider()).ToList();
            model.NomVue = "Validation";
            if (result)
            {
                await MetierFactory.EmailMetier().SendEmailAsync(User.Email, "Validation de ressource", message);
                return View("../Gestion/Gestion", model);
            } 
            else
                return StatusCode(StatusCodes.Status500InternalServerError);


        }

        [HttpPost]
        public async Task<IActionResult> RefuserRessource(int ressourceId, string messageRefus)
        {
            var Ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(ressourceId);
            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            string UserId = Ressource.UtilisateurCreateurId.ToString();
            string message = "";
            if (String.IsNullOrWhiteSpace(messageRefus))
            {
                message = "La validation de : " + Ressource.Titre + ", a été refusé." ;
            }
            else
            {
                message = "La validation de : " + Ressource.Titre + ", a été refusé. Motif : " + messageRefus;
            }
            var User = await UserManager.FindByIdAsync(UserId);
            Ressource.Statut = Statut.Refuser;
            var result = await ressourceMetier.InsertOrUpdate(Ressource);
            var model = new GestionViewModel();
            model.Ressources = (await MetierFactory.CreateRessourceMetier().GetRessourcesNonValider()).ToList();
            model.NomVue = "Validation";
            if (result)
            {
                await MetierFactory.EmailMetier().SendEmailAsync(User.Email, "Validation de ressource", message);
                return View("../Gestion/Gestion", model);
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError);


        }

        [HttpPost]
        public async Task<IActionResult> SupprimerRessource(int ressourceId, string messageSuppression)
        {
            var Ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(ressourceId);
            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            string UserId = Ressource.UtilisateurCreateurId.ToString();
            string message = "";
            var model = PrepareModel<ConsultationViewModel>();
            int tri = 0;
            model.Ressources.TypeTri = tri;
            model.Ressources.Ressources = (await MetierFactory.CreateRessourceMetier().GetAllPaginedRessource((TypeTriBase)tri)).ToList();
            if (String.IsNullOrWhiteSpace(messageSuppression))
            {
                message = "La ressource : " + Ressource.Titre + ", a été supprimé.";
            }
            else
            {
                message = "La ressource : " + Ressource.Titre + ", a été supprimé. Motif : " + messageSuppression;
            }
            var User = await UserManager.FindByIdAsync(UserId);
            Ressource.RessourceSupprime = true;
            Ressource.DateSuppression = DateTimeOffset.Now;
            var result = await ressourceMetier.InsertOrUpdate(Ressource);
            if (result)
            {
                await MetierFactory.EmailMetier().SendEmailAsync(User.Email, "Validation de ressource", message);
                return View("../Consultation/Consultation");
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError);


        }

        [HttpPost]
        public async Task<IActionResult> SuspendreRessource(int ressourceId, string messageSuspendre)
        {
            var Ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(ressourceId);
            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            string UserId = Ressource.UtilisateurCreateurId.ToString();
            string message = "";
            var model = PrepareModel<ConsultationViewModel>();
            int tri = 0;
            model.Ressources.TypeTri = tri;
            model.Ressources.Ressources = (await MetierFactory.CreateRessourceMetier().GetAllPaginedRessource((TypeTriBase)tri)).ToList();
            if (String.IsNullOrWhiteSpace(messageSuspendre))
            {
                message = "La ressource : " + Ressource.Titre + ", a été suspendu.";
            }
            else
            {
                message = "La ressource : " + Ressource.Titre + ", a été suspendu. Motif : " + messageSuspendre;
            }
            var User = await UserManager.FindByIdAsync(UserId);
            Ressource.RessourceSupprime = true;
            Ressource.DateSuppression = DateTimeOffset.Now;
            var result = await ressourceMetier.InsertOrUpdate(Ressource);
            if (result)
            {
                await MetierFactory.EmailMetier().SendEmailAsync(User.Email, "Validation de ressource", message);
                return View("../Consultation/Consultation");
            }
            else
                return StatusCode(StatusCodes.Status500InternalServerError);


        }
    }
}
