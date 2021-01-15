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
        public async Task<IEnumerable<Commentaire>> GetAllCommentairesParentByRessourceId(int __ressourceId)
        {
            var results =  await DataClass.GetAllCommentairesParentByRessourceId(__ressourceId);

            foreach (var result in results)
            {
                result.CommentairesEnfant = result.CommentairesEnfant.OrderBy(c => c.DateModification).ToList();
            }

            return results;
        }

        public async Task<Commentaire> GetCommentaireComplet(int __commentaireId) => await DataClass.GetCommentaireComplet(__commentaireId);
    }
}
