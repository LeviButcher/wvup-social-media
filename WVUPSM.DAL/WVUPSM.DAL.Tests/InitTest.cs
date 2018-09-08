using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using Xunit;

namespace WVUPSM.DAL.Tests
{
    [Collection("FirstTest")]
    public class InitTest : IDisposable
    {
        private readonly SMContext _db;

        public InitTest()
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

        [Fact]
        public void FirstTest()
        {
            Assert.True(true);
        }
    }
}
