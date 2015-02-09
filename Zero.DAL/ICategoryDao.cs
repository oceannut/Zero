﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nega.Data;

using Zero.Domain;

namespace Zero.DAL
{

    public interface ICategoryDao : IDao<Category>
    {

        Category Get(string id, bool? includeParent = null);

        Category GetByCode(int scope, string code, bool? includeParent = null);

        bool IsCodeExisted(int scope, string code);

        int Count(int? scope = null, string parentId = null);

        IEnumerable<Category> List(PagingRequest request, int? scope = null, bool? includeParent = null, string parentId = null);

    }

}
