﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Domain
{

    [DataContract]
    public class KeyVal
    {

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }

    }

}
