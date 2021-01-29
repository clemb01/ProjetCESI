using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using ProjetCESI.Web.Outils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Area
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : BaseController
    {
        //[HttpGet]
        //[StatistiqueFilter]
        //public async Task<IActionResult> Search(string recherche)
        //{
        //    var model = PrepareModel<RechercheRessourceViewModel>();

        //    model.Recherche = recherche;
        //    model.Ressources.TypeTri = 0;
        //    model.Ressources.Ressources = (await MetierFactory.CreateRessourceMetier().GetAllSearchPaginedRessource(model.Recherche, _pageOffset: model.Page)).ToList();

        //    ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
        //    ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
        //    ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

        //    return View(model);
        //}

        private SelectList ToSelectList<T>(List<T> liste)
        {
            var selectList = new SelectList(liste, "Id", "Nom");

            return selectList;
        }

        //[HttpPost]
        //[StatistiqueFilter]
        //public async Task<IActionResult> Search(RechercheRessourceViewModel model)
        //{
        //    model = PrepareModel(model);

        //    if (model.DateFin.HasValue)
        //        model.DateFin = model.DateFin.Value.AddDays(1).AddTicks(-1);

        //    model.Ressources.Ressources = (await MetierFactory.CreateRessourceMetier().GetAllAdvancedSearchPaginedRessource(model.Recherche, model.SelectedCategories, model.SelectedTypeRelation, model.SelectedTypeRessources, model.DateDebut, model.DateFin, (TypeTriBase)model.Ressources.TypeTri, _pageOffset: model.Page)).ToList();

        //    ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
        //    ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
        //    ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

        //    return View("Search", model);
        //}

        [HttpGet]
        public async Task<ConsultationViewModel> Consultation(int tri = 0)
        {
            var model = PrepareModel<ConsultationViewModel>();

            model.Ressources.TypeTri = tri;
            model.Ressources.Ressources = (await MetierFactory.CreateRessourceMetier().GetAllPaginedRessource((TypeTriBase)tri)).ToList();

            return model;
        }
    }
}
