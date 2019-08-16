using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace soundbyteAPI.Model
{
    public partial class soundbyteContext : DbContext
    {
        public soundbyteContext()
        {
        }

        public soundbyteContext(DbContextOptions<soundbyteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Video> Video { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:soundbyte.database.windows.net,1433;Initial Catalog=soundbyte;Persist Security Info=False;User ID=soundadmin;Password=brunoMars27;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Video>(entity =>
            {
                entity.Property(e => e.ThumbnailUrl).IsUnicode(false);

                entity.Property(e => e.VideoTitle).IsUnicode(false);

                entity.Property(e => e.VideoType).IsUnicode(false);

                entity.Property(e => e.WebUrl).IsUnicode(false);
            });
        }
    }
}
