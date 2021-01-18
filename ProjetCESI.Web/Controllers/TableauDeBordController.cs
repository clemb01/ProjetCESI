using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    [Authorize]
    public class TableauDeBordController : BaseController
    {
        [Route("TableauDeBord")]
        public async Task<IActionResult> TableauDeBord(TableauDeBordViewModel model)
        {
            PrepareModel(model);

            if (model.NomVue == "favoris")
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetUserFavoriteRessources(UserId.Value)).ToList();
            else if (model.NomVue == "exploitee")
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetUserRessourcesExploitee(UserId.Value)).ToList();
            else if (model.NomVue == "miscote")
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetUserRessourcesMiseDeCote(UserId.Value)).ToList();
            else if (model.NomVue == "crees")
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetUserRessourcesCreees(UserId.Value)).ToList();
            else
                return RedirectToAction("Accueil", "Accueil");

            return View(model);
        }
    }
}
