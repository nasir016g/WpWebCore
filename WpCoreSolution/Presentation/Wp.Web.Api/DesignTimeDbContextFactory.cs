//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Wp.Data;

//namespace Wp.Web.Api
//{
//    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WpContext>
//    {
//        public WpContext CreateDbContext(string[] args)
//        {
//            IConfigurationRoot configuration = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("appsettings.json")
//                .Build();
//            var builder = new DbContextOptionsBuilder<WpContext>();
//            var connectionString = configuration.GetConnectionString("DefaultConnection");

//            //var connection = @"server=.\sqlexpress;user id=sa;pwd=aq;persist security info=False;initial catalog=WpCore;Trusted_Connection=True;MultipleActiveResultSets=true";
//            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Wp.Web.Api"));
//            return new WpContext(builder.Options);
//        }
//    }
//}
