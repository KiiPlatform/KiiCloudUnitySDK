using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    public class LargeTestBase
    {
        class TestLogger : IKiiLogger
        {
            public void Debug (string message, params object[] args)
            {
                System.Console.WriteLine (message, args);
            }
        }

        protected BaseApp app;

        [SetUp]
        public virtual void SetUp ()
        {
            this.app = new TestApp (); 
            Kii.Initialize (app.AppId, app.AppKey, app.BaseUrl);
            Kii.Logger = new TestLogger ();
        }

        [TearDown]
        public virtual void TearDown ()
        {
            if (KiiUser.CurrentUser != null) {
                try {
                    KiiUser.CurrentUser.Delete ();
                } catch (Exception) {
                    // Nothing to do
                }
            }
            KiiUser.LogOut ();
        }
    }
}

