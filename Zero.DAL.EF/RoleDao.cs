using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;
using Zero.DAL;

namespace Zero.DAL.EF
{
    public class RoleDao : GenericDao<Role, string>, IRoleDao
    {

        private string connectionString;

        public RoleDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override int Save(Role entity)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                if (entity.Users != null && entity.Users.Count > 0)
                {
                    var users = (from user in context.Users select user);
                    foreach (User user in entity.Users)
                    {
                        if (users.Any(e => e.Id == user.Id))
                        {
                            context.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            context.Entry<User>(user).State = System.Data.Entity.EntityState.Added;
                        }
                    }
                }
                context.Roles.Add(entity);
                return context.SaveChanges();
            }
        }

        public override int Update(Role entity)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                var roleGet = context.Roles.FirstOrDefault(e => e.Id == entity.Id);
                if (roleGet != null)
                {
                    roleGet.Name = entity.Name;
                    if ((entity.Users == null || entity.Users.Count == 0) && roleGet.Users != null && roleGet.Users.Count > 0)
                    {
                        roleGet.Users.Clear();
                    }
                    else if (entity.Users != null && entity.Users.Count > 0 && (roleGet.Users == null || roleGet.Users.Count == 0))
                    {
                        var users = (from user in context.Users select user);
                        foreach (User user in entity.Users)
                        {
                            if (users.Any(e => e.Id == user.Id))
                            {
                                context.Entry<User>(user).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                context.Entry<User>(user).State = System.Data.Entity.EntityState.Added;
                            }
                        }
                        roleGet.Users = new List<User>();
                        ((List<User>)roleGet.Users).AddRange(entity.Users);
                    }
                    else if (entity.Users != null && entity.Users.Count > 0 && roleGet.Users != null && roleGet.Users.Count > 0)
                    {
                        for (int j = 0; j < roleGet.Users.Count; j++)
                        {
                            bool save = false;
                            for (int i = 0; i < entity.Users.Count; i++)
                            {
                                if (roleGet.Users[j].Id == entity.Users[i].Id)
                                {
                                    //已被添加，从要求添加的队列中移除。
                                    entity.Users.RemoveAt(i);
                                    save = true;
                                    i--;
                                }
                            }
                            if (!save)
                            {
                                //要求被删除，则从已添加队列中删除
                                roleGet.Users.RemoveAt(j);
                                j--;
                            }
                        }
                        if (entity.Users.Count > 0)
                        {
                            //添加新增的项目。
                            ((List<User>)roleGet.Users).AddRange(entity.Users);
                        }
                    }
                    roleGet.Modification = entity.Modification;
                    return context.SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
        }

        public override int Delete(string id)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                var roleGet = context.Roles.FirstOrDefault(e => e.Id == id);
                if (roleGet != null)
                {
                    roleGet.Users.Clear();
                    context.Roles.Remove(roleGet);
                    return context.SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
        }

        public override Role Get(string id)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                return context.Roles.FirstOrDefault(e => e.Id == (string)id);
            }
        }

        public IList<Role> List(string userId)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                return (from role in context.Roles
                        where role.Users.Any(e => e.Id == userId)
                        select role)
                        .ToList();
            }
        }

    }
}
