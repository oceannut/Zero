using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;

namespace Zero.BLL
{

    public interface IUserService
    {

        void SaveUser(User user);

        Task SaveUserAsync(User user);

        bool UpdateUserPwd(string userId, string oldPwd, string newPwd);

        Task<bool> UpdateUserPwdAsync(string userId, string oldPwd, string newPwd);

        void UpdateUser(string userId, string name, string email);

        Task UpdateUserAsync(string userId, string name, string email);

        void UpdateUser(string userId, string[] roles);

        Task UpdateUserAsync(string userId, string[] roles);

        void DeleteUser(string userId);

        Task DeleteUserAsync(string userId);

        User GetUser(string userId, bool? includeRoles = null);

        Task<User> GetUserAsync(string userId, bool? includeRoles = null);

        Paging<User> GetUserPage(PagingRequest request, string group = null);

        Task<Paging<User>> GetUserPageAsync(PagingRequest request, string group = null);

        long GetUserTotalCount(string group = null);

        Task<long> GetUserTotalCountAsync(string group = null);

        IList<User> GetUserList(string group = null);

        Task<IList<User>> GetUserListAsync(string group = null);

        void SaveRole(Role role);

        Task SaveRoleAsync(Role role);

        void UpdateRole(string roleId, string name);

        Task UpdateRoleAsync(string roleId, string name);

        void UpdateRole(string roleId, string[] users);

        Task UpdateRoleAsync(string roleId, string[] users);

        void DeleteRole(string roleId);

        Task DeleteRoleAsync(string roleId);

        Role GetRole(string roleId, bool? includeUsers = null);

        Task<Role> GetRoleAsync(string roleId, bool? includeUsers = null);

        IList<Role> GetRoleList();

        Task<IList<Role>> GetRoleListAsync();

    }

}
