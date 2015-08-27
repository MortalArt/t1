using System.Data.Entity;

namespace Store.Models
{
    public class StoreContext : DbContext
    {
        public DbSet<Item> Item { get; set; }
        public DbSet<Category> Category { get; set; }
    }
}