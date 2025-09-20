using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace E_Library.Models;

public class E_LibraryDbContext : IdentityDbContext<ApplicationUser>
{
    public E_LibraryDbContext(DbContextOptions options) : base(options)
    {  
    }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<BookToCategoryMapping> BookToCategoryMappings { get; set; }
    public DbSet<UserToBookMapping> UserToBookMappings { get; set; }

}
