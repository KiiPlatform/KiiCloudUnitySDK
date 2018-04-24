using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class ListTopicsTest : LargeTestBase
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
        public void Test_AppScope ()
        {
            // app 'd92535a1' has over 50 app scope topics.
            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            Kii.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsTrue(topics.HasNext);
            Assert.IsNotNull(topics.PaginationKey);
            Assert.AreEqual(50, topics.Result.Count);

            Kii.ListTopics(topics.PaginationKey, (KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsTrue(topics.Result.Count > 0);
        }
        [Test()]
        public void Test_GroupScope ()
        {
            KiiGroup group = Kii.Group("test_group");
            group.Save();
            for (int i = 0; i < 51; i++)
            {
                group.Topic("user_topic" + i).Save();
            }

            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            group.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsTrue(topics.HasNext);
            Assert.IsNotNull(topics.PaginationKey);
            Assert.AreEqual(50, topics.Result.Count);

            string paginationKey = topics.PaginationKey;
            cd = new CountDownLatch (1);
            topics = null;
            exception = null;

            group.ListTopics(paginationKey, (KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(1, topics.Result.Count);
        }
        [Test()]
        public void Test_UserScope ()
        {
            for (int i = 0; i < 51; i++)
            {
                KiiUser.CurrentUser.Topic("user_topic" + i).Save();
            }

            CountDownLatch cd = new CountDownLatch (1);
            KiiListResult<KiiTopic> topics = null;
            Exception exception = null;

            KiiUser.CurrentUser.ListTopics((KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsTrue(topics.HasNext);
            Assert.IsNotNull(topics.PaginationKey);
            Assert.AreEqual(50, topics.Result.Count);

            string paginationKey = topics.PaginationKey;
            cd = new CountDownLatch (1);
            topics = null;
            exception = null;

            KiiUser.CurrentUser.ListTopics(paginationKey, (KiiListResult<KiiTopic> t, Exception e)=>{
                topics = t;
                exception = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }

            Assert.IsNull(exception);
            Assert.IsFalse(topics.HasNext);
            Assert.IsNull(topics.PaginationKey);
            Assert.AreEqual(1, topics.Result.Count);
        }
        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}

