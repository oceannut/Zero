﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;
using Zero.DAL;
using System.Data.Entity.Infrastructure;

namespace Zero.DAL.EF
{
    public class UserDao : GenericPageableDao<User, string>, IUserDao
    {

        private string connectionString;

        public UserDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override int Save(User entity)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                if (entity.Roles != null && entity.Roles.Count > 0)
                {
                    var roles = (from role in context.Roles select role);
                    foreach (Role role in entity.Roles)
                    {
                        if (roles.Any(e => e.Id == role.Id))
                        {
                            context.Entry<Role>(role).State = System.Data.Entity.EntityState.Modified;
                        }
                        else
                        {
                            context.Entry<Role>(role).State = System.Data.Entity.EntityState.Added;
                        }
                    }
                }
                context.Users.Add(entity);
                return context.SaveChanges();
            }
        }

        public override int Update(User entity)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                var userGet = context.Users.FirstOrDefault(e => e.Id == entity.Id);
                if (userGet != null)
                {
                    userGet.Pwd = entity.Pwd;
                    userGet.Name = entity.Name;
                    userGet.Email = entity.Email;
                    if ((entity.Roles == null || entity.Roles.Count == 0) && userGet.Roles != null && userGet.Roles.Count > 0)
                    {
                        userGet.Roles.Clear();
                    }
                    else if (entity.Roles != null && entity.Roles.Count > 0 && (userGet.Roles == null || userGet.Roles.Count == 0))
                    {
                        var roles = (from role in context.Roles select role);
                        foreach (Role role in entity.Roles)
                        {
                            if (roles.Any(e => e.Id == role.Id))
                            {
                                context.Entry<Role>(role).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                context.Entry<Role>(role).State = System.Data.Entity.EntityState.Added;
                            }
                        }
                        userGet.Roles = new List<Role>();
                        ((List<Role>)userGet.Roles).AddRange(entity.Roles);
                    }
                    else if (entity.Roles != null && entity.Roles.Count > 0 && userGet.Roles != null && userGet.Roles.Count > 0)
                    {
                        for (int j = 0; j < userGet.Roles.Count; j++)
                        {
                            bool save = false;
                            for (int i = 0; i < entity.Roles.Count; i++)
                            {
                                if (userGet.Roles[j].Id == entity.Roles[i].Id)
                                {
                                    //已被添加，从要求添加的队列中移除。
                                    entity.Roles.RemoveAt(i);
                                    save = true;
                                    i--;
                                }
                            }
                            if (!save)
                            {
                                //要求被删除，则从已添加队列中删除
                                userGet.Roles.RemoveAt(j);
                                j--;
                            }
                        }
                        if (entity.Roles.Count > 0)
                        {
                            //添加新增的项目。
                            ((List<Role>)userGet.Roles).AddRange(entity.Roles);
                        }
                    }
                    userGet.Modification = entity.Modification;
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
                var userGet = context.Users.FirstOrDefault(e => e.Id == (string)id);
                if (userGet != null)
                {
                    var entry = context.Entry<User>(userGet);
                    entry.Collection(i => i.Roles).CurrentValue.Clear();

                    //userGet.Roles.Clear();
                    context.Users.Remove(userGet);
                    return context.SaveChanges();
                }
                else
                {
                    return 0;
                }
            }
        }

        public override User Get(string id)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                return context.Users.Include("Roles").FirstOrDefault(e => e.Id == id);
            }
        }

        public User GetByUsername(string username)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                return context.Users.Include("Roles").FirstOrDefault(e => e.Username == username);
            }
        }

        public IList<User> List(string roleId)
        {
            using (UserDataContext context = new UserDataContext(connectionString))
            {
                return (from user in context.Users 
                        where user.Roles.Any(e => e.Id == roleId) 
                        select user)
                        .ToList();
            }
        }

    }
}
