using ProjetCESI.Core;
using ProjetCESI.Data;
using ProjetCESI.Metier.Outils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class RessourceMetier : MetierBase<Ressource, RessourceData>, IRessourceMetier
    {
        public async Task<Ressource> GetRessourceComplete(int _ressourceId) => await DataClass.GetRessourceComplete(_ressourceId);

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetAllPaginedRessource(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0, bool __includeShared = false, bool __includePrivate = false) => await DataClass.GetAllPaginedRessource(_tri, _pagination, _pageOffset, __includeShared, __includePrivate);

        public async Task<IEnumerable<Ressource>> GetAllPaginedLastRessource(int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllPaginedLastRessource(_pagination, _pageOffset);

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetAllAdvancedSearchPaginedRessource(string _search, List<int> _categories, List<int> _typeRelation, List<int> _typeRessource, DateTime? _dateDebut, DateTime? _dateFin, TypeTriBase _typeTri = TypeTriBase.DateModification, int _pagination = 10, int _pageOffset = 0) => await DataClass.GetAllAdvancedSearchPaginedRessource(_search, _categories, _typeRelation, _typeRessource, _dateDebut, _dateFin, _typeTri, _pagination, _pageOffset);

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetAllSearchPaginedRessource(string _search, int _pagination = 10, int _pageOffset = 0) => await DataClass.GetAllSearchPaginedRessource(_search, _pagination, _pageOffset);

        public async Task<Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int>> GetUserFavoriteRessources(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            var result = await DataClass.GetUserFavoriteRessources(_userId, _search, _tri, _pagination, _pageOffset);

            return Tuple.Create(result.Item1, (IEnumerable<StatutActivite>)null, result.Item2);
        }

        public async Task<Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int>> GetUserRessourcesMiseDeCote(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            var result = await DataClass.GetUserRessourcesMiseDeCote(_userId, _search, _tri, _pagination, _pageOffset);

            return Tuple.Create(result.Item1, (IEnumerable<StatutActivite>)null, result.Item2);
        }

        public async Task<Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int>> GetUserRessourcesExploitee(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            var result = await DataClass.GetUserRessourcesExploitee(_userId, _search, _tri, _pagination, _pageOffset);

            return Tuple.Create(result.Item1, (IEnumerable<StatutActivite>)null, result.Item2);
        }

        public async Task<Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int>> GetUserRessourcesCreees(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            var result = await DataClass.GetUserRessourcesCreees(_userId, _search, _tri, _pagination, _pageOffset);

            return Tuple.Create(result.Item1, (IEnumerable<StatutActivite>)null, result.Item2);
        }

        public async Task<Tuple<IEnumerable<Ressource>, IEnumerable<StatutActivite>, int>> GetUserRessourcePrivees(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0)
        {
            var result = await DataClass.GetUserRessourcePrivees(_userId, _search, _tri, _pagination, _pageOffset);

            return Tuple.Create(result.Item1, (IEnumerable<StatutActivite>)null, result.Item2);
        }

        public async Task<IEnumerable<Ressource>> GetRessourcesNonValider(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetRessourcesNonValider(_tri, _pagination, _pageOffset);

        public async Task<IEnumerable<Tuple<int, string, string, List<string>, string, string, bool>>> GetRessourcesAccueil(TypeTriBase _tri = TypeTriBase.DateModificationDesc, int _pagination = 5, int _pageOffset = 0)
        {
            var result = await DataClass.GetAllPaginedRessource(_tri, _pagination, _pageOffset);

            return result.Item1.Select(c => Tuple.Create(
                c.Id,
                c.Categorie.Nom,
                c.Titre,
                c.TypeRelationsRessources.Select(a => a.TypeRelation.Nom).ToList(),
                c.TypeRessource.Nom,
                GenerateContenu(c.Contenu, (TypeRessources)c.TypeRessource.Id),
                c.RessourceOfficielle
            ));
        }

        public async Task<int> InitNewRessource(int __userId, TypeUtilisateur _userRole)
        {
            Ressource result = await DataClass.GetFirstEmptyRessource(__userId);

            if(result == null)
            {
                result = new Ressource
                {
                    UtilisateurCreateurId = __userId,
                    Statut = Statut.Empty,
                    RessourceOfficielle = _userRole != TypeUtilisateur.Citoyen
                };

                await DataClass.InsertOrUpdate(result);
            }

            return result.Id;
        }

        private string GenerateContenu(string contenu, TypeRessources typeRessource)
        {
            string content = string.Empty;

            if(typeRessource == TypeRessources.PDF)
            {
                content = "Il s'agit d'un pdf, pour le visualiser, veuillez cliquer sur la ressource.";
            }
            else if (typeRessource == TypeRessources.Video)
            {
                content = "Il s'agit d'une vidéo, pour la regarder, veuillez cliquer sur la ressource.";
            }
            else
            {
                if (contenu.Length > 300)
                {
                    contenu = contenu.TruncateHtml(300);
                }

                content = contenu;
            }

            return content;
        }

        public async Task<IEnumerable<Ressource>> GetRessourcesSuspendu(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetRessourcesSuspendu(_tri, _pagination, _pageOffset);
    }
}
