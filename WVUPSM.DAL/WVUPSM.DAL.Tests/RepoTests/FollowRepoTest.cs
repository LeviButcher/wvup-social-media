using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using WVUPSM.DAL.Repos;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using WVUPSM.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace WVUPSM.DAL.Tests.RepoTests
{
    /// <summary>
    ///     Test collection for operations on the FollowRepo
    /// </summary>
    [Collection("RepoTest")]
    public class FollowRepoTest : IDisposable
    {
        private readonly SMContext _db;
        private FollowRepo repo;

        public UserRepo UserRepo { get; }

        /// <summary>
        ///     Initialize FollowRepo, seed Database
        /// </summary>
        public FollowRepoTest()
        {
            _db = new SMContext();
            DbInitializer.ClearData(_db);
            DbInitializer.InitializeData(_db);
            UserRepo = new UserRepo();
            repo = new FollowRepo();
        }

        /// <summary>
        ///     Wipes Database
        /// </summary>
        public void Dispose()
        {
            DbInitializer.ClearData(_db);
            _db.Dispose();
        }

        /// <summary>
        ///     Tests a User Unfollowing another user
        ///     Users following count should be n - 1 after operation
        /// </summary>
        [Fact]
        public void UnFollowTest()
        {
            var users = UserRepo.GetAllUsers();
            var user = users.First(x => x.FollowingCount > 0);
            var following = repo.GetFollowing(user.UserId).First();
            repo.DeleteFollower(new Follow() { UserId = user.UserId, FollowId = following.UserId });

            Assert.True(user.FollowingCount - 1 == repo.GetFollowingCount(user.UserId));
        }

        /// <summary>
        ///     Tests a User Unfollowing a user and seeing if their UserProfileViewModel updates 
        ///     to reflect the correct following count
        /// </summary>
        [Fact]
        public void UnFollowViewModelTest()
        {
            var users = UserRepo.GetAllUsers();
            var user = users.First(x => x.FollowingCount > 0);
            var following = repo.GetFollowing(user.UserId).First();
            repo.DeleteFollower(new Follow() { UserId = user.UserId, FollowId = following.UserId });
            var userAfterUnFollow = UserRepo.GetUser(user.UserId);

            Assert.True(repo.GetFollowingCount(user.UserId) == user.FollowingCount - 1);
        }

        /// <summary>
        ///     Tests a User Unfollowing a user and making sure the follow count changes
        /// </summary>
        [Fact]
        public void UnFollowChangeFollowersTest()
        {
            var users = UserRepo.GetAllUsers();
            var user = users.First(x => x.FollowingCount > 0);
            var following = repo.GetFollowing(user.UserId).First();

            repo.DeleteFollower(new Follow() { UserId = user.UserId, FollowId = following.UserId });
            int newFollowerCount = repo.GetFollowerCount(following.UserId);
            Assert.True(following.FollowerCount - 1 == newFollowerCount);
        }

        /// <summary>
        ///     Tests to make sure the NavigationProperties on the User for Following works
        /// </summary>
        [Fact]
        public void NavigationPropFollowingTest()
        {
            var user = UserRepo.Table.Include(x => x.Following).First(x => x.UserName == "samB");
            Assert.True(repo.GetFollowingCount(user.Id) == user.Following.Count);
        }

        /// <summary>
        ///     Tests to make sure the NavigationProperties on the User for Follower
        /// </summary>
        [Fact]
        public void NavigationPropFollowerTest()
        {
            var user = UserRepo.Table.Include(x => x.Followers).First(x => x.UserName == "samB");
            Assert.True(repo.GetFollowerCount(user.Id) == user.Followers.Count);
        }

        /// <summary>
        ///     Gets a user's following list and make sure that the amount of records returns matches their following count
        /// </summary>
        [Fact]
        public void GetFollowingTest()
        {
            var user = UserRepo.Table.Include(x => x.Followers).First(x => x.UserName == "leviB");
            var following = repo.GetFollowing(user.Id);
            int followCount = repo.Table.Count(x => x.UserId == user.Id);

            Assert.True(following.Count() == followCount);
        }
    }
}