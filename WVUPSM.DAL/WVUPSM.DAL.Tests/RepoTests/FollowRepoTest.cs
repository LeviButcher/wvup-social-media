using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using WVUPSM.DAL.Repos;
using Xunit;
using System.Linq;
using System.Threading.Tasks;

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
    }
}