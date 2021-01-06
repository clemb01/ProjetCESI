﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Core
{
    public class EntiteBase<T> : IGetPrimaryKey where T : IGetPrimaryKey
    {
        virtual public int GetPrimaryKey()
        {
            throw new NotImplementedException();
        }

        [NotMapped]
        [DataMember]
        public string LastError { get; set; }
    }
}