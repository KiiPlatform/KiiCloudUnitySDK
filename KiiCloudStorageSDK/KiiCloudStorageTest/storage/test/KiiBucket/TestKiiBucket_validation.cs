using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiBucket_validation
    {
        private const string NUMBER_10 = "1234567890";

        [Test(), KiiUTInfo(
            action = "When IsValidBucketName with valid bucket name,",
            expected = "Method must return true"
            )]
        public void Test_0000_IsValidBucketName_OK ()
        {
            Assert.IsTrue(KiiBucket.IsValidBucketName("bucket"));
        }

        [Test(), KiiUTInfo(
            action = "When IsValidBucketName with null,",
            expected = "Method must return false"
            )]
        public void Test_0001_IsValidBucketName_null ()
        {
            Assert.IsFalse(KiiBucket.IsValidBucketName(null));
        }

        [Test(), KiiUTInfo(
            action = "When IsValidBucketName with name that length is 2,",
            expected = "Method must return true"
            )]
        public void Test_0002_IsValidBucketName_len2 ()
        {
            Assert.IsTrue(KiiBucket.IsValidBucketName("aa"));
        }

        [Test(), KiiUTInfo(
            action = "When IsValidBucketName with '_',",
            expected = "Method must return true"
            )]
        public void Test_0003_IsValidBucketName_len2_under ()
        {
            Assert.IsTrue(KiiBucket.IsValidBucketName("__"));
        }

        [Test(), KiiUTInfo(
            action = "When IsValidBucketName with name that length is 64,",
            expected = "Method must return true"
            )]
        public void Test_0004_IsValidBucketName_len50 ()
        {
            Assert.IsTrue(KiiBucket.IsValidBucketName(NUMBER_10 + NUMBER_10 + NUMBER_10 + NUMBER_10 + NUMBER_10 + NUMBER_10 + "1234"));
        }

        [Test(), KiiUTInfo(
            action = "When IsValidBucketName with name that length is 65,",
            expected = "Method must return false"
            )]
        public void Test_0005_IsValidBucketName_len51 ()
        {
            Assert.IsFalse(KiiBucket.IsValidBucketName(NUMBER_10 + NUMBER_10 + NUMBER_10 + NUMBER_10 + NUMBER_10 + NUMBER_10 + "12345"));
        }

        [Test(), KiiUTInfo(
            action = "When IsValidBucketName with name that length is 1,",
            expected = "Method must return false"
            )]
        public void Test_0006_IsValidBucketName_len1 ()
        {
            Assert.IsFalse(KiiBucket.IsValidBucketName("a"));
        }
        
        [Test(), KiiUTInfo(
            action = "When IsValidBucketName with '_',",
            expected = "Method must return false"
            )]
        public void Test_0007_IsValidBucketName_len1_under ()
        {
            Assert.IsFalse(KiiBucket.IsValidBucketName("_"));
        }
    }
}

