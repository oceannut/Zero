using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;

using R = Zero.Domain.Properties.Resources;

namespace Zero.Domain
{

    [DataContract]
    public class Role : ITimestampData
    {

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public virtual IList<User> Users { get; set; }

        //[DataMember]
        //public bool IsAdmin { get; set; }

        [DataMember]
        public DateTime Creation { get; set; }

        [DataMember]
        public DateTime Modification { get; set; }

        static Role()
        {
            //ResourceRegistry.Registrate("role", Resource.METHOD_SAVE, R.RoleSave);
            //ResourceRegistry.Registrate("role", Resource.METHOD_UPDATE, R.RoleUpdate);
            //ResourceRegistry.Registrate("role", Resource.METHOD_GET, R.RoleGet);
            //ResourceRegistry.Registrate("role", Resource.METHOD_DELETE, R.UserDelete);
            //ResourceRegistry.Registrate("role", Resource.METHOD_LIST, R.RoleList);
        }

    }

}
