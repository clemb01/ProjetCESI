using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class EntiteBase : IGetPrimaryKey
    {
        virtual public int GetPrimaryKey()
        {
            return Id;
        }

        [DataMember]
        public int Id { get; set; }

        [NotMapped]
        [DataMember]
        public string LastError { get; set; }
    }
}
