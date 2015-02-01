using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Domain
{

    public class User : Nega.Common.ITimestampData
    {

        public string Id { get; set; }

        public string Username { get; set; }

        public string Pwd { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Group { get; set; }

        public virtual IList<Role> Roles { get; set; }

        public DateTime Creation { get; set; }

        public DateTime Modification { get; set; }

    }

}
