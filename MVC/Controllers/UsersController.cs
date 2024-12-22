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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MVC.Attributes;


namespace MVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public UsersController(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
        }

       
        [AdminAuthorize]
        public IActionResult Index()
        {
            List<UserModel> userList = _userService.GetList();
            return View(userList);
        }

        
        [AdminAuthorize]
        public IActionResult Details(int id)
        {
            UserModel user = _userService.GetItem(id);
            if (user == null)
            {
                return View("Error", "User Not Found!!");
            }
            return View(user);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Register(model);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Login(string email, string password)
        {
            var result = _userService.Login(email, password);
            if (result.IsSuccessful)
            {
                TempData["Message"] = result.Message;
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", result.Message);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // GET: Users/Create
        [AdminAuthorize]
        public IActionResult Create()
        {
            ViewBag.Posts = new MultiSelectList(_postService.Query().ToList(), "Id", "Title");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public IActionResult Create(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                PreparePostsList();
                return View(user);
            }

            var result = _userService.Add(user);
            if (result.IsSuccessful)
            {
                TempData["Message"] = result.Message;
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }

            ModelState.AddModelError("", result.Message);
            PreparePostsList();
            return View(user);
        }

        private void PreparePostsList()
        {
            ViewBag.Posts = new MultiSelectList(_postService.Query().ToList(), "Id", "Title");
        }

      
        [AdminAuthorize]
        public IActionResult Edit(int id)
        {
            UserModel user = _userService.GetItem(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewBag.Posts = new MultiSelectList(_postService.Query().ToList(), "Id", "Title");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public IActionResult Edit(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Update(user);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.Posts = new MultiSelectList(_postService.Query().ToList(), "Id", "Name");
            return View(user);
        }

      
        [AdminAuthorize]
        public IActionResult Delete(int id)
        {
            UserModel user = _userService.GetItem(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _userService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
