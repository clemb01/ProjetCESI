﻿using ProjetCESI.Metier.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Metier
{
    public class MetierFactory
    {
        public MetierFactory()
        {

        }

        private T GetMetier<T>(IMetierBase __metierBase) where T : class, new()
        {
            var metier = new T();
            IMetierBase metierBase = metier as IMetierBase;

            if (metierBase != null)
            {
                // Rien pour l'instant
            }

            return metier;
        }

        public IApplicationRoleMetier CreateApplicationRoleMetier(IMetierBase metierBase = null) => GetMetier<ApplicationRoleMetier>(metierBase);
    }
}