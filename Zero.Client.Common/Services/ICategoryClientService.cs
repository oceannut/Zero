using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Zero.Domain;

namespace Zero.Client.Common
{

    public interface ICategoryClientService
    {

        void SaveCategory(Category category,
            Action<Category> success,
            Action<Exception> failure);

        void UpdateCategory(Category category,
            Action<Category> success,
            Action<Exception> failure);

    }

}
