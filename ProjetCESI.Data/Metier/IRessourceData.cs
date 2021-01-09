﻿using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public interface IRessourceData
    {
        Task<IEnumerable<Ressource>> GetAllPaginedRessource(int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllPaginedLastRessource(int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllAdvancedSearchPaginedRessource(string _search, List<int> _categories, List<int> _typeRelation, List<int> _typeRessource, DateTime? _dateDebut, DateTime? _dateFin, int _pagination = 20, int _pageOffset = 0);
        Task<IEnumerable<Ressource>> GetAllSearchPaginedRessource(string _search, int _pagination = 20, int _pageOffset = 0);
    }
}