using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class IdentityDataTest
    {
        #region Normal Cases
        [Test()]
        public void CreateWithNameTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            IdentityData data = builder.Build();
            Assert.AreEqual("test", data.UserName);
            Assert.IsNull(data.Email);
            Assert.IsNull(data.Phone);
        }
        [Test()]
        public void CreateWithEmailTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail("test@example.com");
            IdentityData data = builder.Build();
            Assert.AreEqual("test@example.com", data.Email);
            Assert.IsNull(data.UserName);
            Assert.IsNull(data.Phone);
        }
        [Test()]
        public void CreateWithGlobalPhoneTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone("+819011112222");
            IdentityData data = builder.Build();
            Assert.AreEqual("+819011112222", data.Phone);
            Assert.IsNull(data.UserName);
            Assert.IsNull(data.Email);
        }
        [Test()]
        public void CreateWithLocalPhoneTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone("09011112222");
            IdentityData data = builder.Build();
            Assert.AreEqual("09011112222", data.Phone);
            Assert.IsNull(data.UserName);
            Assert.IsNull(data.Email);
        }
        [Test()]
        public void WithNameOverwriteTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithName("hoge");
            IdentityData data = builder.Build();
            Assert.AreEqual("hoge", data.UserName);
            Assert.IsNull(data.Email);
            Assert.IsNull(data.Phone);
        }
        [Test()]
        public void WithEmailOverwriteTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail("test@example.com");
            builder.WithEmail("admin@kii.com");
            IdentityData data = builder.Build();
            Assert.AreEqual("admin@kii.com", data.Email);
            Assert.IsNull(data.UserName);
            Assert.IsNull(data.Phone);
        }
        [Test()]
        public void WithGlobalPhoneOverwriteTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone("+819011112222");
            builder.WithPhone("+819033334444");
            IdentityData data = builder.Build();
            Assert.AreEqual("+819033334444", data.Phone);
            Assert.IsNull(data.UserName);
            Assert.IsNull(data.Email);
        }
        [Test()]
        public void WithLocalPhoneOverwriteTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone("09011112222");
            builder.WithPhone("09033334444");
            IdentityData data = builder.Build();
            Assert.AreEqual("09033334444", data.Phone);
            Assert.IsNull(data.UserName);
            Assert.IsNull(data.Email);
        }
        [Test()]
        public void AllFieldsTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithEmail("test@example.com");
            builder.WithPhone("+819011112222");
            IdentityData data = builder.Build();
            Assert.AreEqual("test", data.UserName);
            Assert.AreEqual("test@example.com", data.Email);
            Assert.AreEqual("+819011112222", data.Phone);
        }
        [Test()]
        public void WithNameTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail("test@example.com");
            builder.WithName("test");
            IdentityData data = builder.Build();
            Assert.AreEqual("test", data.UserName);
            Assert.AreEqual("test@example.com", data.Email);
            Assert.IsNull(data.Phone);
        }
        [Test()]
        public void WithEmailTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithEmail("test@example.com");
            IdentityData data = builder.Build();
            Assert.AreEqual("test@example.com", data.Email);
            Assert.AreEqual("test", data.UserName);
            Assert.IsNull(data.Phone);
        }
        [Test()]
        public void WithGlobalPhoneTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithPhone("+819011112222");
            IdentityData data = builder.Build();
            Assert.AreEqual("+819011112222", data.Phone);
            Assert.AreEqual("test", data.UserName);
            Assert.IsNull(data.Email);
        }
        [Test()]
        public void WithLocalPhoneTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithPhone("09011112222");
            IdentityData data = builder.Build();
            Assert.AreEqual("09011112222", data.Phone);
            Assert.AreEqual("test", data.UserName);
            Assert.IsNull(data.Email);
        }
        #endregion

        #region Error Cases
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNameWithNullTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName(null);
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithEmailWithNullTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail(null);
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithPhoneWithNullTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone(null);
            builder.Build();
        }

        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithNameWithEmptyTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("");
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithEmailWithEmptyTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail("");
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void CreateWithPhoneWithEmptyTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone("");
            builder.Build();
        }

        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void WithNameWithNullTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithName(null);
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void WithEmailWithNullTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithEmail(null);
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void WithPhoneWithNullTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithPhone(null);
            builder.Build();
        }

        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void WithNameWithEmptyTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithName("");
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void WithEmailWithEmptyTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithEmail("");
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentNullException))]
        public void WithPhoneWithEmptyTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithPhone("");
            builder.Build();
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void CreateWithNameWithInvalidValueTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("@hoge@");
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void CreateWithEmailWithInvalidValueTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithEmail("hogehoge");
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void CreateWithPhoneWithInvalidValueTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithPhone("abcd");
            builder.Build();
        }

        [Test(), ExpectedException(typeof(ArgumentException))]
        public void WithNameWithInvalidValueTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithName("@hoge@");
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void WithEmailWithInvalidValueTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithEmail("hogehoge");
            builder.Build();
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void WithPhoneWithInvalidValueTest()
        {
            IdentityData.Builder builder = IdentityData.Builder.CreateWithName("test");
            builder.WithPhone("abcd");
            builder.Build();
        }
        #endregion
    }
}

