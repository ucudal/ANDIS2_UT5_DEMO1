// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;
//
// namespace Monolith.Infrastructure;
//
// public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
// {
//     public AppDbContext CreateDbContext(string[] args)
//     {
//         // Usa un connection string fijo y relativo que no requiere que el archivo exista.
//         // EF no intenta conectarse durante el diseño a menos que llames Migrate/EnsureCreated.
//         var cs = "Data Source=Data/andisdemo.db";
//
//         var configuration = new ConfigurationBuilder()
//             .AddEnvironmentVariables()
//             .Build();
//
//         var options = new DbContextOptionsBuilder<AppDbContext>()
//             .UseSqlite(cs)
//             .Options;
//
//         return new AppDbContext(options, configuration);
//     }
// }
