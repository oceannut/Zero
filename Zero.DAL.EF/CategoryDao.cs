using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;
using Zero.DAL;

namespace Zero.DAL.EF
{
    public class CategoryDao : GenericPageableDao<Category>, ICategoryDao
    {
    }
}
