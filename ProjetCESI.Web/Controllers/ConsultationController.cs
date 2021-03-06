﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using ProjetCESI.Web.Outils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    [AllowAnonymous]
    public class ConsultationController : BaseController
    {
        [HttpGet]
        [StatistiqueFilter]
        public async Task<IActionResult> Search(string recherche)
        {
            var model = PrepareModel<RechercheRessourceViewModel>();

            model.Recherche = recherche;
            model.Ressources.TypeTri = 0;
            var result = await MetierFactory.CreateRessourceMetier().GetAllSearchPaginedRessource(model.Recherche, _pageOffset: model.Ressources.Page - 1);
            model.Ressources.Ressources = result.Item1.ToList();
            model.Ressources.NombrePages = result.Item2;

            ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            return View(model);
        }

        [HttpGet]
        [StatistiqueFilter]
        public async Task<IActionResult> AdvancedSearch(RechercheRessourceViewModel model)
        {
            model = PrepareModel(model);

            if (model.DateFin.HasValue)
                model.DateFin = model.DateFin.Value.AddDays(1).AddTicks(-1);

            var result = await MetierFactory.CreateRessourceMetier().GetAllAdvancedSearchPaginedRessource(model.Recherche, model.SelectedCategories, model.SelectedTypeRelation, model.SelectedTypeRessources, model.DateDebut, model.DateFin, (TypeTriBase)model.Ressources.TypeTri, _pageOffset: model.Ressources.Page - 1);
            model.Ressources.Ressources = result.Item1.ToList();
            model.Ressources.NombrePages = result.Item2;

            ViewBag.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            ViewBag.TypeRelation = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            ViewBag.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            return View("Search", model);
        }

        [HttpGet]
        public async Task<IActionResult> Consultation(int tri = 0, int page = 1)
        {
            var model = PrepareModel<ConsultationViewModel>();

            model.Ressources.TypeTri = tri;
            model.Ressources.Page = page;
            var result = await MetierFactory.CreateRessourceMetier().GetAllPaginedRessource((TypeTriBase)tri, _pageOffset: page - 1);
            model.Ressources.Ressources = result.Item1.ToList();
            model.Ressources.NombrePages = result.Item2;

            return View("Consultation", model);
        }

        private SelectList ToSelectList<T>(List<T> liste)
        {
            var selectList = new SelectList(liste, "Id", "Nom");
            
            return selectList;
        }
    }
}
