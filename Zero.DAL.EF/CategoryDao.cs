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
    public class CategoryDao : GenericPageableDao<Category>, ICategoryDao
    {

        private string connectionString;

        public CategoryDao(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override bool Save(Category entity)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                if (entity.Parent != null)
                {
                    var entry = context.Entry<Category>(entity.Parent);
                    entry.State = System.Data.Entity.EntityState.Unchanged;
                }
                context.Categories.Add(entity);
                int rowsAffected = context.SaveChanges();
                return rowsAffected > 0;
            }
        }

        public override bool Update(Category entity)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                var entry = context.Entry<Category>(entity);
                entry.State = System.Data.Entity.EntityState.Modified;
                int rowsAffected = context.SaveChanges();
                return rowsAffected > 0;
            }
        }

        public override bool Delete(object id)
        {
            Category found = Get(id);
            if (found != null)
            {
                using (CategoryDataContext context = new CategoryDataContext(connectionString))
                {
                    var entry = context.Entry<Category>(found);
                    entry.State = System.Data.Entity.EntityState.Deleted;
                    context.Categories.Remove(found);
                    int rowsAffected = context.SaveChanges();
                    return rowsAffected > 0;
                }
            }
            else
            {
                return false;
            }
        }

        public override Category Get(object id)
        {
            using (CategoryDataContext context = new CategoryDataContext(connectionString))
            {
                return context.Categories.Find(id);
            }
        }


        public Category Get(string id, bool? includeParent = null)
        {
            throw new NotImplementedException();
        }

        public Category GetByCode(int scope, string code, bool? includeParent = null)
        {
            throw new NotImplementedException();
        }

        public bool IsCodeExisted(int scope, string code)
        {
            throw new NotImplementedException();
        }

        public int Count(int? scope = null, string parentId = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> List(PagingRequest request, int? scope = null, bool? includeParent = null, string parentId = null)
        {
            throw new NotImplementedException();
        }

    }
}
