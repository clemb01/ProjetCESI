using ProjetCESI.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface ICategorieMetier : IMetier<Categorie>
    {
        Task<bool> DeleteCategorie(Categorie __categorie);
    }
}