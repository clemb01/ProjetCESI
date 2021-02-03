using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class CategorieMetier : MetierBase<Categorie, CategorieData>, ICategorieMetier
    {
        public async Task<bool> DeleteCategorie(Categorie __categorie)
        {
            bool result = false;

            if(await Delete(__categorie))
                result = await DataClass.CreateNewDataClass<RessourceData>().ResetRessourceStatutWhereCategoryIsNull();

            return result;
        }
    }
}
