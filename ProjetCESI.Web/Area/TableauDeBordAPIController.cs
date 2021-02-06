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
    [Route("api/[controller]")]
    [ApiController]
    public class TableauDeBordAPIController : BaseAPIController
    {
        [Route("TableauDeBord")]
        public async Task<TableauDeBordViewModel> TableauDeBord(TableauDeBordViewModel model)
        {
            PrepareModel(model);

            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int> result = null;

            if (model.NomVue == "favoris")
            {
                result = await ressourceMetier.GetUserFavoriteRessources(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
            }
            else if (model.NomVue == "exploitee")
            {
                result = await ressourceMetier.GetUserRessourcesExploitee(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
            }
            else if (model.NomVue == "miscote")
            {
                result = await ressourceMetier.GetUserRessourcesMiseDeCote(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
            }
            else if (model.NomVue == "crees")
            {
                result = await ressourceMetier.GetUserRessourcesCreees(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
            }
            else if (model.NomVue == "activites")
            {
                result = await MetierFactory.CreateUtilisateurRessourceMetier().GetUserActivite(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
            }
            else
                return null;

            UpdateModel(model, result);
            model.Page = model.Page == default ? 1 : model.Page;

            return model;
        }

        private static void UpdateModel(TableauDeBordViewModel model, Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int> result)
        {
            var ressources = new List<RessourceTableauBord>();

            foreach (var res in result.Item1)
            {
                ressources.Add(new RessourceTableauBord
                {
                    Id = res.Id,
                    Categorie = res.Categorie,
                    Statut = res.Statut,
                    StatutActivite = StatutActivite.Demare,
                    Titre = res.Titre,
                    TypeRelationsRessources = res.TypeRelationsRessources,
                    TypeRessource = res.TypeRessource
                });
            }
            model.NombrePages = result.Item3;
        }

        public async Task<TableauDeBordViewModel> Search(TableauDeBordViewModel model)
        {
            PrepareModel(model);

            var ressourceMetier = MetierFactory.CreateRessourceMetier();
            Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int> result = null;

            if (model.NomVue == "favoris")
            {
                result = await ressourceMetier.GetUserFavoriteRessources(UserId.Value);
            }
            else if (model.NomVue == "exploitee")
            {
                result = await ressourceMetier.GetUserRessourcesExploitee(UserId.Value);
            }
            else if (model.NomVue == "miscote")
            {
                result = await ressourceMetier.GetUserRessourcesMiseDeCote(UserId.Value);
            }
            else if (model.NomVue == "crees")
            {
                result = await ressourceMetier.GetUserRessourcesCreees(UserId.Value);
            }
            else if (model.NomVue == "activites")
            {
                result = await MetierFactory.CreateUtilisateurRessourceMetier().GetUserActivite(UserId.Value, model.Recherche, _pageOffset: model.Page > 0 ? model.Page - 1 : model.Page);
            }
            else
                return null;

            UpdateModel(model, result);
            model.Page = model.Page == default ? 1 : model.Page;

            return model;
        }
    }
}
