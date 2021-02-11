using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Data;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Area
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GestionAPIController : BaseAPIController
    {
        [HttpGet("Gestion")]
        public async Task<ResponseAPI> Gestion(GestionViewModel model)
        {
            var response = new ResponseAPI();

            PrepareModel(model);

            if (model.NomVue == "Validation")
            {
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetRessourcesNonValider()).ToList();
                response.Data = model;
                if (model.Ressources == null)
                {
                    response.StatusCode = "500";
                    response.Message = "Une erreur est survenue";
                    response.Data = null;
                }
            }
            else if (model.NomVue == "UserList")
            {
                model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
                response.Data = model;
                if (model.Users == null || model == null)
                {
                    response.StatusCode = "500";
                    response.Message = "Une erreur est survenue";
                    response.Data = null;
                }
            }
            else if (model.NomVue == "statistique")
            {
                await UpdateStatistique(model);
                response.Data = model;
            }
            else if (model.NomVue == "Parametre")
            {
                model.categories = (await MetierFactory.CreateCategorieMetier().GetAll()).ToList();
                response.Data = model;
                if (model.categories == null)
                {
                    response.StatusCode = "500";
                    response.Message = "Une erreur est survenue";
                    response.Data = null;
                }
            }
            else if (model.NomVue == "suspendu")
            {
                model.Ressources = (await MetierFactory.CreateRessourceMetier().GetRessourcesSuspendu()).ToList();
                response.Data = model;
                if (model.Ressources == null)
                {
                    response.StatusCode = "500";
                    response.Message = "Une erreur est survenue";
                    response.Data = null;
                }
            }
            else
            {
                response.StatusCode = "404";
                response.Message = "Erreur page non trouvée";
            }

            return response;
        }

        [HttpPost("ModifParamCategorie")]
        public async Task<ResponseAPI> ModifParamCategorie(int id, string nomCategorie)
        {
            var response = new ResponseAPI();

            var categorie = await MetierFactory.CreateCategorieMetier().GetById(id);
            categorie.Nom = nomCategorie;
            var result = await MetierFactory.CreateCategorieMetier().InsertOrUpdate(categorie);

            response.StatusCode = "200";

            return response;
        }

        [HttpPost("AddCategorie")]
        public async Task<ResponseAPI> AddCategorie(string newCategorie)
        {
            var response = new ResponseAPI();

            Core.Categorie NewCate = new Core.Categorie();
            NewCate.Nom = newCategorie;
            await MetierFactory.CreateCategorieMetier().InsertOrUpdate(NewCate);

            response.StatusCode = "200";

            return response;
        }

        [HttpPost("DeleteCategorie")]
        public async Task<ResponseAPI> DeleteCategorie(int id)
        {
            var response = new ResponseAPI();

            var categorie = await MetierFactory.CreateCategorieMetier().GetById(id);
            var result = await MetierFactory.CreateCategorieMetier().DeleteCategorie(categorie);

            response.StatusCode = "200";

            return response;
        }

        [HttpGet("UpdateTopRechercheDisplay")]
        public async Task<ResponseAPI> UpdateTopRechercheDisplay(int selectedRange)
        {
            var response = new ResponseAPI();

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

            response.StatusCode = "200";
            response.Data = new { parametres = json.Select(c => c.Parametre), count = json.Select(c => c.Count) };

            return response;
        }

        [HttpGet("UpdateTopConsultationDisplay")]
        public async Task<ResponseAPI> UpdateTopConsultationDisplay(int selectedRange)
        {
            var response = new ResponseAPI();

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

            response.StatusCode = "200";
            response.Data = new { parametres = json.Select(c => c.Parametre), count = json.Select(c => c.Count) };

            return response;
        }

        [HttpGet("UpdateTopActionsDisplay")]
        public async Task<ResponseAPI> UpdateTopActionsDisplay(int selectedRange)
        {
            var response = new ResponseAPI();

            object json = new object();
            List<TopObject> result;

            switch (selectedRange)
            {
                case 1:
                    result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Day, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

                    json = new { parametres = result.Select(c => c.Parametre), count = result.Select(c => c.Count) };
                    break;
                case 2:
                    {
                        var firstDayOfWeek = DateTimeOffset.Now.AddDays(-((int)DateTimeOffset.Now.DayOfWeek - (int)System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek));
                        firstDayOfWeek = new DateTimeOffset(firstDayOfWeek.Date, firstDayOfWeek.Offset);

                        int diff = ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday) == 0 ? 7 : ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday);
                        var lastDayOfWeek = DateTimeOffset.Now.AddDays(7 - diff);
                        lastDayOfWeek = new DateTimeOffset(lastDayOfWeek.Date, lastDayOfWeek.Offset).AddHours(23).AddMinutes(59).AddSeconds(59);

                        result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Week, firstDayOfWeek, lastDayOfWeek)).ToList();

                        json = new { parametres = result.Select(c => c.Parametre), count = result.Select(c => c.Count) };

                        break;
                    }
                case 3:
                    result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Month, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

                    json = new { parametres = result.Select(c => c.Parametre), count = result.Select(c => c.Count) };
                    break;
                case 4:
                    result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Year, new DateTimeOffset(DateTimeOffset.Now.Year, 1, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, 12, 31, 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

                    json = new { parametres = result.Select(c => c.Parametre), count = result.Select(c => c.Count) };
                    break;
                default:
                    result = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Month, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

                    json = new { parametres = result.Select(c => c.Parametre), count = result.Select(c => c.Count) };
                    break;
            }

            response.StatusCode = "200";
            response.Data = json;

            return response;
        }

        [HttpGet("ExportCSV")]
        public async Task<ResponseAPI> ExportCSV(TimestampFilter periode)
        {
            var response = new ResponseAPI();

            string filename = string.Empty;

            DateTimeOffset dtBas;
            DateTimeOffset dtHaut;

            switch (periode)
            {
                case TimestampFilter.Day:
                    dtBas = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 0, 0, 0, DateTimeOffset.Now.Offset);
                    dtHaut = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTimeOffset.Now.Day, 23, 59, 59, DateTimeOffset.Now.Offset);
                    filename = $"Export_{dtBas.Date.ToShortDateString()}.csv";
                    break;
                case TimestampFilter.Week:
                    var firstDayOfWeek = DateTimeOffset.Now.AddDays(-((int)DateTimeOffset.Now.DayOfWeek - (int)System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek));
                    dtBas = new DateTimeOffset(firstDayOfWeek.Date, firstDayOfWeek.Offset);

                    int diff = ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday) == 0 ? 7 : ((int)DateTimeOffset.Now.DayOfWeek - (int)DayOfWeek.Sunday);
                    var lastDayOfWeek = DateTimeOffset.Now.AddDays(7 - diff);
                    dtHaut = new DateTimeOffset(lastDayOfWeek.Date, lastDayOfWeek.Offset).AddHours(23).AddMinutes(59).AddSeconds(59);
                    filename = $"Export_{dtBas.Year}_Semaine_{System.Threading.Thread.CurrentThread.CurrentCulture.Calendar.GetWeekOfYear(dtBas.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday)}.csv";
                    break;
                case TimestampFilter.Year:
                    dtBas = new DateTimeOffset(DateTimeOffset.Now.Year, 1, 1, 0, 0, 0, DateTimeOffset.Now.Offset);
                    dtHaut = new DateTimeOffset(DateTimeOffset.Now.Year, 12, 31, 23, 59, 59, DateTimeOffset.Now.Offset);
                    filename = $"Export_Annee_{dtBas.Date.Year}.csv";
                    break;
                case TimestampFilter.Month:
                default:
                    dtBas = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset);
                    dtHaut = new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset);
                    filename = $"Export_{System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(dtBas.Month)}.csv";
                    break;
            }

            try
            {
                var result = await MetierFactory.CreateStatistiqueMetier().GenerateCSVData(10, dtBas, dtHaut, periode);

                byte[] bytes = Encoding.Unicode.GetBytes(result);

                response.StatusCode = "200";
                response.Data = File(bytes, "application/octet-stream", filename);

                return response;
            }
            catch (Exception ex)
            {
                response.IsError = true;
                response.Message = ex.Message;

                return response;
            }
        }

        private async Task UpdateStatistique(GestionViewModel model)
        {
            var recherches = (await MetierFactory.CreateStatistiqueMetier().GetTopRecherche(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

            var consultations = (await MetierFactory.CreateStatistiqueMetier().GetTopConsultation(10, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset))).ToList();

            var actions = (await MetierFactory.CreateStatistiqueMetier().GetNombreActionsMoyenneParUtilisateurs(TimestampFilter.Month, new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, 1, 0, 0, 0, DateTimeOffset.Now.Offset), new DateTimeOffset(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month, DateTime.DaysInMonth(DateTimeOffset.Now.Year, DateTimeOffset.Now.Month), 23, 59, 59, DateTimeOffset.Now.Offset)));

            var exploites = (await MetierFactory.CreateUtilisateurRessourceMetier().GetTopExploitee(10)).ToList();

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
                Parametres = actions.Select(c => c.Parametre).ToList()
            };

            model.TopExploites = new TopStats
            {
                Count = exploites.Select(c => c.Count).ToList(),
                Parametres = exploites.Select(c => c.Parametre).ToList()
            };
        }
    }
}

