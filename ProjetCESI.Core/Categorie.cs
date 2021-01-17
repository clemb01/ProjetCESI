using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class Categorie : EntiteBase
    {
        [DataMember]
        public string Nom { get; set; }
    }
}
