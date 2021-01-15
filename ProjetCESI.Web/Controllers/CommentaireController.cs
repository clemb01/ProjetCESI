using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class CommentaireController : BaseController
    {
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

        public async Task<IActionResult> GetCommentaires(int ressourceId)
        {
            var model = PrepareModel<CommentairesViewModel>();

            model.RessourceId = ressourceId;

            await UpdateModel(model);

            return PartialView("Commentaire", model);
        }
    }
}
