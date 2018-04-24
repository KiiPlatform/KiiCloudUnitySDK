using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestKiiPushMessage
    {

        #region KiiPushMessage
        [Test(), KiiUTInfo(
            action = "When we create KiiPushMessageData and call ToJson()",
            expected = "We can get a JsonObject which contains set fields."
            )]
        public void Test_0001_KiiPushMessage_all_true()
        {
            KiiPushMessageData data = new KiiPushMessageData ();
            data.Put ("payload", "abc");
            KiiPushMessage msg = KiiPushMessage.BuildWith (data)
                    .EnableAPNS(true)
                    .EnableGCM(true)
                    .EnableMqtt(true)
                    .SendAppID(true)
                    .SendObjectScope(true)
                    .SendOrigin(true)
                    .SendSender(true)
                    .SendToDevelopment(true)
                    .SendTopicId(true)
                    .SendToProduction(true)
                    .SendWhen(true)
                    .Build ();
            JsonObject json = msg.ToJson ();
            Assert.AreEqual (true, json.Get ("sendAppID"));
            Assert.AreEqual (true, json.Get ("sendObjectScope"));
            Assert.AreEqual (true, json.Get ("sendOrigin"));
            Assert.AreEqual (true, json.Get ("sendSender"));
            Assert.AreEqual (true, json.Get ("sendToDevelopment"));
            Assert.AreEqual (true, json.Get ("sendTopicID"));
            Assert.AreEqual (true, json.Get ("sendToProduction"));
            Assert.AreEqual (true, json.Get ("sendWhen"));
            Assert.AreEqual (true, json.GetJsonObject("gcm").Get ("enabled"));
            Assert.AreEqual (true, json.GetJsonObject("apns").Get ("enabled"));
            Assert.AreEqual (true, json.GetJsonObject("mqtt").Get ("enabled"));
            Assert.AreEqual ("abc", json.GetJsonObject("data").Get ("payload"));
        }
        [Test(), KiiUTInfo(
            action = "When we create KiiPushMessageData and call ToJson()",
            expected = "We can get a JsonObject which contains set fields."
            )]
        public void Test_0002_KiiPushMessage_all_false()
        {
            KiiPushMessageData data = new KiiPushMessageData ();
            data.Put ("payload", "abc");
            KiiPushMessage msg = KiiPushMessage.BuildWith (data)
                    .EnableAPNS(false)
                    .EnableGCM(false)
                    .EnableMqtt(false)
                    .SendAppID(false)
                    .SendObjectScope(false)
                    .SendOrigin(false)
                    .SendSender(false)
                    .SendToDevelopment(false)
                    .SendTopicId(false)
                    .SendToProduction(false)
                    .SendWhen(false)
                    .Build ();
            JsonObject json = msg.ToJson ();
            Assert.AreEqual (false, json.Get ("sendAppID"));
            Assert.AreEqual (false, json.Get ("sendObjectScope"));
            Assert.AreEqual (false, json.Get ("sendOrigin"));
            Assert.AreEqual (false, json.Get ("sendSender"));
            Assert.AreEqual (false, json.Get ("sendToDevelopment"));
            Assert.AreEqual (false, json.Get ("sendTopicID"));
            Assert.AreEqual (false, json.Get ("sendToProduction"));
            Assert.AreEqual (false, json.Get ("sendWhen"));
            Assert.AreEqual (false, json.GetJsonObject("gcm").Get ("enabled"));
            Assert.AreEqual (false, json.GetJsonObject("apns").Get ("enabled"));
            Assert.AreEqual (false, json.GetJsonObject("mqtt").Get ("enabled"));
            Assert.AreEqual ("abc", json.GetJsonObject("data").Get ("payload"));
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call KiiPushMessage.Builder.WithPushMessageType() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullPushMessageType()
        {
            KiiPushMessageData data = new KiiPushMessageData ();
            data.Put ("payload", "abc");
            KiiPushMessage.BuildWith (data).WithPushMessageType (null);
        }
        #endregion
        
        #region APNSMessage
        [Test(), KiiUTInfo(
            action = "When we create APNSMessage and call ToJson()",
            expected = "We can get a JsonObject which contains set fields."
            )]
        public void Test_0003_APNSMessage()
        {
            APNSData data = new APNSData ();
            data.Put ("payload", "abc");
            APNSMessage apns = APNSMessage.CreateBuilder ()
                    .WithAPNSData (data)
                    .WithAlertActionLocKey("ActionLocKey")
                    .WithAlertBody("Body")
                    .WithAlertLaunchImage("LaunchImage")
                    .WithAlertLocArgs(new string[] {"Args1", "Args2"})
                    .WithAlertLocKey("LocKey")
                    .WithAlertTitle("title")
                    .WithAlertSubtitle("subtitle")
                    .WithBadge(3)
                    .WithContentAvailable(1)
                    .WithSound("Sound")
                    .WithMutableContent(1)
                    .Build();
            JsonObject json = apns.ToJson ();
            Assert.AreEqual (true, json.Get ("enabled"));
            Assert.AreEqual (3, json.Get ("badge"));
            Assert.AreEqual (1, json.Get ("contentAvailable"));
            Assert.AreEqual (1, json.Get ("mutableContent"));
            Assert.AreEqual ("Sound", json.Get ("sound"));
            Assert.AreEqual ("abc", json.GetJsonObject("data").Get ("payload"));
            Assert.AreEqual ("ActionLocKey", json.GetJsonObject ("alert").Get ("actionLocKey"));
            Assert.AreEqual ("title", json.GetJsonObject ("alert").Get ("title"));
            Assert.AreEqual ("subtitle", json.GetJsonObject ("alert").Get ("subtitle"));
            Assert.AreEqual ("Body", json.GetJsonObject ("alert").Get ("body"));
            Assert.AreEqual ("LaunchImage", json.GetJsonObject ("alert").Get ("launchImage"));
            Assert.AreEqual ("Args1", json.GetJsonObject ("alert").GetJsonArray ("locArgs").Get (0));
            Assert.AreEqual ("Args2", json.GetJsonObject ("alert").GetJsonArray ("locArgs").Get (1));
            Assert.AreEqual ("LocKey", json.GetJsonObject ("alert").Get ("locKey"));
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call WithAPNSMessage() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_0004_BuildWithNullAPNSMessage()
        {
            KiiPushMessageData data = new KiiPushMessageData ();
            data.Put ("payload", "abc");
            KiiPushMessage.BuildWith (data).WithAPNSMessage (null);
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call APNSMessage.Builder.WithAlertActionLocKey() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullAlertActionLocKey()
        {
            APNSMessage.CreateBuilder ().WithAlertActionLocKey (null);
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call APNSMessage.Builder.WithAlertBody() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullAlertBody()
        {
            APNSMessage.CreateBuilder ().WithAlertBody (null);
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call APNSMessage.Builder.WithAlertLaunchImage() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullAlertLaunchImage()
        {
            APNSMessage.CreateBuilder ().WithAlertLaunchImage (null);
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call APNSMessage.Builder.WithAlertLocKey() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullAlertLocKey()
        {
            APNSMessage.CreateBuilder ().WithAlertLocKey (null);
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call APNSMessage.Builder.WithAPNSData() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullAPNSData()
        {
            APNSMessage.CreateBuilder ().WithAPNSData (null);
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call APNSMessage.Builder.WithSound() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullSound()
        {
            APNSMessage.CreateBuilder ().WithSound (null);
        }
        #endregion
        
        #region GCMMessage
        [Test(), KiiUTInfo(
            action = "When we create GCMMessage and call ToJson()",
            expected = "We can get a JsonObject which contains set fields."
            )]
        public void Test_0005_GCMMessage()
        {
            GCMData data = new GCMData ();
            data.Put ("payload", "abc");
            GCMMessage apns = GCMMessage.CreateBuilder ()
                    .WithGCMData(data)
                    .WithCollapseKey("CollapseKey")
                    .WithDelayWhileIdle(true)
                    .WithRestrictedPackageName("RestrictedPackageName")
                    .WithTimeToLive(4)
                    .Build();
            JsonObject json = apns.ToJson ();
            Assert.AreEqual (true, json.Get ("enabled"));
            Assert.AreEqual ("CollapseKey", json.Get ("collapseKey"));
            Assert.AreEqual (true, json.Get ("delayWhileIdle"));
            Assert.AreEqual (4, json.Get ("timeToLive"));
            Assert.AreEqual ("RestrictedPackageName", json.Get ("restrictedPackageName"));
            Assert.AreEqual ("abc", json.GetJsonObject("data").Get ("payload"));
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call WithGCMMessage() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_0006_BuildWithNullGCMMessage()
        {
            GCMData data = new GCMData ();
            data.Put ("payload", "abc");
            KiiPushMessage.BuildWith (data).WithGCMMessage (null);
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call GCMMessage.Builder.WithCollapseKey() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullCollapseKey()
        {
            GCMMessage.CreateBuilder ().WithCollapseKey (null);
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call GCMMessage.Builder.WithGCMData() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullGCMData()
        {
            GCMMessage.CreateBuilder ().WithGCMData (null);
        }
        [Test(),
         ExpectedException(typeof(ArgumentNullException)),
         KiiUTInfo(
            action = "When we call GCMMessage.Builder.WithRestrictedPackageName() with null parameter",
            expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullRestrictedPackageName()
        {
            GCMMessage.CreateBuilder ().WithRestrictedPackageName (null);
        }
        #endregion




        #region MqttMessage
        [Test(), KiiUTInfo(
            action = "When we create MqttMessage and call ToJson()",
            expected = "We can get a JsonObject which contains set fields."
        )]
        public void Test_MqttMessage()
        {
            MqttData data = new MqttData ();
            data.Put ("payload", "abc");
            MqttMessage apns = MqttMessage.CreateBuilder ()
                .WithMqttData(data)
                .Build();
            JsonObject json = apns.ToJson ();
            Assert.AreEqual (true, json.Get ("enabled"));
            Assert.AreEqual ("abc", json.GetJsonObject("data").Get ("payload"));
        }
        [Test(),
            ExpectedException(typeof(ArgumentNullException)),
            KiiUTInfo(
                action = "When we call WithMqttMessage() with null parameter",
                expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullMqttMessage()
        {
            MqttData data = new MqttData ();
            data.Put ("payload", "abc");
            KiiPushMessage.BuildWith (data).WithMqttMessage (null);
        }
        [Test(),
            ExpectedException(typeof(ArgumentNullException)),
            KiiUTInfo(
                action = "When we call MqttMessage.Builder.WithMqttData() with null parameter",
                expected = "ArgumentNullException must be thrown."
            )]
        public void Test_BuildWithNullMqttData()
        {
            MqttMessage.CreateBuilder ().WithMqttData (null);
        }
        #endregion

    }
}

