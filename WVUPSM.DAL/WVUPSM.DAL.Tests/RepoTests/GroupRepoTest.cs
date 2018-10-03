using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using System.Linq;
using WVUPSM.DAL.Initiliazers;
using WVUPSM.DAL.Repos;
using Xunit;
using WVUPSM.Models.Entities;
using WVUPSM.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace WVUPSM.DAL.Tests.RepoTests
{
    [Collection("RepoTest")]
    public class GroupRepoTest : IDisposable
    {
        private readonly SMContext _db;
        private GroupRepo repo;

        public UserRepo UserRepo { get; }

        public GroupRepoTest()
        {
            _db = new SMContext();
            DbInitializer.ClearData(_db);
            DbInitializer.InitializeData(_db);
            UserRepo = new UserRepo();
            repo = new GroupRepo();
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
        public void GetGroupTest()
        {
            var group = repo.Table.First();
            var testGroup = repo.GetGroup(group.Id);
            Assert.True(group.Id == testGroup.GroupId);
        }

        [Fact]
        public void CreateGroupTest()
        {
            var users = UserRepo.GetAllUsers();
            var user = users.FirstOrDefault();
            Group group = new Group()
            {
                OwnerId = user.UserId,
                Name = "NewGroup"
            };
            var count = repo.Table.Count();
            var groupCreated = repo.CreateGroup(group);

            Assert.True(repo.Table.Count() == count + 1);
        }

        [Fact]
        public void FindGroupTest()
        {
            var word = "STEM";
            var count = repo.Table.Count(x => x.Name.Contains(word));
            List<GroupViewModel> groups = repo.FindGroups("STEM").ToList();
            Assert.True(count == groups.Count);

        }

        [Fact]
        public async void IsOwnerTest()
        {
            var group = repo.Table.FirstOrDefault();
            var owner = repo.GetOwner(group.Id);
            var result = await repo.IsOwner(owner.UserId, group.Id);

            Assert.True(result);
        }

        [Fact]
        public async void GetMemberCountTest()
        {
            var group = repo.Table.Include(x => x.Members).First();
            var user = UserRepo.GetUsers(0, 1).First();
            var beforeCount = group.Members.Count();
            await repo.JoinGroup(user.UserId, group.Id);


            var afterCount = repo.GetGroup(group.Id).MemberCount;
            Assert.True(beforeCount + 1 == afterCount);
        }      

        [Fact]
        public async void JoinGroupTest()
        {
            var users = UserRepo.GetAllUsers();
            var user = users.FirstOrDefault();
            var group = repo.Table.FirstOrDefault();
            var memberCount = repo.GetMemberCount(group.Id);
            var isMember = await repo.IsMember(user.UserId, group.Id);
           
            await repo.JoinGroup(user.UserId, group.Id);
            var newMemberCount = repo.GetMemberCount(group.Id);

            Assert.True(newMemberCount == memberCount + 1);

        }

        [Fact]
        public void GetOwnerTest()
        {
            var group = repo.Table.FirstOrDefault();
            var ownerId = group.OwnerId;
            var getOwner = repo.GetOwner(group.Id);
            Assert.True(ownerId == getOwner.UserId);
        }
    }
}