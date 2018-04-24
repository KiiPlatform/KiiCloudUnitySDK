using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Identity data.
    /// </summary>
    public class IdentityData
    {
        private string userName;
        private string email;
        private string phone;

        internal IdentityData(string userName, string email, string phone)
        {
            this.userName = userName;
            this.email = email;
            this.phone = phone;
        }
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get
            {
                return this.userName;
            }
        }
        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get
            {
                return this.email;
            }
        }
        /// <summary>
        /// Gets the phone.
        /// </summary>
        /// <value>The phone.</value>
        public string Phone
        {
            get
            {
                return this.phone;
            }
        }

        /// <summary>
        /// Identity data builder.
        /// </summary>
        /// <remarks></remarks>
        public class Builder
        {
            /// <summary>
            /// Create Builder with user name.
            /// </summary>
            /// <returns>IdentityDataBuilder instance.</returns>
            /// <param name="userName">User name.</param>
            /// <remarks></remarks>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid username.
            /// </exception>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null or empty.
            /// </exception>
            public static Builder CreateWithName(string userName)
            {
                return new Builder().WithName(userName);
            }
            /// <summary>
            /// Create Builder with email address.
            /// </summary>
            /// <returns>IdentityDataBuilder instance.</returns>
            /// <param name="email">Email.</param>
            /// <remarks></remarks>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid email address.
            /// </exception>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null or empty.
            /// </exception>
            public static Builder CreateWithEmail(string email)
            {
                return new Builder().WithEmail(email);
            }
            /// <summary>
            /// Create Builder with phone number.
            /// </summary>
            /// <returns>IdentityDataBuilder instance.</returns>
            /// <param name="phone">Phone.</param>
            /// <remarks></remarks>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid phone number.
            /// </exception>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null or empty.
            /// </exception>
            public static Builder CreateWithPhone(string phone)
            {
                return new Builder().WithPhone(phone);
            }

            private string phone;
            private string email;
            private string userName;

            private Builder()
            {
                this.phone = null;
                this.email = null;
                this.userName = null;
            }
            /// <summary>
            /// Set phone number.
            /// </summary>
            /// <remarks>
            /// Phone must be valid.
            /// When called multiple times, it will update existing phone number.
            /// </remarks>
            /// <returns>
            /// Itself.
            /// </returns>
            /// <param name='phone'>
            /// Phone.
            /// </param>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid phone number.
            /// </exception>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null or empty.
            /// </exception>
            public Builder WithPhone(string phone)
            {
                if (Utils.IsEmpty(phone))
                {
                    throw new ArgumentNullException("phone must not be empty.");
                }
                if (!KiiUser.IsValidPhone(phone))
                {
                    throw new ArgumentException(ErrorInfo.KIIUSER_PHONE_FORMAT_INVALID + phone);
                }
                this.phone = phone;
                return this;
            }
            /// <summary>
            /// Set email address.
            /// </summary>
            /// <remarks>
            /// Email must be valid.
            /// When called multiple times, it will update existing email address.
            /// </remarks>
            /// <returns>
            /// Itself
            /// </returns>
            /// <param name='email'>
            /// Email.
            /// </param>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid email address.
            /// </exception>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null or empty.
            /// </exception>
            public Builder WithEmail(string email)
            {
                if (Utils.IsEmpty(email))
                {
                    throw new ArgumentNullException("email must not be empty.");
                }
                if (!KiiUser.IsValidEmail(email))
                {
                    throw new ArgumentException(ErrorInfo.KIIUSER_EMAIL_FORMAT_INVALID + email);
                }
                this.email = email;
                return this;
            }
            /// <summary>
            /// Set username.
            /// </summary>
            /// <remarks>
            /// Username must be valid.
            /// When called multiple times, it will update existing user name.
            /// </remarks>
            /// <returns>
            /// Itself
            /// </returns>
            /// <param name='userName'>
            /// Username.
            /// </param>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid username.
            /// </exception>
            /// <exception cref='ArgumentNullException'>
            /// Is thrown when an argument is null or empty.
            /// </exception>
            public Builder WithName(string userName)
            {
                if (Utils.IsEmpty(userName))
                {
                    throw new ArgumentNullException("userName must not be empty.");
                }
                if (!KiiUser.IsValidUserName(userName))
                {
                    throw new ArgumentException(ErrorInfo.KIIUSER_USERNAME_INVALID + userName);
                }
                this.userName = userName;
                return this;
            }

            /// <summary>
            /// Build IdentityData instance.
            /// </summary>
            /// <remarks>Build IdentityData instance with username, email and phone if they're set.</remarks>
            /// <returns>
            /// IdentityData instance.
            /// </returns>
            public IdentityData Build()
            {
                IdentityData identityData = new IdentityData(this.userName, this.email, this.phone);
                return identityData;
            }
        }
    }
}

