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

        public override int Update(IEnumerable<Category> col)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                foreach (Category entity in col)
                {
                    var entry = context.Entry<Category>(entity);
                    entry.State = System.Data.Entity.EntityState.Modified;
                }
                return context.SaveChanges();
            }
        }

        public override int Delete(Category entity)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                var entry = context.Entry<Category>(entity);
                entry.State = System.Data.Entity.EntityState.Deleted;
                context.Categories.Remove(entity);
                return context.SaveChanges();
            }
        }

        public override int Delete(IEnumerable<Category> col)
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

        public Category Get(int scope, string id)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return context.Categories.Find(id);
            }
        }

        public Category GetByCode(int scope, string code)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return (from categroy in context.Categories.Include("Parent")
                        where categroy.Scope == scope && categroy.Code == code
                        select categroy)
                            .FirstOrDefault();
            }
        }

        public bool IsCodeExisted(int scope, string code)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return context.Categories.Count(categroy => (categroy.Scope == scope && categroy.Code == code)) > 0;
            }
        }

        public int Count(int? scope = null, bool? isDisused = null)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return context.Categories.Count(
                    category =>
                        ((scope == null || category.Scope == scope)
                        && (isDisused == null || category.Disused == isDisused.Value)));
            }
        }

        public IEnumerable<Category> List(int? scope = null, bool? isDisused = null)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return (from category in context.Categories
                        where (scope == null || category.Scope == scope)
                            && (isDisused == null || category.Disused == isDisused.Value)
                        select category)
                        .ToArray();
            }
        }

        public TreeNodeCollection<Category> Tree(int scope)
        {
            var categories = List(scope);
            return Category.BuildTree(categories);
        }

    }

}
