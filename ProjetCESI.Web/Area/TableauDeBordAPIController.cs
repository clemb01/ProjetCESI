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
    public class TableauDeBordAPIController : BaseAPIController
    {
        [HttpGet("")]
        public async Task<ResponseAPI> TableauDeBord([FromQuery] TableauDeBordViewModel model)
        {
            var response = new ResponseAPI();

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

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        private static void UpdateModel(TableauDeBordViewModel model, Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int> result)
        {
            model.Ressources = new List<RessourceTableauBord>();

            if (result.Item2 != null)
            {
                var list = result.Item1.ToList();
                var status = result.Item2.ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    model.Ressources.Add(new RessourceTableauBord
                    {
                        Id = list[i].Id,
                        Categorie = list[i].Categorie,
                        Statut = list[i].Statut,
                        StatutActivite = status[i],
                        Titre = list[i].Titre,
                        TypeRelationsRessources = list[i].TypeRelationsRessources,
                        TypeRessource = list[i].TypeRessource
                    });
                }
            }
            else
            {
                model.Ressources = result.Item1.Select(c => new RessourceTableauBord
                {
                    Id = c.Id,
                    Categorie = c.Categorie,
                    Statut = c.Statut,
                    StatutActivite = null,
                    Titre = c.Titre,
                    TypeRelationsRessources = c.TypeRelationsRessources,
                    TypeRessource = c.TypeRessource
                }).ToList();
            }

            model.NombrePages = result.Item3;
        }

        [HttpGet("Search")]
        public async Task<ResponseAPI> Search([FromQuery] TableauDeBordViewModel model)
        {
            var response = new ResponseAPI();

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

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }
    }
}
