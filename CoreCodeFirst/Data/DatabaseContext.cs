using CoreCodeFirst.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CoreCodeFirst.Data
{
    public class DatabaseContext: DbContext
    {
  
 
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
          

        }
       

        public DbSet<Product> Products { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserType> UserType { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        //to get users list

    }
}
