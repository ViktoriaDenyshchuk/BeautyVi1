using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


public class BeautyViContextFactory : IDesignTimeDbContextFactory<BeautyViContext>
{
    public BeautyViContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "BeautyVi.WebApp")) // Вказуємо правильну папку
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

        var optionsBuilder = new DbContextOptionsBuilder<BeautyViContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseNpgsql(connectionString);

        return new BeautyViContext(optionsBuilder.Options);
    }
}
