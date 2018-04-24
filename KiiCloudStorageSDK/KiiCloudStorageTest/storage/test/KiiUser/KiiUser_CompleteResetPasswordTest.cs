using System;
using NUnit.Framework;
using System.Collections;
using System.Text;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_CompleteResetPasswordTest
    {
        private MockHttpClient client;

        [SetUp()]
        public void SetUp()
        {
            Kii.Initialize("appId", "appKey", Kii.Site.US);
            MockHttpClientFactory factory = new MockHttpClientFactory();
            Kii.HttpClientFactory = factory;
            Kii.AsyncHttpClientFactory = factory;
            client = (MockHttpClient)factory.Client;
        }
        [Test()]
        public void Test_CompleteResetPasswordWithID()
        {
            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse (204, null);
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/complete-reset", client.RequestUrl [0]);
            Assert.AreEqual(String.Format("{{\"newPassword\":\"{0}\",\"pinCode\":\"{1}\"}}", newPassword, pinCode), client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.CompletePasswordResetRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test()]
        public void Test_CompleteResetPasswordWithPhone()
        {
            string identifier = "+818034068125";
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse (204, null);
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/PHONE:" + identifier + "/password/complete-reset", client.RequestUrl [0]);
            Assert.AreEqual(String.Format("{{\"newPassword\":\"{0}\",\"pinCode\":\"{1}\"}}", newPassword, pinCode), client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.CompletePasswordResetRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test()]
        public void Test_CompleteResetPasswordWithEmail()
        {
            string identifier = "test@kii.com";
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse (204, null);
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/EMAIL:" + identifier + "/password/complete-reset", client.RequestUrl [0]);
            Assert.AreEqual(String.Format("{{\"newPassword\":\"{0}\",\"pinCode\":\"{1}\"}}", newPassword, pinCode), client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.CompletePasswordResetRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test()]
        public void Test_CompleteResetPasswordWithoutPassword()
        {
            string identifier = Guid.NewGuid().ToString();
            string pinCode = "1234";
            client.AddResponse (204, null);
            KiiUser.CompleteResetPassword(identifier, pinCode, null);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/complete-reset", client.RequestUrl [0]);
            Assert.AreEqual(String.Format("{{\"pinCode\":\"{0}\"}}", pinCode), client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.CompletePasswordResetRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_CompleteResetPasswordWithoutIdentifier()
        {
            string newPassword = "password";
            string pinCode = "1234";
            KiiUser.CompleteResetPassword(null, pinCode, newPassword);
        }
        [Test(), ExpectedException(typeof(ArgumentException))]
        public void Test_CompleteResetPasswordWithoutPinCode()
        {
            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            KiiUser.CompleteResetPassword(identifier, null, newPassword);
        }
        [Test(), ExpectedException(typeof(BadRequestException))]
        public void Test_CompleteResetPasswordError400()
        {
            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse(new BadRequestException("", null, "", BadRequestException.Reason.__UNKNOWN__));
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword);
        }
        [Test(), ExpectedException(typeof(UnauthorizedException))]
        public void Test_CompleteResetPasswordError401()
        {
            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse(new UnauthorizedException("", null, ""));
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword);
        }
        [Test(), ExpectedException(typeof(ForbiddenException))]
        public void Test_CompleteResetPasswordError403()
        {
            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse(new ForbiddenException("", null, ""));
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword);
        }
        [Test(), ExpectedException(typeof(ConflictException))]
        public void Test_CompleteResetPasswordError409()
        {
            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse(new ConflictException("", null, "", ConflictException.Reason.INVALID_STATUS));
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword);
        }
        [Test()]
        public void Test_CompleteResetPasswordWithID_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse (204, null);
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(ex);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/complete-reset", client.RequestUrl [0]);
            Assert.AreEqual(String.Format("{{\"newPassword\":\"{0}\",\"pinCode\":\"{1}\"}}", newPassword, pinCode), client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.CompletePasswordResetRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test()]
        public void Test_CompleteResetPasswordWithPhone_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string identifier = "+818034068125";
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse (204, null);
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(ex);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/PHONE:" + identifier + "/password/complete-reset", client.RequestUrl [0]);
            Assert.AreEqual(String.Format("{{\"newPassword\":\"{0}\",\"pinCode\":\"{1}\"}}", newPassword, pinCode), client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.CompletePasswordResetRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test()]
        public void Test_CompleteResetPasswordWithEmail_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string identifier = "test@kii.com";
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse (204, null);
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(ex);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/EMAIL:" + identifier + "/password/complete-reset", client.RequestUrl [0]);
            Assert.AreEqual(String.Format("{{\"newPassword\":\"{0}\",\"pinCode\":\"{1}\"}}", newPassword, pinCode), client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.CompletePasswordResetRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test()]
        public void Test_CompleteResetPasswordWithoutPassword_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string identifier = Guid.NewGuid().ToString();
            string pinCode = "1234";
            client.AddResponse (204, null);
            KiiUser.CompleteResetPassword(identifier, pinCode, null, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(ex);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/complete-reset", client.RequestUrl [0]);
            Assert.AreEqual(String.Format("{{\"pinCode\":\"{0}\"}}", pinCode), client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.CompletePasswordResetRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test()]
        public void Test_CompleteResetPasswordWithoutIdentifier_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string newPassword = "password";
            string pinCode = "1234";
            KiiUser.CompleteResetPassword(null, pinCode, newPassword, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex is ArgumentException);
        }
        [Test()]
        public void Test_CompleteResetPasswordWithoutPinCode_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            KiiUser.CompleteResetPassword(identifier, null, newPassword, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex is ArgumentException);
        }
        [Test()]
        public void Test_CompleteResetPasswordError400_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse(new BadRequestException("", null, "", BadRequestException.Reason.__UNKNOWN__));
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex is BadRequestException);
        }
        [Test()]
        public void Test_CompleteResetPasswordError401_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse(new UnauthorizedException("", null, ""));
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex is UnauthorizedException);
        }
        [Test()]
        public void Test_CompleteResetPasswordError403_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse(new ForbiddenException("", null, ""));
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex is ForbiddenException);
        }
        [Test()]
        public void Test_CompleteResetPasswordError409_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;

            string identifier = Guid.NewGuid().ToString();
            string newPassword = "password";
            string pinCode = "1234";
            client.AddResponse(new ConflictException("", null, "", ConflictException.Reason.INVALID_STATUS));
            KiiUser.CompleteResetPassword(identifier, pinCode, newPassword, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex is ConflictException);
        }
    }
}
