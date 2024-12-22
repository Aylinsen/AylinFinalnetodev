using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Business.Services;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var isAdmin = User.Identity.IsAuthenticated
                ? int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
                    ? _userService.GetItem(id)?.IsAdmin ?? false
                    : false
                : false;

            ViewData["IsAdmin"] = isAdmin;
            return View();
        }
    }
}
