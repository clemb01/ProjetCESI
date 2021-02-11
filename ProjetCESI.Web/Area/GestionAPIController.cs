using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class GestionAPIController : BaseAPIController
    {
        [Route("Gestion")]
        public async Task<GestionViewModel> Gestion(GestionViewModel model)
        {
            PrepareModel(model);

            if (model.NomVue == "Validation")
            {
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetRessourcesNonValider()).ToList();
                if (model.Ressources == null)
                {
                    return null;
                }
            }
            else if (model.NomVue == "UserList")
            {
                model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
                if (model.Users == null || model == null)
                {
                    return null;
                }
            }
            else if (model.NomVue == "statistique")
            {
                return model;
            }
            else if (model.NomVue == "suspendu")
            {
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetRessourcesSuspendu()).ToList();
                if (model.Ressources == null)
                {
                    return null;
                }
            }
            else
                return null;

            return model;
        }
    }
}

