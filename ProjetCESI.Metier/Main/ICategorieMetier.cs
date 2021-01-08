using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface ICategorieMetier
    {
        Task<IEnumerable<Categorie>> GetAll();
    }
}