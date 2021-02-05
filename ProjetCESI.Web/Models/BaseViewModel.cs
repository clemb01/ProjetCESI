using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class BaseViewModel
    {
        public string Basepath { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Area { get; set; }
        public string Path { get; set; }
        public string QueryString { get; set; }
        public User Utilisateur { get; set; }
        public TypeUtilisateur UtilisateurRole { get; set; }
        public string Username { get; set; }
    }
}
