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

        public List<UtilisateurRessource> UtilisateurRessources { get; set; }
    }

    public enum TypeUtilisateur
    {
        [EnumMember]
        Aucun,
        [EnumMember]
        Client,
        [EnumMember]
        Moderateur,
        [EnumMember]
        Admin
    }
}
