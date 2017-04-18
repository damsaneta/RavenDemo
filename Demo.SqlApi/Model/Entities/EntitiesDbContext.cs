namespace Demo.SqlApi.Model.Entities
{
    using System.Data.Entity;

    public class EntitiesDbContext : DbContext
    {
        public EntitiesDbContext()
            : base("name=Entities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>();
        }
    }
}
