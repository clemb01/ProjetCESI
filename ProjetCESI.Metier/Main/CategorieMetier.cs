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
        public User GetUser() => DataClass.GetUser();
    }
}
