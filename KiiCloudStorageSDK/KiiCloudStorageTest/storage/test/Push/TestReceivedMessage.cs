using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using KiiCorp.Cloud.Storage;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture()]
    public class TestReceivedMessage
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

        #region Parse
        [Test(), KiiUTInfo(
            action = "When we call Parse(json) with formatted by GCM's PushToAppMessage",
            expected = "We can get a PushToAppMessage."
            )]
        public void Test_0001_Parse_GCM_PushToApp()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"origin\" : \"EVENT\",");
            json.AppendLine ("\"sender\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeType\" : \"APP\",");
            json.AppendLine ("\"objectScopeGroupID\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeUserID\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"bucketID\" : \"BUCKET-000001-AAAAAA\",");
            json.AppendLine ("\"objectID\" : \"OBJECT-000001-AAAAAA\",");
            json.AppendLine ("\"bucketType\" : \"SYNC\"");
            json.AppendLine ("}");

            PushToAppMessage message = (PushToAppMessage)ReceivedMessage.Parse (json.ToString ());

            Assert.AreEqual ("abc", message.GetString("string-property"));
            Assert.AreEqual (10, message.GetInt("int-property"));
            Assert.AreEqual (true, message.GetBoolean("bool-property"));
            Assert.AreEqual (10.5, message.GetDouble("double-property"));
            Assert.AreEqual (ReceivedMessage.MessageType.PUSH_TO_APP, message.PushMessageType);
            Assert.AreEqual ("kiicloud://users/USER-000001-AAAAAA", message.Sender.Uri.ToString());
            Assert.AreEqual (ReceivedMessage.Scope.APP, message.ObjectScope);
            Assert.AreEqual ("kiicloud://groups/GROUP-000001-AAAAAA", message.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual ("kiicloud://users/USER-000001-BBBBBB", message.ObjectScopeUser.Uri.ToString());
            Assert.AreEqual ("kiicloud://buckets/BUCKET-000001-AAAAAA", message.KiiBucket.Uri.ToString());
            Assert.AreEqual ("kiicloud://buckets/BUCKET-000001-AAAAAA/objects/OBJECT-000001-AAAAAA", message.KiiObject.Uri.ToString());
        }

        [Test(), KiiUTInfo(
            action = "When we call Parse(json) with formatted by GCM's PushToUserMessage",
            expected = "We can get a PushToUserMessage."
            )]
        public void Test_0002_Parse_GCM_PushToUser()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"sender\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeType\" : \"APP\",");
            json.AppendLine ("\"objectScopeGroupID\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeUserID\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"topic\" : \"my_topic\"");
            json.AppendLine ("}");
            
            PushToUserMessage message = (PushToUserMessage)ReceivedMessage.Parse (json.ToString ());

            Assert.AreEqual ("abc", message.GetString("string-property"));
            Assert.AreEqual (10, message.GetInt("int-property"));
            Assert.AreEqual (true, message.GetBoolean("bool-property"));
            Assert.AreEqual (10.5, message.GetDouble("double-property"));
            Assert.AreEqual (ReceivedMessage.MessageType.PUSH_TO_USER, message.PushMessageType);
            Assert.AreEqual ("kiicloud://users/USER-000001-AAAAAA", message.Sender.Uri.ToString());
            Assert.AreEqual (ReceivedMessage.Scope.APP, message.ObjectScope);
            Assert.AreEqual ("kiicloud://groups/GROUP-000001-AAAAAA", message.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual ("kiicloud://users/USER-000001-BBBBBB", message.ObjectScopeUser.Uri.ToString());
        }
        [Test(), KiiUTInfo(
            action = "When we call Parse(json) with formatted by GCM's DirectPushMessage",
            expected = "We can get a DirectPushMessage."
            )]
        public void Test_0003_Parse_GCM_DirectPush()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"sender\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeType\" : \"APP\"");
            json.AppendLine ("}");
            
            DirectPushMessage message = (DirectPushMessage)ReceivedMessage.Parse (json.ToString ());

            Assert.AreEqual ("abc", message.GetString("string-property"));
            Assert.AreEqual (10, message.GetInt("int-property"));
            Assert.AreEqual (true, message.GetBoolean("bool-property"));
            Assert.AreEqual (10.5, message.GetDouble("double-property"));
            Assert.AreEqual (ReceivedMessage.MessageType.DIRECT_PUSH, message.PushMessageType);
            Assert.AreEqual (ReceivedMessage.Scope.APP, message.ObjectScope);
        }
        [Test(), KiiUTInfo(
            action = "When we call Parse(json) with formatted by APNS's PushToAppMessage",
            expected = "We can get a PushToAppMessage."
            )]
        public void Test_0004_Parse_APNS_PushToApp()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"aps\" : {},");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"s\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"st\" : \"APP\",");
            json.AppendLine ("\"sg\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"su\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"bi\" : \"BUCKET-000001-AAAAAA\",");
            json.AppendLine ("\"oi\" : \"OBJECT-000001-AAAAAA\",");
            json.AppendLine ("\"bt\" : \"SYNC\"");
            json.AppendLine ("}");
            
            PushToAppMessage message = (PushToAppMessage)ReceivedMessage.Parse (json.ToString ());
            
            Assert.AreEqual ("abc", message.GetString("string-property"));
            Assert.AreEqual (10, message.GetInt("int-property"));
            Assert.AreEqual (true, message.GetBoolean("bool-property"));
            Assert.AreEqual (10.5, message.GetDouble("double-property"));
            Assert.AreEqual (ReceivedMessage.MessageType.PUSH_TO_APP, message.PushMessageType);
            Assert.AreEqual ("kiicloud://users/USER-000001-AAAAAA", message.Sender.Uri.ToString());
            Assert.AreEqual (ReceivedMessage.Scope.APP, message.ObjectScope);
            Assert.AreEqual ("kiicloud://groups/GROUP-000001-AAAAAA", message.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual ("kiicloud://users/USER-000001-BBBBBB", message.ObjectScopeUser.Uri.ToString());
            Assert.AreEqual ("kiicloud://buckets/BUCKET-000001-AAAAAA", message.KiiBucket.Uri.ToString());
            Assert.AreEqual ("kiicloud://buckets/BUCKET-000001-AAAAAA/objects/OBJECT-000001-AAAAAA", message.KiiObject.Uri.ToString());
        }
        [Test(), KiiUTInfo(
            action = "When we call Parse(json) with formatted by APNS's PushToUserMessage",
            expected = "We can get a PushToUserMessage."
            )]
        public void Test_0005_Parse_APNS_PushToUser()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"aps\" : {},");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"s\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"st\" : \"APP\",");
            json.AppendLine ("\"sg\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"su\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"to\" : \"my_topic\"");
            json.AppendLine ("}");
            
            PushToUserMessage message = (PushToUserMessage)ReceivedMessage.Parse (json.ToString ());
            
            Assert.AreEqual ("abc", message.GetString("string-property"));
            Assert.AreEqual (10, message.GetInt("int-property"));
            Assert.AreEqual (true, message.GetBoolean("bool-property"));
            Assert.AreEqual (10.5, message.GetDouble("double-property"));
            Assert.AreEqual (ReceivedMessage.MessageType.PUSH_TO_USER, message.PushMessageType);
            Assert.AreEqual ("kiicloud://users/USER-000001-AAAAAA", message.Sender.Uri.ToString());
            Assert.AreEqual (ReceivedMessage.Scope.APP, message.ObjectScope);
            Assert.AreEqual ("kiicloud://groups/GROUP-000001-AAAAAA", message.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual ("kiicloud://users/USER-000001-BBBBBB", message.ObjectScopeUser.Uri.ToString());
        }
        [Test(), KiiUTInfo(
            action = "When we call Parse(json) with formatted by APNS's DirectPushMessage",
            expected = "We can get a DirectPushMessage."
            )]
        public void Test_0006_Parse_APNS_DirectPush()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"aps\" : {},");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"s\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"st\" : \"APP\"");
            json.AppendLine ("}");
            
            DirectPushMessage message = (DirectPushMessage)ReceivedMessage.Parse (json.ToString ());
            
            Assert.AreEqual ("abc", message.GetString("string-property"));
            Assert.AreEqual (10, message.GetInt("int-property"));
            Assert.AreEqual (true, message.GetBoolean("bool-property"));
            Assert.AreEqual (10.5, message.GetDouble("double-property"));
            Assert.AreEqual (ReceivedMessage.MessageType.DIRECT_PUSH, message.PushMessageType);
            Assert.AreEqual (ReceivedMessage.Scope.APP, message.ObjectScope);
        }
        [Test(),
         KiiUTInfo(
            action = "When we call Parse(json) with formatted by GCM's PushToAppMessage witout bucketType",
            expected = "Pasred as PUSH_TO_APP message"
            )]
        public void Test_0007_Parse_GCM_PushToApp_Without_bucketType()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"origin\" : \"EVENT\",");
            json.AppendLine ("\"sender\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeType\" : \"APP\",");
            json.AppendLine ("\"objectScopeGroupID\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeUserID\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"bucketID\" : \"BUCKET-000001-AAAAAA\",");
            json.AppendLine ("\"objectID\" : \"OBJECT-000001-AAAAAA\"");
            json.AppendLine ("}");

            PushToAppMessage msg = (PushToAppMessage)ReceivedMessage.Parse(json.ToString ());
            Assert.IsTrue(msg.ContainsKiiBucket());
            Assert.IsTrue(msg.ContainsKiiObject());
            Assert.AreEqual("kiicloud://buckets/BUCKET-000001-AAAAAA/objects/OBJECT-000001-AAAAAA", msg.KiiObject.Uri.ToString());
            Assert.AreEqual("BUCKET-000001-AAAAAA", msg.KiiBucket.Name);
            Assert.AreEqual("kiicloud://groups/GROUP-000001-AAAAAA", msg.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual("kiicloud://users/USER-000001-BBBBBB", msg.ObjectScopeUser.Uri.ToString());
            Assert.AreEqual(ReceivedMessage.MessageType.PUSH_TO_APP, msg.PushMessageType);
        }
        [Test(),
         KiiUTInfo(
            action = "When we call Parse(json) with formatted by GCM's PushToAppMessage witout bucketID",
            expected = "Parsed as PUSH_TO_APP message"
            )]
        public void Test_0008_Parse_GCM_PushToApp_Without_bucketID()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"origin\" : \"EVENT\",");
            json.AppendLine ("\"sender\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeType\" : \"APP\",");
            json.AppendLine ("\"objectScopeGroupID\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeUserID\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"objectID\" : \"OBJECT-000001-AAAAAA\",");
            json.AppendLine ("\"bucketType\" : \"SYNC\"");
            json.AppendLine ("}");

            PushToAppMessage msg = (PushToAppMessage)ReceivedMessage.Parse(json.ToString());
            Assert.IsFalse(msg.ContainsKiiBucket());
            Assert.IsFalse(msg.ContainsKiiObject());
            Assert.IsNull(msg.KiiObject);
            Assert.IsNull(msg.KiiBucket);
            Assert.AreEqual("kiicloud://groups/GROUP-000001-AAAAAA", msg.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual("kiicloud://users/USER-000001-BBBBBB", msg.ObjectScopeUser.Uri.ToString());
            Assert.AreEqual(ReceivedMessage.MessageType.PUSH_TO_APP, msg.PushMessageType);
        }
        [Test(),
         KiiUTInfo(
            action = "When we call Parse(json) with formatted by GCM's PushToAppMessage witout sender",
            expected = "Parsed as PUSH_TO_APP message."
            )]
        public void Test_0009_Parse_GCM_PushToApp_Without_sender()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"origin\" : \"EVENT\",");
            json.AppendLine ("\"objectScopeType\" : \"APP\",");
            json.AppendLine ("\"objectScopeGroupID\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"objectScopeUserID\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"bucketID\" : \"BUCKET-000001-AAAAAA\",");
            json.AppendLine ("\"objectID\" : \"OBJECT-000001-AAAAAA\",");
            json.AppendLine ("\"bucketType\" : \"SYNC\"");
            json.AppendLine ("}");
            
            PushToAppMessage msg = (PushToAppMessage)ReceivedMessage.Parse(json.ToString());
            Assert.IsTrue(msg.ContainsKiiBucket());
            Assert.IsTrue(msg.ContainsKiiObject());
            Assert.AreEqual("kiicloud://buckets/BUCKET-000001-AAAAAA/objects/OBJECT-000001-AAAAAA", msg.KiiObject.Uri.ToString());
            Assert.AreEqual("BUCKET-000001-AAAAAA", msg.KiiBucket.Name);
            Assert.AreEqual("kiicloud://groups/GROUP-000001-AAAAAA", msg.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual("kiicloud://users/USER-000001-BBBBBB", msg.ObjectScopeUser.Uri.ToString());
            Assert.AreEqual(ReceivedMessage.MessageType.PUSH_TO_APP, msg.PushMessageType);
        }

        [Test(),
         KiiUTInfo(
            action = "When we call Parse(json) with formatted by APNS's PushToAppMessage witout bucketType",
            expected = "Parsed As PUSH TO APP Message"
            )]
        public void Test_0010_Parse_APNS_PushToApp_Without_bucketType()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"aps\" : {},");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"s\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"st\" : \"APP\",");
            json.AppendLine ("\"sg\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"su\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"bi\" : \"BUCKET-000001-AAAAAA\",");
            json.AppendLine ("\"oi\" : \"OBJECT-000001-AAAAAA\"");
            json.AppendLine ("}");

            PushToAppMessage msg = (PushToAppMessage)ReceivedMessage.Parse (json.ToString ());
            Assert.IsTrue(msg.ContainsKiiBucket());
            Assert.IsTrue(msg.ContainsKiiObject());
            Assert.AreEqual("kiicloud://buckets/BUCKET-000001-AAAAAA/objects/OBJECT-000001-AAAAAA", msg.KiiObject.Uri.ToString());
            Assert.AreEqual("BUCKET-000001-AAAAAA", msg.KiiBucket.Name);
            Assert.AreEqual("kiicloud://groups/GROUP-000001-AAAAAA", msg.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual("kiicloud://users/USER-000001-BBBBBB", msg.ObjectScopeUser.Uri.ToString());
            Assert.AreEqual(ReceivedMessage.MessageType.PUSH_TO_APP, msg.PushMessageType);
        }
        [Test(),
         KiiUTInfo(
            action = "When we call Parse(json) with formatted by APNS's PushToAppMessage witout bucketID",
            expected = "Parsed as PUSH_TO_APP message"
            )]
        public void Test_0011_Parse_APNS_PushToApp_Without_bucketID()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"aps\" : {},");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"s\" : \"USER-000001-AAAAAA\",");
            json.AppendLine ("\"st\" : \"APP\",");
            json.AppendLine ("\"sg\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"su\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"oi\" : \"OBJECT-000001-AAAAAA\",");
            json.AppendLine ("\"bt\" : \"SYNC\"");
            json.AppendLine ("}");

            PushToAppMessage msg = (PushToAppMessage)ReceivedMessage.Parse(json.ToString());
            Assert.IsFalse(msg.ContainsKiiBucket());
            Assert.IsFalse(msg.ContainsKiiObject());
            Assert.IsNull(msg.KiiObject);
            Assert.IsNull(msg.KiiBucket);
            Assert.AreEqual("kiicloud://groups/GROUP-000001-AAAAAA", msg.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual("kiicloud://users/USER-000001-BBBBBB", msg.ObjectScopeUser.Uri.ToString());
            Assert.AreEqual(ReceivedMessage.MessageType.PUSH_TO_APP, msg.PushMessageType);
        }
        [Test(),
         KiiUTInfo(
            action = "When we call Parse(json) with formatted by APNS's PushToAppMessage witout sender",
            expected = "Parsed as PUSH_TO_APP message"
            )]
        public void Test_0012_Parse_APNS_PushToApp_Without_sender()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"aps\" : {},");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"st\" : \"APP\",");
            json.AppendLine ("\"sg\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"su\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"bi\" : \"BUCKET-000001-AAAAAA\",");
            json.AppendLine ("\"oi\" : \"OBJECT-000001-AAAAAA\",");
            json.AppendLine ("\"bt\" : \"SYNC\"");
            json.AppendLine ("}");
            
            PushToAppMessage msg = (PushToAppMessage)ReceivedMessage.Parse(json.ToString());
            Assert.IsTrue(msg.ContainsKiiBucket());
            Assert.IsTrue(msg.ContainsKiiObject());
            Assert.AreEqual("kiicloud://buckets/BUCKET-000001-AAAAAA/objects/OBJECT-000001-AAAAAA", msg.KiiObject.Uri.ToString());
            Assert.AreEqual("BUCKET-000001-AAAAAA", msg.KiiBucket.Name);
            Assert.AreEqual("kiicloud://groups/GROUP-000001-AAAAAA", msg.ObjectScopeGroup.Uri.ToString());
            Assert.AreEqual("kiicloud://users/USER-000001-BBBBBB", msg.ObjectScopeUser.Uri.ToString());
            Assert.AreEqual(ReceivedMessage.MessageType.PUSH_TO_APP, msg.PushMessageType);
        }
        #endregion        

        #region ToJson
        [Test(),
         KiiUTInfo(
            action = "When we call ToJson()",
            expected = "We can get a JsonObject which represents received message."
            )]
        public void Test_0013_ToJson()
        {
            LogIn ();
            StringBuilder json = new StringBuilder ();
            json.AppendLine ("{");
            json.AppendLine ("\"aps\" : {\"content-available\":1},");
            json.AppendLine ("\"string-property\" : \"abc\",");
            json.AppendLine ("\"int-property\" : 10,");
            json.AppendLine ("\"bool-property\" : true,");
            json.AppendLine ("\"double-property\" : 10.5,");
            json.AppendLine ("\"st\" : \"APP\",");
            json.AppendLine ("\"sg\" : \"GROUP-000001-AAAAAA\",");
            json.AppendLine ("\"su\" : \"USER-000001-BBBBBB\",");
            json.AppendLine ("\"bi\" : \"BUCKET-000001-AAAAAA\",");
            json.AppendLine ("\"oi\" : \"OBJECT-000001-AAAAAA\",");
            json.AppendLine ("\"bt\" : \"SYNC\"");
            json.AppendLine ("}");
            
            ReceivedMessage msg = ReceivedMessage.Parse (json.ToString ());
            Assert.AreEqual(1, msg.ToJson().GetJsonObject("aps").GetInt("content-available"));
        }
        #endregion
    }
}

