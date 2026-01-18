using Microsoft.EntityFrameworkCore;

namespace ETicaret.ShoppingCarts.WebAPI.Context
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
