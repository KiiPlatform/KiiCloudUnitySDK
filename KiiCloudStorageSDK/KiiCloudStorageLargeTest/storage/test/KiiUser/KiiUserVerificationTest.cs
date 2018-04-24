using System;
using System.IO;
using System.Net;
using System.Text;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    class TestLogger : IKiiLogger
    {
        public void Debug (string message, params object[] args)
        {
            System.Console.WriteLine (message, args);
        }
    }

    [TestFixture ()]
    public class KiiUserVerificationTest : LargeTestBase
    {
        // This App is managed by satoshi.kumano.
        // Both Email Address Verification and Phone Number Verification are turned on.
        // Configuration should be immutable.
        private const string appID = "3d3a9a6d";
        private const string appKey = "7cfb026430e524d8f44202b8b9e18dac";
        private const string baseUrl = "https://api-jp.kii.com/api";
        private const string clientId = "99bb2abb80f031284baa0c2fecf6cd44";
        private const string clientSecret = "550dfcda8cdcae465d12472d2bd357ca6c6b830983f8aa9b0d7410390f60e02c";

        [SetUp]
        public override void SetUp ()
        {
            Kii.Initialize (appID, appKey, baseUrl);
            Kii.Logger = new TestLogger ();
        }
        [Test()]
        public void EmailVerificationTest ()
        {
            // Register user with email
            string username = "test" + CurrentTimeMillis();
            string email = username +"@kii.com";
            KiiUser user =  KiiUser.BuilderWithEmail(email).SetName(username).Build();
            user.Register("password");
            Assert.AreEqual(email, user.Email);
            Assert.IsFalse(user.EmailVerified);
            Assert.IsNull(user.PendingEmail);

            // verify Email by admin
            verifyEmail(user.ID, user.Email);

            // Check the user
            user.Refresh();
            Assert.AreEqual(email, user.Email);
            Assert.IsTrue(user.EmailVerified);
            Assert.IsNull(user.PendingEmail);

            // Change Email
            string newEmail = "new_" + email;
            KiiUser.ChangeEmail(newEmail);

            Assert.AreEqual(email, KiiUser.CurrentUser.Email);
            Assert.IsTrue(KiiUser.CurrentUser.EmailVerified);
            Assert.IsNull(KiiUser.CurrentUser.PendingEmail);

            // Check the user
            user.Refresh();
            Assert.AreEqual(email, user.Email);
            Assert.IsTrue(user.EmailVerified);
            Assert.AreEqual(newEmail, user.PendingEmail);

            // verify Email by admin
            verifyEmail(user.ID, user.PendingEmail);

            // Check the user
            user.Refresh();
            Assert.AreEqual(newEmail, user.Email);
            Assert.IsTrue(user.EmailVerified);
            Assert.IsNull(user.PendingEmail);
        }
            
        [Test()]
        public void PhoneVerificationTest ()
        {
            // Register user with phone number
            string username = "test" + CurrentTimeMillis();
            string phone = GenerateGlobalPhoneNumber();
            KiiUser user =  KiiUser.BuilderWithPhone(phone).SetName(username).Build();
            user.Register("password");
            Assert.AreEqual(phone, user.Phone);
            Assert.IsFalse(user.PhoneVerified);
            Assert.IsNull(user.PendingPhone);

            // verify Phone by admin
            verifyPhone(user.ID, user.Phone);

            // Check the user
            user.Refresh();
            Assert.AreEqual(phone, user.Phone);
            Assert.IsTrue(user.PhoneVerified);
            Assert.IsNull(user.PendingPhone);

            // Change phone number
            string newPhone = GenerateGlobalPhoneNumber();
            KiiUser.ChangePhone(newPhone);

            Assert.AreEqual(phone, KiiUser.CurrentUser.Phone);
            Assert.IsTrue(KiiUser.CurrentUser.PhoneVerified);
            Assert.IsNull(KiiUser.CurrentUser.PendingPhone);

            // Check the user
            user.Refresh();
            Assert.AreEqual(phone, user.Phone);
            Assert.IsTrue(user.PhoneVerified);
            Assert.AreEqual(newPhone, user.PendingPhone);

            // verify Phone by admin
            verifyPhone(user.ID, user.PendingPhone);

            // Check the user
            user.Refresh();
            Assert.AreEqual(newPhone, user.Phone);
            Assert.IsTrue(user.PhoneVerified);
            Assert.IsNull(user.PendingPhone);
        }
        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
        private void verifyEmail(string userID, string email)
        {
            string url = baseUrl + "/apps/" + appID + "/users/" + userID + "/email-address";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            JsonObject json = new JsonObject();
            json.Put("emailAddress", email);
            json.Put("verified", true);
            byte[] body = Encoding.GetEncoding("UTF-8").GetBytes(json.ToString());
            request.ContentLength = body.Length;
            request.ContentType = "application/vnd.kii.EmailAddressModificationRequest+json";
            request.Method = "PUT";
            request.Headers.Add("X-Kii-AppID", appID);
            request.Headers.Add("X-Kii-AppKey", appKey);
            request.Headers.Add("Authorization", "Bearer " + getAdminToken());
            request.GetRequestStream().Write(body, 0, body.Length);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("server returned http status:" +  response.StatusCode);
            }
        }
        private void verifyPhone(string userID, string phone)
        {
            string url = baseUrl + "/apps/" + appID + "/users/" + userID + "/phone-number";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            JsonObject json = new JsonObject();
            json.Put("phoneNumber", phone);
            json.Put("verified", true);
            byte[] body = Encoding.GetEncoding("UTF-8").GetBytes(json.ToString());
            request.ContentLength = body.Length;
            request.ContentType = "application/vnd.kii.PhoneNumberModificationRequest+json";
            request.Method = "PUT";
            request.Headers.Add("X-Kii-AppID", appID);
            request.Headers.Add("X-Kii-AppKey", appKey);
            request.Headers.Add("Authorization", "Bearer " + getAdminToken());
            request.GetRequestStream().Write(body, 0, body.Length);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception("server returned http status:" +  response.StatusCode);
            }
        }
        private string getAdminToken()
        {
            string url = baseUrl + "/oauth2/token";
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            JsonObject json = new JsonObject();
            json.Put("client_id", clientId);
            json.Put("client_secret", clientSecret);
            byte[] body = Encoding.GetEncoding("UTF-8").GetBytes(json.ToString());
            request.ContentType = "application/json";
            request.ContentLength = body.Length;
            request.Method = "POST";
            request.Headers.Add("X-Kii-AppID", appID);
            request.Headers.Add("X-Kii-AppKey", appKey);
            request.GetRequestStream().Write(body, 0, body.Length);
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            string responseBody = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("UTF-8")).ReadToEnd();
            return new JsonObject(responseBody).GetString("access_token");
        }
        private string GenerateGlobalPhoneNumber()
        {
            string currentTime = CurrentTimeMillis().ToString();
            return "+874" + currentTime.Substring(currentTime.Length - 10, 10);
        }
    }
}

