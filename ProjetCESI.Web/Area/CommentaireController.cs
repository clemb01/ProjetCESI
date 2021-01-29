﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Area
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentaireController : BaseController
    {
        public async Task<CommentairesViewModel> AjouterCommentaire(string contenu, int ressourceId, int utilisateurId)
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

            return model;
        }

        public async Task<CommentairesViewModel> RepondreCommentaire(string contenu, int ressourceId, int utilisateurId, int commentaireParentId)
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

            return model;
        }

        private async Task UpdateModel(CommentairesViewModel model)
        {
            model = PrepareModel(model);

            model.Commentaires = (await MetierFactory.CreateCommentaireMetier().GetAllCommentairesParentByRessourceId(model.RessourceId)).ToList();
        }

        public async Task<CommentairesViewModel> GetCommentaires(int ressourceId)
        {
            var model = PrepareModel<CommentairesViewModel>();

            model.RessourceId = ressourceId;

            await UpdateModel(model);

            return model;
        }
    }
}
