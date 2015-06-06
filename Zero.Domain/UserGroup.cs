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
    public class UserGroup : ITimestampData
    {

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual IList<User> Users { get; set; }

        [DataMember]
        public virtual IList<Role> Roles { get; set; }

        [DataMember]
        public DateTime Creation { get; set; }

        [DataMember]
        public DateTime Modification { get; set; }

    }

}
