using System;
using System.Collections.Generic;
using System.Linq;
using FBClone.Data;
using FBClone.Data.Infrastructure;
using FBClone.Data.Repositories;
using FBClone.Model;
using System.Linq.Expressions;

namespace FBClone.Service
{
    public interface ICategoryService
    {
        Category GetById(string Id);
        IEnumerable<Category> GetAll();
        IEnumerable<Category> GetMany(string userName);
        IQueryable<Category> Query(Expression<Func<Category, bool>> where);
        Category Add(Category category);
        Category Update(Category category);
        void Delete(string id);
    }
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICategoryRepository categoryRepository;
        public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository)
        {
            this.unitOfWork = unitOfWork;
            this.categoryRepository = categoryRepository;
        }

        public Category GetById(string id)
        {
            return categoryRepository.GetById(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return categoryRepository.GetAll();
        }

        public IEnumerable<Category> Query(Expression<Func<Category, bool>> where)
        {
            return categoryRepository.Query(where);
        }

        public IEnumerable<Category> GetMany(string name)
        {
            return categoryRepository.GetMany(x => x.Name == name);
        }

        IQueryable<Category> ICategoryService.Query(Expression<Func<Category, bool>> where)
        {
            return categoryRepository.Query(where);
        }

        public Category Add(Category category)
        {
            categoryRepository.Add(category);
            unitOfWork.SaveChanges();
            return category;
        }

        public Category Update(Category category)
        {
            categoryRepository.Update(category);
            unitOfWork.SaveChanges();
            return category;
        }

        public void Delete(string id)
        {
            var category = categoryRepository.GetById(id);
            var categories = categoryRepository.GetMany(x => x.Id == id); //<--Anything that has foreign key relationships

            foreach (var item in categories)
            {
                categoryRepository.Delete(item);
            }

            if (category != null)
            {
                categoryRepository.Delete(category);
                unitOfWork.SaveChanges();
            }
        }

    }
}
