using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using WVUPSM.DAL.Repos;
using WVUPSM.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using WVUPSM.Models.ViewModels;
using System.Linq;

namespace WVUPSM.DAL.Tests.RepoTests
{
    /// <summary>
    ///     Test Collection for the User Repo
    /// </summary>
    [Collection("RepoTest")]
    public class UserRepoTest : IDisposable
    {
        private readonly SMContext _db;
        private UserRepo repo;

        /// <summary>
        ///     Initiliases the database
        /// </summary>
        public UserRepoTest()
        {
            _db = new SMContext();
            DbInitializer.ClearData(_db);
            DbInitializer.InitializeData(_db);
            repo = new UserRepo();
        }

        /// <summary>
        ///     Clears the database
        /// </summary>
        public void Dispose()
        {
            DbInitializer.ClearData(_db);
            _db.Dispose();
        }

        /// <summary>
        ///     Repo setup test
        /// </summary>
        [Fact]
        public void RepoTest()
        {
            Assert.True(repo != null);
        }

        /// <summary>
        ///     Tests creating a user
        /// </summary>
        [Fact]
        public void CreateUserTest()
        {
            Assert.True(true);
        }

        /// <summary>
        /// Tests finding a user
        /// </summary>
        [Fact]
        public void FindUserTest()
        {
            var word = "s";
            var count = repo.Table.Count(x => x.Email.Contains(word) || x.UserName.Contains(word));
            List<UserProfile> users = repo.FindUsers("s").ToList();
            Assert.True(count == users.Count);
        }

        /// <summary>
        ///     Tests getting all the user from the database
        /// </summary>
        [Fact]
        public void GetAllUsersTest()
        {
            var userCount = repo.Table.Count();
            var users = repo.GetAllUsers();
            Assert.True(userCount == users.Count());
        }

        /// <summary>
        ///     Tests getting a single user in the database
        /// </summary>
        [Fact]
        public void GetUserTest()
        {
            var userId = repo.Table.First().Id;
            var user = repo.GetUser(userId);
            Assert.True(userId == user.UserId);
        }
    }
}
