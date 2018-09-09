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

namespace WVUPSM.DAL.Tests.RepoTests
{
    [Collection("RepoTest")]
    public class UserRepoTest : IDisposable
    {
        private readonly SMContext _db;
        private UserRepo repo;

        public UserRepoTest()
        {
            _db = new SMContext();
            DbInitializer.ClearData(_db);
            DbInitializer.InitializeData(_db);
            repo = new UserRepo();
        }

        public void Dispose()
        {
            DbInitializer.ClearData(_db);
            _db.Dispose();
        }

        [Fact]
        public void RepoTest()
        {
            Assert.True(repo != null);
        }

        [Fact]
        public void CreateUserTest()
        {
            Assert.True(true);
        }
    }
}
