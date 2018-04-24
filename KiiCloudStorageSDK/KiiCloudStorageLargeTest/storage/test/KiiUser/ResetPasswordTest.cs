using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    [TestFixture ()]
    public class ResetPasswordTest : LargeTestBase
    {
        private string phone = "+8718013033339"; // TODO: Input your real global phone number
        private string email = "unity_test@kii.com"; // TODO: Input your real email address
        private KiiUser user;
        [SetUp()]
        public override void SetUp ()
        {
            base.SetUp ();
            this.user = KiiUser.BuilderWithName("real-test-user").WithEmail(this.email).WithPhone(this.phone).Build();
            this.user.Register("password");
        }
        [TearDown()]
        public override void TearDown ()
        {
            if (this.user != null)
            {
                user.Delete();
            }
            base.TearDown ();
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with email and NotificationMethod.SMS,",
            expected = "We should receive the password reset notification via SMS.")]
        public void ResetPasswordViaSmsTest ()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;
            KiiUser.ResetPassword(this.email, KiiUser.NotificationMethod.SMS, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(ex);
        }
        [Test(), KiiUTInfo(
            action = "When we call ResetPassword() with phone number and NotificationMethod.EMAIL,",
            expected = "We should receive the password reset notification via email.")]
        public void ResetPasswordViaEmailTest ()
        {
            CountDownLatch cd = new CountDownLatch(1);
            Exception ex = null;
            KiiUser.ResetPassword(this.phone, KiiUser.NotificationMethod.EMAIL, (Exception e)=>{
                ex = e;
                cd.Signal();
            });
            if (!cd.Wait(new TimeSpan(0, 0, 0, 3)))
            {
                Assert.Fail("Callback not fired.");
            }
            Assert.IsNull(ex);
        }
    }
}

