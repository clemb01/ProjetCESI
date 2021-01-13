using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class CommentaireController : BaseController
    {
        public async Task<IActionResult> AjouterCommentaire(CommentaireViewModel model)
        {


            return View("Commentaire");
        }

        public async Task<IActionResult> GetCommentaires(int ressourceId)
        {
            var model = PrepareModel<CommentairesViewModel>();

            model.Commentaires = (await MetierFactory.CreateCommentaireMetier().GetAllCommentairesParentByRessourceId(ressourceId)).ToList();

            return PartialView("Commentaire", model);
        }
    }
}
