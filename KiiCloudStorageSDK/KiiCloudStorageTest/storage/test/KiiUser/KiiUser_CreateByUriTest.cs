using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_CreateByUriTest
    {

        #region KiiUser.CreateByUri(Uri)
        [Test(), KiiUTInfo(
            action = "When we call CreateByUri() with valid uri,",
            expected = "We can get id correctly"
            )]
        public void Test_0000_CreateByUri_ok()
        {
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd"));
            Assert.AreEqual("abcd", user.ID);
        }

        [Test(), KiiUTInfo(
            action = "When we call CreateByUri() with uri that ends with '/',",
            expected = "We can get id correctly"
            )]
        public void Test_0001_CreateByUri_endWith_slash()
        {
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd/"));
            Assert.AreEqual("abcd", user.ID);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with null,",
            expected = "ArgumentException must be throws"
            )]
        public void Test_0010_CreateByUri_null ()
        {
            KiiUser.CreateByUri(null);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with wrong scheme,",
            expected = "ArgumentException must be throws"
            )]
        public void Test_0011_CreateByUri_wrong_scheme()
        {
            KiiUser.CreateByUri(new Uri("http://users/abcd"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with uri whose authority is not 'users',",
            expected = "ArgumentException must be throws"
            )]
        public void Test_0012_CreateByUri_not_users ()
        {
            KiiUser.CreateByUri(new Uri("kiicloud://user/abcd"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with uri whose segment length is 0,",
            expected = "ArgumentException must be throws"
            )]
        public void Test_0013_CreateByUri_seg0 ()
        {
            KiiUser.CreateByUri(new Uri("kiicloud://users"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with uri whose segment length is 1,",
            expected = "ArgumentException must be throws"
            )]
        public void Test_0014_CreateByUri_seg1 ()
        {
            KiiUser.CreateByUri(new Uri("kiicloud://users/"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with uri whose segment length is 3,",
            expected = "ArgumentException must be throws"
            )]
        public void Test_0015_CreateByUri_seg3 ()
        {
            KiiUser.CreateByUri(new Uri("kiicloud://users/abcd/buckets"));
        }
        #endregion
    }
}

