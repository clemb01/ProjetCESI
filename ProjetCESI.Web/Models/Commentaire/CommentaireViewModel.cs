using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Models
{
    public class CommentaireViewModel : BaseViewModel
    {
        public Commentaire Commentaire { get; set; }
    }

    public class CommentairesViewModel : BaseViewModel
    {
        public List<Commentaire> Commentaires { get; set; }
    }
}
