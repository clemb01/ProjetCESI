using Microsoft.EntityFrameworkCore;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data
{
    public class CommentaireData : Repository<Commentaire>, ICommentaireData
    {
        public async Task<IEnumerable<Commentaire>> GetAllCommentairesParentByRessourceId(int __ressourceId)
        {
            using(var ctx = GetContext())
            {
                return await ctx.Set<Commentaire>()
                            .Include(c => c.Utilisateur)
                            .Include(c => c.CommentairesEnfant)
                            .ThenInclude(c => c.Utilisateur)
                            .Where(c => c.Statut != StatutCommentaire.Supprime && c.RessourceId == __ressourceId && c.CommentaireParent == null)
                            .OrderByDescending(c => c.DateCreation)
                            .ToListAsync();
            }
        }

        public async Task<Commentaire> GetCommentaireComplet(int __commentaireId)
        {
            using (var ctx = GetContext())
            {
                return await ctx.Set<Commentaire>()
                            .Include(c => c.Utilisateur)
                            .Include(c => c.CommentairesEnfant)
                            .ThenInclude(c => c.Utilisateur)
                            .FirstOrDefaultAsync(c => c.Id == __commentaireId);
            }
        }
    }
}
