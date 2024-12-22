using Business.Models;
using Business.Services.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Results;
using DataAccess.Results.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public interface IPostService
    {
        IQueryable<PostModel> Query();
        Result Add(PostModel model);
        Result Update(PostModel model);
        Result Delete(int id);
    }

    public class PostService : ServiceBase, IPostService
    {
        private readonly ILogger<PostService> _logger;

        public PostService(Db db, ILogger<PostService> logger) : base(db)
        {
            _logger = logger;
        }

        public IQueryable<PostModel> Query()
        {
            return _db.Posts.Select(p => new PostModel()
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                PostedOn = p.PostedOn,
                UserId = p.UserId,
                CategoryId = p.CategoryId
            });
        }

        public Result Add(PostModel model)
        {
            try
            {
                Post entity = new Post()
                {
                    Title = model.Title,
                    Content = model.Content,
                    PostedOn = DateTime.Now,
                    UserId = model.UserId,
                    CategoryId = model.CategoryId
                };

                _db.Posts.Add(entity);
                _db.SaveChanges();

                model.Id = entity.Id;

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a post.");
                return new ErrorResult("An error occurred while adding the post.");
            }
        }

        public Result Update(PostModel model)
        {
            Post entity = _db.Posts.Find(model.Id);
            if (entity is null)
                return new ErrorResult("Post not found!");

            entity.Title = model.Title;
            entity.Content = model.Content;
            entity.UserId = model.UserId;
            entity.CategoryId = model.CategoryId;

            _db.Posts.Update(entity);
            _db.SaveChanges();

            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            Post entity = _db.Posts.Find(id);
            if (entity is null)
                return new ErrorResult("Post not found!");

            _db.Posts.Remove(entity);
            _db.SaveChanges();

            return new SuccessResult();
        }

        public List<PostModel> GetList() => Query().ToList();

        public PostModel GetItem(int id) => Query().SingleOrDefault(q => q.Id == id);
    }
}
