using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class TypeRessource : EntiteBase
    {
        [DataMember]
        public string Nom { get; set; }

        public static bool operator ==(TypeRessource a, TypeRessources b)
        {
            return a.Id == (int)b;
        }

        public static bool operator !=(TypeRessource a, TypeRessources b)
        {
            return a.Id != (int)b;
        }
    }

    public enum TypeRessources
    {
        ActiviteJeu = 1,
        Article,
        CarteDefi,
        PDF,
        Exercice,
        FicheLecture,
        Jeu,
        Video
    }
}
