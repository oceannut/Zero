using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;

namespace Zero.Domain
{

    /// <summary>
    /// 类型。
    /// </summary>
    [DataContract]
    public class Category : ITimestampData, 
        ICategoryable<string>,
        IDisuseable<Category>, 
        ICloneable
    {

        public const string RESOURCE_CATEGORY = "category";

        /// <summary>
        /// 标识。
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// 类型编码。
        /// </summary>
        [DataMember]
        public string Code { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 详细。
        /// </summary>
        [DataMember]
        public string Desc { get; set; }

        /// <summary>
        /// 排序号。
        /// </summary>
        [DataMember]
        public long Sequence { get; set; }

        /// <summary>
        /// 应用范围。
        /// </summary>
        [DataMember]
        public int Scope { get; set; }

        /// <summary>
        /// 上级类型标识。
        /// </summary>
        [DataMember]
        public string ParentId { get; set; }

        private Category parent;
        /// <summary>
        /// 上级类型。
        /// </summary>
        [DataMember]
        public virtual Category Parent
        {
            get { return parent; }
            set
            {
                if (parent != value)
                {
                    parent = value;
                    ParentId = parent == null ? null : parent.Id;
                }
            }
        }

        /// <summary>
        /// 指示是否被废弃；true表示废弃，不再使用；false表示未废弃，仍在使用。
        /// </summary>
        [DataMember]
        public bool Disused { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        [DataMember]
        public DateTime Creation { get; set; }

        /// <summary>
        /// 修改时间。
        /// </summary>
        [DataMember]
        public DateTime Modification { get; set; }

        static Category()
        {
            ResourceRegistry.Registrate(RESOURCE_CATEGORY, Resource.METHOD_LIST, "");
        }

        /// <summary>
        /// 保存。
        /// </summary>
        /// <param name="tree">类型树。</param>
        /// <param name="action">保存操作。</param>
        /// <param name="timestampFactory">获取统一时间的操作。</param>
        /// <param name="isCodeExistedPredicate">判断编号是否已被使用的操作。</param>
        public void Save(TreeNodeCollection<Category> tree, 
            Action<Category> action,
            Func<DateTime> timestampFactory = null,
            Func<int, string, bool> isCodeExistedPredicate = null)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new InvalidOperationException("Name is not set yet.");
            }
            if (!string.IsNullOrWhiteSpace(this.Id) && !string.IsNullOrWhiteSpace(this.ParentId) && tree == null)
            {
                throw new InvalidOperationException("Please provide tree when Id and ParentId are not null.");
            }
            if (!string.IsNullOrWhiteSpace(this.Id) && !string.IsNullOrWhiteSpace(this.ParentId) && IsCyclicReference(tree))
            {
                throw new CyclicInheritanceException();
            }
            if (isCodeExistedPredicate != null && isCodeExistedPredicate(Scope, Code))
            {
                throw new ObjectAlreadyExistedException<Category, string>(this, this.Code);
            }

            if (string.IsNullOrWhiteSpace(Code))
            {
                Code = Id;
            }
            DateTime timestamp = timestampFactory == null ? DateTime.Now : timestampFactory();
            Creation = timestamp;
            Modification = timestamp;

            if (action != null)
            {
                action(this);
            }
        }

        public void Update(TreeNodeCollection<Category> tree,
            Action<IEnumerable<Category>> action,
            Func<DateTime> timestampFactory = null)
        {
            if (tree == null)
            {
                throw new ArgumentNullException();
            }
            if (!IsRequiredAllSet())
            {
                throw new InvalidOperationException();
            }
            if (IsCyclicReference(tree))
            {
                throw new CyclicInheritanceException();
            }

            DateTime timestamp = timestampFactory == null ? DateTime.Now : timestampFactory();
            Modification = timestamp;

            List<Category> list = new List<Category>();
            IEnumerable<Category> categories = GetDescendants(tree);
            foreach (var category in categories)
            {
                if (category.Disused != this.Disused)
                {
                    category.Disused = this.Disused;
                    category.Modification = timestamp;
                    list.Add(category);
                }
            }
            list.Add(this);

            if (action != null)
            {
                action(list);
            }
        }

        /// <summary>
        /// 删除类型。
        /// </summary>
        /// <param name="tree">类型树。</param>
        /// <param name="action">删除操作。</param>
        public void Delete(TreeNodeCollection<Category> tree,
            Action<IEnumerable<Category>> action)
        {
            if (tree == null)
            {
                throw new ArgumentNullException();
            }
            IEnumerable<Category> categories = GetDescendants(tree, false);
            List<Category> list = new List<Category>(categories);
            list.Add(this);

            if (action != null)
            {
                action(list);
            }
        }

        /// <summary>
        /// 深度拷贝。
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Category copy = ShallowClone();
            if (this.Parent != null)
            {
                copy.Parent = this.Parent.Clone() as Category;
            }

            return copy;
        }

        /// <summary>
        /// 浅度拷贝。
        /// </summary>
        /// <returns></returns>
        public Category ShallowClone()
        {
            Category copy = new Category();
            copy.Id = this.Id;
            copy.Code = this.Code;
            copy.Name = this.Name;
            copy.Desc = this.Desc;
            copy.Sequence = this.Sequence;
            copy.Disused = this.Disused;
            copy.Scope = this.Scope;
            copy.ParentId = this.ParentId;
            copy.Creation = this.Creation;
            copy.Modification = this.Modification;

            return copy;
        }

        /// <summary>
        /// 获取以当前类型为根节点的子树上的所有子类型。
        /// </summary>
        /// <param name="tree">类型树。</param>
        /// <param name="isPreorder">是否以先序顺序访问子树。</param>
        /// <returns></returns>
        public IEnumerable<Category> GetDescendants(TreeNodeCollection<Category> tree,
            bool? isPreorder = null)
        {
            if (tree == null)
            {
                throw new ArgumentNullException();
            }

            List<Category> list = new List<Category>();
            bool contains = false;
            Tree<Category>.PreorderTraverse(tree,
                (e) =>
                {
                    if (e.Data.Id == this.Id)
                    {
                        contains = true;
                        if (!isPreorder.HasValue || isPreorder.Value)
                        {
                            Tree<Category>.PreorderTraverse(e.Children,
                                (node) =>
                                {
                                    list.Add(node.Data);
                                });
                        }
                        else
                        {
                            Tree<Category>.PostorderTraverse(e.Children,
                                (node) =>
                                {
                                    list.Add(node.Data);
                                });
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            if (!contains)
            {
                throw new ArgumentException("The category is not existed in the tree.");
            }

            return list;
        }

        /// <summary>
        /// 判断类型的父类型或祖先类型是否存在环形引用。
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        public bool IsCyclicReference(TreeNodeCollection<Category> tree)
        {
            if (tree == null)
            {
                throw new ArgumentNullException();
            }

            bool result = false;
            if (!string.IsNullOrWhiteSpace(this.Id) && !string.IsNullOrWhiteSpace(this.ParentId))
            {
                if (this.Id == this.ParentId)
                {
                    result = true;
                }
                else
                {
                    bool contains = false;
                    Tree<Category>.PreorderTraverse(tree,
                        (e) =>
                        {
                            if (e.Data.Id == this.ParentId)
                            {
                                contains = true;
                                TreeNode<Category> parent = e;
                                while (parent != null)
                                {
                                    if (this.Id == parent.Data.Id)
                                    {
                                        result = true;
                                        break;
                                    }
                                    parent = parent.Parent;
                                }
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        });
                    if (!contains)
                    {
                        throw new ArgumentException("The category is not existed in the tree.");
                    }
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!(obj is Category))
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(Id) && Id.Equals(((Category)obj).Id);
        }

        public override int GetHashCode()
        {
            if (!string.IsNullOrWhiteSpace(Id))
            {
                return Id.GetHashCode();
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public static TreeNodeCollection<Category> BuildTree(IEnumerable<Category> col)
        {
            TreeNodeCollection<Category> tree = new TreeNodeCollection<Category>();

            if (col != null && col.Count() > 0)
            {
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
                SortTree(tree);
            }

            return tree;
        }

        public static void CleanupTree(TreeNodeCollection<Category> tree)
        {
            if (tree != null && tree.Count > 0)
            {
                tree.Clear();
            }
        }

        public static void SortTree(TreeNodeCollection<Category> tree)
        {
            if (tree == null || tree.Count == 0)
            {
                return;
            }

            tree.Sort(CompareToBySequence);
            Tree<Category>.PreorderTraverse(tree,
                (e) =>
                {
                    if (!e.IsLeaf)
                    {
                        e.Children.Sort(CompareToBySequence);
                    }
                });
        }

        public static int CompareToBySequence(TreeNode<Category> node1, TreeNode<Category> node2)
        {
            return node2.Data.Sequence.CompareTo(node1.Data.Sequence);
        }

        private bool IsRequiredAllSet()
        {
            return (!string.IsNullOrWhiteSpace(Id)
                && !string.IsNullOrWhiteSpace(Code)
                && !string.IsNullOrWhiteSpace(Name)
                && Creation != DateTime.MinValue
                && Modification != DateTime.MinValue);
        }

    }

}
