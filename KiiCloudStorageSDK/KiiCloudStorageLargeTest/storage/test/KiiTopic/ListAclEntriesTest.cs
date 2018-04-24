using System;
using System.Collections.Generic;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class ListAclEntriesTest : LargeTestBase
    {
        [SetUp()]
        public override void SetUp ()
        {
            base.SetUp ();
            string uname = "Test-" + CurrentTimeMillis ();
            AppUtil.CreateNewUser (uname, "password");
        }

        [TearDown()]
        public override void TearDown ()
        {
            AppUtil.DeleteUser (KiiUser.CurrentUser);
            base.TearDown ();
        }

        [Test()]
        public void Test_GroupScopeAsync ()
        {
            KiiGroup group = Kii.Group("test_group");
            group.Save();
            group.Topic("group_topic").Save();

            CountDownLatch cd = new CountDownLatch (1);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = null;
            Exception exception = null;

            KiiTopic topic = group.Topic("group_topic");
            topic.ListAclEntries((IList<KiiACLEntry<KiiTopic, TopicAction>> result, Exception e)=>{
                entries = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.AreEqual(4, entries.Count);
        }
        [Test()]
        public void Test_UserScopeAsync ()
        {
            KiiUser.CurrentUser.Topic("user_topic").Save();

            CountDownLatch cd = new CountDownLatch (1);
            IList<KiiACLEntry<KiiTopic, TopicAction>> entries = null;
            Exception exception = null;

            KiiTopic topic = KiiUser.CurrentUser.Topic("user_topic");
            topic.ListAclEntries((IList<KiiACLEntry<KiiTopic, TopicAction>> result, Exception e)=>{
                entries = result;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.AreEqual(2, entries.Count);
        }
        protected long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}

