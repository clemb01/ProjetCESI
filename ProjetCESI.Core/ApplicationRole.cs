using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class ApplicationRole : IdentityRole<int>, IGetPrimaryKey
    {
        public ApplicationRole() { }
        public ApplicationRole(string name) { Name = name; }

        public int GetPrimaryKey()
        {
            return Id;
        }
    }
}
