using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class CommentaireMetier : MetierBase<Commentaire, CommentaireData>, ICommentaireMetier
    {
        public async Task<IEnumerable<Commentaire>> GetAllCommentairesParentByRessourceId(int __ressourceId) => await DataClass.GetAllCommentairesParentByRessourceId(__ressourceId);
    }
}
