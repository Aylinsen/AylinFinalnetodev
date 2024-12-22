using Business.Models;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Business.Services
{
    public interface IUserService
    {
        IQueryable<UserModel> Query();
        Result Add(UserModel model);
        Result Update(UserModel model);
        Result Delete(int id);

        List<UserModel> GetList();
        UserModel GetItem(int id);
        Result Register(UserModel model); 
        Result Login(string email, string password); 
    }
     
    public class UserService : ServiceBase, IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(Db db, IHttpContextAccessor httpContextAccessor) : base(db)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IQueryable<UserModel> Query()
        {
	        return _db.Users.Include(u => u.PostOwners).ThenInclude(po => po.Post)
	        .OrderByDescending(u => u.IsAdmin).ThenBy(u => u.BirthDate).ThenBy(u => u.Username).ThenBy(u => u.Username)
	        .Select(u => new UserModel()
	        {
                Id = u.Id,
                Username = u.Username,
                Prof = u.Prof,
                BirthDate = u.BirthDate,
                IsAdmin = u.IsAdmin,
                Email = u.Email,
                Password = u.Password,

                BirthDateOutput = u.BirthDate.ToString("MM/dd/yyyy"),
                EmailOutput = u.Email,
                PasswordOutput = u.Password,
                IsAdminOutput = u.IsAdmin ? "Yes" : "No",
                FullNameOutput = u.Username,
                PostIdsInput = u.PostOwners.Select(po => po.PostId).ToList(),

                PostsNamesOutput = string.Join("<br />", u.PostOwners.Select(po => po.Post.Title))

            });
        }
        public Result Register(UserModel model)
        {
            if (_db.Users.Any(u => u.Email.ToLower() == model.Email.ToLower()))
            {
                return new ErrorResult("Email already exists!");
            }

            User entity = new User()
            {
                Username = model.Username,
                Prof = model.Prof,
                BirthDate = model.BirthDate,
                IsAdmin = model.IsAdmin,
                Email = model.Email,
                Password = model.Password,
                PostOwners = model.PostIdsInput?.Select(postId => new PostOwner()
                {
                    PostId = postId
                }).ToList()
            };

            _db.Users.Add(entity);
            _db.SaveChanges();

            model.Id = entity.Id;

            return new SuccessResult("User has been registered.");
        }

        public Result Login(string email, string password)
        {
      
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return new ErrorResult("Email and password cannot be empty.");
            }

            var user = _db.Users.SingleOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (user == null || user.Password != password)
            {
                return new ErrorResult("Invalid email or password!");
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User") 
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties).Wait();
            }

            return new SuccessResult("Aylin App e giris saglandı");
        }


        public Result Add(UserModel model)
        {
			if (_db.Users.Any(o => o.Username.ToLower() == model.Username.ToLower().Trim() && o.Username.ToLower() == model.Username.ToLower().Trim()))
				return new ErrorResult("Owner with same name and surname exists!");

			User entity = new User()
            {
                Username = model.Username,
                Prof = model.Prof,
                BirthDate = model.BirthDate,
                IsAdmin = model.IsAdmin,
                Email = model.Email,
                Password = model.Password,

		        PostOwners = model.PostIdsInput?.Select(postId => new PostOwner()
					{
						PostId = postId
					}).ToList()
			};

            _db.Users.Add(entity);
            _db.SaveChanges();

            model.Id = entity.Id;

            return new SuccessResult("User CREATED Succesfully!!");
        }

        public Result Update(UserModel model)
        {
			if (_db.Users.Any(o => o.Id != model.Id && o.Username.ToLower() == model.Username.ToLower().Trim() && o.Username.ToLower() == model.Username.ToLower().Trim()))
				return new ErrorResult("Owner with same name and surname exists!");
			var entity = _db.Users.Include(o => o.PostOwners).SingleOrDefault(o => o.Id == model.Id);
			_db.PostOwners.RemoveRange(entity.PostOwners);

			
			if (entity is null)
                return new ErrorResult("User not found!");

            entity.Username = model.Username;
            entity.Prof = model.Prof;
            entity.BirthDate = model.BirthDate;
            entity.IsAdmin = model.IsAdmin;
            entity.Email = model.Email;
            entity.Password = model.Password;
			entity.PostOwners = model.PostIdsInput?.Select(postId => new PostOwner()
			{
				PostId = postId
			}).ToList();

			_db.Users.Update(entity);
            _db.SaveChanges();

            return new SuccessResult("User UPDATED Succesfully!!");
        }

        public Result Delete(int id)
        {
			var entity = _db.Users.Include(o => o.PostOwners).SingleOrDefault(o => o.Id == id);
			if (entity is null)
                return new ErrorResult("User not found!");
			_db.PostOwners.RemoveRange(entity.PostOwners);

			_db.Users.Remove(entity);
            _db.SaveChanges();

            return new SuccessResult("User Deleted Succesfully!!");
        }
        public List<UserModel> GetList() => Query().ToList();

        public UserModel GetItem(int id) => Query().SingleOrDefault(q => q.Id == id);
    }
}
