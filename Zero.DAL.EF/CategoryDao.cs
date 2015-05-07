using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Data;

using Zero.Domain;
using Zero.DAL;

namespace Zero.DAL.EF
{
    public class CategoryDao : GenericDao<Category, string>, ICategoryDao
    {

        private string connectionString;

        public CategoryDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override int Save(Category entity)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                if (entity.Parent != null)
                {
                    var entry = context.Entry<Category>(entity.Parent);
                    entry.State = System.Data.Entity.EntityState.Unchanged;
                }
                context.Categories.Add(entity);
                return context.SaveChanges();
            }
        }

        public override int Update(Category entity)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                var entry = context.Entry<Category>(entity);
                entry.State = System.Data.Entity.EntityState.Modified;
                return context.SaveChanges();
            }
        }

        public override int Delete(ICollection<Category> col)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                foreach (Category entity in col)
                {
                    var entry = context.Entry<Category>(entity);
                    entry.State = System.Data.Entity.EntityState.Deleted;
                }
                context.Categories.RemoveRange(col);
                return context.SaveChanges();
            }
        }

        //public override int Delete(string id)
        //{
        //    Category found = Get(id);
        //    if (found != null)
        //    {
        //        using (CategoryDataContext context = new CategoryDataContext(connectionString))
        //        {
        //            var entry = context.Entry<Category>(found);
        //            entry.State = System.Data.Entity.EntityState.Deleted;
        //            context.Categories.Remove(found);
        //            return context.SaveChanges();
        //        }
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        //public override int Delete(Category entity)
        //{
        //    using (CategoryDataContext context = new CategoryDataContext(connectionString))
        //    {
        //        var entry = context.Entry<Category>(entity);
        //        entry.State = System.Data.Entity.EntityState.Deleted;
        //        context.Categories.Remove(entity);
        //        return context.SaveChanges();
        //    }
        //}

        public override Category Get(string id)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return context.Categories.Find(id);
            }
        }

        public Category Get(string id, bool? includeParent = null)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                if (includeParent.HasValue && includeParent.Value)
                {
                    return (from categroy in context.Categories.Include("Parent")
                            where categroy.Id == id
                            select categroy)
                            .FirstOrDefault();
                }
                else
                {
                    return context.Categories.Find(id);
                }
            }
        }

        public Category GetByCode(int scope, string code, bool? includeParent = null)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                if (includeParent.HasValue && includeParent.Value)
                {
                    return (from categroy in context.Categories.Include("Parent")
                            where categroy.Scope == scope && categroy.Code == code
                            select categroy)
                            .FirstOrDefault();
                }
                else
                {
                    return (from categroy in context.Categories
                            where categroy.Scope == scope && categroy.Code == code
                            select categroy)
                            .FirstOrDefault();
                }
            }
        }

        public bool IsCodeExisted(int scope, string code)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return context.Categories.Count(categroy=> (categroy.Scope == scope && categroy.Code == code)) > 0;
            }
        }

        public int Count(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return context.Categories.Count(
                    category =>
                        ((scope == null || category.Scope == scope)
                        && (parentId == null || ((parentId == string.Empty && category.ParentId == null) || category.ParentId == parentId))
                        && (isDisused == null || category.Disused == isDisused.Value)));
            }
        }

        public IEnumerable<Category> List(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return (from category in context.Categories
                        where (scope == null || category.Scope == scope)
                            && (parentId == null || ((parentId == string.Empty && category.ParentId == null) || category.ParentId == parentId))
                            && (isDisused == null || category.Disused == isDisused.Value)
                        select category)
                        .ToArray();
            }
        }

        public TreeNodeCollection<Category> Tree(int scope)
        {
            return Category.BuildTree(List(scope));
        }

    }
}
