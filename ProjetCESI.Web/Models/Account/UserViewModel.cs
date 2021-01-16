using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class UserViewModel
    {
        public User Utilisateur { get; set; }

        public enum TypeUtilisateur
        {
            [EnumMember]
            Aucun,
            [EnumMember]
            Citoyen,
            [EnumMember]
            Moderateur,
            [EnumMember]
            Admin,
            [EnumMember]
            SuperAdmin
        }
    }
}
