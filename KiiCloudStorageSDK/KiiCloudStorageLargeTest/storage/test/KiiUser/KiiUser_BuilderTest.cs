using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class KiiUser_BuilderTest : LargeTestBase
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

        [Test()]
        public void Test_BuilderWithIdentifier_Username()
        {
            string identifier = "kii1234";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.Build().Register("123456");

            KiiUser user = KiiUser.CurrentUser;
            Assert.AreEqual(identifier, user.Username);
            Assert.IsNull(user.Email);
            Assert.IsNull(user.Phone);
            Assert.IsNull(user.Country);
        }

        [Test()]
        public void Test_BuilderWithIdentifier_Phone()
        {
            string identifier = "+8719011112222";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.Build().Register("123456");

            KiiUser user = KiiUser.CurrentUser;
            Assert.IsNull(user.Username);
            Assert.IsNull(user.Email);
            Assert.AreEqual(identifier, user.Phone);
            Assert.IsNull(user.Country);
        }

        [Test()]
        public void Test_BuilderWithIdentifier_Email()
        {
            string identifier = "test@kii.com";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.Build().Register("123456");

            KiiUser user = KiiUser.CurrentUser;
            Assert.IsNull(user.Username);
            Assert.AreEqual(identifier, user.Email);
            Assert.IsNull(user.Phone);
            Assert.IsNull(user.Country);
        }

        [Test()]
        public void Test_SetGlobalPhone()
        {
            string identifier = "kii1234";
            string phone = "+8719011112222";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);
            builder.SetGlobalPhone(phone);

            builder.Build().Register("123456");

            KiiUser user = KiiUser.CurrentUser;
            Assert.AreEqual(identifier, user.Username);
            Assert.IsNull(user.Email);
            Assert.AreEqual(phone, user.Phone);
            Assert.IsNull(user.Country);
        }

        [Test()]
        public void Test_SetEmail()
        {
            string identifier = "+8719011112222";
            string email = "test@kii.com";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.SetEmail(email);

            builder.Build().Register("123456");

            KiiUser user = KiiUser.CurrentUser;
            Assert.IsNull(user.Username);
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(identifier, user.Phone);
            Assert.IsNull(user.Country);
        }
        
        [Test()]
        public void Test_SetName()
        {
            string identifier = "test@kii.com";
            string username = "kii1234";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.SetName(username);

            builder.Build().Register("123456");

            KiiUser user = KiiUser.CurrentUser;
            Assert.AreEqual(username, user.Username);
            Assert.AreEqual(identifier, user.Email);
            Assert.IsNull(user.Phone);
            Assert.IsNull(user.Country);
        }
    }
}

