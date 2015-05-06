using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Modularity;

using Zero.Domain;
using Zero.DAL;

namespace Zero.DAL.Caching
{
    public class CategoryCache : ICategoryDao, IModule
    {

        private ICategoryDao dao;
        private CacheManager cacheManager;

        public string Name
        {
            get { return "Category Cache"; }
        }

        public IModuleLicence Licence
        {
            get { return Unlimited.Solo; }
        }

        public CategoryCache(ICategoryDao dao, CacheManager cacheManager)
        {
            if (dao == null || cacheManager == null)
            {
                throw new ArgumentNullException();
            }

            this.dao = dao;
            this.cacheManager = cacheManager;
        }

        public int Save(Category entity)
        {
            int count = dao.Save(entity);

            TreeNodeCollection<Category> tree = this.cacheManager.GetCache(Name) as TreeNodeCollection<Category>;
            TreeNode<Category> parent = Tree<Category>.Find(tree,
                        (e) => e.Data.Id == entity.ParentId);
            AttachToParent(tree, parent,
                new TreeNode<Category>
                {
                    Data = entity
                });

            return count;
        }

        public int Save(ICollection<Category> col)
        {
            throw new NotImplementedException();
        }

        public int Update(Category entity)
        {
            throw new NotImplementedException();
        }

        public int Update(ICollection<Category> col)
        {
            throw new NotImplementedException();
        }

        public int Delete(string id)
        {
            throw new NotImplementedException();
        }

        public int Delete(ICollection<string> col)
        {
            throw new NotImplementedException();
        }

        public int Delete(Category entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(ICollection<Category> col)
        {
            throw new NotImplementedException();
        }

        public Category Get(string id)
        {
            throw new NotImplementedException();
        }

        public bool IsExist(string id)
        {
            throw new NotImplementedException();
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

        public int Count(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Category> List(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            IEnumerable<Category> categories = this.dao.List();
            if (categories != null && categories.Count() > 0)
            {
                Dictionary<int, List<Category>> categoryDict = new Dictionary<int, List<Category>>();
                foreach (Category category in categories)
                {
                    List<Category> list = null;
                    categoryDict.TryGetValue(category.Scope, out list);
                    if (list == null)
                    {
                        list = new List<Category>();
                        categoryDict.Add(category.Scope, list);
                    }
                    list.Add(category);
                }

                foreach (int scope in categoryDict.Keys)
                {
                    List<Category> list = categoryDict[scope];
                    TreeNodeCollection<Category> tree = BuildTree(list);
                    if (tree != null && tree.Count > 0)
                    {
                        tree.Sort(SortBySequence);
                        Tree<Category>.PreorderTraverse(tree,
                            (e) =>
                            {
                                if (!e.IsLeaf)
                                {
                                    e.Children.Sort(SortBySequence);
                                }
                            });
                        this.cacheManager.GetCache(Name).Add(scope.ToString(), tree);
                    }
                }
            }
        }

        public void Destroy()
        {
            this.cacheManager.GetCache(Name).Dispose();
        }

        public void Dispose()
        {
            Destroy();
        }

        private void AttachToParent(TreeNodeCollection<Category> tree, TreeNode<Category> parent, TreeNode<Category> node)
        {
            if (parent == null)
            {
                tree.Add(node);
                tree.Sort(SortBySequence);
            }
            else
            {
                parent.Children.Add(node);
                parent.Children.Sort(SortBySequence);
            }
        }

        private void UnattachToParent(TreeNodeCollection<Category> tree, TreeNode<Category> parent, TreeNode<Category> node)
        {
            if (parent == null)
            {
                tree.Remove(node);
            }
            else
            {
                parent.Children.Remove(node);
            }
        }

        private int SortBySequence(TreeNode<Category> node1, TreeNode<Category> node2)
        {
            return node2.Data.Sequence.CompareTo(node1.Data.Sequence);
        }

        private TreeNodeCollection<Category> BuildTree(List<Category> col)
        {
            if (col == null || col.Count == 0)
            {
                throw new ArgumentNullException();
            }

            TreeNodeCollection<Category> tree = new TreeNodeCollection<Category>();

            List<TreeNode<Category>> list = (from item in col
                                             select new TreeNode<Category>
                                                  {
                                                      Data = item
                                                  })
                                                  .ToList();
            Queue<TreeNode<Category>> queue = new Queue<TreeNode<Category>>();
            foreach (var item in list)
            {
                queue.Enqueue(item);
            }
            while (queue.Count > 0)
            {
                TreeNode<Category> current = queue.Dequeue();
                if (string.IsNullOrWhiteSpace(current.Data.ParentId))
                {
                    tree.Add(current);
                }
                else
                {
                    TreeNode<Category> parent = Tree<Category>.Find(tree,
                        (e) => e.Data.Id == current.Data.ParentId);
                    if (parent == null)
                    {
                        queue.Enqueue(current);
                    }
                    else
                    {
                        parent.Children.Add(current);
                    }
                }
            }

            return tree;
        }

    }
}
