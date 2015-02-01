using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;
using Zero.BLL;

namespace Zero.BLL.Managers
{
    public class UserManager : IUserService
    {

        public void SaveUser(User user)
        {
            throw new NotImplementedException();
        }

        public Task SaveUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserPwd(string userId, string oldPwd, string newPwd)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserPwdAsync(string userId, string oldPwd, string newPwd)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(string userId, string name, string email)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(string userId, string name, string email)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(string userId, string[] roles)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(string userId, string[] roles)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string userId, bool? includeRoles = null)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserAsync(string userId, bool? includeRoles = null)
        {
            throw new NotImplementedException();
        }

        public Paging<User> GetUserPage(PagingRequest request, string group = null)
        {
            throw new NotImplementedException();
        }

        public Task<Paging<User>> GetUserPageAsync(PagingRequest request, string group = null)
        {
            throw new NotImplementedException();
        }

        public long GetUserTotalCount(string group = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetUserTotalCountAsync(string group = null)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetUserList(string group = null)
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUserListAsync(string group = null)
        {
            throw new NotImplementedException();
        }

        public void SaveRole(Role role)
        {
            throw new NotImplementedException();
        }

        public Task SaveRoleAsync(Role role)
        {
            throw new NotImplementedException();
        }

        public void UpdateRole(string roleId, string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRoleAsync(string roleId, string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateRole(string roleId, string[] users)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRoleAsync(string roleId, string[] users)
        {
            throw new NotImplementedException();
        }

        public void DeleteRole(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRoleAsync(string roleId)
        {
            throw new NotImplementedException();
        }

        public Role GetRole(string roleId, bool? includeUsers = null)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetRoleAsync(string roleId, bool? includeUsers = null)
        {
            throw new NotImplementedException();
        }

        public IList<Role> GetRoleList()
        {
            throw new NotImplementedException();
        }

        public Task<IList<Role>> GetRoleListAsync()
        {
            throw new NotImplementedException();
        }

    }
}
