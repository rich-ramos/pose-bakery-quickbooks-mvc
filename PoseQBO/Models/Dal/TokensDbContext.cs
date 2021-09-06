using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoseQBO.Models.Dal
{
    public class TokensDbContext : DbContext
    {
        public TokensDbContext() { }

        public TokensDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Token> Token { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) { }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(e => e.RealmId);

                entity.Property(e => e.RealmId)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.AccessToken)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.RefreshToken)
                    .IsRequired()
                    .HasMaxLength(1000);
            });
        }
    }
}
