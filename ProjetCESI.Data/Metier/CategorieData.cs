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

            listeCategorie.Add(new Categorie { Nom = "Communication" });
            listeCategorie.Add(new Categorie { Nom = "Cultures" });
            listeCategorie.Add(new Categorie { Nom = "Développement personnel" });
            listeCategorie.Add(new Categorie { Nom = "Intelligence émotionnelle" });
            listeCategorie.Add(new Categorie { Nom = "Loisirs" });
            listeCategorie.Add(new Categorie { Nom = "Monde professionnel" });
            listeCategorie.Add(new Categorie { Nom = "Parentalité" });
            listeCategorie.Add(new Categorie { Nom = "Qualité de vie" });
            listeCategorie.Add(new Categorie { Nom = "Recherche de sens" });
            listeCategorie.Add(new Categorie { Nom = "Santé physique" });
            listeCategorie.Add(new Categorie { Nom = "Santé psychique" });
            listeCategorie.Add(new Categorie { Nom = "Spiritualité" });
            listeCategorie.Add(new Categorie { Nom = "Vie affective" });

            await InsertOrUpdate(listeCategorie);

            return listeCategorie;
        }
    }
}
