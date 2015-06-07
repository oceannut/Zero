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
    public class User : ITimestampData
    {

        public const string RESOURCE_USER = "user";

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        public string Pwd { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public virtual IList<Role> Roles { get; set; }

        [DataMember]
        public DateTime Creation { get; set; }

        [DataMember]
        public DateTime Modification { get; set; }

        static User()
        {
            ResourceRegistry.Registrate(RESOURCE_USER, Resource.METHOD_SAVE, R.UserSave);
            ResourceRegistry.Registrate(RESOURCE_USER, Resource.METHOD_UPDATE, R.UserUpdate);
            ResourceRegistry.Registrate(RESOURCE_USER, Resource.METHOD_GET, R.UserDelete);
            ResourceRegistry.Registrate(RESOURCE_USER, Resource.METHOD_DELETE, R.UserDelete);
            ResourceRegistry.Registrate(RESOURCE_USER, Resource.METHOD_LIST, R.UserList);
        }

    }

}
