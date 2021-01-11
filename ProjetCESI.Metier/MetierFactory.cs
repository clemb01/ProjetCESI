using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class MetierFactory
    {
        private int? _userId;

        public MetierFactory(int? _userId)
        {
            this._userId = _userId;
        }

        private T GetMetier<T>(IMetierBase __metierBase) where T : class, new()
        {
            var metier = new T();
            IMetierBase metierBase = metier as IMetierBase;

            if (metierBase != null)
            {
                metierBase.UserId = _userId;
            }

            return metier;
        }

        public IApplicationRoleMetier CreateApplicationRoleMetier(IMetierBase metierBase = null) => GetMetier<ApplicationRoleMetier>(metierBase);
        public ICategorieMetier CreateCategorieMetier(IMetierBase metierBase = null) => GetMetier<CategorieMetier>(metierBase);
        public ICommentaireMetier CreateCommentaireMetier(IMetierBase metierBase = null) => GetMetier<CommentaireMetier>(metierBase);
        public IRessourceMetier CreateRessourceMetier(IMetierBase metierBase = null) => GetMetier<RessourceMetier>(metierBase);
        public ITypeRelationMetier CreateTypeRelationMetier(IMetierBase metierBase = null) => GetMetier<TypeRelationMetier>(metierBase);
        public ITypeRelationRessourceMetier CreateTypeRelationRessourceMetier(IMetierBase metierBase = null) => GetMetier<TypeRelationRessourceMetier>(metierBase);
        public ITypeRessourceMetier CreateTypeRessourceMetier(IMetierBase metierBase = null) => GetMetier<TypeRessourceMetier>(metierBase);
        public IUtilisateurRessourceMetier CreateUtilisateurRessourceMetier(IMetierBase metierBase = null) => GetMetier<UtilisateurRessourceMetier>(metierBase);
        public IStatistiqueMetier CreateStatistiqueMetier(IMetierBase metierBase = null) => GetMetier<StatistiqueMetier>(metierBase);
    }
}
