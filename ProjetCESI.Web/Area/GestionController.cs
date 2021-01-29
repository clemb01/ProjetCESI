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
    public class GestionController : BaseController
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

