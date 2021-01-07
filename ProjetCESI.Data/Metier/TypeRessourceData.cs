using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data.Metier
{
    public class TypeRessourceData : Repository<TypeRessource>, ITypeRessourceData
    {
        public override async Task<IEnumerable<TypeRessource>> CreationDonneesTable()
        {
            List<TypeRessource> listeRessource = new List<TypeRessource>();

            listeRessource.Add(new TypeRessource { NomRessource = "Activité / Jeu à réaliser" });
            listeRessource.Add(new TypeRessource { NomRessource = "Article" });
            listeRessource.Add(new TypeRessource { NomRessource = "Carte défi" });
            listeRessource.Add(new TypeRessource { NomRessource = "Cours au format PDF" });
            listeRessource.Add(new TypeRessource { NomRessource = "Exercice / Atelier" });
            listeRessource.Add(new TypeRessource { NomRessource = "Fiche de lecture" });
            listeRessource.Add(new TypeRessource { NomRessource = "Jeu en ligne" });
            listeRessource.Add(new TypeRessource { NomRessource = "Vidéo" });

            await InsertOrUpdate(listeRessource);

            return listeRessource;
        }
    }
}
