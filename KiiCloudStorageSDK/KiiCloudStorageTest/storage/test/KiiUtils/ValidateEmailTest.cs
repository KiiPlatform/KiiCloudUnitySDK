using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class ValidateEmailTest
    {
        [Test()]
        public void Test()
        {
            Assert.IsTrue(Utils.ValidateEmail("test@gmail.com"));
            Assert.IsTrue(Utils.ValidateEmail("_test_@gmail.com"));
            Assert.IsTrue(Utils.ValidateEmail("____@gmail.com"));
            Assert.IsTrue(Utils.ValidateEmail("_@gmail.com"));
            Assert.IsTrue(Utils.ValidateEmail("_abc-ABC.123@kii.com"));
            Assert.IsTrue(Utils.ValidateEmail("_abc-ABC.123@kii.c-om"));
            Assert.IsTrue(Utils.ValidateEmail("test@1234.567"));

            // username is over 256 characters.
            Assert.IsFalse(Utils.ValidateEmail(new String('a', 257) + "@gmail.com"));
            // hostname is over 66 characters.
            Assert.IsFalse(Utils.ValidateEmail("test@" + new String('a', 66) + ".com"));
            // top level domain is over 27 characters.
            Assert.IsFalse(Utils.ValidateEmail("test@gmail." + new String('a', 27)));
            // Missing @
            Assert.IsFalse(Utils.ValidateEmail("test.com"));
            // hostname has _
            Assert.IsFalse(Utils.ValidateEmail("test@_kii.com"));
            // hostname has _
            Assert.IsFalse(Utils.ValidateEmail("test@kii_.com"));
            // hostname starts with -
            Assert.IsFalse(Utils.ValidateEmail("test@-kii.com"));
            // top level domain starts with -
            Assert.IsFalse(Utils.ValidateEmail("_abc-ABC.123@kii.-com"));
            // duplicate @
            Assert.IsFalse(Utils.ValidateEmail("test@kii@com"));
            // Missing top level dmain
            Assert.IsFalse(Utils.ValidateEmail("test@12345"));
        }
    }
}

