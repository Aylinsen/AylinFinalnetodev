using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class Db : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PostOwner> PostOwners { get; set; }

        public DbSet<Category> Categories { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }

    }
}
