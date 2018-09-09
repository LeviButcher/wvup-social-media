using System;
using System.Collections.Generic;
using System.Text;

namespace WVUPSM.Service.Tests.APITest.Base
{
    public abstract class BaseTestClass : IDisposable
    {
        protected string ServiceAddress = "http://localhost:51117/";
        protected string RootAddress = String.Empty;

        public void Dispose()
        {
           
        }
    }
}
