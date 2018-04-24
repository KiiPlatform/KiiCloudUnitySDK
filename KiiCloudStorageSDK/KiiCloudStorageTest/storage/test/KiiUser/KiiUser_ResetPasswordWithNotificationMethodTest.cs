using System;
using NUnit.Framework;
using System.Collections;
using System.Text;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class KiiUser_ResetPasswordWithNotificationMethodTest
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

        [Test(), ExpectedException(typeof(ArgumentException)), KiiUTInfo(
            action = "When we call ResetPassword() with null,",
            expected = "ArgumentException must be thrown")]
        public void Test_IdentifyIsNull()
        {
            client.AddResponse (204, null);
            KiiUser.ResetPassword(null, KiiUser.NotificationMethod.SMS);
        }
        [Test(), ExpectedException(typeof(NotFoundException)), KiiUTInfo(
            action = "When we call ResetPassword() and target user not exists in the cloud,",
            expected = "NotFoundException must be thrown")]
        public void Test_UserNotFound()
        {
            string identifier = "notfound@kii.com";
            client.AddResponse(new NotFoundException("", null, "", NotFoundException.Reason.USER_NOT_FOUND));
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS);
        }
        [Test(), ExpectedException(typeof(ConflictException)), KiiUTInfo(
            action = "When we call ResetPassword() with NotificationMethod.EMAIL and target user doesn't have email,",
            expected = "ConflictException must be thrown")]
        public void Test_UserNotHasEmail()
        {
            string identifier = "notfound@kii.com";
            client.AddResponse(new ConflictException("", null, "", ConflictException.Reason.INVALID_STATUS));
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.EMAIL);
        }
        [Test(), ExpectedException(typeof(BadRequestException)), KiiUTInfo(
            action = "When we call ResetPassword() with bad request,",
            expected = "BadRequestException must be thrown")]
        public void Test_BadRequest()
        {
            string identifier = "400@kii@com";
            client.AddResponse(new BadRequestException("", null, "", BadRequestException.Reason.INVALID_JSON));
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with phone number and NotificationMethod.SMS,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByPhoneViaSms()
        {
            string identifier = "PHONE:+819011112222";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"URL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with phone number and NotificationMethod.SMS_PIN,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByPhoneViaSmsPin()
        {
            string identifier = "PHONE:+819011112222";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS_PIN);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"PIN\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with email and NotificationMethod.SMS,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByEmailViaSms()
        {
            string identifier = "EMAIL:test@kii.com";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"URL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with email and NotificationMethod.SMS_PIN,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByEmailViaSmsPin()
        {
            string identifier = "EMAIL:test@kii.com";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS_PIN);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"PIN\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with userID and NotificationMethod.SMS,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByIdViaSms()
        {
            string identifier = "USERID-00001-00001";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"URL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with userID and NotificationMethod.SMS_PIN,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByIdViaSmsPin()
        {
            string identifier = "USERID-00001-00001";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS_PIN);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"PIN\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with phone number and NotificationMethod.EMAIL,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByPhoneViaEmail()
        {
            string identifier = "PHONE:+819011112222";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.EMAIL);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"EMAIL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with local phone number and NotificationMethod.EMAIL,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByLocalPhoneViaEmail()
        {
            string identifier = "09011112222";
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.EMAIL);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"EMAIL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with global phone number and NotificationMethod.EMAIL,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByGlobalPhoneViaEmail()
        {
            string identifier = "+819011112222";
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.EMAIL);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + Uri.EscapeUriString("PHONE:" + identifier) + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"EMAIL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with email and NotificationMethod.EMAIL,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByEmailViaEmail()
        {
            string identifier = "EMAIL:test@kii.com";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.EMAIL);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"EMAIL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with userID and NotificationMethod.EMAIL,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByIdViaEmail()
        {
            string identifier = "USERID-00001-00001";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.EMAIL);

            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"EMAIL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }

        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with null asynchronously,",
            expected = "ArgumentException must be passed in the callback")]
        public void Test_IdentifyIsNull_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(null, KiiUser.NotificationMethod.SMS, (Exception e)=>{
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
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() and target user not exists in the cloud,",
            expected = "NotFoundException must be passed in the callback")]
        public void Test_UserNotFound_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "notfound@kii.com";
            client.AddResponse(new NotFoundException("", null, "", NotFoundException.Reason.USER_NOT_FOUND));
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNotNull(ex);
            Assert.IsTrue(ex is NotFoundException);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with bad request,",
            expected = "BadRequestException must be passed in the callback")]
        public void Test_BadRequest_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "400@kii@com";
            client.AddResponse(new BadRequestException("", null, "", BadRequestException.Reason.INVALID_JSON));
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS, (Exception e)=>{
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
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with phone number and NotificationMethod.SMS asynchronously,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByPhoneViaSms_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "PHONE:+819011112222";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(ex);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"URL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with phone number and NotificationMethod.SMS_PIN asynchronously,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByPhoneViaSmsPin_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "PHONE:+819011112222";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS_PIN, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(ex);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"PIN\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with email and NotificationMethod.SMS asynchronously,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByEmailViaSms_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "EMAIL:test@kii.com";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(ex);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"URL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with email and NotificationMethod.SMS_PIN asynchronously,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByEmailViaSmsPin_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "EMAIL:test@kii.com";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS_PIN, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(ex);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"PIN\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with userID and NotificationMethod.SMS asynchronously,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByIdViaSms_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "USERID-00001-00001";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(ex);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"URL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with userID and NotificationMethod.SMS_PIN asynchronously,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByIdViaSmsPin_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "USERID-00001-00001";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.SMS_PIN, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(ex);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"SMS\",\"smsResetMethod\":\"PIN\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with phone number and NotificationMethod.EMAIL asynchronously,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByPhoneViaEmail_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "PHONE:+819011112222";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.EMAIL, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(ex);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"EMAIL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with email and NotificationMethod.EMAIL asynchronously,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByEmailViaEmail_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "EMAIL:test@kii.com";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.EMAIL, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(ex);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"EMAIL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with userID and NotificationMethod.EMAIL asynchronously,",
            expected = "HTTP request should be sent to server as expected.")]
        public void Test_IdentifyByIdViaEmail_Async()
        {
            CountDownLatch cd = new CountDownLatch(1);
            string identifier = "USERID-00001-00001";
            identifier = Uri.EscapeUriString(identifier);
            client.AddResponse (204, null);
            Exception ex = null;
            KiiUser.ResetPassword(identifier, KiiUser.NotificationMethod.EMAIL, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(ex);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/users/" + identifier + "/password/request-reset", client.RequestUrl [0]);
            Assert.AreEqual("{\"notificationMethod\":\"EMAIL\"}", client.RequestBody[0]);
            Assert.AreEqual("application/vnd.kii.ResetPasswordRequest+json", client.RequestHeader[0]["content-type"]);
        }
    }
}

