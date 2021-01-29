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
            else if (model.NomVue == "Parametre")
            {
                model.categories = (await MetierFactory.CreateCategorieMetier().GetAll()).ToList();
                if (model.categories == null)
                {
                    return View();
                }
            }
            else if (model.NomVue == "suspendu")
            {
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetRessourcesSuspendu()).ToList();
                if (model.Ressources == null)
                {
                    return View();
                }
            }
            else
                return RedirectToAction("Accueil", "Accueil");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ModifParamCategorie(int id, string nomCategorie)
        {
            var categorie = await MetierFactory.CreateCategorieMetier().GetById(id);
            categorie.Nom = nomCategorie;
            var result = await MetierFactory.CreateCategorieMetier().InsertOrUpdate(categorie);
            return RedirectToAction("Gestion", new { nomVue = "Parametre" });

        }

        [HttpPost]
        public async Task<IActionResult> AddCategorie(string newCategorie)
        {
            Core.Categorie NewCate = new Core.Categorie();
            NewCate.Nom = newCategorie;
            await MetierFactory.CreateCategorieMetier().InsertOrUpdate(NewCate);
            return RedirectToAction("Gestion", new { nomVue = "Parametre" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
            var categorie = await MetierFactory.CreateCategorieMetier().GetById(id);
            var result = await MetierFactory.CreateCategorieMetier().Delete(categorie);
            return RedirectToAction("Gestion", new { nomVue = "Parametre" });
        }


    }
}
