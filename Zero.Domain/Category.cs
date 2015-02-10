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
    public class Category : ITimestampData, ICategoryable, IDisuseable<Category>
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
        /// 排序号。
        /// </summary>
        [DataMember]
        public int Order { get; set; }

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

        /// <summary>
        /// 上级类型。
        /// </summary>
        [DataMember]
        public virtual Category Parent { get; set; }

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
            Func<string, Category> parentAccessor = null)
        {
            if (string.IsNullOrWhiteSpace(Id)
                || string.IsNullOrWhiteSpace(Name)
                || Scope < 1)
            {
                throw new InvalidOperationException();
            }

            if (string.IsNullOrWhiteSpace(Code))
            {
                Code = Id;
            }
            if (isCodeExistedPredicate != null && isCodeExistedPredicate(Scope, Code))
            {
                throw new Nega.Common.ObjectAlreadyExistedException<Category, string>(this, this.Code);
            }
            if(Parent != null && string.IsNullOrWhiteSpace(Parent.Id))
            {
                ParentId = Parent.Id;
            }
            if ((!string.IsNullOrWhiteSpace(ParentId) && Id == ParentId)
                || IsCyclicReference(parentAccessor))
            {
                throw new Nega.Common.CyclicInheritanceException();
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
        /// 更改名称。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="action">定义保存操作。</param>
        /// <param name="timestampFactory">定义获取统一时间的操作。</param>
        public void ChangeName(string name, 
            Action<Category> action,
            Func<DateTime> timestampFactory = null)
        {
            if (!IsRequiredAllSet())
            {
                throw new InvalidOperationException();
            }

            if (Name != name)
            {
                Name = name;

                Update(action, timestampFactory);
            }
        }

        /// <summary>
        /// 更改排序号。
        /// </summary>
        /// <param name="order">排序号。</param>
        /// <param name="action">定义保存操作。</param>
        /// <param name="timestampFactory">定义获取统一时间的操作。</param>
        public void ChangeOrder(int order,
            Action<Category> action,
            Func<DateTime> timestampFactory = null)
        {
            if (!IsRequiredAllSet())
            {
                throw new InvalidOperationException();
            }

            if (Order != order)
            {
                Order = order;

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
            Func<string, Category> parentAccessor = null)
        {
            if (!IsRequiredAllSet())
            {
                throw new InvalidOperationException();
            }

            if (Parent != parent)
            {
                Parent = parent;
                ParentId = parent == null ? null : parent.Id;
                if ((!string.IsNullOrWhiteSpace(ParentId) && Id == ParentId)
                    || (parent != null && IsCyclicReference(parentAccessor)))
                {
                    throw new Nega.Common.CyclicInheritanceException();
                }

                Update(action, timestampFactory);
            }
        }

        /// <summary>
        /// 判断当前类型与其上级类型是否形成了环引用。
        /// </summary>
        /// <param name="parentAccessor">定义获取上级类型的操作。</param>
        /// <returns>如果有环引用，则返回true。</returns>
        public bool IsCyclicReference(Func<string, Category> parentAccessor)
        {
            if (string.IsNullOrWhiteSpace(ParentId) && Parent != null)
            {
                ParentId = Parent.Id;
            }
            if (string.IsNullOrWhiteSpace(ParentId))
            {
                return false;
            }
            else
            {
                if (Parent != null)
                {
                    return this.Equals(Parent) ? true : Parent.IsCyclicReference(parentAccessor);
                }
                else
                {
                    Category found = parentAccessor == null ? null : parentAccessor(ParentId);
                    if(found != null)
                    {
                        return this.Equals(found) ? true : found.IsCyclicReference(parentAccessor);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public void Disuse(Action<Category> action)
        {
            if (!IsRequiredAllSet())
            {
                throw new InvalidOperationException();
            }

            if (!this.Disused)
            {
                this.Disused = true;

                Update(action, null);
            }
        }

        public void Use(Action<Category> action)
        {
            if (!IsRequiredAllSet())
            {
                throw new InvalidOperationException();
            }

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

        private void Update(Action<Category> action,
            Func<DateTime> timestampFactory = null)
        {
            DateTime timestamp = timestampFactory == null ? DateTime.Now : timestampFactory();
            Modification = timestamp;

            if (action != null)
            {
                action(this);
            }
        }

        private bool IsRequiredAllSet()
        {
            return (!string.IsNullOrWhiteSpace(Id)
                && !string.IsNullOrWhiteSpace(Code)
                && !string.IsNullOrWhiteSpace(Name)
                && Scope > 0
                && Creation != DateTime.MinValue
                && Modification != DateTime.MinValue);
        }

    }

}
