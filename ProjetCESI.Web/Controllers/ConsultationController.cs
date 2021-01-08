using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class ConsultationController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Search(string recherche)
        {
            var model = PrepareModel<RechercheRessourceViewModel>();

            model.Recherche = recherche;
            model.Ressources = (await MetierFactory.CreateRessourceMetier().GetAllSearchPaginedRessource(model.Recherche, _pageOffset: model.Page)).ToList();

            ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(RechercheRessourceViewModel model)
        {
            model = PrepareModel(model);

            model.Ressources = (await MetierFactory.CreateRessourceMetier().GetAllSearchPaginedRessource(model.Recherche, _pageOffset: model.Page)).ToList();
            ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            return View("Search", model);
        }

        private SelectList ToSelectList<T>(List<T> liste)
        {
            var selectList = new SelectList(liste, "Id", "Nom");
            
            return selectList;
        }
    }
}
