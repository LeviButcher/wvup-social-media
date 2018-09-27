using System;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using WVUPSM.DAL.Repos;
using WVUPSM.Models.Entities;
using System.Linq;
using Xunit;

namespace WVUPSM.DAL.Tests.RepoTests
{
    /// <summary>
    ///     Collection of Tests for the Post repo
    /// </summary>
    [Collection("RepoTest")]
    public class PostRepoTest : IDisposable
    {
        private readonly SMContext _db;
        private PostRepo repo;

        public UserRepo UserRepo { get; }

        /// <summary>
        ///     Initializes the database and repos
        /// </summary>
        public PostRepoTest()
        {
            _db = new SMContext();
            DbInitializer.ClearData(_db);
            DbInitializer.InitializeData(_db);
            UserRepo = new UserRepo();
            repo = new PostRepo();
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
        ///     Tests creating a new post and making sure it exist
        /// </summary>
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

        /// <summary>
        ///     Test deletion of a posts from the database
        /// </summary>
        [Fact]
        public void DeletePostTest()
        {
            var count = repo.Table.Count();
            var post = repo.GetBasePost(1);
            var postDeleted = repo.DeletePost(post);

            Assert.True(repo.Table.Count() == count - 1);
        }

        /// <summary>
        ///     Gets the post from the user that a user is follow and makes sure it returns a amount less then or equal to the take
        /// </summary>
        [Fact]
        public void GetFollowingPostSkipTakeTest()
        {
            var user = UserRepo.GetUsers().First();
            int skip = 5;
            int take = 2;
            var posts = repo.GetFollowingPosts(user.UserId, skip, take);
            Assert.True(posts.Count() <= take);
        }

        [Fact]
        public void TimeSinceCreationTest()
        {
            //Checks to make sure ViewModel is working correctly
            var now = DateTime.Now;
            var post = repo.GetPost(1);
            post.DateCreated = new DateTime(2018, 9, 1);
            var diff = now - post.DateCreated;

            Assert.True(diff.Days.ToString() + " days ago" == post.TimeSinceCreation);
        }
    }
}
