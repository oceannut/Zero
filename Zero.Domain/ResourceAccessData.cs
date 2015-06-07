using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;

namespace Zero.Domain
{

    [DataContract]
    public class ResourceAccessData : ITimestampData
    {

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public Resource Resource { get; set; }

        [DataMember]
        public ResourceAccess Access { get; set; }

        [DataMember]
        public DateTime Creation { get; set; }

        [DataMember]
        public DateTime Modification { get; set; }

    }

}
