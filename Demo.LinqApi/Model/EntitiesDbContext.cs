using System.Data.Entity;
using Demo.Model.EF.Entities;

namespace Demo.LinqApi.Model
{
    public class EntitiesDbContext : DbContext
    {
        public EntitiesDbContext()
            : base("name=Entities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>();
            modelBuilder.Entity<ProductSubcategory>();
            modelBuilder.Entity<UnitMeasure>();
            modelBuilder.Entity<Location>();
            //modelBuilder.Entity<ProductCategory>()
            //    .HasMany(e => e.ProductSubcategory)
            //    .WithRequired(e => e.ProductCategory)
            //    .WillCascadeOnDelete(false);
        }
    }
}