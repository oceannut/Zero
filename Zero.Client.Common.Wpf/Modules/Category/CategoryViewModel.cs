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

        private readonly Category model;
        public Category Model
        {
            get { return model; }
        }

        public CategoryViewModel() :
            this(new Category())
        {

        }

        public CategoryViewModel(Category model)
        {
            if (model == null)
            {
                throw new ArgumentNullException();
            }

            this.model = model;
            this.Name = model.Name;
        }

        public CategoryViewModel(int scope, int sequence)
            : this()
        {
            this.model.Scope = scope;
            this.model.Sequence = sequence;
        }

        public CategoryViewModel(int scope, int sequence, 
            Category parent)
            : this(scope, sequence)
        {
            this.model.Parent = parent;
        }

        internal CategoryViewModel Next()
        {
            CategoryViewModel nextViewModel = new CategoryViewModel();
            nextViewModel.Model.Scope = this.model.Scope;
            nextViewModel.Model.Sequence = this.model.Sequence + 1;
            nextViewModel.Model.Parent = this.model.Parent;

            return nextViewModel;
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
                    if (string.IsNullOrWhiteSpace(current.Model.ParentId))
                    {
                        tree.Add(current);
                    }
                    else
                    {
                        TreeNodeModel parent = Tree.Find(tree,
                            (e) => (e as CategoryViewModel).Model.Id == current.Model.ParentId) as TreeNodeModel;
                        if (parent == null)
                        {
                            queue.Enqueue(current);
                        }
                        else
                        {
                            parent.AddChild(current);
                        }
                    }
                }
            }

            return tree;
        }

        public static void CleanupTree(ObservableCollection<TreeNodeModel> tree)
        {
            if (tree != null && tree.Count > 0)
            {
                Tree.PostorderTraverse(tree,
                    (e) =>
                    {
                        CategoryViewModel current = e as CategoryViewModel;
                        if (current.Children != null && current.Children.Count > 0)
                        {
                            for (int i = current.Children.Count - 1; i >= 0; i--)
                            {
                                current.RemoveChildAt(i);
                            }
                        }
                    });
                tree.Clear();
            }
        }

    }

}
