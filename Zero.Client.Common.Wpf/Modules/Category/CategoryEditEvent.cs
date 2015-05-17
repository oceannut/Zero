using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zero.Client.Common.Wpf
{

    public enum CategoryEditAction
    {
        Save,
        Cancel
    }

    public class CategoryEditEvent
    {

        public CategoryEditAction Action { get; set; }

        public CategoryViewModel ViewModel { get; set; }

        public Exception Exception { get; set; }

        public CategoryEditEvent() { }

        public CategoryEditEvent(CategoryEditAction action, 
            CategoryViewModel viewModel) 
        {
            this.Action = action;
            this.ViewModel = viewModel;
        }

        public CategoryEditEvent(CategoryEditAction action,
            CategoryViewModel viewModel,
            Exception ex)
        {
            this.Action = action;
            this.ViewModel = viewModel;
            this.Exception = ex;
        }

    }

}
