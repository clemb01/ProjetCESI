using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class RessourceMetier : MetierBase<Ressource, RessourceData>, IRessourceMetier
    {
        public async Task<Ressource> GetRessourceComplete(int _ressourceId) => await DataClass.GetRessourceComplete(_ressourceId);

        public async Task<IEnumerable<Ressource>> GetAllPaginedRessource(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllPaginedRessource(_tri, _pagination, _pageOffset);

        public async Task<IEnumerable<Ressource>> GetAllPaginedLastRessource(int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllPaginedLastRessource(_pagination, _pageOffset);

        public async Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<int> _categories, List<int> _typeRelation, List<int> _typeRessource, DateTime? _dateDebut, DateTime? _dateFin, TypeTriBase _typeTri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllAdvancedSearchPaginedRessource(_search, _categories, _typeRelation, _typeRessource, _dateDebut, _dateFin, _typeTri, _pagination, _pageOffset);

        public async Task<IEnumerable<Ressource>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllSearchPaginedRessource(_search, _pagination, _pageOffset);

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetUserFavoriteRessources(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetUserFavoriteRessources(_userId, _search, _tri, _pagination, _pageOffset);

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesMiseDeCote(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetUserRessourcesMiseDeCote(_userId, _search, _tri, _pagination, _pageOffset);

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesExploitee(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetUserRessourcesExploitee(_userId, _search, _tri, _pagination, _pageOffset);

        public async Task<Tuple<IEnumerable<Ressource>, int>> GetUserRessourcesCreees(int _userId, string _search = null, TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetUserRessourcesCreees(_userId, _search, _tri, _pagination, _pageOffset);

        public async Task<IEnumerable<Ressource>> GetRessourcesNonValider(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetRessourcesNonValider(_tri, _pagination, _pageOffset);

        public async Task SaveRessource(Ressource ressource)
        {
            await DataClass.InsertOrUpdate(ressource);
        }

        public async Task<IEnumerable<Tuple<int, string, string, List<string>, string, string>>> GetRessourcesAccueil(TypeTriBase _tri = TypeTriBase.DateModificationDesc, int _pagination = 5, int _pageOffset = 0)
        {
            var result = await DataClass.GetAllPaginedRessource(_tri, _pagination, _pageOffset);

            return result.Select(c => Tuple.Create(
                c.Id,
                c.Categorie.Nom,
                c.Titre,
                c.TypeRelationsRessources.Select(a => a.TypeRelation.Nom).ToList(),
                c.TypeRessource.Nom,
                GenerateContenu(c.Contenu, (TypeRessources)c.TypeRessource.Id)
            ));
        }

        public async Task<int> InitNewRessource(int __userId)
        {
            Ressource result = await DataClass.GetFirstEmptyRessource(__userId);

            if(result == null)
            {
                result = new Ressource
                {
                    UtilisateurCreateurId = __userId,
                    Statut = Statut.Empty
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
                if (!string.IsNullOrEmpty(contenu) && contenu.Length > 100)
                    content = contenu.Substring(0, 100) + "...";
                else
                    content = contenu;
            }

            return content;
        }

        public async Task<IEnumerable<Ressource>> GetRessourcesSuspendu(TypeTriBase _tri = TypeTriBase.DateModification, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetRessourcesSuspendu(_tri, _pagination, _pageOffset);
    }
}
