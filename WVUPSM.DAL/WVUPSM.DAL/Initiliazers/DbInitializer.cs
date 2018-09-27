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
            context.Database.ExecuteSqlCommand("DELETE FROM [SM].[Groups]");
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
