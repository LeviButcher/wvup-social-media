using System;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using Xunit;

namespace WVUPSM.DAL.Tests.RepoTests
{
    [Collection("RepoTest")]
    public class PostRepoTest : IDisposable
    {
        private readonly SMContext _db;

        public PostRepoTest()
        {
            _db = new SMContext();
            DbInitializer.ClearData(_db);
            DbInitializer.InitializeData(_db);
        }

        public void Dispose()
        {
            DbInitializer.ClearData(_db);
            _db.Dispose();
        }
    }
}
