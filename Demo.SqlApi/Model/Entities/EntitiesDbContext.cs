namespace Demo.SqlApi.Model.Entities
{
    using System.Data.Entity;

    public partial class EntitiesDbContext : DbContext
    {
        public EntitiesDbContext()
            : base("name=Entities")
        {
        }

        public virtual DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
