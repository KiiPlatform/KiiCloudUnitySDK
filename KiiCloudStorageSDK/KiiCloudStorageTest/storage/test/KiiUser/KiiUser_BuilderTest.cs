using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_BuilderTest
    {
        private const string NUMBER_10 = "1234567890";

        #region KiiUser.BuilderWithName(string)
        [Test(), KiiUTInfo(
            action = "When we call BuilderWithName(),",
            expected = "We can get specified Username"
            )]
        public void Test_0000_BuilderWithName_OK()
        {
            KiiUser.Builder builder = KiiUser.BuilderWithName("kii1234");
            Assert.AreEqual("kii1234", builder.Build().Username);
        }

        [Test(), KiiUTInfo(
            action = "When we call BuilderWithName() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0001_BuilderWithName_invalid_name()
        {
            try
            {
                KiiUser.BuilderWithName(null);
                Assert.Fail("Argument Exception must be thrown");
            }
            catch (ArgumentException)
            {
                // OK
            }
            catch
            {
                Assert.Fail("Argument Exception must be thrown" );
            }
        }

        #endregion

        #region KiiUser.BuilderWithEmail(string)
        [Test(), KiiUTInfo(
            action = "When we call BuilderWithEmail(),",
            expected = "We can get specified Email"
            )]
        public void Test_0100_BuilderWithEmail_OK()
        {
            KiiUser.Builder builder = KiiUser.BuilderWithEmail("test@kii.com");
            Assert.AreEqual("test@kii.com", builder.Build().Email);
        }

        [Test(), KiiUTInfo(
            action = "When we call BuilderWithEmail() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0101_BuilderWithEmail_invalid_Email()
        {
            try
            {
                KiiUser.BuilderWithEmail(null);
                Assert.Fail("Argument Exception must be thrown");
            }
            catch (ArgumentException)
            {
                // OK
            }
            catch
            {
                Assert.Fail("Argument Exception must be thrown" );
            }
        }
        #endregion

        #region KiiUser.BuilderWithPhone(string)
        [Test(), KiiUTInfo(
            action = "When we call BuilderWithPhone(),",
            expected = "We can get specified phone number"
            )]
        public void Test_0200_BuilderWithPhone_OK()
        {
            KiiUser.Builder builder = KiiUser.BuilderWithPhone("+819011112222");
            Assert.AreEqual("+819011112222", builder.Build().Phone);
        }

        [Test(), KiiUTInfo(
            action = "When we call BuilderWithPhone() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0201_BuilderWithPhone_invalid_phone()
        {
            try
            {
                KiiUser.BuilderWithPhone(null);
                Assert.Fail("Argument Exception must be thrown");
            }
            catch (ArgumentException)
            {
                // OK
            }
            catch
            {
                Assert.Fail("Argument Exception must be thrown" );
            }
        }

        #endregion

        #region Builder with some field
        [Test(), KiiUTInfo(
            action = "When we call BuilderWithName/Email/Phone() at the same time,",
            expected = "We can get specified Username/Email/Phone"
            )]
        public void Test_0300_Builde_All()
        {
            KiiUser.Builder builder = KiiUser.BuilderWithName("kii1234").WithEmail("test@kii.com").WithPhone("09011112222");
            KiiUser user = builder.Build();
            Assert.AreEqual("kii1234", user.Username);
            Assert.AreEqual("test@kii.com", user.Email);
            Assert.AreEqual("09011112222", user.Phone);
        }

        #endregion

        #region KiiUser.BuilderWithLocalPhone(string, string)
        [Test(), KiiUTInfo(
            action = "When we call BuilderWithLocalPhone(),",
            expected = "We can get specified Phone"
            )]
        public void Test_0400_BuilderWithLocalPhone_OK()
        {
            string phone = "09011112222";
            string country = "JP";
            KiiUser.Builder builder = KiiUser.BuilderWithLocalPhone(phone, country);

            KiiUser user = builder.Build();
            Assert.IsNull(user.Username);
            Assert.IsNull(user.Email);
            Assert.AreEqual(phone, user.Phone);
            Assert.AreEqual(country, user.Country);
        }
        
        [Test(),
         ExpectedException(
            typeof(ArgumentException), 
            ExpectedMessage="Invalid Phone format:",
            MatchType=MessageMatch.Exact
            ),
         KiiUTInfo(
            action = "When we call BuilderWithLocalPhone() with invalid phone number,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0401_BuilderWithLocalPhone_Invalid_Phone()
        {
            string phone = "";
            string country = "JP";

            KiiUser.BuilderWithLocalPhone(phone, country);
        }
        
        [Test(),
         ExpectedException(
            typeof(ArgumentException),
            ExpectedMessage="Invalid country format:",
            MatchType=MessageMatch.Exact
            ),
         KiiUTInfo(
            action = "When we call BuilderWithLocalPhone() with invalid country,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0402_BuilderWithLocalPhone_Invalid_Country()
        {
            string phone = "09011112222";
            string country = "";

            KiiUser.BuilderWithLocalPhone(phone, country);
        }
        #endregion

        #region KiiUser.BuilderWithIdentifier(string)
        [Test(), KiiUTInfo(
            action = "When we call BuilderWithIdentifier() with username,",
            expected = "We can get specified Username"
            )]
        public void Test_0500_BuilderWithIdentifier_Username_OK()
        {
            string identifier = "kii1234";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            KiiUser user = builder.Build();
            Assert.AreEqual(identifier, user.Username);
            Assert.IsNull(user.Email);
            Assert.IsNull(user.Phone);
            Assert.IsNull(user.Country);
        }

        [Test(), KiiUTInfo(
            action = "When we call BuilderWithIdentifier() with phone number,",
            expected = "We can get specified Phone number"
            )]
        public void Test_0501_BuilderWithIdentifier_Phone_OK()
        {
            string identifier = "+819011112222";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            KiiUser user = builder.Build();
            Assert.IsNull(user.Username);
            Assert.IsNull(user.Email);
            Assert.AreEqual(identifier, user.Phone);
            Assert.IsNull(user.Country);
        }

        [Test(), KiiUTInfo(
            action = "When we call BuilderWithIdentifier() with email address,",
            expected = "We can get specified Email"
            )]
        public void Test_0502_BuilderWithIdentifier_Email_OK()
        {
            string identifier = "test@kii.com";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            KiiUser user = builder.Build();
            Assert.IsNull(user.Username);
            Assert.AreEqual(identifier, user.Email);
            Assert.IsNull(user.Phone);
            Assert.IsNull(user.Country);
        }

        [Test(),
         ExpectedException(
            typeof(ArgumentException),
            ExpectedMessage="Unspecified identifier:",
            MatchType=MessageMatch.Exact
            ),
         KiiUTInfo(
            action = "When we call BuilderWithIdentifier() with null,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0503_BuilderWithIdentifier_invalid()
        {
            KiiUser.BuilderWithIdentifier(null);
        }

        #endregion

        #region SetGlobalPhone(string)
        [Test(), KiiUTInfo(
            action = "When we call SetGlobalPhone(),",
            expected = "We can get specified Phone"
            )]
        public void Test_0600_SetGlobalPhone_OK()
        {
            string local = "09011112222";
            string country = "JP";
            string global = "+819011112222";
            KiiUser.Builder builder = KiiUser.BuilderWithLocalPhone(local, country);

            KiiUser user1 = builder.Build();
            Assert.IsNull(user1.Username);
            Assert.IsNull(user1.Email);
            Assert.AreEqual(local, user1.Phone);
            Assert.AreEqual(country, user1.Country);

            builder.SetGlobalPhone(global);

            KiiUser user2 = builder.Build();
            Assert.IsNull(user2.Username);
            Assert.IsNull(user2.Email);
            Assert.AreEqual(global, user2.Phone);
            Assert.IsNull(user2.Country);
        }

        [Test(),
         ExpectedException(
            typeof(ArgumentException),
            ExpectedMessage="Invalid Phone format:09011112222",
            MatchType=MessageMatch.Exact
            ),
         KiiUTInfo(
            action = "When we call SetGlobalPhone() with local phone number,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0601_SetGlobalPhone_Invalid()
        {
            string identifier = "kii1234";
            string phone = "09011112222";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.SetGlobalPhone(phone);
        }
        #endregion

        #region SetLocalPhone(string, string)
        [Test(), KiiUTInfo(
            action = "When we call SetLocalPhone(),",
            expected = "We can get specified Phone"
            )]
        public void Test_0700_SetLocalPhone_OK()
        {
            string identifier = "+819011112222";
            string phone = "09011112222";
            string country = "JP";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            KiiUser user1 = builder.Build();
            Assert.IsNull(user1.Username);
            Assert.IsNull(user1.Email);
            Assert.AreEqual(identifier, user1.Phone);
            Assert.IsNull(user1.Country);

            builder.SetLocalPhone(phone, country);

            KiiUser user2 = builder.Build();
            Assert.IsNull(user2.Username);
            Assert.IsNull(user2.Email);
            Assert.AreEqual(phone, user2.Phone);
            Assert.AreEqual(country, user2.Country);
        }

        [Test(),
         ExpectedException(
            typeof(ArgumentException),
            ExpectedMessage="Invalid Phone format:",
            MatchType=MessageMatch.Exact
            ),
         KiiUTInfo(
            action = "When we call SetLocalPhone() with invalid phone number,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0701_SetLocalPhone_Invalid_Phone()
        {
            string identifier = "+819011112222";
            string phone = "";
            string country = "JP";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.SetLocalPhone(phone, country);
        }

        [Test(),
         ExpectedException(
            typeof(ArgumentException),
            ExpectedMessage="Invalid country format:",
            MatchType=MessageMatch.Exact
            ),
         KiiUTInfo(
            action = "When we call SetLocalPhone() with invalid country,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0702_SetLocalPhone_Invalid_Country()
        {
            string identifier = "+819011112222";
            string phone = "09011112222";
            string country = "";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.SetLocalPhone(phone, country);
        }
        #endregion

        #region SetEmail(string)
        [Test(), KiiUTInfo(
            action = "When we call SetEmail(),",
            expected = "We can get specified Email"
            )]
        public void Test_0800_SetEmail_OK()
        {
            string identifier = "kii1234";
            string email = "test@kii.com";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            KiiUser user1 = builder.Build();
            Assert.AreEqual(identifier, user1.Username);
            Assert.IsNull(user1.Email);
            Assert.IsNull(user1.Phone);
            Assert.IsNull(user1.Country);

            builder.SetEmail(email);

            KiiUser user2 = builder.Build();
            Assert.AreEqual(identifier, user2.Username);
            Assert.AreEqual(email, user2.Email);
            Assert.IsNull(user2.Phone);
            Assert.IsNull(user2.Country);
        }

        [Test(),
         ExpectedException(
            typeof(ArgumentException),
            ExpectedMessage="Invalid Email format:testOkii.com",
            MatchType=MessageMatch.Exact
            ),
         KiiUTInfo(
            action = "When we call SetEmail() with invalid email address,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0801_SetEmail_Invalid()
        {
            string identifier = "+819011112222";
            string email = "testOkii.com";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.SetEmail(email);
        }
        #endregion

        #region SetName(string)
        [Test(), KiiUTInfo(
            action = "When we call SetName(),",
            expected = "We can get specified Email"
            )]
        public void Test_0900_SetName_OK()
        {
            string identifier = "test@kii.com";
            string username = "kii1234";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            KiiUser user1 = builder.Build();
            Assert.IsNull(user1.Username);
            Assert.AreEqual(identifier, user1.Email);
            Assert.IsNull(user1.Phone);
            Assert.IsNull(user1.Country);

            builder.SetName(username);

            KiiUser user2 = builder.Build();
            Assert.AreEqual(username, user2.Username);
            Assert.AreEqual(identifier, user2.Email);
            Assert.IsNull(user2.Phone);
            Assert.IsNull(user2.Country);
        }
        
        [Test(),
         ExpectedException(
            typeof(ArgumentException),
            ExpectedMessage="Invalid username format:",
            MatchType=MessageMatch.Exact
            ),
         KiiUTInfo(
            action = "When we call SetName() with invalid user name,",
            expected = "ArgumentException must be thrown"
            )]
        public void Test_0901_SetName_Invalid()
        {
            string identifier = "test@kii.com";
            string username = "";
            KiiUser.Builder builder = KiiUser.BuilderWithIdentifier(identifier);

            builder.SetName(username);
        }
        #endregion
    }
}

