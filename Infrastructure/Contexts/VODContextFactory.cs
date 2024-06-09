using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Contexts
{
    internal class VODContextFactory : IDesignTimeDbContextFactory<VODContext>
    {
        public VODContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VODContext>();

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\son van\\source\\repos\\webServerMedia\\Admind\\appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);

            return new VODContext(optionsBuilder.Options);
        }
    }
}
