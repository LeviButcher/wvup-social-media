using System;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using WVUPSM.DAL.Repos;
using WVUPSM.Models.Entities;
using System.Linq;
using Xunit;

namespace WVUPSM.DAL.Tests.RepoTests
{
    [Collection("RepoTest")]
    public class PostRepoTest : IDisposable
    {
        private readonly SMContext _db;
        private PostRepo repo;

        public UserRepo UserRepo { get; }

        public PostRepoTest()
        {
            _db = new SMContext();
            DbInitializer.ClearData(_db);
            DbInitializer.InitializeData(_db);
            UserRepo = new UserRepo();
            repo = new PostRepo();
        }

        public void Dispose()
        {
            DbInitializer.ClearData(_db);
            _db.Dispose();
        }

        [Fact]
        public void CreatePostTest()
        {
            var count = repo.Table.Count();
            var userId = UserRepo.Table.First().Id;
            var user = UserRepo.GetUser(userId);
            Post post = new Post()
            {
                Text = "Hi",
                UserId = userId
            };
            var postCreated = repo.CreatePost(post);

            Assert.True(repo.Table.Count() == count + 1);
        }

        [Fact]
        public void DeletePostTest()
        {
            var count = repo.Table.Count();
            var post = repo.GetBasePost(1);
            var postDeleted = repo.DeletePost(post);

            Assert.True(repo.Table.Count() == count - 1);
        }

        [Fact]
        public void GetFollowersPostSkipTakeTest()
        {
            var user = UserRepo.GetUsers().First();
            int skip = 5;
            int take = 2;
            var posts = repo.GetFollowingPosts(user.UserId, skip, take);
            Assert.True(posts.Count() <= take);
        }
    }
}
