using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.Data;
using Nega.Modularity;

using Zero.Domain;
using Zero.DAL;

namespace Zero.DAL.Caching
{
    public class CategoryCache : GenericDao<Category, string>, ICategoryDao, IModule
    {

        private ICategoryDao dao;
        private ICache cache;

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
            this.cache = cacheManager.GetCache(Name) as ICache;
        }

        public override int Save(Category entity)
        {
            int count = dao.Save(entity);

            TreeNodeCollection<Category> tree = Tree(entity.Scope);
            TreeNode<Category> parent = null;
            if (!string.IsNullOrWhiteSpace(entity.ParentId))
            {
                parent = Tree<Category>.Find(tree,
                             (e) => e.Data.Id == entity.ParentId);
            }
            AttachToParent(tree, parent,
                new TreeNode<Category>
                {
                    Data = entity
                });

            return count;
        }

        public override int Update(Category entity)
        {
            int count = dao.Update(entity);

            TreeNodeCollection<Category> tree = Tree(entity.Scope);
            TreeNode<Category> node = Tree<Category>.Find(tree,
                        (e) => e.Data.Id == entity.Id);
            if (node == null)
            {
                TreeNode<Category> parent = null;
                if (!string.IsNullOrWhiteSpace(entity.ParentId))
                {
                    parent = Tree<Category>.Find(tree,
                                 (e) => e.Data.Id == entity.ParentId);
                }
                AttachToParent(tree, parent,
                    new TreeNode<Category>
                    {
                        Data = entity
                    });
            }
            else
            {
                TreeNode<Category> oldParent = node.Parent;
                UnattachToParent(tree, oldParent, node);
                TreeNode<Category> newParent = null;
                if (!string.IsNullOrWhiteSpace(entity.ParentId))
                {
                    newParent = Tree<Category>.Find(tree,
                                 (e) => e.Data.Id == entity.ParentId);
                }
                node.Data = entity;
                AttachToParent(tree, newParent, node);
            }

            return count;
        }

        public override int Delete(ICollection<Category> col)
        {
            int count = dao.Delete(col);

            foreach (var item in col)
            {
                TreeNodeCollection<Category> tree = Tree(item.Scope);
                Tree<Category>.PostorderTraverse(tree,
                    (e) =>
                    {
                        if (item.Id == e.Data.Id)
                        {
                            UnattachToParent(tree, e.Parent, e);
                            if (!e.IsLeaf)
                            {
                                Tree<Category>.PostorderTraverse(e,
                                    (subNode) =>
                                    {
                                        if (subNode.Parent != null)
                                        {
                                            subNode.Parent.Children.Remove(subNode);
                                        }
                                    });
                            }
                        }

                    });
            }

            return count;
        }

        public Category Get(int scope, string id)
        {
            Category found = null;
            TreeNodeCollection<Category> tree = Tree(scope);
            Tree<Category>.PreorderTraverse(tree,
                (e) =>
                {
                    if (id == e.Data.Id)
                    {
                        found = e.Data.ShallowClone();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

            return found;
        }

        public Category GetByCode(int scope, string code, bool? includeParent = null)
        {
            Category found = null;
            TreeNodeCollection<Category> tree = Tree(scope);
            Tree<Category>.PreorderTraverse(tree,
                (e) =>
                {
                    if (code == e.Data.Code)
                    {
                        found = e.Data.ShallowClone();
                        if (includeParent.HasValue && includeParent.Value)
                        {
                            TreeNode<Category> parent = e.Parent;
                            while (parent != null)
                            {
                                found.Parent = parent.Data.ShallowClone();
                                parent = parent.Parent;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

            return found;
        }

        public bool IsCodeExisted(int scope, string code)
        {
            bool isExisted = false;
            TreeNodeCollection<Category> tree = Tree(scope);
            Tree<Category>.PreorderTraverse(tree,
                (e) =>
                {
                    if (code == e.Data.Code)
                    {
                        isExisted = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

            return isExisted;
        }

        public int Count(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            int count = 0;
            if (scope.HasValue)
            {
                TreeNodeCollection<Category> tree = Tree(scope.Value);
                Filter(false, e => count++, tree, parentId, isDisused);
            }
            else
            {
                foreach (var item in this.cache.Items)
                {
                    TreeNodeCollection<Category> tree = item.Value as TreeNodeCollection<Category>;
                    Filter(false, e => count++, tree, parentId, isDisused);
                }
            }

            return count;
        }

        public IEnumerable<Category> List(int? scope = null, string parentId = null, bool? isDisused = null)
        {
            List<Category> result = new List<Category>();
            if (scope.HasValue)
            {
                TreeNodeCollection<Category> tree = Tree(scope.Value);
                Filter(true, e => result.Add(e), tree, parentId, isDisused);
            }
            else
            {
                foreach (var item in this.cache.Items)
                {
                    TreeNodeCollection<Category> tree = item.Value as TreeNodeCollection<Category>;
                    Filter(true, e => result.Add(e), tree, parentId, isDisused);
                }
            }

            return result;
        }

        public TreeNodeCollection<Category> Tree(int scope)
        {
            TreeNodeCollection<Category> tree = this.cache.Get(scope.ToString()) as TreeNodeCollection<Category>;
            if (tree == null)
            {
                tree = dao.Tree(scope);
                this.cache.Add(scope.ToString(), tree);
            }

            return tree;
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
                    this.cache.Add(scope.ToString(), Category.BuildTree(categoryDict[scope]));
                }
            }
        }

        public void Destroy()
        {
            foreach (var item in this.cache.Items)
            {
                TreeNodeCollection<Category> tree = item.Value as TreeNodeCollection<Category>;
                Category.CleanupTree(tree);
            }
            this.cache.Clear();
        }

        public void Dispose()
        {
            Destroy();
            this.cache.Dispose();
        }

        private void AttachToParent(TreeNodeCollection<Category> tree, TreeNode<Category> parent, TreeNode<Category> node)
        {
            if (parent == null)
            {
                tree.Add(node);
                tree.Sort(Category.CompareToBySequence);
            }
            else
            {
                parent.Children.Add(node);
                parent.Children.Sort(Category.CompareToBySequence);
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

        private void Filter(bool needClone, Action<Category> action, TreeNodeCollection<Category> tree, 
            string parentId = null, bool? isDisused = null)
        {
            Tree<Category>.PreorderTraverse(tree,
                        (e) =>
                        {
                            if ((parentId == null || (parentId == string.Empty && e.Data.ParentId == null) || parentId == e.Data.ParentId)
                                && (isDisused == null || isDisused.Value == e.Data.Disused))
                            {
                                if (needClone)
                                {
                                    action(e.Data.ShallowClone());
                                }
                                else
                                {
                                    action(e.Data);
                                }
                            }
                        });
        }

    }

}
