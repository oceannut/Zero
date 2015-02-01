using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Domain
{
    public class Role : Nega.Common.ITimestampData
    {

        public string Id { get; set; }

        public string Name { get; set; }

        public virtual IList<User> Users { get; set; }

        public DateTime Creation { get; set; }

        public DateTime Modification { get; set; }

    }
}
