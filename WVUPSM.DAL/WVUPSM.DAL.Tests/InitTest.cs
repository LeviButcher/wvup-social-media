using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;
using Xunit;

namespace WVUPSM.DAL.Tests
{
    /// <summary>
    ///     Quick Test class to make sure everything compiles with no errors
    /// </summary>
    [Collection("RepoTest")]
    public class InitTest : IDisposable
    {
        private readonly SMContext _db;

        /// <summary>
        ///     Seeds Database
        /// </summary>
        public InitTest()
        {
            _db = new SMContext();
            DbInitializer.ClearData(_db);
            DbInitializer.InitializeData(_db);
        }

        /// <summary>
        ///     Clears out Database
        /// </summary>
        public void Dispose()
        {
            DbInitializer.ClearData(_db);
            _db.Dispose();
        }

        /// <summary>
        ///     If this test fails then everything is wrong
        /// </summary>
        [Fact]
        public void FirstTest()
        {
            Assert.True(true);
        }
    }
}
