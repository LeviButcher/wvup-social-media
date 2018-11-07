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
        private SMContext _smContext;

        public DbInitializer(SMContext smContext)
        {
            _smContext = smContext;
        }
        /// <summary>
        ///     Clears and Seeds database
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void InitializeData(IServiceProvider serviceProvider, SMContext smContext)
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
        ///     Dependcy Injection Initializer for production
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void ProductionInitializeData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<SMContext>();
            ProductionInitializeData(context);
        }

        /// <summary>
        ///     Initializes database for production
        /// </summary>
        /// <param name="context"></param>
        public static void ProductionInitializeData(SMContext context)
        {
            ProdSeedData(context);
        }

        /// <summary>
        ///     Clears the database of all records and resets incremental keys
        /// </summary>
        /// <param name="context"></param>
        public static void ClearData(SMContext context)
        {
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Posts]");
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Groups]");
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Messages]");
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Comments]");
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Follows]");
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

            if (!context.Groups.Any())
            {
                context.Groups.AddRange(SampleData.GetGroups(context.UserAccounts.ToList()));
                List<Group> groups = context.Groups.ToList();
                foreach (Group allGroups in groups)
                {
                    context.UserGroups.Add(new UserGroup()
                    {
                        GroupId = allGroups.Id,
                        UserId = allGroups.OwnerId
                    });
                }
                context.SaveChanges();
            }

            foreach (UserGroup allGroups in SampleData.GetUserGroups(context.UserAccounts.ToList(), context.Groups.ToList()))
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

            if(!context.Messages.Any())
            {
                List<User> userList = context.UserAccounts.ToList();

                context.Messages.AddRange(SampleData.CreateConversations(userList.Where(x => x.Email == "leviB@develop.com").First(), userList.Where(x => x.Email == "seanR@develop.com").First()));
                context.Messages.AddRange(SampleData.CreateConversations(userList.ElementAt(2), userList.ElementAt(5)));
                context.Messages.AddRange(SampleData.CreateConversations(userList.ElementAt(3), userList.ElementAt(1)));
                context.Messages.AddRange(SampleData.CreateConversations(userList.ElementAt(6), userList.ElementAt(7)));
                context.Messages.AddRange(SampleData.CreateConversations(userList.ElementAt(6), userList.ElementAt(2)));

            }

            context.SaveChanges();

            if(!context.Comments.Any())
            {
                List<Post> postList = context.Posts.ToList();
                List<User> userList = context.UserAccounts.ToList();

                context.Comments.AddRange(SampleData.CreateComments(postList, userList));
            }

            context.SaveChanges();
        }

        /// <summary>
        ///     Production Seed data
        /// </summary>
        /// <param name="context">Connection to the DB</param>
        private static void ProdSeedData(SMContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(SampleData.GetRoles);
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(SampleData.GetProdUsers());
                context.SaveChanges();
            }
        }
    }
}
