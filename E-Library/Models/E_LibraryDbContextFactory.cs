//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System.IO;

//namespace E_Library.Models
//{
//    public class E_LibraryDbContextFactory : IDesignTimeDbContextFactory<E_LibraryDbContext>
//    {
//        public E_LibraryDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<E_LibraryDbContext>();

//            optionsBuilder.UseSqlServer(
//            "Server=DESKTOP-UIS5U63;Database=E_Library;Trusted_Connection=True;TrustServerCertificate=True");
//            //"Server=(localdb)\\MSSQLLocalDB;Database=E_Library;Trusted_Connection=True;MultipleActiveResultSets=true");

//            return new E_LibraryDbContext(optionsBuilder.Options);
//        }
//    }
//}