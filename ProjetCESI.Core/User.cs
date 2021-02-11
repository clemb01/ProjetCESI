using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class User : IdentityUser<int>, IGetPrimaryKey
    {
        public int GetPrimaryKey()
        {
            return Id;
        }

        public bool UtilisateurSupprime { get; set; }

        public List<UtilisateurRessource> UtilisateurRessources { get; set; }
        public List<Commentaire> Commentaires { get; set; }
        public List<Ressource> RessourcesCree { get; set; }
    }

    public enum TypeUtilisateur
    {
        [EnumMember]
        Citoyen = 1,
        [EnumMember]
        Moderateur,
        [EnumMember]
        Admin,
        [EnumMember]
        SuperAdmin
    }
}
