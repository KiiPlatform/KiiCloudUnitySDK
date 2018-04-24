using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiPushInstallation
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
        private void LogIn()
        {
            // set Response
            client.AddResponse(200, "{" +
                               "\"id\" : \"user1234\"," +
                               "\"access_token\" : \"cdef\"," +
                               "\"expires_in\" : 9223372036854775}");
            KiiUser.LogIn("kii1234", "pass1234");
        }

        #region Install
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) with Android params",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0001_Install_Android()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (false).Install ("deviceID1", KiiPushInstallation.DeviceType.ANDROID);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations", client.RequestUrl [0]);
            Assert.AreEqual ("{\"installationRegistrationID\":\"deviceID1\",\"deviceType\":\"ANDROID\"}", client.RequestBody[0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) with Android params and development",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0002_Install_Android_Development()
        {
            this.LogIn();
            ClearClientRequest ();

            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Install ("deviceID2", KiiPushInstallation.DeviceType.ANDROID);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations", client.RequestUrl [0]);
            Assert.AreEqual ("{\"installationRegistrationID\":\"deviceID2\",\"deviceType\":\"ANDROID\",\"development\":true}", client.RequestBody[0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) with Android params",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0003_Install_iOS()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (false).Install ("deviceID3", KiiPushInstallation.DeviceType.IOS);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations", client.RequestUrl [0]);
            Assert.AreEqual ("{\"installationRegistrationID\":\"deviceID3\",\"deviceType\":\"IOS\"}", client.RequestBody[0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) with Android params and development",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0004_Install_iOS_Development()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Install ("deviceID4", KiiPushInstallation.DeviceType.IOS);
            Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations", client.RequestUrl [0]);
            Assert.AreEqual ("{\"installationRegistrationID\":\"deviceID4\",\"deviceType\":\"IOS\",\"development\":true}", client.RequestBody[0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) by async with Android params",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0005_Install_Async_Android()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (false).Install ("deviceID1", KiiPushInstallation.DeviceType.ANDROID, (Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations", client.RequestUrl [0]);
                Assert.AreEqual ("{\"installationRegistrationID\":\"deviceID1\",\"deviceType\":\"ANDROID\"}", client.RequestBody[0]);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) by async with Android params and development",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0006_Install_Async_Android_Development()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Install ("deviceID2", KiiPushInstallation.DeviceType.ANDROID, (Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations", client.RequestUrl [0]);
                Assert.AreEqual ("{\"installationRegistrationID\":\"deviceID2\",\"deviceType\":\"ANDROID\",\"development\":true}", client.RequestBody[0]);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) by async with Android params",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0007_Install_Async_iOS()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (false).Install ("deviceID3", KiiPushInstallation.DeviceType.IOS, (Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations", client.RequestUrl [0]);
                Assert.AreEqual ("{\"installationRegistrationID\":\"deviceID3\",\"deviceType\":\"IOS\"}", client.RequestBody[0]);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) by async with Android params and development",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0008_Install_Async_iOS_Development()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Install ("deviceID4", KiiPushInstallation.DeviceType.IOS, (Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.POST, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations", client.RequestUrl [0]);
                Assert.AreEqual ("{\"installationRegistrationID\":\"deviceID4\",\"deviceType\":\"IOS\",\"development\":true}", client.RequestBody[0]);
            });
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) with null device",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0009_Install_With_Null_Device()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Install (null, KiiPushInstallation.DeviceType.IOS);
        }
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) by async with null device",
            expected = "ArgumentException must be passed to callback."
            )]
        public void Test_0010_Install_With_Null_Device_Async()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Install (null, KiiPushInstallation.DeviceType.IOS, (Exception e)=>{
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentException), e);
            });
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) with empty device",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_Install_With_Empty_Device()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Install ("", KiiPushInstallation.DeviceType.IOS);
        }
        [Test(), KiiUTInfo(
            action = "When we call Install(deviceId, deviceType) by async with empty device",
            expected = "ArgumentException must be passed to callback."
            )]
        public void Test_Install_With_Empty_Device_Async()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Install ("", KiiPushInstallation.DeviceType.IOS, (Exception e)=>{
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentException), e);
            });
        }
        #endregion

        #region Uninstall
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) with Android params",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0011_Uninstall_Android()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (false).Uninstall ("deviceID1", KiiPushInstallation.DeviceType.ANDROID);
            Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations/ANDROID:deviceID1", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) with Android params and development",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0012_Uninstall_Android_Development()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Uninstall ("deviceID2", KiiPushInstallation.DeviceType.ANDROID);
            Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations/ANDROID:deviceID2", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) with Android params",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0013_Uninstall_iOS()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (false).Uninstall ("deviceID3", KiiPushInstallation.DeviceType.IOS);
            Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations/IOS:deviceID3", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) with Android params and development",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0014_Uninstall_iOS_Development()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Uninstall ("deviceID4", KiiPushInstallation.DeviceType.IOS);
            Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
            Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations/IOS:deviceID4", client.RequestUrl [0]);
        }
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) by async with Android params",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0015_Uninstall_Async_Android()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (false).Uninstall ("deviceID1", KiiPushInstallation.DeviceType.ANDROID, (Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations/ANDROID:deviceID1", client.RequestUrl [0]);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) by async with Android params and development",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0016_Uninstall_Async_Android_Development()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Uninstall ("deviceID2", KiiPushInstallation.DeviceType.ANDROID, (Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations/ANDROID:deviceID2", client.RequestUrl [0]);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) by async with Android params",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0017_Uninstall_Async_iOS()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (false).Uninstall ("deviceID3", KiiPushInstallation.DeviceType.IOS, (Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations/IOS:deviceID3", client.RequestUrl [0]);
            });
        }
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) by async with Android params and development",
            expected = "Api request is sent to KiiColud."
            )]
        public void Test_0018_Uninstall_Async_iOS_Development()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Uninstall ("deviceID4", KiiPushInstallation.DeviceType.IOS, (Exception e)=>{
                Assert.AreEqual (KiiHttpMethod.DELETE, client.RequestMethod [0]);
                Assert.AreEqual ("https://api.kii.com/api/apps/appId/installations/IOS:deviceID4", client.RequestUrl [0]);
            });
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) with null device",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_0019_Uninstall_With_Null_Device()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Uninstall (null, KiiPushInstallation.DeviceType.IOS);
        }
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) by async with null device",
            expected = "ArgumentException must be passed to callback."
            )]
        public void Test_0020_Uninstall_With_Null_Device_Async()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Uninstall (null, KiiPushInstallation.DeviceType.IOS, (Exception e)=>{
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentException), e);
            });
        }
        [Test(),
         ExpectedException(typeof(ArgumentException)),
         KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) with empty device",
            expected = "ArgumentException must be thrown."
            )]
        public void Test_Uninstall_With_Empty_Device()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Uninstall ("", KiiPushInstallation.DeviceType.IOS);
        }
        [Test(), KiiUTInfo(
            action = "When we call Uninstall(deviceId, deviceType) by async with empty device",
            expected = "ArgumentException must be passed to callback."
            )]
        public void Test_Uninstall_With_Empty_Device_Async()
        {
            this.LogIn();
            ClearClientRequest ();
            
            client.AddResponse (200, null);
            KiiUser.PushInstallation (true).Uninstall ("", KiiPushInstallation.DeviceType.IOS, (Exception e)=>{
                Assert.IsNotNull(e);
                Assert.IsInstanceOfType(typeof(ArgumentException), e);
            });
        }
        #endregion

        private void ClearClientRequest()
        {
            client.RequestUrl.Clear ();
            client.RequestHeader.Clear ();
            client.RequestBody.Clear ();
            client.RequestMethod.Clear ();
        }
    }
}

