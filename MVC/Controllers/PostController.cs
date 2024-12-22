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
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IPostService postService, ICategoryService categoryService, IUserService userService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _categoryService = categoryService;
            _userService = userService;
            _logger = logger;
        }

        // GET: Posts
        public IActionResult Index()
        {
            List<PostModel> postList = _postService.Query().ToList();
            return View(postList);
        }

        // GET: Posts/Details/5
        public IActionResult Details(int id)
        {
            PostModel post = _postService.Query().SingleOrDefault(s => s.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            LoadViewData();
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostModel post)
        {
            if (!ModelState.IsValid)
            {
                LoadViewData();
                return View(post);
            }

            try
            {
                var result = _postService.Add(post);
                if (!result.IsSuccessful)
                {
                    ModelState.AddModelError("", result.Message);
                    LoadViewData();
                    return View(post);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Post creation failed");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                LoadViewData();
                return View(post);
            }
        }

        // GET: Posts/Edit/5
        public IActionResult Edit(int id)
        {
            PostModel post = _postService.Query().SingleOrDefault(s => s.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            LoadViewData();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PostModel post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Result result = _postService.Update(post);
                    if (result.IsSuccessful)
                    {
                        TempData["Message"] = result.Message;
                        return RedirectToAction(nameof(Details), new { id = post.Id });
                    }
                    ModelState.AddModelError("", result.Message);
                }
                LoadViewData();
                return View(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing a post.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                LoadViewData();
                return View(post);
            }
        }

        // GET: Posts/Delete/5
        public IActionResult Delete(int id)
        {
            PostModel post = _postService.Query().SingleOrDefault(s => s.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                Result result = _postService.Delete(id);
                TempData["Message"] = result.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a post.");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                return View();
            }
        }

        private void LoadViewData()
        {
            var categories = _categoryService.Query().ToList();
            var users = _userService.Query().ToList();

            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            ViewBag.Users = new SelectList(users, "Id", "Username");
        }
    }
}
