using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Metier.Outils;
using ProjetCESI.Web.Models;
using ProjetCESI.Web.Outils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Area
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RessourceAPIController : BaseAPIController
    {
        [HttpGet("{id}")]
        [AllowAnonymous]
        [StatistiqueFilter]
        public async Task<ResponseAPI> Ressource(int id, string shareLink = null)
        {
            var response = new ResponseAPI();

            var model = PrepareModel<RessourceViewModel>();

            var ressourceMetier = MetierFactory.CreateRessourceMetier();

            Ressource ressource = await ressourceMetier.GetRessourceComplete(id);

            if (ressource.UtilisateurCreateurId != UserId)
                if (ressource.Statut != Statut.Accepter)
                    if (User.IsInRole(Enum.GetName(TypeUtilisateur.Citoyen)))
                    {
                        response.IsError = false;
                        response.Message = "Vous ne pouvez pas consulter cette ressource!";
                        response.StatusCode = "401";

                        return response;
                    }

            if (ressource.UtilisateurCreateurId != UserId)
                if (ressource.TypePartage != TypePartage.Public && shareLink != ressource.KeyLink)
                {
                    response.IsError = false;
                    response.Message = "Vous ne pouvez pas consulter cette ressource!";
                    response.StatusCode = "401";

                    return response;
                }

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
            model.TypePartage = ressource.TypePartage;
            model.ShareURL = ressource.ShareLink;

            if (User.Identity.IsAuthenticated)
            {
                UtilisateurRessource utilisateurRessource = await MetierFactory.CreateUtilisateurRessourceMetier().GetByUtilisateurAndRessourceId(Utilisateur.Id, id, model.TypeRessource == TypeRessources.ActiviteJeu);

                model.EstExploite = utilisateurRessource.EstExploite;
                model.EstFavoris = utilisateurRessource.EstFavoris;
                model.EstMisDeCote = utilisateurRessource.EstMisDeCote;
                model.StatutActivite = utilisateurRessource.StatutActivite;
            }

            if (ressource.TypeRessource.Id == (int)TypeRessources.PDF && !string.IsNullOrEmpty(ressource.ContenuOriginal))
            {
                string uploads = Path.Combine(HostingEnvironnement.WebRootPath, "uploads");
                string blobFile = ressource.ContenuOriginal.Split("||").Last();
                await BlobStorage.GetBlobData(blobFile.Substring(blobFile.LastIndexOf("stockage/") + 9), Path.Combine(uploads, blobFile.Substring(blobFile.LastIndexOf("/") + 1)));
            }

            ressource.TypeRelationsRessources = null;
            ressource.TypeRessource = null;
            ressource.Categorie = null;
            ressource.Commentaires = null;
            ressource.UtilisateurCreateur = null;
            ressource.UtilisateurRessources = null;

            await ressourceMetier.InsertOrUpdate(ressource);

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        [HttpGet("ValidateRessource/{id}")]
        public async Task<ResponseAPI> ValidateRessource(int id)
        {
            var response = new ResponseAPI();

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

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        [HttpPost("AjouterFavoris")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> AjouterFavoris(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().AjouterFavoris(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("SupprimerFavoris")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> SupprimerFavoris(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().SupprimerFavoris(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("AjouterMettreDeCote")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> AjouterMettreDeCote(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().MettreDeCote(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("SupprimerMettreDeCote")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> SupprimerMettreDeCote(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().DeMettreDeCote(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("AjouterExploite")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> AjouterExploite(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().EstExploite(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("SupprimerExploite")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> SupprimerExploite(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().PasExploite(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("DemarrerActivite")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> DemarrerActivite(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().DemarrerActivite(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("SuspendreActivite")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> SuspendreActivite(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().SuspendreActivite(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("QuitterActivite")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> QuitterActivite(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().QuitterActivite(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("ReprendreActivite")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> ReprendreActivite(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().ReprendreActivite(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("TerminerActivite")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> TerminerActivite(int ressourceId)
        {
            var response = new ResponseAPI();

            bool result = await MetierFactory.CreateUtilisateurRessourceMetier().TerminerActivite(Utilisateur.Id, ressourceId);

            if (result)
                response.StatusCode = "200";
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("ValiderRessource")]
        public async Task<ResponseAPI> ValiderRessource(int ressourceId)
        {
            var response = new ResponseAPI();

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
                response.StatusCode = "200";
            }
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("RefuserRessource")]
        public async Task<ResponseAPI> RefuserRessource(int ressourceId, string messageRefus)
        {
            var response = new ResponseAPI();

            var Ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(ressourceId);
            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            string UserId = Ressource.UtilisateurCreateurId.ToString();
            string message = "";
            if (String.IsNullOrWhiteSpace(messageRefus))
            {
                message = "La validation de : " + Ressource.Titre + ", a été refusé.";
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
                response.StatusCode = "200";
            }
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("SupprimerRessource")]
        public async Task<ResponseAPI> SupprimerRessource(int ressourceId, string messageSuppression)
        {
            var response = new ResponseAPI();

            var Ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(ressourceId);
            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            string UserId = Ressource.UtilisateurCreateurId.ToString();
            string message = "";
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
                await MetierFactory.EmailMetier().SendEmailAsync(User.Email, "Suppression de la ressource", message);
                response.StatusCode = "200";
            }
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("SuspendreRessource")]
        public async Task<ResponseAPI> SuspendreRessource(int ressourceId, string messageSuspendre)
        {
            var response = new ResponseAPI();

            var Ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(ressourceId);
            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            string UserId = Ressource.UtilisateurCreateurId.ToString();
            string message = "";
            if (String.IsNullOrWhiteSpace(messageSuspendre))
            {
                message = "La ressource : " + Ressource.Titre + ", a été suspendu.";
            }
            else
            {
                message = "La ressource : " + Ressource.Titre + ", a été suspendu. Motif : " + messageSuspendre;
            }
            var User = await UserManager.FindByIdAsync(UserId);
            Ressource.Statut = Statut.Suspendre;
            var result = await ressourceMetier.InsertOrUpdate(Ressource);
            if (result)
            {
                await MetierFactory.EmailMetier().SendEmailAsync(User.Email, "Suspension de la ressource", message);
                response.StatusCode = "200";
            }
            else
                response.StatusCode = "500";

            return response;
        }

        [HttpPost("ReactivateRessource")]
        public async Task<ResponseAPI> ReactivateRessource(int ressourceId, string messageReactivate)
        {
            var response = new ResponseAPI();

            var Ressource = await MetierFactory.CreateRessourceMetier().GetRessourceComplete(ressourceId);
            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            string UserId = Ressource.UtilisateurCreateurId.ToString();
            string message = "";
            if (String.IsNullOrWhiteSpace(messageReactivate))
            {
                message = "La ressource : " + Ressource.Titre + ", a été réactivé.";
            }
            else
            {
                message = "La ressource : " + Ressource.Titre + ", a été réactivé. Motif : " + messageReactivate;
            }
            var User = await UserManager.FindByIdAsync(UserId);
            Ressource.Statut = Statut.Accepter;
            var result = await ressourceMetier.InsertOrUpdate(Ressource);
            if (result)
            {
                await MetierFactory.EmailMetier().SendEmailAsync(User.Email, "Réactivation de la ressource", message);
                response.StatusCode = "200";
            }
            else
                response.StatusCode = "500";

            return response;
        }
    }
}
