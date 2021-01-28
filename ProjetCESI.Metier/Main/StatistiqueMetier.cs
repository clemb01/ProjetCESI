using ProjetCESI.Core;
using ProjetCESI.Data.Metier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjetCESI.Data.Metier.StatistiqueData;

namespace ProjetCESI.Metier
{
    public class StatistiqueMetier : MetierBase<Statistique, StatistiqueData>, IStatistiqueMetier
    {
        public async Task<IEnumerable<TopActions>> GetNombreActionsMoyenneParUtilisateurs(TimestampFilter __filter, DateTimeOffset __whereBas, DateTimeOffset __whereHaut)
        {
            IEnumerable<TopActions> resultats = await DataClass.GetNombreActionsMoyenneParUtilisateurs(__filter, __whereBas, __whereHaut);

            List<TopActions> result = new List<TopActions>();

            switch(__filter)
            {
                case TimestampFilter.Day:
                {
                    for (int i = 0; i < 8; i++)
                    {
                        result.Add(new TopActions
                        {
                            Date = new DateTime(__whereBas.Year, __whereBas.Month, __whereBas.Day, i * 3, 0, 0),
                            Count = resultats.Where(c => c.Date.TimeOfDay >= TimeSpan.FromHours(i * 3) && c.Date.TimeOfDay < TimeSpan.FromHours(i * 3 + 3)).Sum(c => c.Count)
                        });
                    }

                    break;
                    }
                case TimestampFilter.Week:
                {
                    for (int i = 0; i < 7; i++)
                    {
                        result.Add(new TopActions
                        {
                            Date = __whereBas.Date.AddDays(i),
                            Count = resultats.Where(c => c.Date.DayOfYear == __whereBas.Date.AddDays(i).DayOfYear).Sum(c => c.Count)
                        });
                    }

                    break;
                }
                case TimestampFilter.Month:
                {
                    for (int i = 1; i <= 31; i += 5)
                    {
                        var date = new DateTime(__whereBas.Year, __whereBas.Month, i > DateTime.DaysInMonth(__whereBas.Year, __whereBas.Month) ? DateTime.DaysInMonth(__whereBas.Year, __whereBas.Month) : i, 0, 0, 0);

                        result.Add(new TopActions
                        {
                            Date = date,
                            Count = resultats.Where(c => c.Date >= date && c.Date < date.AddDays(5).AddSeconds(-1)).Sum(c => c.Count)
                        });
                    }

                    break;
                }
                case TimestampFilter.Year:
                {
                    for (int i = 1; i <= 12; i++)
                    {
                        result.Add(new TopActions
                        {
                            Date = new DateTime(__whereBas.Year, i, 1, 0, 0, 0),
                            Count = resultats.Where(c => c.Date.Month == i).Sum(c => c.Count)
                        });
                    }

                    break;
                }
                default:
                {
                    for (int i = 0; i < 8; i++)
                    {
                        result.Add(new TopActions
                        {
                            Date = new DateTime(__whereBas.Year, __whereBas.Month, __whereBas.Day, i * 3, 0, 0),
                            Count = resultats.Where(c => c.Date.TimeOfDay >= TimeSpan.FromHours(i * 3) && c.Date.TimeOfDay < TimeSpan.FromHours(i * 3 + 3)).Sum(c => c.Count)
                        });
                    }

                    break;
                }
            }

            return result;
        }

        public async Task<IEnumerable<TopObject>> GetTopRecherche(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut)
        {
            var resultatsFinaux = new List<TopObject>();
            int recherche = __nbRecherche;
            int offset = 0;

            while (resultatsFinaux.Count < __nbRecherche)
            {
                var resultats = (await DataClass.GetTopRecherche(recherche, __whereBas, __whereHaut, offset)).ToList();

                if (!resultats.Any())
                    break;

                foreach (var resultat in resultats)
                {
                    if (resultat.Parametre.IndexOf("&") != -1)
                        resultat.Parametre = resultat.Parametre[(resultat.Parametre.IndexOf("=") + 1)..resultat.Parametre.IndexOf("&")];
                    else
                        resultat.Parametre = resultat.Parametre[(resultat.Parametre.IndexOf("=") + 1)..];
                }

                resultatsFinaux.AddRange(resultats);
                resultatsFinaux = resultatsFinaux.GroupBy(c => c.Parametre, StringComparer.InvariantCultureIgnoreCase).Select(c => new TopObject { Parametre = c.Key, Count = c.Sum(c => c.Count) }).ToList();

                if (resultatsFinaux.Count < __nbRecherche)
                {
                    offset = recherche;
                    recherche *= 2;
                }
            }

            return resultatsFinaux.Take(__nbRecherche);
        }

        public async Task<IEnumerable<TopObject>> GetTopConsultation(int __nbRecherche, DateTimeOffset __whereBas, DateTimeOffset __whereHaut)
        {
            var resultatsFinaux = new List<TopObject>();
            int recherche = __nbRecherche;
            int offset = 0;

            while (resultatsFinaux.Count < __nbRecherche)
            {
                var resultats = (await DataClass.GetTopConsultation(recherche, __whereBas, __whereHaut, offset)).ToList();

                if (!resultats.Any())
                    break;

                foreach (var resultat in resultats)
                {
                    if (resultat.Parametre.IndexOf("&") != -1)
                        resultat.Parametre = resultat.Parametre[(resultat.Parametre.IndexOf("=") + 1)..resultat.Parametre.IndexOf("&")];
                    else
                        resultat.Parametre = resultat.Parametre[(resultat.Parametre.IndexOf("=") + 1)..];
                }

                resultatsFinaux.AddRange(resultats);
                resultatsFinaux = resultatsFinaux.GroupBy(c => c.Parametre, StringComparer.InvariantCultureIgnoreCase).Select(c => new TopObject { Parametre = c.Key, Count = c.Sum(c => c.Count) }).ToList();

                if (resultatsFinaux.Count < __nbRecherche)
                {
                    offset = recherche;
                    recherche *= 2;
                }
            }

            return resultatsFinaux.Take(__nbRecherche);
        }
    }
}
