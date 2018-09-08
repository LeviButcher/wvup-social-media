using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WVUPSM.DAL.Initiliazers
{
    public class DbInitializer
    {
        public static void InitializeData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<SMContext>();
            InitializeData(context);
        }

        public static void InitializeData(SMContext context)
        {
            context.Database.Migrate();
            ClearData(context);
            SeedData(context);
        }

        public static void ClearData(SMContext context)
        {
            context.Posts.FromSql("TRUNCATE TABLE [SM].[POSTS]");
            context.Follows.FromSql("TRUNCATE TABLE [SM].[FOLLOWS]");
            context.Users.RemoveRange(context.Users);
        }
        

        private static void SeedData(SMContext context)
        {
            context.SaveChanges();
        }
    }
}
