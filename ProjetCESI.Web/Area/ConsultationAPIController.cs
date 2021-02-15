using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ConsultationAPIController : BaseAPIController
    {
        [HttpGet("Search")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> Search([FromQuery] string recherche)
        {
            var response = new ResponseAPI();

            var model = PrepareModel<RechercheRessourceViewModelAPI>();

            model.Recherche = recherche;
            model.Ressources.TypeTri = 0;
            var result = await MetierFactory.CreateRessourceMetier().GetAllSearchPaginedRessource(model.Recherche, _pageOffset: model.Ressources.Page - 1);
            model.Ressources.Ressources = result.Item1.ToList();
            model.Ressources.NombrePages = result.Item2;
            model.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            model.TypeRelations = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            model.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        private SelectList ToSelectList<T>(List<T> liste)
        {
            var selectList = new SelectList(liste, "Id", "Nom");

            return selectList;
        }

        [HttpPost("Search")]
        [StatistiqueFilter]
        public async Task<ResponseAPI> Search([FromBody] RechercheRessourceViewModelAPI model)
        {
            var response = new ResponseAPI();
            model = PrepareModel(model);

            if (model.DateFin.HasValue)
                model.DateFin = model.DateFin.Value.AddDays(1).AddTicks(-1);

            var result = await MetierFactory.CreateRessourceMetier().GetAllAdvancedSearchPaginedRessource(model.Recherche, model.SelectedCategories, model.SelectedTypeRelation, model.SelectedTypeRessources, model.DateDebut, model.DateFin, (TypeTriBase)model.Ressources.TypeTri, _pageOffset: model.Ressources.Page - 1);
            model.Ressources.Ressources = result.Item1.ToList();
            model.Ressources.NombrePages = result.Item2;
            model.Categories = ToSelectList((await MetierFactory.CreateCategorieMetier().GetAll()).ToList());
            model.TypeRelations = ToSelectList((await MetierFactory.CreateTypeRelationMetier().GetAll()).ToList());
            model.TypeRessources = ToSelectList((await MetierFactory.CreateTypeRessourceMetier().GetAll()).ToList());

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        [HttpGet("Consultation")]
        public async Task<ResponseAPI> Consultation(int tri = 0)
        {
            var response = new ResponseAPI();

            var model = PrepareModel<ConsultationViewModel>();

            model.Ressources.TypeTri = tri;
            var result = await MetierFactory.CreateRessourceMetier().GetAllPaginedRessource((TypeTriBase)tri);
            model.Ressources.Ressources = result.Item1.ToList();
            model.Ressources.NombrePages = result.Item2;

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }
    }
}
