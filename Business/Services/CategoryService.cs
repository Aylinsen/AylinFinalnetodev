using Business.Models;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Business.Services
{
    public interface ICategoryService
    {
        IQueryable<CategoryModel> Query();
        Result Add(CategoryModel model);
        Result Update(CategoryModel model);
        Result Delete(int id);
    }

    public class CategoryService : ServiceBase, ICategoryService
    {

        public CategoryService(Db db) : base(db)
        {
           
        }

        // Read
        public IQueryable<CategoryModel> Query()
        {
            return _db.Categories.Select(c => new CategoryModel()
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                PostCountOutput = c.Posts.Count,
                PostNamesOutput = string.Join(", ", c.Posts.Select(p => p.Title))
            });
        }

        // Create
        public Result Add(CategoryModel model)
        {
            var normalizedName = model.CategoryName.Trim().ToLower();
            var exists = _db.Categories
                .Select(c => c.CategoryName.ToLower())
                .Contains(normalizedName);

            if (exists)
                return new ErrorResult("Category with the same name exists!");

            var newCategory = new Category
            {
                CategoryName = model.CategoryName.Trim()
            };

            _db.Categories.Add(newCategory);
            _db.SaveChanges();

            return new SuccessResult("Category added successfully.");
        }

        // Update
        public Result Update(CategoryModel model)
        {
            var trimmedName = model.CategoryName.Trim().ToLower();
            var existingCategory = _db.Categories
                .FirstOrDefault(c => c.Id != model.Id &&
                                    EF.Functions.Like(c.CategoryName.ToLower(), trimmedName));

            if (existingCategory != null)
                return new ErrorResult("Category with the same name exists!");

            var categoryToUpdate = _db.Categories.Find(model.Id);
            if (categoryToUpdate == null)
                return new ErrorResult("Category not found!");

            categoryToUpdate.CategoryName = model.CategoryName.Trim();
            _db.SaveChanges();

            return new SuccessResult("Category updated successfully.");
        }

        // Delete
        public Result Delete(int id)
        {
            Category entity = _db.Categories.Find(id);
            if (entity is null)
                return new ErrorResult("Category not found!");
            _db.Categories.Remove(entity);
            _db.SaveChanges();
            return new SuccessResult("Category deleted successfully.");
        }
    }
}
