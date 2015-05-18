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

        public string Code
        {
            get { return this.model.Code; }
            set
            {
                if (this.model.Code != value)
                {
                    this.model.Code = value;
                    OnPropertyChanged("Code");
                }
            }
        }

        public override string Name
        {
            get { return this.model.Name; }
            set
            {
                if (this.model.Name != value)
                {
                    this.model.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Desc
        {
            get { return this.model.Desc; }
            set
            {
                if (this.model.Desc != value)
                {
                    this.model.Desc = value;
                    OnPropertyChanged("Desc");
                }
            }
        }

        public long Sequence
        {
            get { return this.model.Sequence; }
            set
            {
                if (this.model.Sequence != value)
                {
                    this.model.Sequence = value;
                    OnPropertyChanged("Sequence");
                }
            }
        }

        public override TreeNodeModel Parent
        {
            get
            {
                return base.Parent;
            }
            set
            {
                base.Parent = value;

                if (base.Parent != null)
                {
                    this.model.Parent = (base.Parent as CategoryViewModel).Model;
                }
                else
                {
                    this.model.Parent = null;
                }
            }
        }

        public int Scope
        {
            get { return this.model.Scope; }
            set { this.model.Scope = value; }
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
        }

        internal CategoryViewModel Next()
        {
            CategoryViewModel nextViewModel = new CategoryViewModel();
            nextViewModel.Scope = this.Scope;
            nextViewModel.Sequence = this.Sequence + 1;
            nextViewModel.Parent = this.Parent;

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

        public static void ClearTree(ObservableCollection<TreeNodeModel> tree)
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
                                current.RemoveChild(current.Children[i]);
                            }
                        }
                    });
                tree.Clear();
            }
        }

    }

}
