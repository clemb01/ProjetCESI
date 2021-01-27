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
    public class GestionController : BaseController
    {
        [Route("Gestion")]
        public async Task<IActionResult> Gestion(GestionViewModel model)
        {
            PrepareModel(model);

            if (model.NomVue == "Validation")
            {
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetRessourcesNonValider()).ToList();
                if (model.Ressources == null)
                {
                    return View();
                }
            }
            else if (model.NomVue == "UserList")
            {
                model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
                if (model.Users == null || model == null)
                {
                    return View();
                }
            }
            else if (model.NomVue == "statistique")
            {
                return View(model);
            }
            else
                return RedirectToAction("Accueil", "Accueil");

            return View(model);
        }
    }
}
