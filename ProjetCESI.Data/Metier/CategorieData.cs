using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class CategorieData : Repository<Categorie>, ICategorieData
    {
        public override async Task<IEnumerable<Categorie>> CreationDonneesTable()
        {
            List<Categorie> listeCategorie = new List<Categorie>();

            listeCategorie.Add(new Categorie { NomCategorie = "Communication" });
            listeCategorie.Add(new Categorie { NomCategorie = "Cultures" });
            listeCategorie.Add(new Categorie { NomCategorie = "Développement personnel" });
            listeCategorie.Add(new Categorie { NomCategorie = "Intelligence émotionnelle" });
            listeCategorie.Add(new Categorie { NomCategorie = "Loisirs" });
            listeCategorie.Add(new Categorie { NomCategorie = "Monde professionnel" });
            listeCategorie.Add(new Categorie { NomCategorie = "Parentalité" });
            listeCategorie.Add(new Categorie { NomCategorie = "Qualité de vie" });
            listeCategorie.Add(new Categorie { NomCategorie = "Recherche de sens" });
            listeCategorie.Add(new Categorie { NomCategorie = "Santé physique" });
            listeCategorie.Add(new Categorie { NomCategorie = "Santé psychique" });
            listeCategorie.Add(new Categorie { NomCategorie = "Spiritualité" });
            listeCategorie.Add(new Categorie { NomCategorie = "Vie affective" });

            await InsertOrUpdate(listeCategorie);

            return listeCategorie;
        }

        public User GetUser()
        {
            using(DbContext ctx = GetContext())
            {
                var result = ctx.Set<User>().FirstOrDefault();

                return result;
            }
        }
    }
}
