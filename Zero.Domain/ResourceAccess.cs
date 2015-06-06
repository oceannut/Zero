using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Domain
{

    [DataContract]
    public class ResourceAccess : BaseAccess
    {

        public const int METHOD_CREATE = 1;
        public const int METHOD_UPDATE = 2;
        public const int METHOD_RETRIEVE = 4;
        public const int METHOD_DELETE = 8;

        [DataMember]
        public int Scope { get; set; }

        //[DataMember]
        //public string Resource { get; set; }

        [DataMember]
        public int Method { get; set; }

    }

}
