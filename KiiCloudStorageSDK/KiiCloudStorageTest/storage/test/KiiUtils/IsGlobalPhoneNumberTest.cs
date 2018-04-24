using System;
using JsonOrg;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class IsGlobalPhoneNumberTest
    {

        #region Normal Cases
        [Test()]
        public void ValidFormatPhoneNumberTest()
        {
            Assert.IsTrue(Utils.IsGlobalPhoneNumber("+818012345678"));
        }
        #endregion

        #region Error Cases
        [Test()]
        public void NullPhoneNumberTest()
        {
            Assert.IsFalse(Utils.IsGlobalPhoneNumber(null));
        }

        [Test()]
        public void EmptyPhoneNumberTest()
        {
            Assert.IsFalse(Utils.IsGlobalPhoneNumber(""));
        }

        [Test()]
        public void PhoneNumberWithoutPlusPrefixTest()
        {
            Assert.IsFalse(Utils.IsGlobalPhoneNumber("818012345678"));
        }
        #endregion
    }
}

