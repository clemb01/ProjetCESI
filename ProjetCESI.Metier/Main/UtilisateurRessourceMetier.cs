using ProjetCESI.Core;
using ProjetCESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class UtilisateurRessourceMetier : MetierBase<UtilisateurRessource, UtilisateurRessourceData>, IUtilisateurRessourceMetier
    {
        public async Task<UtilisateurRessource> GetByUtilisateurAndRessourceId(int __utilisateurId, int __ressourceId)
        {
            var result = await DataClass.GetByUtilisateurAndRessourceId(__utilisateurId, __ressourceId);

            if(result == null)
            {
                result = new UtilisateurRessource()
                {
                    EstExploite = false,
                    EstFavoris = false,
                    EstMisDeCote = false,
                    RessourceId = __ressourceId,
                    UtilisateurId = __utilisateurId
                };

                await DataClass.InsertOrUpdate(result);
            }

            return result;
        }

        public async Task<bool> AjouterFavoris(int _utilisateurId, int _ressourceId)
        {
            var ur = await DataClass.GetByUtilisateurAndRessourceId(_utilisateurId, _ressourceId);

            ur.EstFavoris = true;

            return await DataClass.InsertOrUpdate(ur);
        }

        public async Task<bool> SupprimerFavoris(int _utilisateurId, int _ressourceId)
        {
            var ur = await DataClass.GetByUtilisateurAndRessourceId(_utilisateurId, _ressourceId);

            ur.EstFavoris = false;

            return await DataClass.InsertOrUpdate(ur);
        }

        public async Task<bool> MettreDeCote(int _utilisateurId, int _ressourceId)
        {
            var ur = await DataClass.GetByUtilisateurAndRessourceId(_utilisateurId, _ressourceId);

            ur.EstMisDeCote = true;

            return await DataClass.InsertOrUpdate(ur);
        }

        public async Task<bool> DeMettreDeCote(int _utilisateurId, int _ressourceId)
        {
            var ur = await DataClass.GetByUtilisateurAndRessourceId(_utilisateurId, _ressourceId);

            ur.EstMisDeCote = false;

            return await DataClass.InsertOrUpdate(ur);
        }

        public async Task<bool> EstExploite(int _utilisateurId, int _ressourceId)
        {
            var ur = await DataClass.GetByUtilisateurAndRessourceId(_utilisateurId, _ressourceId);

            ur.EstExploite = true;

            return await DataClass.InsertOrUpdate(ur);
        }

        public async Task<bool> PasExploite(int _utilisateurId, int _ressourceId)
        {
            var ur = await DataClass.GetByUtilisateurAndRessourceId(_utilisateurId, _ressourceId);

            ur.EstExploite = false;

            return await DataClass.InsertOrUpdate(ur);
        }
    }
}
