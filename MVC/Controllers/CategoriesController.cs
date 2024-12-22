#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Contexts;
using DataAccess.Entities;
using Business.Services;
using Business.Models;
using DataAccess.Results.Bases;
using Microsoft.AspNetCore.Authorization;
using MVC.Attributes;

//Generated from Custom Template.
namespace MVC.Controllers
{
    [Authorize]
    [AdminAuthorize] // Tüm eylemler için admin yetkilendirme ekliyoruz
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        public IActionResult Index()
        {
            List<CategoryModel> categoryList = _categoryService.Query().ToList();
            return View(categoryList);
        }

        // GET: Categories/Details/5
        public IActionResult Details(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(s => s.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel category)
        {
            return !ModelState.IsValid
                ? View(category)
                : _categoryService.Add(category).IsSuccessful
                    ? RedirectToAction(nameof(Index))
                    : (IActionResult)View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(s => s.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                Result result = _categoryService.Update(category);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = category.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(s => s.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Result result = _categoryService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
