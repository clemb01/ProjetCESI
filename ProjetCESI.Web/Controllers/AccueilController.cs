using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetCESI.Web.Controllers;
using ProjetCESI.Web.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    [AllowAnonymous]
    public class AccueilController : BaseController
    {
        public async Task<IActionResult> Accueil()
        {
            var model = PrepareModel<AccueilViewModel>();

            var ressourceMetier = MetierFactory.CreateRessourceMetier();

            var ressourcesPlusVues = (await ressourceMetier.GetRessourcesAccueil(Core.TypeTriBase.NombreConsultationDesc));
            var ressourcesPlusRecente = (await ressourceMetier.GetRessourcesAccueil(Core.TypeTriBase.DateModificationDesc));

            model.RessourcesPlusVues = ressourcesPlusVues.Select(c => new RessourceAccueil
            {
                Id = c.Item1,
                Categorie = c.Item2,
                Titre = c.Item3,
                TypeRelations = c.Item4,
                TypeRessource = c.Item5,
                Apercu = c.Item6,
                RessourceOfficelle = c.Item7
            }).ToList();

            model.RessourcesPlusRecentes = ressourcesPlusRecente.Select(c => new RessourceAccueil
            {
                Id = c.Item1,
                Categorie = c.Item2,
                Titre = c.Item3,
                TypeRelations = c.Item4,
                TypeRessource = c.Item5,
                Apercu = c.Item6,
                RessourceOfficelle = c.Item7
            }).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult CookiePolicy()
        {
            return View();
        }
    }
}
