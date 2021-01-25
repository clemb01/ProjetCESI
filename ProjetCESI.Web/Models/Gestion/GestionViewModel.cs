using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class GestionViewModel : BaseViewModel
    {
        public string NomVue { get; set; }
        public List<Ressource> Ressources { get; set; }
        public List<User> Users { get; set; }
    }
}
