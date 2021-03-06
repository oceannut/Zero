﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

using Nega.Common;
using Nega.WcfCommon;

using Zero.Domain;
using Zero.BLL;
using R = Zero.Service.Rest.Properties.Resources;

namespace Zero.Service.Rest
{

    public class CategoryRestServiceImpl : ICategoryRestService
    {

        private readonly ICategoryService categoryService;

        public CategoryRestServiceImpl(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public Category SaveCategory(string scope, string name, string desc, string parentId)
        {
            int scopeInt = Scope2Int(scope);
            ValidateNameAndDesc(name, desc);
            
            try
            {
                Category parent = GetParent(scopeInt, parentId);
                Category category = new Category
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Desc = desc,
                    Scope = scopeInt,
                    Parent = parent
                };
                TreeNodeCollection<Category> tree = this.categoryService.TreeCategory(scopeInt);
                category.Save(tree,
                    (e) =>
                    {
                        this.categoryService.SaveCategory(category);
                    });
                return category;
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Replace(ex);
            }
        }

        public Category UpdateCategory(string scope, string id, string name, string desc)
        {
            int scopeInt = Convert.ToInt32(scope);
            ValidateId(id);
            ValidateNameAndDesc(name, desc);

            try
            {
                TreeNodeCollection<Category> tree = this.categoryService.TreeCategory(scopeInt);
                Category category = this.categoryService.GetCategory(scopeInt, id);
                category.Name = name;
                category.Desc = desc;
                category.Update(tree,
                    (e) =>
                    {
                        this.categoryService.UpdateCategory(category);
                    });

                return category;
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Replace(ex);
            }
        }

        public void DeleteCategory(string scope, string id)
        {
            int scopeInt = Convert.ToInt32(scope);
            ValidateId(id);

            try
            {
                Category category = this.categoryService.GetCategory(scopeInt, id);
                this.categoryService.DeleteCategory(category);
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Replace(ex);
            }
        }

        public Category GetCategory(string scope, string id)
        {
            throw new NotImplementedException();
        }

        public Category GetCategoryByCode(string scope, string code)
        {
            throw new NotImplementedException();
        }

        public Category[] ListCategories(string scope)
        {
            int scopeInt = Convert.ToInt32(scope);

            try
            {
                return this.categoryService.ListCategory(scopeInt).ToArray();
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.Replace(ex);
            }
        }

        private int Scope2Int(string scope)
        {
            int scopeInt = 0;
            try
            {
                scopeInt = Convert.ToInt32(scope);
            }
            catch (Exception)
            {
                throw new WebFaultException<string>(R.ExCategoryScope, HttpStatusCode.BadRequest);
            }
            if (scopeInt < 1)
            {
                throw new WebFaultException<string>(R.ExCategoryScope, HttpStatusCode.BadRequest);
            }

            return scopeInt;
        }

        private void ValidateId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new WebFaultException<string>(R.ExCategoryIdRequired, HttpStatusCode.BadRequest);
            }
        }

        private void ValidateNameAndDesc(string name, string desc)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new WebFaultException<string>(R.ExCategoryNameRequired, HttpStatusCode.BadRequest);
            }
        }

        private Category GetParent(int scope, string parentId)
        {
            Category parent = null;
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                parent = this.categoryService.GetCategory(scope, parentId);
            }

            return parent;
        }

    }

}
