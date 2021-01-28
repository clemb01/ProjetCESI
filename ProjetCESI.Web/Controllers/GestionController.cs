using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Data.Metier;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    [Authorize]
    public class GestionController : BaseController
    {
        [Route("Gestion")]
        public async Task<IActionResult> Gestion(GestionViewModel model)
        {
            PrepareModel(model);

            if (model.NomVue == "Validation")
            {
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetRessourcesNonValider()).ToList();
                if (model.Ressources == null)
                {
                    return View();
                }
            }
            else if (model.NomVue == "UserList")
            {
                model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
                if (model.Users == null)
                {
                    return View();
                }
            }
            else if (model.NomVue == "statistique")
            {
                await UpdateStatistique(model);

                return View(model);
            }
            else
                return RedirectToAction("Accueil", "Accueil");

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> UpdateTopRechercheDisplay(int selectedRange)
        {
            List<TopObject> json = new List<TopObject>();

            switch (selectedRange)
            {
                case 1:
                    json = (await MetierFactory.CreateStatistiqueMetier().GetTopRecherche(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();
                    break;
                case 2:
                    {
                        var firstDayOfWeek = DateTimeOffset.Now.AddDays(-((int)DateTimeOffset.Now.DayOfWeek - (int)System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek));
                        firstDayOfWeek = new DateTimeOffset(firstDayOfWeek.Date, firstDayOfWeek.Offset);

                        int diff = ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday) == 0 ? 7 : ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday);
                        var lastDayOfWeek = DateTimeOffset.Now.AddDays(7 - diff);
                        lastDayOfWeek = new DateTimeOffset(lastDayOfWeek.Date, lastDayOfWeek.Offset).AddHours(23).AddMinutes(59).AddSeconds(59);

                        json = (await MetierFactory.CreateStatistiqueMetier().GetTopRecherche(10, firstDayOfWeek, lastDayOfWeek)).ToList();

                        break;
                    }
                case 3:
                    json = (await MetierFactory.CreateStatistiqueMetier().GetTopRecherche(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();
                    break;
                case 4:
                    json = (await MetierFactory.CreateStatistiqueMetier().GetTopRecherche(10, new DateTimeOffset(DateTimeOffset.Now.Year, 1, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, 12, 31, 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();
                    break;
                default:
                    json = (await MetierFactory.CreateStatistiqueMetier().GetTopRecherche(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();
                    break;
            }

            return Json(new { parametres = json.Select(c => c.Parametre), count = json.Select(c => c.Count) });
        }

        [HttpGet]
        public async Task<JsonResult> UpdateTopConsultationDisplay(int selectedRange)
        {
            List<TopObject> json = new List<TopObject>();

            switch (selectedRange)
            {
                case 1:
                    json = (await MetierFactory.CreateStatistiqueMetier().GetTopConsultation(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();
                    break;
                case 2:
                    {
                        var firstDayOfWeek = DateTimeOffset.Now.AddDays(-((int)DateTimeOffset.Now.DayOfWeek - (int)System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek));
                        firstDayOfWeek = new DateTimeOffset(firstDayOfWeek.Date, firstDayOfWeek.Offset);

                        int diff = ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday) == 0 ? 7 : ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday);
                        var lastDayOfWeek = DateTimeOffset.Now.AddDays(7 - diff);
                        lastDayOfWeek = new DateTimeOffset(lastDayOfWeek.Date, lastDayOfWeek.Offset).AddHours(23).AddMinutes(59).AddSeconds(59);

                        json = (await MetierFactory.CreateStatistiqueMetier().GetTopConsultation(10, firstDayOfWeek, lastDayOfWeek)).ToList();

                        break;
                    }
                case 3:
                    json = (await MetierFactory.CreateStatistiqueMetier().GetTopConsultation(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();
                    break;
                case 4:
                    json = (await MetierFactory.CreateStatistiqueMetier().GetTopConsultation(10, new DateTimeOffset(DateTimeOffset.Now.Year, 1, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, 12, 31, 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();
                    break;
                default:
                    json = (await MetierFactory.CreateStatistiqueMetier().GetTopConsultation(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();
                    break;
            }

            return Json(new { parametres = json.Select(c => c.Parametre), count = json.Select(c => c.Count) });
        }

        [HttpGet]
        public async Task<JsonResult> UpdateTopActionsDisplay(int selectedRange)
        {
            object json = new object();
            List<TopActions> result;

            switch (selectedRange)
            {
                case 1:
                    result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Day, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

                    json = new { parametres = result.Select(c => c.Date), count = result.Select(c => c.Count) };
                    break;
                case 2:
                    {
                        var firstDayOfWeek = DateTimeOffset.Now.AddDays(-((int)DateTimeOffset.Now.DayOfWeek - (int)System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek));
                        firstDayOfWeek = new DateTimeOffset(firstDayOfWeek.Date, firstDayOfWeek.Offset);

                        int diff = ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday) == 0 ? 7 : ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday);
                        var lastDayOfWeek = DateTimeOffset.Now.AddDays(7 - diff);
                        lastDayOfWeek = new DateTimeOffset(lastDayOfWeek.Date, lastDayOfWeek.Offset).AddHours(23).AddMinutes(59).AddSeconds(59);

                        result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Week, firstDayOfWeek, lastDayOfWeek)).ToList();

                        json = new { parametres = result.Select(c => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(c.Date.DayOfWeek)), count = result.Select(c => c.Count) };

                        break;
                    }
                case 3:
                    result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Month, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

                    json = new { parametres = result.Select(c => c.Date.ToShortDateString()), count = result.Select(c => c.Count) };
                    break;
                case 4:
                    result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Year, new DateTimeOffset(DateTimeOffset.Now.Year, 1, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, 12, 31, 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

                    json = new { parametres = result.Select(c => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(c.Date.Month)), count = result.Select(c => c.Count) };
                    break;
                default:
                    result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Month, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

                    json = new { parametres = result.Select(c => c.Date.ToShortDateString()), count = result.Select(c => c.Count) };
                    break;
            }

            return Json(json);
        }

        private async Task UpdateStatistique(GestionViewModel model)
        {
            var recherches = (await MetierFactory.CreateStatistiqueMetier().GetTopRecherche(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

            var consultations = (await MetierFactory.CreateStatistiqueMetier().GetTopConsultation(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

            var actions = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Month, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset)));

            model.TopRecherches = new TopStats
            {
                Count = recherches.Select(c => c.Count).ToList(),
                Parametres = recherches.Select(c => c.Parametre).ToList()
            };

            model.TopConsultations = new TopStats
            {
                Count = consultations.Select(c => c.Count).ToList(),
                Parametres = consultations.Select(c => c.Parametre).ToList()
            };

            model.TopActions = new TopStats
            {
                Count = actions.Select(c => c.Count).ToList(),
                Parametres = actions.Select(c => c.Date.ToString()).ToList()
            };
        }
    }
}
