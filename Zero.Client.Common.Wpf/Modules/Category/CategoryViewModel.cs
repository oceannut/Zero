using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.WpfCommon;

using Zero.Domain;

namespace Zero.Client.Common.Wpf
{

    public class CategoryViewModel : TreeNodeModel
    {

        private Category category;
        public Category Primitive
        {
            get { return category; }
        }

        public CategoryViewModel(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException();
            }

            this.category = category;
            this.Name = category.Name;
        }

        public static ObservableCollection<TreeNodeModel> BuildTree(IEnumerable<Category> col)
        {
            ObservableCollection<TreeNodeModel> tree = new ObservableCollection<TreeNodeModel>();

            if (col != null && col.Count() > 0)
            {
                List<CategoryViewModel> list = (from item in col
                                                select new CategoryViewModel(item))
                                                 .ToList();
                Queue<TreeNodeModel> queue = new Queue<TreeNodeModel>();
                foreach (var item in list)
                {
                    queue.Enqueue(item);
                }
                while (queue.Count > 0)
                {
                    CategoryViewModel current = queue.Dequeue() as CategoryViewModel;
                    if (string.IsNullOrWhiteSpace(current.Primitive.ParentId))
                    {
                        tree.Add(current);
                    }
                    else
                    {
                        TreeNodeModel parent = Tree.Find(tree,
                            (e) => (e as CategoryViewModel).Primitive.Id == current.Primitive.ParentId) as TreeNodeModel;
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
            }

            return tree;
        }

    }

}
