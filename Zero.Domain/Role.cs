using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Domain
{

    [DataContract]
    public class Role : Nega.Common.ITimestampData
    {

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual IList<User> Users { get; set; }

        [DataMember]
        public DateTime Creation { get; set; }

        [DataMember]
        public DateTime Modification { get; set; }

    }

}
