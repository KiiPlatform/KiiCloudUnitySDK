using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class EUTest : ListAclEntriesTest
    {
        [SetUp]
        public override void SetUp ()
        {
            EUApp app = new EUApp (); 
            Kii.Initialize (app.AppId, app.AppKey, app.BaseUrl);
            Kii.Logger = new TestLogger ();
            string uname = "Test-" + CurrentTimeMillis ();
            AppUtil.CreateNewUser (uname, "password");
        }
    }
}

