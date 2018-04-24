using System;
using NUnit.Framework;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Large Test for Reset Password with PIN Code in receipt SMS
    /// You need to execute the this test manually step by step.
    /// 
    /// 1. Prepares real device that can be received a SMS.
    /// 2. Sets a phone number to realPhoneNumber.
    /// 3. Runs Step01_ResetPasswordViaSmsPin test.
    /// 4. Your device will be received a PIN code via SMS.
    /// 5. Sets a PIN code to realPinCode.
    /// 6. Runs Step02_ResetPasswordViaSmsPin test.
    /// 7. Your device will be received a new password via SMS.
    /// 8. Sets a received password to realNewPassword.
    /// 9. Runs Step03_Cleanup test.
    /// </summary>
    [TestFixture ()]
    public class CompleteRestPasswordTest : LargeTestBase
    {
        private string realPhoneNumber = "+819025514236"; // Step1: set real your phone number
        private string realPinCode = "890272"; // Step2: set received pinCode after Step1
        private string realNewPassword = "AKsjff"; // Step3 set received new password after Stef2
        private KiiUser user;

        [TearDown]
        public override void TearDown ()
        {
        }

        [Test()]
        [Ignore("")]
        public void Step01_ResetPasswordViaSmsPin()
        {
            this.user = KiiUser.BuilderWithName("user" + CurrentTimeMillis()).SetGlobalPhone(this.realPhoneNumber).Build();
            this.user.Register("password");
            KiiUser.ResetPassword(this.realPhoneNumber, KiiUser.NotificationMethod.SMS_PIN);
        }
        [Test()]
        [Ignore("")]
        public void Step02_ResetPasswordViaSmsPin()
        {
            KiiUser user = KiiUser.LogIn(this.realPhoneNumber, "password");
            KiiUser.CompleteResetPassword(user.ID, this.realPinCode, null);
        }
        [Test()]
        [Ignore("")]
        public void Step03_Cleanup()
        {
            KiiUser user = KiiUser.LogIn(this.realPhoneNumber, this.realNewPassword);
            user.Delete();
        }
        private long CurrentTimeMillis ()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}

