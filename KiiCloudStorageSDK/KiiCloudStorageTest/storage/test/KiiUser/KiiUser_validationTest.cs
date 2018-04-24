using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_validationTest
    {
        private const string NUMBER_10 = "1234567890";
        private const string NUMBER_50 = NUMBER_10 + NUMBER_10 + NUMBER_10 + NUMBER_10 + NUMBER_10;
        private const string NUMBER_100 = NUMBER_50 + NUMBER_50;


        #region KiiUser.IsValidUserName(string)
        [Test(), KiiUTInfo(
            action = "When we call IsValidUserName() with valid username,",
            expected = "Method must return true"
            )]
        public void Test_0000_isValidUsername_OK()
        {
            Assert.IsTrue(KiiUser.IsValidUserName("kii1234"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidUserName() with null,",
            expected = "Method must return false"
            )]
        public void Test_0001_isValidUsername_null()
        {
            Assert.IsFalse(KiiUser.IsValidUserName(null));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidUserName() with empty string,",
            expected = "Method must return false"
            )]
        public void Test_0002_isValidUsername_empty()
        {
            Assert.IsFalse(KiiUser.IsValidUserName(""));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidUserName() with username whose length is 2,",
            expected = "Method must return false"
            )]
        public void Test_0003_isValidUsername_len2()
        {
            Assert.IsFalse(KiiUser.IsValidUserName("ab"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidUserName() with username whose length is 3,",
            expected = "Method must return true"
            )]
        public void Test_0004_isValidUsername_len3()
        {
            Assert.IsTrue(KiiUser.IsValidUserName("abc"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidUserName() with username whose length is 64,",
            expected = "Method must return true"
            )]
        public void Test_0005_isValidUsername_len64()
        {
            Assert.IsTrue(KiiUser.IsValidUserName(NUMBER_50 + NUMBER_10 + "1234"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidUserName() with username whose length is 65,",
            expected = "Method must return false"
            )]
        public void Test_0006_isValidUsername_len65()
        {
            Assert.IsFalse(KiiUser.IsValidUserName(NUMBER_50 + NUMBER_10 + "12345"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidUserName() with name that has '-._',",
            expected = "Method must return true"
            )]
        public void Test_0007_isValidUsername_mark()
        {
            Assert.IsTrue(KiiUser.IsValidUserName("-._"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidUserName() with name that has '\',",
            expected = "Method must return false"
            )]
        public void Test_0008_isValidUsername_invalid_mark()
        {
            Assert.IsFalse(KiiUser.IsValidUserName("-._\\"));
        }
        #endregion

        #region KiiUser.IsValidEmail(string)
        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with valid email,",
            expected = "Method must return true"
            )]
        public void Test_0100_isValidEmail_OK()
        {
            Assert.IsTrue(KiiUser.IsValidEmail("test@kii.com"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with null,",
            expected = "Method must return false"
            )]
        public void Test_0101_isValidEmail_null()
        {
            Assert.IsFalse(KiiUser.IsValidEmail(null));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with empty string,",
            expected = "Method must return false"
            )]
        public void Test_0102_isValidEmail_empty()
        {
            Assert.IsFalse(KiiUser.IsValidEmail(""));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with minimum valid email,",
            expected = "Method must return false"
            )]
        public void Test_0103_isValidEmail_minimum()
        {
            Assert.IsFalse(KiiUser.IsValidEmail("a@.ab"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with email whose first part has 256 characters,",
            expected = "Method must return true"
            )]
        public void Test_0104_isValidEmail_FirstPart_256()
        {
            Assert.IsTrue(KiiUser.IsValidEmail(NUMBER_100 + NUMBER_100 + NUMBER_50 + "123456@kii.com"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with email whose first part has 257 characters,",
            expected = "Method must return false"
            )]
        public void Test_0105_isValidEmail_FirstPart_257()
        {
            Assert.IsFalse(KiiUser.IsValidEmail(NUMBER_100 + NUMBER_100 + NUMBER_50 + "1234567@kii.com"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with email whose second part has 65 characters,",
            expected = "Method must return true"
            )]
        public void Test_0106_isValidEmail_SecondPart_65()
        {
            Assert.IsTrue(KiiUser.IsValidEmail("test@" + NUMBER_50 + NUMBER_10 + "12345.jp"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with email whose second part has 66 characters,",
            expected = "Method must return false"
            )]
        public void Test_0107_isValidEmail_SecondPart_66()
        {
            Assert.IsFalse(KiiUser.IsValidEmail("test@" + NUMBER_50 + NUMBER_10 + "123456.jp"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with email whose 3rd part has 1 character,",
            expected = "Method must return true"
            )]
        public void Test_0108_isValidEmail_ThirdPart_1()
        {
            Assert.IsTrue(KiiUser.IsValidEmail("test@kii.a"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with email whose 3rd part has 26 characters,",
            expected = "Method must return true"
            )]
        public void Test_0109_isValidEmail_ThirdPart_26()
        {
            Assert.IsTrue(KiiUser.IsValidEmail("test@kii." + NUMBER_10 + NUMBER_10 + "123456"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with email whose 3rd part has 27 characters,",
            expected = "Method must return false"
            )]
        public void Test_0110_isValidEmail_ThirdPart_27()
        {
            Assert.IsFalse(KiiUser.IsValidEmail("test@kii." + NUMBER_10 + NUMBER_10 + "1234567"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidEmail() with email that doesn't have '.',",
            expected = "Method must return false"
            )]
        public void Test_0111_isValidEmail_Nodot()
        {
            Assert.IsFalse(KiiUser.IsValidEmail("test@kii"));
        }

        #endregion

        #region KiiUser.IsValidPhone(string)

        [Test(), KiiUTInfo(
            action = "When we call IsValidPhone() with valid phone number,",
            expected = "Method must return true"
            )]
        public void Test_0200_isValidPhone_OK()
        {
            Assert.IsTrue(KiiUser.IsValidPhone("09011112222"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPhone() with phone number that starts with '+',",
            expected = "Method must return true"
            )]
        public void Test_0201_isValidPhone_plus()
        {
            Assert.IsTrue(KiiUser.IsValidPhone("+819011112222"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPhone() with null,",
            expected = "Method must return false"
            )]
        public void Test_0202_isValidPhone_null()
        {
            Assert.IsFalse(KiiUser.IsValidPhone(null));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPhone() with '+',",
            expected = "Method must return false"
            )]
        public void Test_0203_isValidPhone_plus_only()
        {
            Assert.IsFalse(KiiUser.IsValidPhone("+"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPhone() with phone that has '.',",
            expected = "Method must return true"
            )]
        public void Test_0204_isValidPhone_dot()
        {
            Assert.IsTrue(KiiUser.IsValidPhone("+81.9011112222"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPhone() with phone that has '-',",
            expected = "Method must return true"
            )]
        public void Test_0205_isValidPhone_hyphen()
        {
            Assert.IsTrue(KiiUser.IsValidPhone("+81-90-1111-2222"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPhone() with phone that has character(not number),",
            expected = "Method must return false"
            )]
        public void Test_0206_isValidPhone_character()
        {
            Assert.IsFalse(KiiUser.IsValidPhone("+81A9011112222"));
        }

        #endregion

        #region KiiUser.IsValidPassword(string)
        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with valid password,",
            expected = "Method must return true"
            )]
        public void Test_0300_isValidPassword_OK()
        {
            Assert.IsTrue(KiiUser.IsValidPassword("123ABC"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with null,",
            expected = "Method must return false"
            )]
        public void Test_0301_isValidPassword_null()
        {
            Assert.IsFalse(KiiUser.IsValidPassword(null));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has 3 characters,",
            expected = "Method must return false"
            )]
        public void Test_0302_isValidPassword_len3()
        {
            Assert.IsFalse(KiiUser.IsValidPassword("abc"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has 4 characters,",
            expected = "Method must return true"
            )]
        public void Test_0303_isValidPassword_len4()
        {
            Assert.IsTrue(KiiUser.IsValidPassword("abcd"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has 50 characters,",
            expected = "Method must return true"
            )]
        public void Test_0304_isValidPassword_len50()
        {
            Assert.IsTrue(KiiUser.IsValidPassword(NUMBER_50));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has 65 characters,",
            expected = "Method must return false"
            )]
        public void Test_0305_isValidPassword_len51()
        {
            Assert.IsFalse(KiiUser.IsValidPassword(NUMBER_50 + "1"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has '.',",
            expected = "Method must return false"
            )]
        public void Test_0306_isValidPassword_mark()
        {
            Assert.IsTrue(KiiUser.IsValidPassword("abc.d"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has 'u0019',",
            expected = "Method must return false"
            )]
        public void Test_0307_isValidPassword_u0019()
        {
            Assert.IsFalse(KiiUser.IsValidPassword("abc\u0019"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has 'u0020',",
            expected = "Method must return true"
            )]
        public void Test_0308_isValidPassword_u0020()
        {
            Assert.IsTrue(KiiUser.IsValidPassword("abc\u0020"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has 'u007E',",
            expected = "Method must return true"
            )]
        public void Test_0309_isValidPassword_u007E()
        {
            Assert.IsTrue(KiiUser.IsValidPassword("abc\u007E"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has 'u007F',",
            expected = "Method must return false"
            )]
        public void Test_0310_isValidPassword_u007F()
        {
            Assert.IsFalse(KiiUser.IsValidPassword("abc\u007F"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has multibyte characters,",
            expected = "Method must return false"
            )]
        public void Test_0311_isValidPassword_multibyte()
        {
            Assert.IsFalse(KiiUser.IsValidPassword("abcＫｉｉ"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has multibyte spaces,",
            expected = "Method must return false"
            )]
        public void Test_0312_isValidPassword_multibyte_space()
        {
            Assert.IsFalse(KiiUser.IsValidPassword("abc　"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidPassword() with password that has space,",
            expected = "Method must return true"
            )]
        public void Test_0313_isValidPassword_space()
        {
            Assert.IsTrue(KiiUser.IsValidPassword("ab c"));
        }

        #endregion

        #region KiiUser.IsValidDisplayName(string)
        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with valid displayName,",
            expected = "Method must return true"
            )]
        public void Test_0400_IsValidDisplayName_OK()
        {
            Assert.IsTrue(KiiUser.IsValidDisplayName("kiiUser"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with null,",
            expected = "Method must return false"
            )]
        public void Test_0401_IsValidDisplayName_null()
        {
            Assert.IsFalse(KiiUser.IsValidDisplayName(null));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with displayName that has 3 characters,",
            expected = "Method must return true"
            )]
        public void Test_0402_IsValidDisplayName_len3()
        {
            Assert.IsTrue(KiiUser.IsValidDisplayName("kii"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with displayName that has 4 characters,",
            expected = "Method must return true"
            )]
        public void Test_0403_IsValidDisplayName_len4()
        {
            Assert.IsTrue(KiiUser.IsValidDisplayName("kii2"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with displayName that has 50 characters,",
            expected = "Method must return true"
            )]
        public void Test_0404_IsValidDisplayName_len50()
        {
            Assert.IsTrue(KiiUser.IsValidDisplayName(NUMBER_50));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with displayName that has 51 characters,",
            expected = "Method must return false"
            )]
        public void Test_0405_IsValidDisplayName_len51()
        {
            Assert.IsFalse(KiiUser.IsValidDisplayName(NUMBER_50 + "a"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with displayName that has multibyte characters,",
            expected = "Method must return true"
            )]
        public void Test_0406_IsValidDisplayName_multiByte()
        {
            Assert.IsTrue(KiiUser.IsValidDisplayName("にほんごめい"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with displayName that has 50 multibyte characters,",
            expected = "Method must return true"
            )]
        public void Test_0407_IsValidDisplayName_multiByte_len50()
        {
            Assert.IsTrue(KiiUser.IsValidDisplayName("１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with displayName that has 51 multibyte characters,",
            expected = "Method must return false"
            )]
        public void Test_0408_IsValidDisplayName_multiByte_len51()
        {
            Assert.IsFalse(KiiUser.IsValidDisplayName("１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１２３４５６７８９０１"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with displayName that has 1 characters,",
            expected = "Method must return true"
            )]
        public void Test_0409_IsValidDisplayName_len1()
        {
            Assert.IsTrue(KiiUser.IsValidDisplayName("a"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with displayName that has 1 characters,",
            expected = "Method must return true"
            )]
        public void Test_0410_IsValidDisplayName_multibyte_len1()
        {
            Assert.IsTrue(KiiUser.IsValidDisplayName("進"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidDisplayName() with empty,",
            expected = "Method must return false"
            )]
        public void Test_0401_IsValidDisplayName_empty()
        {
            Assert.IsFalse(KiiUser.IsValidDisplayName(""));
        }

        #endregion

        #region KiiUser.IsValidCountry(string)
        [Test(), KiiUTInfo(
            action = "When we call IsValidCountry() with valid country code,",
            expected = "Method must return true"
            )]
        public void Test_0500_IsValidCountry_OK()
        {
            Assert.IsTrue(KiiUser.IsValidCountry("JP"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidCountry() with null,",
            expected = "Method must return false"
            )]
        public void Test_0501_IsValidCountry_null()
        {
            Assert.IsFalse(KiiUser.IsValidCountry(null));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidCountry() with 1 character,",
            expected = "Method must return false"
            )]
        public void Test_0502_IsValidCountry_len1()
        {
            Assert.IsFalse(KiiUser.IsValidCountry("J"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidCountry() with 3 characters,",
            expected = "Method must return false"
            )]
        public void Test_0503_IsValidCountry_len3()
        {
            Assert.IsFalse(KiiUser.IsValidCountry("USA"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidCountry() with lower cases,",
            expected = "Method must return false"
            )]
        public void Test_0504_IsValidCountry_lower()
        {
            Assert.IsFalse(KiiUser.IsValidCountry("jp"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidCountry() with number,",
            expected = "Method must return false"
            )]
        public void Test_0505_IsValidCountry_number()
        {
            Assert.IsFalse(KiiUser.IsValidCountry("J2"));
        }

        #endregion

        #region KiiUser.IsValidGlobalPhone(string)

        [Test(), KiiUTInfo(
            action = "When we call IsValidGlobalPhone() with valid phone number,",
            expected = "Method must return true"
            )]
        public void Test_0600_isValidGlobalPhone_OK()
        {
            Assert.IsTrue(KiiUser.IsValidGlobalPhone("+819011112222"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidGlobalPhone() with local phone number,",
            expected = "Method must return false"
            )]
        public void Test_0601_isValidGlobalPhone_local_phone()
        {
            Assert.IsFalse(KiiUser.IsValidGlobalPhone("09011112222"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidGlobalPhone() with null,",
            expected = "Method must return false"
            )]
        public void Test_0602_isValidGlobalPhone_null()
        {
            Assert.IsFalse(KiiUser.IsValidGlobalPhone(null));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidGlobalPhone() with '+',",
            expected = "Method must return false"
            )]
        public void Test_0603_isValidGlobalPhone_plus_only()
        {
            Assert.IsFalse(KiiUser.IsValidGlobalPhone("+"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidGlobalPhone() with phone that has '.',",
            expected = "Method must return true"
            )]
        public void Test_0604_isValidGlobalPhone_dot()
        {
            Assert.IsTrue(KiiUser.IsValidGlobalPhone("+81.9011112222"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidGlobalPhone() with phone that has '-',",
            expected = "Method must return true"
            )]
        public void Test_0605_isValidGlobalPhone_hyphen()
        {
            Assert.IsTrue(KiiUser.IsValidGlobalPhone("+81-90-1111-2222"));
        }

        [Test(), KiiUTInfo(
            action = "When we call IsValidGlobalPhone() with phone that has character(not number),",
            expected = "Method must return false"
            )]
        public void Test_0206_isValidGlobalPhone_character()
        {
            Assert.IsFalse(KiiUser.IsValidGlobalPhone("+81A9011112222"));
        }

        #endregion

        #region
        [Test(), KiiUTInfo(
            action = "When we call CreateByUri() with valid URI,",
            expected = "We can get UserID specified by URI"
            )]
        public void Test_1000_CreateByUri_OK()
        {
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd"));
            Assert.AreEqual("abcd", user.ID);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with wrong scheme,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_1001_CreateByUri_WrongScheme()
        {
            KiiUser.CreateByUri(new Uri("Kii://users/abcd"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with wrong authority(not users),",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_1002_CreateByUri_Wrong_authority()
        {
            KiiUser.CreateByUri(new Uri("kiicloud://User/abcd"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with empty UserID,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_1003_CreateByUri_empty_id()
        {
            KiiUser.CreateByUri(new Uri("kiicloud://users/"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with empty UserID and '/',",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_1004_CreateByUri_empty_id()
        {
            KiiUser.CreateByUri(new Uri("kiicloud://users//"));
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call CreateByUri() with 3 segments,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_1005_CreateByUri_seg3()
        {
            KiiUser.CreateByUri(new Uri("kiicloud://users/abcd/e"));
        }

        #endregion

        #region Bucket(string)
        [Test(), KiiUTInfo(
            action = "When we create KiiUser by CreateByUri() and call Bucket(),",
            expected = "We can get KiiBucket with specified name"
            )]
        public void Test_1100_Bucket_OK()
        {
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd"));
            KiiBucket bucket = user.Bucket("myBucket");
            Assert.IsNotNull(bucket);
            Assert.AreEqual("myBucket", bucket.Name);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we create KiiUser by CreateByUri() and call Bucket() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_1101_Bucket_null()
        {
            KiiUser user = KiiUser.CreateByUri(new Uri("kiicloud://users/abcd"));
            user.Bucket(null);
        }
        #endregion
    }
}

