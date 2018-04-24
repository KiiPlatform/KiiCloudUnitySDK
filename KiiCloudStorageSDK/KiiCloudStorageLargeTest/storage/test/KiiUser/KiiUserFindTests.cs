using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class KiiUserFindTests : LargeTestBase
    {
        [SetUp ()]
        public override void SetUp ()
        {
            base.SetUp ();
        }

        [TearDown ()]
        public override void TearDown ()
        {
            AppUtil.DeleteUser (KiiUser.CurrentUser);
            base.TearDown ();
        }

        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        [Test ()]
        public void Test_FindUserByEmail_Synchronous_ForEscapingAtmark ()
        {
            // Create KiiUser to find
            string emailDomain = "@testkii.com";
            string emailLocalPart = "kiiuserfindmail." + CurrentTimeMillis ();
            string emailAddress = emailLocalPart + emailDomain;
            KiiUser findUser = KiiUser.BuilderWithEmail (emailAddress).Build ();
            findUser.Register ("123456");
            string findUserUriStr = findUser.Uri.ToString ();
            KiiUser.LogOut ();

            // Create new user
            string uname = "kiiuserfindtest-" + CurrentTimeMillis ();
            AppUtil.CreateNewUser (uname, "123456");

            KiiUser searchUser = KiiUser.FindUserByEmail (emailAddress);
            Assert.IsNotNull (searchUser);
            Assert.AreEqual (emailAddress, searchUser.Email);
            Assert.AreEqual (findUserUriStr, searchUser.Uri.ToString ());
        }

        [Test ()]
        public void Test_FindUserByEmail_Asynchronous_ForEscapingAtmark ()
        {
            // Create KiiUser to find
            string emailDomain = "@testkii.com";
            string emailLocalPart = "kiiuserfindmail." + CurrentTimeMillis ();
            string emailAddress = emailLocalPart + emailDomain;
            KiiUser findUser = KiiUser.BuilderWithEmail (emailAddress).Build ();
            findUser.Register ("123456");
            string findUserUriStr = findUser.Uri.ToString ();
            KiiUser.LogOut ();

            // Create new user
            string uname = "kiiuserfindtest-" + CurrentTimeMillis ();
            AppUtil.CreateNewUser (uname, "123456");

            CountDownLatch cd = new CountDownLatch (1);
            KiiUser outUser = null;
            Exception outExp = null;
            KiiUser.FindUserByEmail (emailAddress, (KiiUser retUser, Exception retExp) => {
                outUser = retUser;
                outExp = retExp;
                cd.Signal ();
            });

            if (!cd.Wait (new TimeSpan (0, 0, 0, 3)))
            {
                Assert.Fail ("Callback not fired.");
            }

            Assert.IsNull (outExp);
            Assert.IsNotNull (outUser);
            Assert.AreEqual (emailAddress, outUser.Email);
            Assert.AreEqual (findUserUriStr, outUser.Uri.ToString ());
        }
    }
}

