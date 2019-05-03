using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoAlbum.Data
{
    public class Context : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Images> Images { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
    }
}
