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
    public enum BaseAccessMode
    {
        [EnumMember]
        Allowed,
        [EnumMember]
        Denied
    }

    [DataContract]
    public class BaseAccess : ITimestampData
    {

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string RoleId { get; set; }

        [DataMember]
        public virtual Role Role { get; set; }

        [DataMember]
        public BaseAccessMode Mode { get; set; }

        [DataMember]
        public DateTime? Offset { get; set; }

        [DataMember]
        public int Duration { get; set; }

        [DataMember]
        public DateTime Creation { get; set; }

        [DataMember]
        public DateTime Modification { get; set; }

    }

}
