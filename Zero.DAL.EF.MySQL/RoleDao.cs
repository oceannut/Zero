using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;
using Zero.DAL;

namespace Zero.DAL.EF.MySQL
{
    public class RoleDao : GenericPageableDao<Role>, IRoleDao
    {

        public override bool Save(Role entity)
        {
            using (FragileDataContext context = new FragileDataContext())
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
                int rowsAffected = context.SaveChanges();
                return rowsAffected > 0;
            }
        }

        public override bool Update(Role entity)
        {
            using (FragileDataContext context = new FragileDataContext())
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
                    int rowsAffected = context.SaveChanges();
                    return rowsAffected > 0;
                }
                else
                {
                    return false;
                }
            }
        }

        public override bool Delete(object id)
        {
            using (FragileDataContext context = new FragileDataContext())
            {
                var roleGet = context.Roles.FirstOrDefault(e => e.Id == (string)id);
                if (roleGet != null)
                {
                    roleGet.Users.Clear();
                    context.Roles.Remove(roleGet);
                    int rowsAffected = context.SaveChanges();
                    return rowsAffected > 0;
                }
                else
                {
                    return false;
                }
            }
        }

        public override Role Get(object id)
        {
            using (FragileDataContext context = new FragileDataContext())
            {
                return context.Roles.FirstOrDefault(e => e.Id == (string)id);
            }
        }

        public IList<Role> List(string userId)
        {
            using (FragileDataContext context = new FragileDataContext())
            {
                return (from role in context.Roles
                        where role.Users.Any(e => e.Id == userId)
                        select role)
                        .ToList();
            }
        }

    }
}
