using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;

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
            context.Database.ExecuteSqlCommand("Delete FROM [SM].[Posts]");
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Follows]");
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Groups]");
            context.Database.ExecuteSqlCommand("DELETE FROM [dbo].[AspNetUsers]");
            context.Database.ExecuteSqlCommand("DELETE FROM [dbo].[AspNetRoles]");
        }
        
        /// <summary>
        ///     Seeds the database with constant records if the database doesn't already have records
        /// </summary>
        /// <param name="context">Database connection</param>
        private static void SeedData(SMContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Roles.Any())
            {
                context.Roles.AddRange(SampleData.GetRoles);
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(SampleData.GetUsers());
                context.SaveChanges();
            }
            if (!context.UserRoles.Any())
            {
                context.UserRoles.AddRange(SampleData.GetUserWithRole(context.UserAccounts.ToList(), context.Roles.ToList()));
                context.SaveChanges();
            }
            if(!context.Follows.Any())
            {
                context.Follows.AddRange(SampleData.GetFollowing(context.UserAccounts.ToList()));
                context.SaveChanges();
            }
            
            if(!context.Groups.Any())
            {
                context.Groups.AddRange(SampleData.GetGroups(context.UserAccounts.ToList()));
                List<Group> groups = context.Groups.ToList();
                foreach (Group allGroups in groups)
                {
                    context.UserGroups.Add( new UserGroup()
                    {
                        GroupId = allGroups.Id,
                        UserId = allGroups.OwnerId
                    });
                }
                context.SaveChanges();
            }

            foreach(UserGroup allGroups in SampleData.GetUserGroups(context.UserAccounts.ToList(), context.Groups.ToList()))
            {
                context.UserGroups.Add(new UserGroup()
                {
                    GroupId = allGroups.GroupId,
                    UserId = allGroups.UserId
                });
            }

            context.SaveChanges();           

            context.Posts.AddRange(SampleData.GetGroupPosts(context.UserAccounts.ToList(), context.Groups.ToList()));

            context.SaveChanges();
        }
    }
}
