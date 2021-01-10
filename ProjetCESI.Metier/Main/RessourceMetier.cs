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

        public async Task SaveRessource(Ressource ressource)
        {
            await DataClass.InsertOrUpdate(ressource);

        }
    }
}
