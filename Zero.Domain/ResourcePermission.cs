using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zero.Domain
{


    public class ResourcePermission : IPermission
    {

        private readonly IEnumerable<ResourceAccess> accessList;
        public IEnumerable<ResourceAccess> AccessList
        {
            get { return accessList; }
        }

        private readonly bool isAllowedFirst;
        public bool IsAllowedFirst
        {
            get { return isAllowedFirst; }
        }

        public ResourcePermission(IEnumerable<ResourceAccess> accessList)
            : this(accessList, false)
        {

        }

        public ResourcePermission(IEnumerable<ResourceAccess> accessList, 
            bool isAllowedFirst)
        {
            this.accessList = accessList;
            this.isAllowedFirst = isAllowedFirst;
        }

        public IPermission Copy()
        {
            throw new NotImplementedException();
        }

        public void Demand()
        {
            if (this.accessList == null || this.accessList.Count() == 0)
            {
                throw new SecurityException();
            }
            IPrincipal principal = Thread.CurrentPrincipal;
            bool isAuthorzied = false;
            foreach (var access in this.accessList)
            {
                if (principal.IsInRole(access.RoleId))
                {
                    if (!this.isAllowedFirst && access.Mode == BaseAccessMode.Denied)
                    {
                        throw new SecurityException();
                    }
                    else if (access.Mode == BaseAccessMode.Allowed)
                    {
                        isAuthorzied = isAuthorzied | true;
                    }
                }
            }
            if (!isAuthorzied)
            {
                throw new SecurityException();
            }
        }

        public IPermission Intersect(IPermission target)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IPermission target)
        {
            throw new NotImplementedException();
        }

        public IPermission Union(IPermission target)
        {
            throw new NotImplementedException();
        }

        public void FromXml(SecurityElement e)
        {
            throw new NotImplementedException();
        }

        public SecurityElement ToXml()
        {
            throw new NotImplementedException();
        }

    }

}
