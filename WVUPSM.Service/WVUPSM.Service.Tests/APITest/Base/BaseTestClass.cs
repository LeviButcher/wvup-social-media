using System;
using System.Collections.Generic;
using System.Text;
using WVUPSM.DAL.EF;
using WVUPSM.DAL.Initiliazers;

namespace WVUPSM.Service.Tests.APITest.Base
{
    public abstract class BaseTestClass : IDisposable
    {
        protected string ServiceAddress = "http://localhost:51117/";
        protected string RootAddress = String.Empty;
        SMContext context = new SMContext();

        public void Dispose()
        {

        }
    }
}
