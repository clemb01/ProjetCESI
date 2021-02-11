using Microsoft.AspNetCore.Mvc.Filters;
using ProjetCESI.Core;
using ProjetCESI.Metier;
using ProjetCESI.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            MetierFactory metier = null;

            if (context.HttpContext != null && context.HttpContext.User != null && context.HttpContext.User.Identity != null && !string.IsNullOrWhiteSpace(context.HttpContext.User.Identity.Name))
            {
                Claim claim = ((ClaimsIdentity)context.HttpContext.User.Identity).FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null && claim.Value != null)
                {
                    int userId = Convert.ToInt32(claim.Value);

                    metier = new MetierFactory(userId);
                    stat.UtilisateurId = userId;
                }
                else
                {
                    metier = new MetierFactory(null);
                    stat.UtilisateurId = null;
                }
            }
            else
            {
                metier = new MetierFactory(null);
                stat.UtilisateurId = null;
            }

            stat.Controller = context.ActionDescriptor.RouteValues["controller"];
            stat.Action = context.ActionDescriptor.RouteValues["action"];

            if ((stat.Controller == "Consultation" || stat.Controller == "ConsultationAPI") && stat.Action == "Search")
            {
                if (context.ActionArguments.ContainsKey("model"))
                {
                    if (stat.Controller == "Consultation")
                    {
                        var model = (RechercheRessourceViewModel)context.ActionArguments["model"];

                        if (model != null)
                        {
                            stat.Parametre = GenerateParametreRecherche(model);
                        }
                    }
                    else
                    {
                        var model = (RechercheRessourceViewModelAPI)context.ActionArguments["model"];

                        if (model != null)
                        {
                            stat.Parametre = GenerateParametreRecherche(model);
                        }
                    }
                }
                else
                {
                    stat.Parametre = context.ActionArguments.ContainsKey("recherche") ? "Recherche=" + (string)context.ActionArguments["recherche"] : string.Empty;
                    
                    if(string.IsNullOrEmpty(stat.Parametre))
                        return;
                }
            }
            else if ((stat.Controller == "Ressource" || stat.Controller == "RessourceAPI") && stat.Action == "Ressource") 
            {
                stat.Parametre = $"ressourceId={(int)context.ActionArguments["id"]}";
            }
            else if (stat.Controller == "CreateArticle" || stat.Controller == "CreateArticleAPI")
            {
                if (context.ActionArguments.ContainsKey("model"))
                {
                    var model = (CreateRessourceViewModel)context.ActionArguments["model"];

                    if (model != null)
                    {
                        stat.Parametre = GenerateParametreRecherche(model);
                    }
                }
                else
                {
                    stat.Parametre = context.ActionArguments.ContainsKey("ressourceId") ? $"ressourceId={(int)context.ActionArguments["ressourceId"]}" : string.Empty;
                }
            }
            else if ((stat.Controller == "Ressource" || stat.Controller == "RessourceAPI") && (stat.Action.Contains("Ajouter") || stat.Action.Contains("Supprimer") || stat.Action.Contains("Activite")))
            {
                stat.Parametre = $"ressourceId={(int)context.ActionArguments["ressourceId"]}";
            }

            metier.CreateStatistiqueMetier().InsertOrUpdate(stat).GetAwaiter().GetResult();

            base.OnActionExecuting(context);
        }

        private string GenerateParametreRecherche<T>(T model) where T : new()
        {
            string parametreRecherche = string.Empty;

            var properties = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
            
            foreach (var property in properties)
            {
                if(property.GetValue(model) != null && property.Name != "Ressources" && typeof(BaseViewModel).IsAssignableFrom(typeof(T)))
                {
                    if(parametreRecherche.Length > 0 && parametreRecherche.LastIndexOf('&') != parametreRecherche.Length)
                    {
                        parametreRecherche += "&";
                    }

                    if(property.PropertyType != typeof(string) && typeof(ICollection).IsAssignableFrom(property.PropertyType))
                    {
                        parametreRecherche += $"{property.Name}=";

                        var list = ((ICollection)property.GetValue(model));
                        var e = list.GetEnumerator();

                        for (int i = 0; i < list.Count; i++)
                        {
                            e.MoveNext();
                            parametreRecherche += $"{e.Current}";

                            if (i < list.Count - 1)
                                parametreRecherche += "|";
                        }
                    }
                    else
                    {
                        parametreRecherche += $"{property.Name}={property.GetValue(model)}";
                    }
                }
            }

            return parametreRecherche;
        }
    }
}
