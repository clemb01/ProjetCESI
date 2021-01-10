using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class ConsultationViewModel : BaseViewModel
    {
        public ListRessourceViewModel Ressources { get; set; } = new ListRessourceViewModel();
    }
}
