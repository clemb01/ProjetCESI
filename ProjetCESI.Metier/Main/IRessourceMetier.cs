using ProjetCESI.Core;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public interface IRessourceMetier
    {
        Task SaveRessource(Ressource ressource);
    }
}