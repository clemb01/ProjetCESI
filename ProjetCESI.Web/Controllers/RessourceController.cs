using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class RessourceController : BaseController
    {
        [HttpGet]
        [Route("Ressource/{id}")]
        public async Task<IActionResult> Ressource(int id = 0)
        {
            var model = PrepareModel<RessourceViewModel>();

            model.Ressource = await MetierFactory.CreateRessourceMetier().GetById(id);

            return View(model);
        }
    }
}
