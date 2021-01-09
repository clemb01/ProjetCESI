﻿using ProjetCESI.Core;
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
        public async Task<IEnumerable<Ressource>> GetAllPaginedRessource(int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllPaginedRessource(_pagination, _pageOffset);

        public async Task<IEnumerable<Ressource>> GetAllPaginedLastRessource(int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllPaginedRessource(_pagination, _pageOffset);

        public async Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<int> _categories, List<int> _typeRelation, List<int> _typeRessource, DateTime? _dateDebut, DateTime? _dateFin, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllAdvancedSearchPaginedRessource(_search, _categories, _typeRelation, _typeRessource, _dateDebut, _dateFin, _pagination, _pageOffset);

        public async Task<IEnumerable<Ressource>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0) => await DataClass.GetAllSearchPaginedRessource(_search, _pagination, _pageOffset);
    }
}
