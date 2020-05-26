using HealthyService.Core.Database.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace HealthyService.Core.Database
{
    public class HealthyServiceContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<UserDetails> UsersDetails { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<ProductMeal> ProductMeals { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=HealthyService;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Email).HasMaxLength(30).IsRequired();

                entity.Property(p => p.Password).HasMaxLength(50).IsRequired();

                entity.Property(p => p.Name).HasMaxLength(20).IsRequired();
                
                entity.Property(p => p.Login).HasMaxLength(20).IsRequired();

                entity.Property(p => p.SureName).HasMaxLength(25).IsRequired();

                entity.Property(p => p.IsActive).IsRequired();

                entity.Property(p => p.IsDeleted).IsRequired();

                entity.Property(p => p.CreateDate).IsRequired();

                entity.HasMany(p => p.Products).WithOne(p => p.User);
                entity.HasMany(p => p.Meals).WithOne(p => p.User);
                entity.HasMany(p => p.UsersDetails).WithOne(p => p.User);
            });

            modelBuilder.Entity<UserDetails>(entity =>
            {
                entity.ToTable("UsersDetails");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Weight);

                entity.Property(p => p.Height);

                entity.Property(p => p.Age);

                entity.Property(p => p.ActivityLevel).HasConversion<string>();

                entity.Property(p => p.Gender).HasConversion<string>();

                entity.Property(p => p.UserProteinLevel);

                entity.Property(p => p.UserCarboLevel);

                entity.Property(p => p.UserFatLevel);

                entity.Property(p => p.UserDemendLevel);

                entity.Property(p => p.WaistCircumference); //Pas

                entity.Property(p => p.HipCircumference); //Biodro

                entity.Property(p => p.ChestCircumference); //Klatka

                entity.Property(p => p.CalfCircumference); //Łydka

                entity.Property(p => p.ThighCircumference); //Udo

                entity.Property(p => p.ArmCircumference); //Biceps

                entity.Property(p => p.ForearmCircumference); //Przedramie

                entity.HasOne(p => p.User).WithMany(p => p.UsersDetails).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name).HasMaxLength(30).IsRequired();

                entity.Property(p => p.Protein).IsRequired();

                entity.Property(p => p.Carbo).IsRequired();

                entity.Property(p => p.Fat).IsRequired();

                entity.Property(p => p.ProductMeasure).HasConversion<string>().IsRequired();

                entity.Property(p => p.IsActive).IsRequired();

                entity.Property(p => p.IsDeleted).IsRequired();

                entity.Property(p => p.CreateDate).IsRequired();

                entity.HasMany(p => p.Meals).WithOne(p => p.Product);

                entity.HasOne(p => p.User).WithMany(p => p.Products).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.ToTable("Meals");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name).HasMaxLength(30).IsRequired();

                entity.Property(p => p.MealDateTime).IsRequired();

                entity.HasOne(p => p.User).WithMany(p => p.Meals).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict).HasConstraintName("FK_UMU");
                entity.HasMany(p => p.Products).WithOne(p => p.Meal);

            });

            modelBuilder.Entity<ProductMeal>()
                .HasKey(t => new { t.ProductId, t.MealId });
            modelBuilder.Entity<ProductMeal>()
                .HasOne(pm => pm.Product)
                .WithMany(p => p.Meals)
                .HasForeignKey(pm => pm.ProductId);

            modelBuilder.Entity<ProductMeal>()
                .HasOne(pm => pm.Meal)
                .WithMany(p => p.Products)
                .HasForeignKey(pm => pm.MealId);

            modelBuilder.Entity<ProductMeal>()
                .Property(q => q.Amount);
        }
    }
}
