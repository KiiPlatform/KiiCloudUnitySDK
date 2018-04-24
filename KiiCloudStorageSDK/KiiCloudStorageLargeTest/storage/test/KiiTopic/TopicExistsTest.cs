using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class TopicExistsTest : LargeTestBase
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
            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;

            Kii.ListTopics((KiiListResult<KiiTopic> t, Exception e1)=>{
                if (e1 != null)
                {
                    exception = e1;
                    cd.Signal();
                    return;
                }
                if (t.Result.Count == 0)
                {
                    exception = new Exception("There are no app scope topic in App");
                    cd.Signal();
                    return;
                }
                foreach (KiiTopic topic in t.Result)
                {
                    topic.Exists((bool? result, Exception e2)=>{
                        exception = e2;
                        existence = result;
                        cd.Signal();
                        return;
                    });
                }
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 60)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsTrue(existence.Value);
        }
        [Test()]
        public void Test_GroupScope ()
        {
            KiiGroup group = Kii.Group("test_group");
            group.Save();
            group.Topic("group_topic").Save();

            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;

            group.Topic("group_topic").Exists((bool? result, Exception e)=>{
                exception = e;
                existence = result;
                cd.Signal();
                return;
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsTrue(existence.Value);
        }
        [Test()]
        public void Test_UserScope ()
        {
            KiiUser.CurrentUser.Topic("user_topic").Save();

            CountDownLatch cd = new CountDownLatch (1);
            bool? existence = null;
            Exception exception = null;

            KiiUser.CurrentUser.Topic("user_topic").Exists((bool? result, Exception e)=>{
                exception = e;
                existence = result;
                cd.Signal();
                return;
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 20)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(exception);
            Assert.IsTrue(existence.Value);
        }
        protected long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}

