using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Area
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableauDeBordAPIController : BaseAPIController
    {
        [Route("TableauDeBord")]
        public async Task<TableauDeBordViewModel> TableauDeBord(TableauDeBordViewModel model)
        {
            PrepareModel(model);

            var ressourceMetier = MetierFactory.CreateRessourceMetier();

            if (model.NomVue == "favoris")
            {
                var result = await ressourceMetier.GetUserFavoriteRessources(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
                model.Ressources = result.Item1.ToList();
                model.NombrePages = result.Item2;
            }
            else if (model.NomVue == "exploitee")
            {
                var result = await ressourceMetier.GetUserRessourcesExploitee(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
                model.Ressources = result.Item1.ToList();
                model.NombrePages = result.Item2;
            }
            else if (model.NomVue == "miscote")
            {
                var result = await ressourceMetier.GetUserRessourcesMiseDeCote(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
                model.Ressources = result.Item1.ToList();
                model.NombrePages = result.Item2;
            }
            else if (model.NomVue == "crees")
            {
                var result = await ressourceMetier.GetUserRessourcesCreees(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
                model.Ressources = result.Item1.ToList();
                model.NombrePages = result.Item2;
            }
            else
                return null;

            model.Page = model.Page == default ? 1 : model.Page;

            return model;
        }

        public async Task<TableauDeBordViewModel> Search(TableauDeBordViewModel model)
        {
            PrepareModel(model);

            var ressourceMetier = MetierFactory.CreateRessourceMetier();

            if (model.NomVue == "favoris")
            {
                var result = await ressourceMetier.GetUserFavoriteRessources(UserId.Value);
                model.Ressources = result.Item1.ToList();
                model.NombrePages = result.Item2;
            }
            else if (model.NomVue == "exploitee")
            {
                var result = await ressourceMetier.GetUserRessourcesExploitee(UserId.Value);
                model.Ressources = result.Item1.ToList();
                model.NombrePages = result.Item2;
            }
            else if (model.NomVue == "miscote")
            {
                var result = await ressourceMetier.GetUserRessourcesMiseDeCote(UserId.Value);
                model.Ressources = result.Item1.ToList();
                model.NombrePages = result.Item2;
            }
            else if (model.NomVue == "crees")
            {
                var result = await ressourceMetier.GetUserRessourcesCreees(UserId.Value);
                model.Ressources = result.Item1.ToList();
                model.NombrePages = result.Item2;
            }
            else
                return null;

            model.Page = model.Page == default ? 1 : model.Page;

            return model;
        }
    }
}
