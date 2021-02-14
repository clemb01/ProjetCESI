using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Area
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentaireAPIController : BaseAPIController
    {
        [HttpPost("AjouterCommentaire")]
        public async Task<ResponseAPI> AjouterCommentaire([FromBody] CommentaireViewModel model)
        {
            var response = new ResponseAPI();

            var date = DateTimeOffset.Now;

            Commentaire commentaire = new Commentaire()
            {
                DateCreation = date,
                DateModification = date,
                RessourceId = model.RessourceId,
                Texte = model.Contenu.Replace("\n", "\\n"),
                UtilisateurId = Utilisateur.Id
            };

            await MetierFactory.CreateCommentaireMetier().InsertOrUpdate(commentaire);

            var models = new CommentairesViewModel() { RessourceId = model.RessourceId };

            await UpdateModel(models);

            response.StatusCode = "200";
            response.Data = models;

            return response;
        }

        [HttpPost("RepondreCommentaire")]
        public async Task<ResponseAPI> RepondreCommentaire([FromBody] CommentaireViewModel model)
        {
            var response = new ResponseAPI();

            var date = DateTimeOffset.Now;

            Commentaire commentaire = new Commentaire()
            {
                DateCreation = date,
                DateModification = date,
                RessourceId = model.RessourceId,
                Texte = model.Contenu.Replace("\n", "\\n"),
                UtilisateurId = Utilisateur.Id,
                CommentaireParentId = model.CommentaireParentId
            };

            await MetierFactory.CreateCommentaireMetier().InsertOrUpdate(commentaire);

            var models = new CommentairesViewModel() { RessourceId = model.RessourceId };

            await UpdateModel(models);

            response.StatusCode = "200";
            response.Data = models;

            return response;
        }

        private async Task UpdateModel(CommentairesViewModel model)
        {
            model = PrepareModel(model);

            model.Commentaires = (await MetierFactory.CreateCommentaireMetier().GetAllCommentairesParentByRessourceId(model.RessourceId)).ToList();
        }

        [HttpGet("GetCommentaires")]
        public async Task<ResponseAPI> GetCommentaires(int ressourceId)
        {
            var response = new ResponseAPI();

            var model = PrepareModel<CommentairesViewModel>();

            model.RessourceId = ressourceId;

            await UpdateModel(model);

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        [HttpPost("SuppressionCommentaire")]
        public async Task<ResponseAPI> SuppressionCommentaire([FromBody] SuppressionModel model)
        {
            var response = new ResponseAPI();

            Commentaire commentaire = await MetierFactory.CreateCommentaireMetier().GetCommentaireComplet(model.IdComm);

            if (commentaire.CommentairesEnfant.Count() == 0)
            {
                await MetierFactory.CreateCommentaireMetier().Delete(commentaire);
                response.Message = "Commentaire supprimé";
            }
            else
            {
                commentaire.Texte = "Ce commentaire a été suspendu";
                await MetierFactory.CreateCommentaireMetier().InsertOrUpdate(commentaire);
                response.Message = "Commentaire suspendu";
            }

            response.StatusCode = "200";

            return response;

        }
    }

    public class SuppressionModel
    {
        public int IdComm { get; set; }
        public int RessourceIdComm { get; set; }
    }
}
