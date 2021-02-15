using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetCESI.Data;

namespace ProjetCESI.Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class CommentaireController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> AjouterCommentaire(string contenu, int ressourceId, int utilisateurId)
        {
            var date = DateTimeOffset.Now;

            Commentaire commentaire = new Commentaire()
            {
                DateCreation = date,
                DateModification = date,
                RessourceId = ressourceId,
                Texte = contenu.Replace("\n", "\\n"),
                UtilisateurId = utilisateurId
            };

            await MetierFactory.CreateCommentaireMetier().InsertOrUpdate(commentaire);

            var model = new CommentairesViewModel() { RessourceId = ressourceId };

            await UpdateModel(model);

            return PartialView("Commentaire", model);
        }

        [HttpPost]
        public async Task<IActionResult> RepondreCommentaire(string contenu, int ressourceId, int utilisateurId, int commentaireParentId)
        {
            var date = DateTimeOffset.Now;

            Commentaire commentaire = new Commentaire()
            {
                DateCreation = date,
                DateModification = date,
                RessourceId = ressourceId,
                Texte = contenu.Replace("\n", "\\n"),
                UtilisateurId = utilisateurId,
                CommentaireParentId = commentaireParentId
            };

            await MetierFactory.CreateCommentaireMetier().InsertOrUpdate(commentaire);

            var model = new CommentairesViewModel() { RessourceId = ressourceId };

            await UpdateModel(model);

            return PartialView("Commentaire", model);
        }

        private async Task UpdateModel(CommentairesViewModel model)
        {
            model = PrepareModel(model);

            model.Commentaires = (await MetierFactory.CreateCommentaireMetier().GetAllCommentairesParentByRessourceId(model.RessourceId)).ToList();
        }

        [AllowAnonymous]
        public async Task<IActionResult> GetCommentaires(int ressourceId)
        {
            var model = PrepareModel<CommentairesViewModel>();

            model.RessourceId = ressourceId;

            await UpdateModel(model);

            return PartialView("Commentaire", model);
        }

        [HttpPost]
        public async Task<IActionResult> SuppressionCommentaire(int commId, int ressourceId)
        {
            var model = PrepareModel<CommentairesViewModel>();
            model.RessourceId = ressourceId;

            Commentaire commentaire = await MetierFactory.CreateCommentaireMetier().GetCommentaireComplet(commId);

            if (commentaire.CommentairesEnfant.Count == 0)
            {
                commentaire.Statut = StatutCommentaire.Supprime;
                commentaire.DateModification = DateTimeOffset.Now;
                commentaire.DateSuppression = DateTimeOffset.Now;
            }
            else
            {
                commentaire.Texte = "Ce commentaire a été suspendu";
                commentaire.DateModification = DateTimeOffset.Now;
                commentaire.Statut = StatutCommentaire.Suspendu;
            }

            await MetierFactory.CreateCommentaireMetier().InsertOrUpdate(commentaire);

            await UpdateModel(model);

            return PartialView("Commentaire", model);
        }
    }
}
