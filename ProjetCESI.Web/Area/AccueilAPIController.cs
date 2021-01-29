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
    public class AccueilAPIController : BaseAPIController
    {
        public async Task<AccueilViewModel> Accueil()
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
                Apercu = c.Item6
            }).ToList();

            model.RessourcesPlusRecentes = ressourcesPlusRecente.Select(c => new RessourceAccueil
            {
                Id = c.Item1,
                Categorie = c.Item2,
                Titre = c.Item3,
                TypeRelations = c.Item4,
                TypeRessource = c.Item5,
                Apercu = c.Item6
            }).ToList();

            return model;
        }
    }
}

