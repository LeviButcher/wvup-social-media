using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WVUPSM.Models.Entities;

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
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [SM].[Posts]");
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Follows]");
            context.Database.ExecuteSqlCommand("DELETE FROM [dbo].[AspNetUsers]");
        }
        

        private static void SeedData(SMContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.AddRange(SampleData.GetUsers());
                context.SaveChanges();
            }
            if(!context.Follows.Any())
            {
                context.Follows.AddRange(SampleData.GetFollowing(context.UserAccounts.ToList()));
                context.SaveChanges();
            }

            context.SaveChanges();
        }
    }
}
