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
    /// <summary>
    ///     Static Database Initiliazer with behaviors for clearing and seeding the database
    /// </summary>
    public class DbInitializer
    {
        /// <summary>
        ///     Clears and Seeds database 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitializeData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<SMContext>();
            InitializeData(context);
        }

        /// <summary>
        ///     Clears and seeds the database
        /// </summary>
        /// <param name="context"></param>
        public static void InitializeData(SMContext context)
        {
            context.Database.Migrate();
            ClearData(context);
            SeedData(context);
        }

        /// <summary>
        ///     Clears the database of all records and resets incremental keys 
        /// </summary>
        /// <param name="context"></param>
        public static void ClearData(SMContext context)
        {
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [SM].[Posts]");
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Follows]");
            context.Database.ExecuteSqlCommand("DELETE FROM [dbo].[AspNetUsers]");
        }
        
        /// <summary>
        ///     Seeds the database with constant records if the database doesn't already have records
        /// </summary>
        /// <param name="context">Database connection</param>
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
