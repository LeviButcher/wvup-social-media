﻿using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using WVUPSM.DAL.Repos;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;

namespace WVUPSM.DAL.Tests.RepoTests
{
    [Collection("RepoTest")]
    public class FollowRepoTest : IDisposable
    {
        private readonly SMContext _db;
        private FollowRepo repo;

        public UserRepo UserRepo { get; }

        public FollowRepoTest()
        {
            _db = new SMContext();
            DbInitializer.ClearData(_db);
            DbInitializer.InitializeData(_db);
            repo = new FollowRepo();
            UserRepo = new UserRepo();
        }

        public void Dispose()
        {
            DbInitializer.ClearData(_db);
            _db.Dispose();
        }

        [Fact]
        public void UnFollowTest()
        {
            var users = UserRepo.GetAllUsers();
            var user = users.First(x => x.FollowingCount > 0);
            var following = repo.GetFollowing(user.UserId).First();
            repo.DeleteFollower(new Follow() { UserId = user.UserId, FollowId = following.UserId });

            Assert.True(user.FollowingCount - 1 == repo.GetFollowingCount(user.UserId));
        }

        [Fact]
        public void UnFollowViewModelTest()
        {
            var users = UserRepo.GetAllUsers();
            var user = users.First(x => x.FollowingCount > 0);
            var following = repo.GetFollowing(user.UserId).First();
            repo.DeleteFollower(new Follow() { UserId = user.UserId, FollowId = following.UserId });
            var userAfterUnFollow = UserRepo.GetUser(user.UserId);

            Assert.True(userAfterUnFollow.FollowingCount == user.FollowingCount - 1);
        }
    }
}