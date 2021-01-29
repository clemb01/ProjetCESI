using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class TypeRessourceData : Repository<TypeRessource>, ITypeRessourceData
    {
        public override async Task<IEnumerable<TypeRessource>> CreationDonneesTable()
        {
            List<TypeRessource> listeRessource = new List<TypeRessource>();

            listeRessource.Add(new TypeRessource { Nom = "Activité / Jeu à réaliser" });
            listeRessource.Add(new TypeRessource { Nom = "Article" });
            listeRessource.Add(new TypeRessource { Nom = "Carte défi" });
            listeRessource.Add(new TypeRessource { Nom = "Cours au format PDF" });
            listeRessource.Add(new TypeRessource { Nom = "Exercice / Atelier" });
            listeRessource.Add(new TypeRessource { Nom = "Fiche de lecture" });
            listeRessource.Add(new TypeRessource { Nom = "Jeu en ligne" });
            listeRessource.Add(new TypeRessource { Nom = "Vidéo" });

            await InsertOrUpdate(listeRessource);

            return listeRessource;
        }
    }
}
