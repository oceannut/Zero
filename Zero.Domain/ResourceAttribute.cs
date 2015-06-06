using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Domain
{

    [DataContract]
    [AttributeUsage(AttributeTargets.Method)]
    public class ResourceAttribute : Attribute 
    {

        [DataMember]
        public int Scope { get; set; }

        //[DataMember]
        //public string Resource { get; set; }

        [DataMember]
        public int Method { get; set; }

    }

}
