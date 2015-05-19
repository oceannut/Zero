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
    public class Category : ITimestampData, ICategoryable<string>, IDisuseable<Category>, ICloneable
    {

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

        /// <summary>
        /// 保存。
        /// </summary>
        /// <param name="action">定义保存操作。</param>
        /// <param name="timestampFactory">定义获取统一时间的操作。</param>
        /// <param name="isCodeExistedPredicate">定义判断编号是否已被使用的操作。</param>
        /// <param name="parentAccessor">定义获取上级类型的操作。</param>
        public void Save(Action<Category> action,
            Func<DateTime> timestampFactory = null,
            Func<int, string, bool> isCodeExistedPredicate = null,
            Func<Category, bool> isCategoryCyclicReference = null)
        {
            if (string.IsNullOrWhiteSpace(Id)
                || string.IsNullOrWhiteSpace(Name))
            {
                throw new InvalidOperationException();
            }

            if (string.IsNullOrWhiteSpace(Code))
            {
                Code = Id;
            }
            if (isCodeExistedPredicate != null && isCodeExistedPredicate(Scope, Code))
            {
                throw new ObjectAlreadyExistedException<Category, string>(this, this.Code);
            }
            if (isCategoryCyclicReference != null && isCategoryCyclicReference(this))
            {
                throw new CyclicInheritanceException();
            }
            DateTime timestamp = timestampFactory == null ? DateTime.Now : timestampFactory();
            Creation = timestamp;
            Modification = timestamp;

            if (action != null)
            {
                action(this);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="action">定义保存操作。</param>
        /// <param name="timestampFactory">定义获取统一时间的操作。</param>
        public void Update(Action<Category> action,
            Func<DateTime> timestampFactory = null)
        {
            if (!IsRequiredAllSet())
            {
                throw new InvalidOperationException();
            }

            DateTime timestamp = timestampFactory == null ? DateTime.Now : timestampFactory();
            Modification = timestamp;

            if (action != null)
            {
                action(this);
            }
        }

        /// <summary>
        /// 更改排序号。
        /// </summary>
        /// <param name="sequence">排序号。</param>
        /// <param name="action">定义保存操作。</param>
        /// <param name="timestampFactory">定义获取统一时间的操作。</param>
        public void ChangeSequence(int sequence,
            Action<Category> action,
            Func<DateTime> timestampFactory = null)
        {
            if (Sequence != sequence)
            {
                Sequence = sequence;

                Update(action, timestampFactory);
            }
        }

        /// <summary>
        /// 更改上级类型。
        /// </summary>
        /// <param name="parent">上级类型。</param>
        /// <param name="action">定义保存操作。</param>
        /// <param name="timestampFactory">定义获取统一时间的操作。</param>
        /// <param name="parentAccessor">定义获取上级类型的操作。</param>
        public void ChangeParent(Category parent,
            Action<Category> action,
            Func<DateTime> timestampFactory = null,
            Func<Category, bool> isCategoryCyclicReference = null)
        {
            if (Parent != parent)
            {
                Parent = parent;
                if (isCategoryCyclicReference(this))
                {
                    throw new CyclicInheritanceException();
                }

                Update(action, timestampFactory);
            }
        }

        /// <summary>
        /// 更改类型不再使用。
        /// </summary>
        /// <param name="action">定义保存操作。</param>
        public void Disuse(Action<Category> action)
        {
            if (!this.Disused)
            {
                this.Disused = true;

                Update(action, null);
            }
        }

        /// <summary>
        /// 更改类型使用。
        /// </summary>
        /// <param name="action">定义保存操作。</param>
        public void Use(Action<Category> action)
        {
            if (this.Disused)
            {
                this.Disused = false;

                Update(action, null);
            }
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

        public object Clone()
        {
            Category copy = ShallowClone();
            if (this.Parent != null)
            {
                copy.Parent = this.Parent.Clone() as Category;
            }

            return copy;
        }

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
