using Microsoft.AspNetCore.Mvc.Filters;
using ProjetCESI.Core;
using ProjetCESI.Metier;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Outils
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
    public class StatistiqueFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Statistique stat = new Statistique()
            {
                DateRecherche = DateTimeOffset.Now
            };

            if(context.ActionArguments.ContainsKey("model"))
            {
                var model = (RechercheRessourceViewModel)context.ActionArguments["model"];

                if (model != null)
                {
                    stat.RechercheEffectue = model.Recherche;
                    stat.ParametreRecherche = GenerateParametreRecherche(model);
                }
            }
            else
            {
                stat.RechercheEffectue = context.ActionArguments.ContainsKey("recherche") ? (string)context.ActionArguments["recherche"] : string.Empty;
            }

            if (context.HttpContext != null && context.HttpContext.User != null && context.HttpContext.User.Identity != null && !string.IsNullOrWhiteSpace(context.HttpContext.User.Identity.Name))
            {
                Claim claim = ((ClaimsIdentity)context.HttpContext.User.Identity).FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null && claim.Value != null)
                    stat.UtilisateurId = Convert.ToInt32(claim.Value.ToString());
            }

            MetierFactory metier = new MetierFactory(stat.UtilisateurId);
            metier.CreateStatistiqueMetier().InsertOrUpdate(stat).GetAwaiter().GetResult();

            base.OnActionExecuting(context);
        }

        private string GenerateParametreRecherche(RechercheRessourceViewModel model)
        {
            string parametreRecherche = string.Empty;

            if(model.SelectedCategories != null && model.SelectedCategories.Any())
            {
                parametreRecherche += "SelectedCategories=";

                for (int i = 0; i < model.SelectedCategories.Count; i++)
                {
                    parametreRecherche += model.SelectedCategories[i].ToString();

                    if (i < model.SelectedCategories.Count - 1)
                        parametreRecherche += "|";
                }

                parametreRecherche += "&";
            }

            if (model.SelectedTypeRelation != null && model.SelectedTypeRelation.Any())
            {
                parametreRecherche += "SelectedTypeRelation=";

                for (int i = 0; i < model.SelectedTypeRelation.Count; i++)
                {
                    parametreRecherche += model.SelectedTypeRelation[i].ToString();

                    if (i < model.SelectedTypeRelation.Count - 1)
                        parametreRecherche += "|";
                }

                parametreRecherche += "&";
            }

            if (model.SelectedTypeRessources != null && model.SelectedTypeRessources.Any())
            {
                parametreRecherche += "SelectedTypeRessources=";

                for (int i = 0; i < model.SelectedTypeRessources.Count; i++)
                {
                    parametreRecherche += model.SelectedTypeRessources[i].ToString();

                    if (i < model.SelectedTypeRessources.Count - 1)
                        parametreRecherche += "|";
                }
            }

            return parametreRecherche;
        }
    }
}
