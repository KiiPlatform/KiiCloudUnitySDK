// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using NUnit.Framework;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiUser_ChangeAPI
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            
            client = (MockHttpClient)factory.Client;
        }

        private void LogIn()
        {
            // set Response
            client.AddResponse(200, "{" +
                               "\"id\" : \"user1234\"," +
                               "\"access_token\" : \"cdef\"," +
                               "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn("kii1234", "pass1234");
        }

        #region KiiUser.ChangePassword(newPassword, oldPassword)

        [Test(), KiiUTInfo(
            action = "When we call ChangePassword() and Server returns HTTP 204,",
            expected = "No exception is thrown"
            )]
        public void Test_0000_ChangePassword ()
        {
            this.LogIn();

            string newPassword = "newPassword";
            string oldPassword = "oldPassword";

            // set Response
            client.AddResponse(204, "");

            KiiUser.ChangePassword(newPassword, oldPassword);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call ChangePassword() with wrong old password and Server returns HTTP 401,",
            expected = "CloudException is thrown"
            )]
        public void Test_0001_ChangePassword_wrongPassword ()
        {
            this.LogIn();
            
            string newPassword = "newPassword";
            string oldPassword = "oldPassword_wrong";
            
            // set Response
            client.AddResponse(new CloudException(401, 
                    "{" +
                    "\"errorCode\" : \"WRONG_PASSWORD\"," +
                    "\"message\" : \"The provided password is wrong\"," +
                    "\"appID\" : \"appId\"," +
                    "\"userID\" : \"86f3cdcf-950d-4d26-aa1c-443573de1736\"," +
                    "\"suppressed\" : [ ]" +
                    "}"));
            
            KiiUser.ChangePassword(newPassword, oldPassword);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call ChangePassword() with null newPassword,",
            expected = "ArgumentException is thrown"
            )]
        public void Test_0010_ChangePassword_newPassword_null ()
        {
            this.LogIn();
            
            string newPassword = null;
            string oldPassword = "oldPassword";
            
            // set Response
            client.AddResponse(204, "");
            
            KiiUser.ChangePassword(newPassword, oldPassword);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call ChangePassword() with null oldPassword,",
            expected = "ArgumentException is thrown"
            )]
        public void Test_0011_ChangePassword_oldPassword_null ()
        {
            this.LogIn();
            
            string newPassword = "newPassword";
            string oldPassword = null;
            
            // set Response
            client.AddResponse(204, "");
            
            KiiUser.ChangePassword(newPassword, oldPassword);
        }
        #endregion

        #region KiiUser.ChangeEmail(string email)
        [Test(), KiiUTInfo(
            action = "When we call ChangeEmail() and Server returns HTTP 204,",
            expected = "No exception is thrown"
            )]
        public void Test_0100_ChangeEmail ()
        {
            this.LogIn();
            
            string newEmail = "test@test.com";
            
            // set Response
            client.AddResponse(204, "");
            
            KiiUser.ChangeEmail(newEmail);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call ChangeEmail() and Server returns HTTP 409(Already exists),",
            expected = "CloudException is thrown"
            )]
        public void Test_0101_ChangeEmail_already_exists ()
        {
            this.LogIn();
            
            string newEmail = "test@test.com";
            
            // set Response
            client.AddResponse(new CloudException(409, 
                    "{" +
                    "\"errorCode\" : \"USER_ALREADY_EXISTS\"," +
                    "\"message\" : \"User with emailAddress test@test.com already exists\"," +
                    "\"value\" : \"test@test.com\"," +
                    "\"field\" : \"emailAddress\"," +
                    "\"suppressed\" : [ ]" +
                    "}"));
            
            KiiUser.ChangeEmail(newEmail);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call ChangeEmail() with null,",
            expected = "ArgumentException is thrown"
            )]
        public void Test_0110_ChangeEmail_null ()
        {
            this.LogIn();
            
            string newEmail = null;
            
            // set Response
            client.AddResponse(204, "");
            
            KiiUser.ChangeEmail(newEmail);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call ChangeEmail() with invalid email,",
            expected = "ArgumentException is thrown"
            )]
        public void Test_0111_ChangeEmail_invalid_email ()
        {
            this.LogIn();
            
            string newEmail = "invalidEmailFormat";
            
            // set Response
            client.AddResponse(204, "");
            
            KiiUser.ChangeEmail(newEmail);
        }
        #endregion

        #region KiiUser.ChangePhone(string phoneNumber)
        [Test(), KiiUTInfo(
            action = "When we call ChangePhone() and Server returns HTTP 204,",
            expected = "No exception is thrown"
            )]
        public void Test_0200_ChangePhone ()
        {
            this.LogIn();
            
            string newPhone = "+818011112222";
            
            // set Response
            client.AddResponse(204, "");
            
            KiiUser.ChangePhone(newPhone);
        }

        [Test(), ExpectedException(typeof(CloudException)), KiiUTInfo(
            action = "When we call ChangePhone() and Server returns HTTP 409(Already exists),",
            expected = "CloudException is thrown"
            )]
        public void Test_0201_ChangePhone_already_exists ()
        {
            this.LogIn();
            
            string newPhone = "+818011112222";
            
            // set Response
            client.AddResponse(new CloudException(409, 
                    "{" +
                    "\"errorCode\" : \"USER_ALREADY_EXISTS\"," +
                    "\"message\" : \"User with phoneNumber +818011112222 already exists\"," +
                    "\"value\" : \"+818011112222\"," +
                    "\"field\" : \"phoneNumber\"," +
                    "\"suppressed\" : [ ]" +
                    "}"));
            
            KiiUser.ChangePhone(newPhone);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call ChangePhone() witu null,",
            expected = "ArgumentException is thrown"
            )]
        public void Test_0210_ChangePhone_null ()
        {
            this.LogIn();
            
            string newPhone = null;
            
            // set Response
            client.AddResponse(204, "");
            
            KiiUser.ChangePhone(newPhone);
        }

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call ChangePhone() witu invalid phone number,",
            expected = "ArgumentException is thrown"
            )]
        public void Test_0211_ChangePhone_invalid_phone_number ()
        {
            this.LogIn();
            
            string newPhone = "invalidPhoneNumber";
            
            // set Response
            client.AddResponse(204, "");
            
            KiiUser.ChangePhone(newPhone);
        }
        #endregion

    }
}

