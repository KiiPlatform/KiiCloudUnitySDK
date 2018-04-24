using System;
using System.Collections.Generic;

using JsonOrg;
using KiiCorp.Cloud.Storage.Connector;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Provides APIs for user management features.
    /// </summary>
    /// <remarks>
    /// Represent User on KiiCloud. You can define some fields(key-value storage).
    /// Reserved keys are the following
    /// <list type="bullet">
    ///   <item><term>loginName</term></item>
    ///   <item><term>emailAddress</term></item>
    ///   <item><term>phoneNumber</term></item>
    ///   <item><term>displayName</term></item>
    ///   <item><term>country</term></item>
    ///   <item><term>userID</term></item>
    ///   <item><term>internalUserID</term></item>
    ///   <item><term>phoneNumberVerified</term></item>
    ///   <item><term>emailAddressVerified</term></item>
    /// </list>
    /// </remarks>
    public class KiiUser : KiiBaseObject, KiiScope, KiiSubject, IEquatable<KiiUser>
    {
        /// <summary>
        /// Notification method.
        /// </summary>
        public enum NotificationMethod {
            /// <summary>
            /// Sends email include link URL to reset the password.
            /// Reset password is done by clicking link in email.
            /// </summary>
            EMAIL,
            /// <summary>
            /// Sends SMS include link URL to reset the password.
            /// Reset password is done by clicking link in SMS.
            /// </summary>
            SMS,
            /// <summary>
            /// Sends SMS include PIN Code to registered phone number.
            /// Reset password is done by sending new Password and PIN code in received SMS.
            /// </summary>
            SMS_PIN
        }
        internal const string PROPERTY_USERNAME = "loginName";
        internal const string PROPERTY_EMAIL = "emailAddress";
        internal const string PROPERTY_PENDING_EMAIL = "_emailAddressPending";
        internal const string PROPERTY_PHONE = "phoneNumber";
        internal const string PROPERTY_PENDING_PHONE = "_phoneNumberPending";
        internal const string PROPERTY_DISPLAYNAME = "displayName";
        internal const string PROPERTY_COUNTRY = "country";
        internal const string PROPERTY_LOCALE = "locale";
        internal const string PROPERTY_ID = "userID";
        internal const string PROPERTY_PASSWORD = "password";
        internal const string PROPERTY_INTERNAL_USER_ID = "internalUserID";
        internal const string PROPERTY_PHONE_NUMBER_VERIFIED = "phoneNumberVerified";
        internal const string PROPERTY_EMAIL_ADDRESS_VERIFIED = "emailAddressVerified";
        internal const string PROPERTY_HAS_PASSWORD = "_hasPassword";
        internal const string PROPERTY_DISABLED = "_disabled";
        internal static Dictionary<string, object> ReservedKeys;
        private string mId;

        private DateTime mAccessTokenExpiresAt;
        private Dictionary<string, object> mSocialAccessTokenDictionary;
        static KiiUser()
        {
            ReservedKeys = new Dictionary<string, object>();
            ReservedKeys[PROPERTY_USERNAME] = true;
            ReservedKeys[PROPERTY_EMAIL] = true;
            ReservedKeys[PROPERTY_PENDING_EMAIL] = true;
            ReservedKeys[PROPERTY_PHONE] = true;
            ReservedKeys[PROPERTY_PENDING_PHONE] = true;
            ReservedKeys[PROPERTY_DISPLAYNAME] = true;
            ReservedKeys[PROPERTY_COUNTRY] = true;
            ReservedKeys[PROPERTY_LOCALE] = true;
            ReservedKeys[PROPERTY_ID] = true;
            ReservedKeys[PROPERTY_PASSWORD] = true;
            ReservedKeys[PROPERTY_INTERNAL_USER_ID] = true;
            ReservedKeys[PROPERTY_PHONE_NUMBER_VERIFIED] = true;
            ReservedKeys[PROPERTY_EMAIL_ADDRESS_VERIFIED] = true;
            ReservedKeys[PROPERTY_HAS_PASSWORD] = true;
            ReservedKeys[PROPERTY_DISABLED] = true;
        }

        private KiiUser() : base(ReservedKeys)
        {
        }

        /// <summary>
        /// Determines whether an argument is valid user name.
        /// </summary>
        /// <remarks>Valid username is
        /// <list type="bullet">
        ///  <item><term>Not null.</term></item>
        ///  <item><term>matches [a-zA-Z0-9-_\\.]{3,64}$ </term></item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// <c>true</c> if an argument is valid user name ; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='username'>
        /// username you want to check
        /// </param>
        public static bool IsValidUserName(string username)
        {
            return !Utils.IsEmpty(username) && Utils.ValidateUsername(username);
        }

        /// <summary>
        /// Determines whether an argument is valid email.
        /// </summary>
        /// <remarks>Valid email is
        /// <list type="bullet">
        ///   <item><term>Not null</term></item>
        ///   <item><term>Not empty</term></item>
        ///   <item><term>matches ^[a-zA-Z0-9\\+\\.\\_\\%\\-\\+]{1,256}\\@[a-zA-Z0-9][a-zA-Z0-9\\-]{0,64}(\\.[a-zA-Z0-9][a-zA-Z0-9\\-]{0,25})+$ </term></item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// <c>true</c> if an argument is valid email ; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='email'>
        /// Email you want to check
        /// </param>
        public static bool IsValidEmail(string email)
        {
            return !Utils.IsEmpty(email) && Utils.ValidateEmail(email);
        }

        /// <summary>
        /// Determines whether an argument is valid phone.
        /// </summary>
        /// <remarks>Valid phone is
        /// <list type="bullet">
        ///   <item><term>Not null</term></item>
        ///   <item><term>matches ^[\\+]?[0-9.-]+ </term></item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// <c>true</c> if an argument is valid phone ; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='phone'>
        /// Phone number you want to check
        /// </param>
        public static bool IsValidPhone(string phone)
        {
            return !Utils.IsEmpty(phone) && Utils.ValidatePhoneNumber(phone);
        }

        /// <summary>
        /// Determines whether an argument is valid global phone.
        /// </summary>
        /// <remarks>Valid global phone is
        /// </remarks>
        /// <returns>
        /// <c>true</c> if an argument is valid global phone ; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='phone'>
        /// Global phone number you want to check
        /// </param>
        public static bool IsValidGlobalPhone(string phone)
        {
            return Utils.ValidateGlobalPhoneNumber(phone);
        }
        /// <summary>
        /// Determines whether an argument is valid local phone.
        /// </summary>
        /// <remarks>Valid local phone is
        /// </remarks>
        /// <returns>
        /// <c>true</c> if an argument is valid local phone ; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='phone'>
        /// Local phone number you want to check
        /// </param>
        public static bool IsValidLocalPhone(string phone)
        {
            return !Utils.IsEmpty(phone) && Utils.ValidateLocalPhoneNumber(phone);
        }
        private static bool IsValidUserIdentifier(string userIdentifier)
        {
            return IsValidUserName(userIdentifier) || IsValidPhone(userIdentifier) || IsValidEmail(userIdentifier);
        }

        /// <summary>
        /// Determines whether an argument is valid password.
        /// </summary>
        /// <remarks>Valid password is
        /// <list type="bullet">
        ///   <item><term>Not null</term></item>
        ///   <item><term>Matches ^[a-zA-Z0-9]{4,64}$</term></item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// <c>true</c> if an argument is valid password ; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='password'>
        /// Password you want to check.
        /// </param>
        public static bool IsValidPassword(string password)
        {
            return !Utils.IsEmpty(password) && Utils.ValidatePassword(password);
        }

        /// <summary>
        /// Determines whether an argument is valid display name.
        /// </summary>
        /// <remarks>Valid display name is
        /// <list type="bullet">
        ///   <item><term>Not null</term></item>
        ///   <item><term>Length is 1 - 50</term></item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// <c>true</c> if an argument is valid display name ; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='displayname'>
        /// Display name you want to check.
        /// </param>
        public static bool IsValidDisplayName(string displayname)
        {
            return Utils.ValidateDisplayname(displayname);
        }

        /// <summary>
        /// Determines whether an argument is valid country.
        /// </summary>
        /// <remarks>Valid country is
        /// <list type="bullet">
        ///   <item><term>Not null</term></item>
        ///   <item><term>Matches ^[A-Z]{2}$</term></item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// <c>true</c> if an argument is valid country ; otherwise, <c>false</c>.
        /// </returns>
        /// <param name='country'>
        /// Country you want to check.
        /// </param>
        public static bool IsValidCountry(string country)
        {
            return !Utils.IsEmpty(country) && Utils.ValidateCountry(country);
        }
        /// <summary>
        /// Get KiiPushInstallation instance of current logged in user.
        /// The installation will be created for production.
        /// (Same as calling <see cref="PushInstallation(bool)"/> with false.)
        /// You can control sending message to production/development installation, by specifying the flag when sending push message.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>The installation.</returns>
        public static KiiPushInstallation PushInstallation()
        {
            return PushInstallation(false);
        }
        /// <summary>
        /// Get KiiPushInstallation instance of current logged in user.
        /// development : true would be used for testing push before release
        /// application / new version. other wise, use <see cref="PushInstallation()"/>.
        /// You can control sending message to production/ development installation, by specifying the flag when sending push message.
        /// </summary>
        /// <remarks></remarks>
        /// <returns>The installation.</returns>
        /// <param name="development">If set to <c>true</c> development.</param>
        public static KiiPushInstallation PushInstallation(bool development)
        {
            Utils.CheckInitialize(true);
            return new KiiPushInstallation(development);
        }
        #region Blocking APIs
        /// <summary>
        /// Login with username/email/phone and password.
        /// </summary>
        /// <remarks>
        /// UserIdentifier is username, email or phone.
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='userIdentifier'>
        /// Username, email or phone.
        /// </param>
        /// <param name='password'>
        /// User's password.
        /// </param>
        public static KiiUser LogIn(string userIdentifier, string password)
        {
            KiiUser me = new KiiUser();
            ExecLogin(me, userIdentifier, password, Kii.HttpClientFactory, (KiiUser user, Exception e) => 
            {
                if (e != null)
                {
                    throw e;
                }
                me.ExecRefresh(Kii.HttpClientFactory, (KiiUser user2, Exception e2) => 
                {
                    if (e2 != null)
                    {
                        throw e2;
                    }
                });
            });
            return me;
        }

        /// <summary>
        /// LogIn with local phone number, country and password.
        /// </summary>
        /// <remarks>
        /// Phone number must be valid local Phone number. Country must be valid country code.
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='localPhoneNumber'>
        /// Local phone number. Must be valid.
        /// </param>
        /// <param name='password'>
        /// Password. Must be valid.
        /// </param>
        /// <param name='country'>
        /// Country. Must be valid.
        /// </param>
        public static KiiUser LogInWithLocalPhone(string localPhoneNumber, string password, string country)
        {
            KiiUser me = new KiiUser();
            ExecLogInWithLocalPhone(me, localPhoneNumber, password, country, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                me.ExecRefresh(Kii.HttpClientFactory, (KiiUser user2, Exception e2) => 
                {
                    if (e2 != null)
                    {
                        throw e2;
                    }
                });
            });
            return me;
        }

        /// <summary>
        /// Login with Access Token.
        /// </summary>
        /// <remarks>
        /// If successful, the user is cached inside SDK as current user, 
        /// and accessible via <see cref="KiiUser.CurrentUser"/>.
        /// Specified token is also cached and can be get by 
        /// <see cref="KiiUser.GetAccessTokenDictionary"/>. Note that, token
        /// expiration time is not cached and set to DateTime.MaxValue in the dicitonary.
        /// If you want token expiration time also be cached, use 
        /// <see cref="KiiUser.LoginWithToken(string, DateTime)"/> instead.
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='token'>
        /// Access token.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when Server sends Json string which is not parserable.
        /// </exception>
        public static KiiUser LoginWithToken(string token)
        {
            KiiUser me = null;
            ExecLoginWithToken(token, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                me = user;
            });
            return me;
        }

        /// <summary>
        /// Login with Access Token.
        /// </summary>
        /// <remarks>
        /// Specified expiresAt won't be used by SDK. IF login successful,
        /// we set this property so that you can get it later along with token
        /// by <see cref="KiiUser.GetAccessTokenDictionary()"/>.
        /// Also, if successful, returned user is cached inside SDK 
        /// as current user, and accessible via <see cref="KiiUser.CurrentUser"/>.
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='token'>
        /// Access token.
        /// </param>
        /// <param name='expiresAt'>
        /// Access token expire time that has received by
        /// <see cref="KiiUser.GetAccessTokenDictionary()"/>.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when specified token is null/empty.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when Server sends Json string which is not parserable.
        /// </exception>
        public static KiiUser LoginWithToken(string token, DateTime expiresAt)
        {
            if (Utils.IsEmpty(token))
                throw new ArgumentException("Specified token is null/empty");

            KiiUser me = null;
            ExecLoginWithToken(token, Kii.HttpClientFactory, (KiiUser user, Exception e) => {
                if (e != null)
                {
                    throw e;
                }
                me = user;
                me.mAccessTokenExpiresAt = expiresAt;
            });
            return me;
        }

        /// <summary>
        /// Login with Access Token.
        /// </summary>
        /// <remarks>
        /// Specified expiresAt won't be used by SDK. IF login successful,
        /// we set this property so that you can get it later along with token
        /// by <see cref="KiiUser.GetAccessTokenDictionary()"/>.
        /// Also, if successful, returned user is cached inside SDK 
        /// as current user, and accessible via <see cref="KiiUser.CurrentUser"/>.
        /// </remarks>
        /// <param name='token'>
        /// Access token.
        /// </param>
        /// <param name='expiresAt'>
        /// Access token expire time that has received by
        /// <see cref="KiiUser.GetAccessTokenDictionary()"/>.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when specified token is null/empty or callback is null.
        /// </exception>
        public static void LoginWithToken(string token, DateTime expiresAt, KiiUserCallback callback)
        {
            if (callback == null)
                throw new ArgumentException("KiiUserCallback must not be null");

            if (Utils.IsEmpty(token))
                throw  new ArgumentException("Specified token is null/empty");

            ExecLoginWithToken(token, Kii.AsyncHttpClientFactory, (KiiUser user, Exception e) => {
                if (e == null)
                {
                    user.mAccessTokenExpiresAt = expiresAt;
                }
                callback(user, e);
            });
        }
        /// <summary>
        /// Login with Facebook Token.
        /// </summary>
        /// <remarks>
        /// If token is valid, The user is stored as CurrentUser. Facebook app id must be configured in app settings on the developer console.
        /// If successful, the user is cached inside SDK as current user, and accessible via <see cref="KiiUser.CurrentUser"/>.
        /// This user token and token expiration is also cached and can be get by <see cref="KiiUser.GetAccessTokenDictionary"/>.
        /// Access token won't be expired unless you set it explicitly by <see cref="Kii.AccessTokenExpiration"/>.
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='facebookToken'>
        /// Facebook Access token.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when Server sends Json string which is not parserable.
        /// </exception>
        public static KiiUser LoginWithFacebookToken(string facebookToken)
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", facebookToken);
            return LoginWithSocialNetwork(accessCredential, Provider.FACEBOOK);
        }
        /// <summary>
        /// Logins the with social network token.
        /// </summary>
        /// <returns>KiiUser instance.</returns>
        /// <param name="accessCredential">
        /// following parameters can be assigned to bundle:
        /// <list type="table">
        ///   <listheader>
        ///     <term>Key</term>
        ///     <description>Value</description>
        ///   </listheader>
        ///   <item>
        ///     <term>accessToken</term>
        ///     <description>Access token</description>
        ///   </item>
        ///   <item>
        ///     <term>accessTokenSecret</term>
        ///     <description>Access Token Secret for Twitter integration</description>
        ///   </item>
        ///   <item>
        ///     <term>openID</term>
        ///     <description>OpenID for QQ integration</description>
        ///   </item>
        /// </list>
        /// </param>
        /// <param name="provider">
        /// Social Network Provider.It must be FACEBOOK, GOOGLE, GOOGLEPLUS, TWITTER, RENREN or QQ.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when Server sends Json string which is not parserable.
        /// </exception>
        public static KiiUser LoginWithSocialNetwork(Dictionary<string, string> accessCredential, Provider provider)
        {
            KiiUser me = null;
            bool kiiNewUser = false;
            ExecLoginWithSocialNetwork(accessCredential, provider, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                kiiNewUser = user.mJSON.GetBoolean("new_user_created");
                user.ExecRefresh(Kii.HttpClientFactory, (KiiUser user2, Exception e2) => 
                {
                    if (e2 != null)
                    {
                        throw e2;
                    }
                    me = user2;
                });
            });
            // update social access token dictionary
            me.UpdateSocialAccessTokenDictionary(provider, accessCredential, kiiNewUser);
            return me;
        }
        /// <summary>
        /// Links the with social network.
        /// </summary>
        /// <returns>KiiUser instance.</returns>
        /// <param name="accessCredential">
        /// following parameters can be assigned to bundle:
        /// <list type="table">
        ///   <listheader>
        ///     <term>Key</term>
        ///     <description>Value</description>
        ///   </listheader>
        ///   <item>
        ///     <term>accessToken</term>
        ///     <description>Access token</description>
        ///   </item>
        ///   <item>
        ///     <term>accessTokenSecret</term>
        ///     <description>Access Token Secret for Twitter integration</description>
        ///   </item>
        ///   <item>
        ///     <term>openID</term>
        ///     <description>OpenID for QQ integration</description>
        ///   </item>
        /// </list>
        /// </param>
        /// <param name="provider">
        /// Social Network Provider.It must be FACEBOOK, GOOGLE, GOOGLEPLUS, TWITTER, RENREN or QQ.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        public KiiUser LinkWithSocialNetwork(Dictionary<string, string> accessCredential, Provider provider)
        {
            KiiUser me = null;
            ExecLinkWithSocialNetwork(accessCredential, provider, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                user.ExecRefresh(Kii.HttpClientFactory, (KiiUser user2, Exception e2) => 
                {
                    if (e2 != null)
                    {
                        throw e2;
                    }
                    me = user2;
                    me.UpdateSocialAccessTokenDictionary(provider, accessCredential, false);
                });
            });
            return me;
        }
        /// <summary>
        /// Uns the link with social network.
        /// </summary>
        /// <returns>KiiUser instance.</returns>
        /// <param name="provider">
        /// Social Network Provider.It must be FACEBOOK, GOOGLE, GOOGLEPLUS, TWITTER, RENREN or QQ.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        public KiiUser UnLinkWithSocialNetwork(Provider provider)
        {
            KiiUser me = null;
            ExecUnLinkWithSocialNetwork(provider, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                        throw e;
                }
                user.ExecRefresh(Kii.HttpClientFactory, (KiiUser user2, Exception e2) => 
                {
                    if (e2 != null)
                    {
                        throw e2;
                    }
                    me = user2;
                    me.ClearSocialAccessTokenDictionary();
                });
            });
            return me;
        }
        internal void UpdateSocialAccessTokenDictionary(Provider provider, Dictionary<string, string> accessCredential, bool kiiNewUser){
            if (provider == Provider.GOOGLE)
            {
                provider = Provider.GOOGLEPLUS;
            }
            Dictionary<string, object> dictionary =
            new Dictionary<string, object>() {
                { KiiUser.SocialResultParams.KII_NEW_USER, kiiNewUser },
                { KiiUser.SocialResultParams.OAUTH_TOKEN, accessCredential["accessToken"]},
                { KiiUser.SocialResultParams.PROVIDER, provider}
            };
            if (this.LinkedSocialAccounts.ContainsKey(provider))
            {
                dictionary.Add(KiiUser.SocialResultParams.PROVIDER_USER_ID, this.LinkedSocialAccounts[provider].SocialAccountId);
            }
            if (provider == Provider.TWITTER)
            {
                dictionary.Add(KiiUser.SocialResultParams.OAUTH_TOKEN_SECRET, accessCredential["accessTokenSecret"]);
            }
            if (provider == Provider.QQ)
            {
                dictionary.Add(KiiUser.SocialResultParams.OPEN_ID, accessCredential["openID"]);
            }
            this.mSocialAccessTokenDictionary = dictionary;
        }
        internal void ClearSocialAccessTokenDictionary()
        {
            if (this.mSocialAccessTokenDictionary != null)
            {
                this.mSocialAccessTokenDictionary.Clear();
            }
        }
        /// <summary>
        /// Register this user on KiiCloud.
        /// </summary>
        /// <remarks>
        /// Register this user on KiiCloud. Password must be valid.
        /// </remarks>
        /// <param name='password'>
        /// Password.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends illegal JSON object.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Register(string password)
        {
            ExecRegister(password, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                ExecLogin(this, Identifier, password, Kii.HttpClientFactory, (KiiUser user2, Exception e2) =>
                {
                    if (e2 != null)
                    {
                        throw e2;
                    }
                    ExecRefresh(Kii.HttpClientFactory, (KiiUser user3, Exception e3) =>
                    {
                        if (e3 != null)
                        {
                            throw e3;
                        }
                    });
                });
            });
        }
        /// <summary>
        /// Register as pseudo user on KiiCloud.
        /// </summary>
        /// <remarks>
        /// When successfully registered, get access token by <see cref="KiiUser.AccessToken"/> and
        /// store it for later use.
        /// </remarks>
        /// <param name="userFields">
        /// Optional. If null is passed, display name, country and other custom field would not be set.
        /// </param>
        /// <returns>
        /// Registered KiiUser instance.
        /// </returns>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public static KiiUser RegisterAsPseudoUser(UserFields userFields)
        {
            KiiUser result = null;
            ExecRegisterAsPseudoUser(userFields, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                result = user;
            });
            return result;
        }
        /// <summary>
        /// Puts identity data to pseudo user.
        /// This method is exclusive to pseudo user.
        /// If this method is called for user who has identity, <see cref="AlreadyHasIdentityException"/> will be thrown.
        /// </summary>
        /// <remarks>
        /// This user must be current user.
        /// password is mandatory and needs to provide at least one of login name, email address or phone number.
        /// Local modification done by <see cref="Country"/>, <see cref="Displayname"/> and other properties in <see cref="KiiUser"/> will be ignored.
        /// Please make sure to set new values in UserFields.
        /// </remarks>
        /// <param name="identityData">Mandatory.</param>
        /// <param name="userFields">
        /// Optional. If null is passed, display name, country and other custom field would not be updated.
        /// To update those fields, create userFields instance and pass to this api.
        /// Fields which is not included in this instance would not be updated. (current value would be retained)
        /// </param>
        /// <param name="password">Mandatory. New password is required to put Identity.</param>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when current user is not pseudo user.
        /// </exception>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when identityData is null.
        /// </exception>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <exception cref='AlreadyHasIdentityException'>
        /// if a pseudo user already has identity.
        /// </exception>
        public void PutIdentity(IdentityData identityData, UserFields userFields, string password)
        {
            ExecPutIdentity(identityData, userFields, password, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Refresh this instance.
        /// </summary>
        /// <remarks>
        /// Getting user profile from KiiCloud and refresh related properties.
        /// </remarks>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this instance does not have ID
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json object.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Refresh()
        {
            ExecRefresh(Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Update user.
        /// </summary>
        /// <remarks>
        /// This user must be current user.
        /// Before call this method, calling the Refresh() method is necessary to keep custom fields, otherwise custom fields will be gone.
        /// </remarks>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when user is not current user.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sernds error response.
        /// </exception>
        [Obsolete("Update() is deprecated, please use Update(IdentityData, UserFields) instead", false)]
        public void Update()
        {
            ExecUpdate(Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Update user attributes.
        /// If this method is called for pseudo user with IdentityData, <see cref="ArgumentException"/> will be thrown.
        /// To update IdentityData of pseudo user, use <see cref="PutIdentity(IdentityData, UserFields, string)"/>
        /// </summary>
        /// <remarks>
        /// Local modification done by <see cref="Country"/>, <see cref="Displayname"/> and other properties in <see cref="KiiUser"/> will be ignored and lost.
        /// Please make sure to set new values in UserFields.
        /// </remarks>
        /// <param name="identityData">
        /// Optional. If null is passed, Identity data would not be updated and current value would be retained.
        /// </param>
        /// <param name="userFields">
        /// Optional. If null is passed, display name, country and other custom field would not be updated.
        /// To update those fields, create userFields instance and pass to this api.
        /// Fields which is not included in this instance would not be updated. (current value would be retained.)
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when method is called for pseudo user
        /// Is thrown when specified local Phone number with invalid country code.
        /// </exception>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when both arguments are null or empty.
        /// </exception>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when user is not current user.
        /// </exception>
        public void Update(IdentityData identityData, UserFields userFields)
        {
            ExecUpdate(identityData, userFields, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Update user attributes.
        /// To update IdentityData of pseudo user, use <see cref="PutIdentity(IdentityData, UserFields, string)"/>
        /// </summary>
        /// <remarks>
        /// Local modification done by <see cref="Country"/>, <see cref="Displayname"/> and other properties in <see cref="KiiUser"/> will be ignored and lost.
        /// Please make sure to set new values in UserFields.
        /// </remarks>
        /// <param name="userFields">
        /// To update those fields, create userFields instance and pass to this api.
        /// Fields which is not included in this instance would not be updated. (current value would be retained.)
        /// </param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null or empty.
        /// </exception>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when user is not current user.
        /// </exception>
        public void Update(UserFields userFields)
        {
            ExecUpdate(null, userFields, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Delete this user from KiiCloud.
        /// </summary>
        /// <remarks>
        /// If this user currently logged in, LogOut will be done and cached user info and token in the SDK will be cleared.
        /// </remarks>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when user ID is empty.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void Delete()
        {
            ExecDelete(Kii.HttpClientFactory, (Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <remarks>
        /// Reset the password of current login user. Url for password reset will be sent to the specified identifier..
        /// </remarks>
        /// <param name='identifier'>
        /// Email address which is binded to this user.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is not a valid Email address.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public static void ResetPassword(string identifier)
        {
            ExecResetPassword(identifier, Kii.HttpClientFactory, (Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <remarks>
        /// Reset the password of user specified by given identifier.
        /// This api does not execute login after reset completion.
        /// </remarks>
        /// <param name='identifier'>
        /// should be valid email address, global phone number or user identifier obtained by 
        /// <see cref="ID"/>
        /// </param>
        /// <param name="notificationMethod">
        /// specify destination of message includes reset password link url. 
        /// different type of identifier and destination can be used 
        /// as long as user have verified email, phone.
        /// (ex. User registers both email and phone. Identifier is email and
        /// notificationMethod is SMS.)
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is not a valid user identifier.
        /// </exception>
        /// <exception cref='ConflictException'>
        /// Is thrown when an action has been invoked at an illegal or inappropriate time.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public static void ResetPassword(string identifier, NotificationMethod notificationMethod)
        {
            ExecResetPassword(identifier, notificationMethod, Kii.HttpClientFactory, (Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Reset password with the PIN code in receipt SMS.
        /// After <see cref="ResetPassword(string, NotificationMethod)"/> is called with <see cref="NotificationMethod.SMS_PIN"/>,
        /// SMS include the PIN code will be sent to the user's phone.
        /// User can set a new password for login with the PIN code.
        /// Need to call <see cref="LogIn(string, string)"/> to login after the new password is determined.
        /// </summary>
        /// <param name="identifier">
        /// should be valid email address, global phone number or user identifier obtained by <see cref="ID"/>.
        /// </param>
        /// <param name="pinCode">
        /// Received PIN code.
        /// </param>
        /// <param name="newPassword">
        /// New password for login.
        /// If the 'Password Reset Flow' in app's security setting is set to 'Generate password', it would be ignored and null can be passed.
        /// In this case, new password is generated on Kii Cloud and sent to user's phone. Otherwise valid password is required.
        /// </param>
        /// <exception cref='BadRequestException'>
        /// Is thrown when newPassword is invalid.
        /// </exception>
        /// <exception cref='ForbiddenException'>
        /// Is thrown when PIN code doesn’t match with sent via SMS.
        /// </exception>
        /// <exception cref='ConflictException'>
        /// Is thrown when the user does not have a verified phone number.
        /// </exception>
        /// <exception cref='GoneException'>
        /// Is thrown when PIN code to reset password has expired
        /// </exception>
        public static void CompleteResetPassword(string identifier, string pinCode, string newPassword)
        {
            ExecCompleteResetPassword(identifier, pinCode, newPassword, Kii.HttpClientFactory, (Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Changes the password of CurrentUser.
        /// </summary>
        /// <remarks>
        /// NewPassword must be valid.
        /// </remarks>
        /// <param name='newPassword'>
        /// New password.
        /// </param>
        /// <param name='oldPassword'>
        /// Old password.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when password is invalid.
        /// </exception>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this user is not CurrentUser.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public static void ChangePassword(string newPassword, string oldPassword)
        {
            ExecChangePassword(newPassword, oldPassword, Kii.HttpClientFactory, (Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Change email of logged in user.
        /// If the email address verification is required by your app configuration,
        /// User's email would not changed to new one until the new email verification has been done.
        /// In this case, new mail address can be obtained by <see cref="PendingEmail"/>.
        /// This API does not refresh the KiiUser automatically.
        /// Please execute <see cref="Refresh()"/> before checking the value of <see cref="Email"/> or <see cref="PendingEmail"/>
        /// </summary>
        /// <remarks>
        /// Email must be valid
        /// </remarks>
        /// <param name='email'>
        /// New email.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        [Obsolete("ChangeEmail(string) is deprecated, please use Update(IdentityData, CustomFields) instead", false)]
        public static void ChangeEmail(string email)
        {
            ExecChangeEmail(email, Kii.HttpClientFactory, (Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Change phone number of logged in user.
        /// If the phone number verification is required by your app configuration,
        /// User's phone number would not changed to new one until the new phone number verification has been done.
        /// In this case, new phone can be obtained by <see cref="PendingPhone"/>
        /// This API does not refresh the KiiUser automatically.
        /// Please execute <see cref="Refresh()"/> before checking the value of <see cref="Phone"/> or <see cref="PendingPhone"/>
        /// </summary>
        /// <remarks>
        /// Phone number must be valid.
        /// </remarks>
        /// <param name='phoneNumber'>
        /// New phone number.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is null or invalid.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        [Obsolete("ChangePhone(string) is deprecated, please use Update(IdentityData, CustomFields) instead", false)]
        public static void ChangePhone(string phoneNumber)
        {
            ExecChangePhone(phoneNumber, Kii.HttpClientFactory, (Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });

        }

        /// <summary>
        /// Requests for letting server send email verification.
        /// </summary>
        /// <remarks>
        /// LogIn is required.
        /// </remarks>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public static void RequestResendEmailVerification()
        {
            ExecRequestResendEmailVerification(Kii.HttpClientFactory, (Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Requests for letting server send phone verification code.
        /// </summary>
        /// <remarks>
        /// LogIn is required.
        /// </remarks>
        /// <exception cref='CloudException'>
        /// Is thrown when server sernds error response.
        /// </exception>
        public static void RequestResendPhoneVerificationCode()
        {
            ExecRequestResendPhoneVerificationCode(Kii.HttpClientFactory, (Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }
        /// <summary>
        /// Verifies the user.
        /// </summary>
        /// <remarks>
        /// <para>Verify this user's phone number with verification code 
        /// in SMS sent by KiiCloud.
        /// </para>
        /// <para>Verification code is sent from Kii Cloud when new
        /// user is registered with phone number or user requested
        /// to change their phone number in the application which
        /// requires phone verification.
        /// </para>
        /// <para>(You can enable/disable phone verification
        /// through the console in developer.kii.com)
        /// </para>
        /// <para>After the verification succeeded, new phone
        /// number becomes users phone number and user is able to
        /// login with the phone number.
        /// </para>
        /// <para>To get the new phone number,
        /// please call
        /// <see cref="Refresh(KiiCorp.Cloud.Storage.KiiUserCallback)"/>
        /// before access to <see cref="Phone"/>
        /// Before completion of
        /// <see cref="Refresh(KiiCorp.Cloud.Storage.KiiUserCallback)"/>,
        /// value of <see cref="Phone"/> is cached phone number.
        /// It could be old phone number or null.
        /// </para>
        /// </remarks>
        /// <param name='code'>
        /// Verificatoin conde on SMS sent by KiiCloud.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is empty.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json.
        /// </exception>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        public void VerifyPhone(string code)
        {
            ExecVerifyPhone(code, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
            });
        }

        /// <summary>
        /// Finds the user by UserName
        /// </summary>
        /// <remarks>
        /// Username must be valid. See <see cref="IsValidUserName(string)"/>
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='username'>
        /// User's Username.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json.
        /// </exception>
        public static KiiUser FindUserByUserName(string username)
        {
            if (!IsValidUserName(username))
            {
                throw new ArgumentException(ErrorInfo.KIIUSER_USERNAME_INVALID);
            }

            KiiUser found = null;
            ExecFindUser("LOGIN_NAME:" + username, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                found = user;
            });
            return found;
        }

        /// <summary>
        /// Finds the user by Email.
        /// </summary>
        /// <remarks>
        /// Email must be valid. See <see cref="IsValidEmail(string)"/>
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='email'>
        /// User's Email.
        /// </param>
        public static KiiUser FindUserByEmail(string email)
        {
            if (!IsValidEmail(email))
            {
                throw new ArgumentException(ErrorInfo.KIIUSER_EMAIL_FORMAT_INVALID);
            }

            KiiUser found = null;
            ExecFindUser("EMAIL:" + EscapeEmailString(email), Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                found = user;
            });
            return found;
        }

        /// <summary>
        /// Finds the user by phone.
        /// </summary>
        /// <remarks>
        /// Phone must be valid. See <see cref="IsValidPhone(string)"/>
        /// </remarks>
        /// <returns>
        /// KiiUser instance
        /// </returns>
        /// <param name='phone'>
        /// User's Phone.
        /// </param>
        public static KiiUser FindUserByPhone(string phone)
        {
            if (!IsValidPhone(phone))
            {
                throw new ArgumentException(ErrorInfo.KIIUSER_PHONE_FORMAT_INVALID);
            }

            KiiUser found = null;
            ExecFindUser("PHONE:" + phone, Kii.HttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                found = user;
            });
            return found;
        }

        /// <summary>
        /// Returns the list of groups which the user is a member of.
        /// </summary>
        /// <remarks>
        /// Group item in the list does not contain all the property of group.
        /// To get all the property from cloud, a KiiGroup.Refresh() is necessary.
        /// </remarks>
        /// <returns>
        /// The list of groups.
        /// </returns>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this instance doesn't have ID
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json
        /// </exception>
        public IList<KiiGroup> MemberOfGroups()
        {
            IList<KiiGroup> groups = null;
            ExecMemberOfGroups(Kii.HttpClientFactory, (IList<KiiGroup> list, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                groups = list;
            });
            return groups;
        }
        /// <summary>
        /// Returns the groups owned by this user.
        /// </summary>
        /// <remarks>
        /// Group in the list does not contain all the property of group.
        /// To get all the property from cloud, a <see cref="KiiGroup.Refresh()"/> is necessary.
        /// </remarks>
        /// <returns>
        /// The list of groups.
        /// </returns>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this instance doesn't have ID
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json
        /// </exception>
        public IList<KiiGroup> OwnerOfGroups()
        {
            IList<KiiGroup> groups = null;
            ExecOwnerOfGroups(Kii.HttpClientFactory, (IList<KiiGroup> list, Exception e) =>
            {
                if (e != null)
                {
                    throw e;
                }
                groups = list;
            });
            return groups;
        }
        /// <summary>
        /// Gets the list of topics in this user scope.
        /// </summary>
        /// <returns>A list of the topics in this user scope.</returns>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <exception cref='UnauthorizedException'>
        /// Is thrown when this method called by anonymous user.
        /// </exception>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this instance does not have ID
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public KiiListResult<KiiTopic> ListTopics()
        {
            return ListTopics((string)null);
        }
        /// <summary>
        /// Gets the list of next page of topics in this user scope.
        /// </summary>
        /// <param name="paginationKey">
        /// Specifies the pagination key that is obtained by <see cref="KiiListResult{T}.PaginationKey"/>.
        /// If specified null or empty, it's same as the <see cref="ListTopics()"/>.
        /// </param>
        /// <returns>A list of the topics in this user scope.</returns>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <exception cref='UnauthorizedException'>
        /// Is thrown when this method called by anonymous user.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public KiiListResult<KiiTopic> ListTopics(string paginationKey)
        {
            KiiListResult<KiiTopic> result = null;
            ExecListTopics(Kii.HttpClientFactory, paginationKey, (KiiListResult<KiiTopic> topics, Exception e) => {
                if (e != null)
                {
                    throw e;
                }
                result = topics;
            });
            return result;
        }
        #endregion

        #region Async APIs
        /// <summary>
        /// Login with username/email/phone and password.
        /// </summary>
        /// <remarks>
        /// UserIdentifier is username, email or phone.
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='userIdentifier'>
        /// Username, email or phone.
        /// </param>
        /// <param name='password'>
        /// User's password.
        /// </param>
        /// <param name='callback'>
        /// Callback delegate. If exception is null, execution is succeeded.
        /// </param>
        public static void LogIn(string userIdentifier, string password, KiiUserCallback callback)
        {
            KiiUser me = new KiiUser();
            ExecLogin(me, userIdentifier, password, Kii.AsyncHttpClientFactory, (KiiUser user, Exception e1) => 
            {
                if (e1 != null)
                {
                    if (callback != null)
                    {
                        callback(null, e1);
                    }
                    return;
                }
                me.ExecRefresh(Kii.AsyncHttpClientFactory, callback);
            });
        }

        /// <summary>
        /// LogIn with local phone number, country and password.
        /// </summary>
        /// <remarks>
        /// Phone number must be valid local Phone number. Country must be valid country code.
        /// </remarks>
        /// <param name='localPhoneNumber'>
        /// Local phone number. Must be valid.
        /// </param>
        /// <param name='password'>
        /// Password. Must be valid.
        /// </param>
        /// <param name='country'>
        /// Country. Must be valid.
        /// </param>
        /// <param name='callback'>
        /// Callback
        /// </param>
        public static void LogInWithLocalPhone(string localPhoneNumber, string password, string country, KiiUserCallback callback)
        {
            KiiUser me = new KiiUser();
            ExecLogInWithLocalPhone(me, localPhoneNumber, password, country, Kii.AsyncHttpClientFactory, (KiiUser user, Exception e) => 
            {
                if (e != null)
                {
                    if (callback != null)
                    {
                        callback(null, e);
                    }
                    return;
                }
                me.ExecRefresh(Kii.AsyncHttpClientFactory, callback);
            });
        }

        /// <summary>
        /// Login with Access Token.
        /// </summary>
        /// <remarks>
        /// If successful, the user is cached inside SDK as current user, 
        /// and accessible via <see cref="KiiUser.CurrentUser"/>.
        /// Specified token is also cached and can be get by 
        /// <see cref="KiiUser.GetAccessTokenDictionary()"/>. Note that, token
        /// expiration time is not cached and set to DateTime.MaxValue in the dicitonary.
        /// If you want token expiration time also be cached, use 
        /// <see cref="KiiUser.LoginWithToken(string, DateTime, KiiUserCallback)"/> instead.
        /// Login again to renew the token.
        /// </remarks>
        /// <param name='token'>
        /// Access token.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when Server sends Json string which is not parserable.
        /// </exception>
        public static void LoginWithToken(string token, KiiUserCallback callback)
        {
            ExecLoginWithToken(token, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Login with Facebook Token.
        /// </summary>
        /// <remarks>
        /// If token is valid, The user is stored as CurrentUser. Facebook app id must be configured in app settings on the developer console.
        /// </remarks>
        /// <param name='facebookToken'>
        /// Access token.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when Server sends Json string which is not parserable.
        /// </exception>
        public static void LoginWithFacebookToken(string facebookToken, KiiUserCallback callback)
        {
            Dictionary<string, string> accessCredential = new Dictionary<string, string>();
            accessCredential.Add("accessToken", facebookToken);
            LoginWithSocialNetwork(accessCredential, Provider.FACEBOOK, (KiiUser u, Exception e)=>{
                callback(u, e);
            });
        }
        /// <summary>
        /// Login with Social Network Token.
        /// </summary>
        /// <param name="accessCredential">
        /// following parameters can be assigned to bundle:
        /// <list type="table">
        ///   <listheader>
        ///     <term>Key</term>
        ///     <description>Value</description>
        ///   </listheader>
        ///   <item>
        ///     <term>accessToken</term>
        ///     <description>Access token</description>
        ///   </item>
        ///   <item>
        ///     <term>accessTokenSecret</term>
        ///     <description>Access Token Secret for Twitter integration</description>
        ///   </item>
        ///   <item>
        ///     <term>openID</term>
        ///     <description>OpenID for QQ integration</description>
        ///   </item>
        /// </list>
        /// </param>
        /// <param name="provider">
        /// Social Network Provider.It must be FACEBOOK, GOOGLE, GOOGLEPLUS, TWITTER, RENREN or QQ.
        /// </param>
        /// <param name="callback">Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when callback is null.
        /// </exception>
        public static void LoginWithSocialNetwork(Dictionary<string, string> accessCredential, Provider provider, KiiGenericsCallback<KiiUser> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiGenericsCallback must not be null");
            }
            bool kiiNewUser = false;
            ExecLoginWithSocialNetwork(accessCredential, provider, Kii.AsyncHttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null)
                    {
                        callback(null, e);
                    }
                    return;
                }
                kiiNewUser = user.mJSON.GetBoolean("new_user_created");
                user.ExecRefresh(Kii.AsyncHttpClientFactory, (KiiUser user2, Exception e2) => {
                    if (e2 != null)
                    {
                        if (callback != null)
                        {
                            callback(null, e2);
                        }
                        return;
                    }
                    user2.UpdateSocialAccessTokenDictionary(provider, accessCredential, kiiNewUser);
                    if (callback != null)
                    {
                        callback(user2, null);
                    }
                });
            });
        }
        /// <summary>
        /// Links the with social network.
        /// </summary>
        /// <param name="accessCredential">
        /// following parameters can be assigned to bundle:
        /// <list type="table">
        ///   <listheader>
        ///     <term>Key</term>
        ///     <description>Value</description>
        ///   </listheader>
        ///   <item>
        ///     <term>accessToken</term>
        ///     <description>Access token</description>
        ///   </item>
        ///   <item>
        ///     <term>accessTokenSecret</term>
        ///     <description>Access Token Secret for Twitter integration</description>
        ///   </item>
        ///   <item>
        ///     <term>openID</term>
        ///     <description>OpenID for QQ integration</description>
        ///   </item>
        /// </list>
        /// </param>
        /// <param name="provider">
        /// Social Network Provider.It must be FACEBOOK, GOOGLE, GOOGLEPLUS, TWITTER, RENREN or QQ.
        /// </param>
        /// <param name="callback">Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when callback is null.
        /// </exception>
        public void LinkWithSocialNetwork(Dictionary<string, string> accessCredential, Provider provider, KiiGenericsCallback<KiiUser> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiGenericsCallback must not be null");
            }
            ExecLinkWithSocialNetwork(accessCredential, provider, Kii.AsyncHttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null)
                    {
                        callback(null, e);
                    }
                    return;
                }
                user.ExecRefresh(Kii.AsyncHttpClientFactory, (KiiUser user2, Exception e2)=>{
                    if (e2 != null)
                    {
                        if (callback != null)
                        {
                            callback(null, e2);
                        }
                        return;
                    }
                    user2.UpdateSocialAccessTokenDictionary(provider, accessCredential, false);
                    callback(user2, e2);
                });
            });
        }
        /// <summary>
        /// Uns the link with social network.
        /// </summary>
        /// <param name="provider">
        /// Social Network Provider.It must be FACEBOOK, GOOGLE, GOOGLEPLUS, TWITTER, RENREN or QQ.
        /// </param>
        /// <param name="callback">Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when callback is null.
        /// </exception>
        public void UnLinkWithSocialNetwork(Provider provider, KiiGenericsCallback<KiiUser> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiGenericsCallback must not be null");
            }
            ExecUnLinkWithSocialNetwork(provider, Kii.AsyncHttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null)
                    {
                        callback(null, e);
                    }
                    return;
                }
                user.ClearSocialAccessTokenDictionary();
                user.ExecRefresh(Kii.AsyncHttpClientFactory, (KiiUser user2, Exception e2)=>{
                    callback(user2, e2);
                });
            });
        }
        /// <summary>
        /// Register this user on KiiCloud.
        /// </summary>
        /// <remarks>
        /// Register this user on KiiCloud. Password must be valid.
        /// </remarks>
        /// <param name='password'>
        /// Password.
        /// </param>
        /// <param name="callback">
        /// Callback.
        /// </param>
        public void Register(string password, KiiUserCallback callback)
        {
            ExecRegister(password, Kii.AsyncHttpClientFactory, (KiiUser user, Exception e) =>
            {
                if (e != null)
                {
                    if (callback != null)
                    {
                        callback(this, e);
                    }
                    return;
                }
                ExecLogin(this, Identifier, password, Kii.AsyncHttpClientFactory, (KiiUser user2, Exception e2) =>
                {
                    if (e2 != null)
                    {
                        if (callback != null)
                        {
                            callback(this, e2);
                        }
                        return;
                    }
                    ExecRefresh(Kii.AsyncHttpClientFactory, callback);
                });
            });
        }
        /// <summary>
        /// Register as pseudo user on KiiCloud.
        /// </summary>
        /// <remarks>
        /// When successfully registered, get access token by <see cref="KiiUser.AccessToken"/> and
        /// store it for later use.
        /// </remarks>
        /// <param name="userFields">
        /// Optional. If null is passed, display name, country and other custom field would not be set.
        /// </param>
        /// <param name="callback">Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when callback is null.
        /// </exception>
        public static void RegisterAsPseudoUser(UserFields userFields, KiiUserCallback callback)
        {
            ExecRegisterAsPseudoUser(userFields, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Puts identity data to pseudo user.
        /// This method is exclusive to pseudo user.
        /// If this method is called for user who has identity, <see cref="AlreadyHasIdentityException"/> will be passed to callback.
        /// </summary>
        /// <remarks>
        /// This user must be current user.
        /// password is mandatory and needs to provide at least one of login name, email address or phone number.
        /// Local modification done by <see cref="Country"/>, <see cref="Displayname"/> and other properties in <see cref="KiiUser"/> will be ignored.
        /// Please make sure to set new values in UserFields.
        /// </remarks>
        /// <param name="identityData">Mandatory.</param>
        /// <param name="userFields">
        /// Optional. If null is passed, display name, country and other custom field would not be updated.
        /// To update those fields, create userFields instance and pass to this api.
        /// Fields which is not included in this instance would not be updated. (current value would be retained)
        /// </param>
        /// <param name="password">Mandatory. New password is required to put Identity.</param>
        /// <param name='callback'>Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null or empty.
        /// </exception>
        public void PutIdentity(IdentityData identityData, UserFields userFields, string password, KiiUserCallback callback)
        {
            ExecPutIdentity(identityData, userFields, password, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Refresh this instance.
        /// </summary>
        /// <remarks>
        /// Getting user profile from KiiCloud and refresh related properties.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Refresh(KiiUserCallback callback)
        {
            ExecRefresh(Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Update user.
        /// </summary>
        /// <remarks>
        /// This user must be current user.
        /// Before call this method, calling the Refresh() method is necessary to keep custom fields, otherwise custom fields will be gone.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        [Obsolete("Update(KiiUserCallback) is deprecated, please use Update(IdentityData, UserFields, KiiUserCallback) instead", false)]
        public void Update(KiiUserCallback callback)
        {
            ExecUpdate(Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Update user attributes.
        /// If this method is called for pseudo user with IdentityData, <see cref="BadRequestException"/> will be thrown.
        /// To update IdentityData of pseudo user, use <see cref="PutIdentity(IdentityData, UserFields, string)"/>
        /// </summary>
        /// <remarks>
        /// Local modification done by <see cref="Country"/>, <see cref="Displayname"/> and other properties in <see cref="KiiUser"/> will be ignored.
        /// Please make sure to set new values in UserFields.
        /// </remarks>
        /// <param name="identityData">
        /// Optional. If null is passed, Identity data would not be updated and current value would be retained.
        /// </param>
        /// <param name="userFields">
        /// Optional. If null is passed, display name, country and other custom field would not be updated.
        /// To update those fields, create userFields instance and pass to this api.
        /// Fields which is not included in this instance would not be updated. (current value would be retained.)
        /// </param>
        /// <param name='callback'>Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null or empty.
        /// </exception>
        public void Update(IdentityData identityData, UserFields userFields, KiiUserCallback callback)
        {
            ExecUpdate(identityData, userFields, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Update user attributes.
        /// To update IdentityData of pseudo user, use <see cref="PutIdentity(IdentityData, UserFields, string)"/>
        /// </summary>
        /// <remarks>
        /// Local modification done by <see cref="Country"/>, <see cref="Displayname"/> and other properties in <see cref="KiiUser"/> will be ignored.
        /// Please make sure to set new values in UserFields.
        /// </remarks>
        /// <param name="userFields">
        /// To update those fields, create userFields instance and pass to this api.
        /// Fields which is not included in this instance would not be updated. (current value would be retained.)
        /// </param>
        /// <param name='callback'>Callback.</param>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when an argument is null or empty.
        /// </exception>
        public void Update(UserFields userFields, KiiUserCallback callback)
        {
            ExecUpdate(null, userFields, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Delete this user from KiiCloud.
        /// </summary>
        /// <remarks>
        /// If this user currently logged in, LogOut will be done and cached user info and token in the SDK will be cleared.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void Delete(KiiCallback callback)
        {
            ExecDelete(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <remarks>
        /// Reset the password of current login user. Url for password reset will be sent to the specified identifier..
        /// </remarks>
        /// <param name='identifier'>
        /// Email address which is binded to this user.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when SDK is not initialized.
        /// </exception>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when a callback is null.
        /// </exception>
        public static void ResetPassword(string identifier, KiiCallback callback)
        {
            ExecResetPassword(identifier, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Reset password with the PIN code in receipt SMS.
        /// After <see cref="ResetPassword(string, NotificationMethod)"/> is called with <see cref="NotificationMethod.SMS_PIN"/>,
        /// SMS include the PIN code will be sent to the user's phone.
        /// User can set a new password for login with the PIN code.
        /// Need to call <see cref="LogIn(string, string)"/> to login after the new password is determined.
        /// </summary>
        /// <param name="identifier">
        /// should be valid email address, global phone number or user identifier obtained by <see cref="ID"/>.
        /// </param>
        /// <param name="pinCode">
        /// Received PIN code.
        /// </param>
        /// <param name="newPassword">
        /// New password for login.
        /// If the 'Password Reset Flow' in app's security setting is set to 'Generate password', it would be ignored and null can be passed.
        /// In this case, new password is generated on Kii Cloud and sent to user's phone. Otherwise valid password is required.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public static void CompleteResetPassword(string identifier, string pinCode, string newPassword, KiiCallback callback)
        {
            ExecCompleteResetPassword(identifier, pinCode, newPassword, Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <remarks>
        /// Reset the password of user specified by given identifier.
        /// This api does not execute login after reset completion.
        /// </remarks>
        /// <param name='identifier'>
        /// should be valid email address, global phone number or user identifier obtained by 
        /// <see cref="ID"/>
        /// </param>
        /// <param name="notificationMethod">
        /// specify destination of message includes reset password link url. 
        /// different type of identifier and destination can be used 
        /// as long as user have verified email, phone.
        /// (ex. User registers both email and phone. Identifier is email and
        /// notificationMethod is SMS.)
        /// </param>
        /// <param name="callback">
        /// Callback called after completion.
        /// </param>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when SDK is not initialized.
        /// </exception>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when a callback is null.
        /// </exception>
        public static void ResetPassword(string identifier, NotificationMethod notificationMethod, KiiCallback callback)
        {
            ExecResetPassword(identifier, notificationMethod, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Changes the password of CurrentUser.
        /// </summary>
        /// <remarks>
        /// NewPassword must be valid.
        /// </remarks>
        /// <param name='newPassword'>
        /// New password.
        /// </param>
        /// <param name='oldPassword'>
        /// Old password.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public static void ChangePassword(string newPassword, string oldPassword, KiiCallback callback)
        {
            ExecChangePassword(newPassword, oldPassword, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Changes the email of CurrentUser
        /// </summary>
        /// <remarks>
        /// Email must be valid
        /// </remarks>
        /// <param name='email'>
        /// New email.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        [Obsolete("ChangeEmail(string, KiiCallback) is deprecated, please use Update(IdentityData, CustomFields, KiiUserCallback) instead", false)]
        public static void ChangeEmail(string email, KiiCallback callback)
        {
            ExecChangeEmail(email, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Changes the phone number of CurrentUser.
        /// </summary>
        /// <remarks>
        /// Phone number must be valid.
        /// </remarks>
        /// <param name='phoneNumber'>
        /// New phone number.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        [Obsolete("ChangePhone(string, KiiCallback) is deprecated, please use Update(IdentityData, CustomFields, KiiUserCallback) instead", false)]
        public static void ChangePhone(string phoneNumber, KiiCallback callback)
        {
            ExecChangePhone(phoneNumber, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Requests for letting server send email verification.
        /// </summary>
        /// <remarks>
        /// LogIn is required.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public static void RequestResendEmailVerification(KiiCallback callback)
        {
            ExecRequestResendEmailVerification(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Requests for letting server send phone verification code.
        /// </summary>
        /// <remarks>
        /// LogIn is required.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public static void RequestResendPhoneVerificationCode(KiiCallback callback)
        {
            ExecRequestResendPhoneVerificationCode(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Verifies the user.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Verify this user with verification code on SMS sent by KiiCloud.
        /// </para>
        /// <para>
        /// This method is asynchronous version of
        /// <see cref="VerifyPhone(string)"/>
        /// </para>
        /// </remarks>
        /// <param name='code'>
        /// Verificatoin conde on SMS sent by KiiCloud.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public void VerifyPhone(string code, KiiUserCallback callback)
        {
            ExecVerifyPhone(code, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Finds the user by UserName
        /// </summary>
        /// <remarks>
        /// Username must be valid. See <see cref="IsValidUserName(string)"/>
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='username'>
        /// User's Username.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json.
        /// </exception>
        public static void FindUserByUserName(string username, KiiUserCallback callback)
        {
            if (!IsValidUserName(username))
            {
                ArgumentException e = new ArgumentException(ErrorInfo.KIIUSER_USERNAME_INVALID);
                callback(null, e);
                return;
            }
            ExecFindUser("LOGIN_NAME:" + username, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Finds the user by Email.
        /// </summary>
        /// <remarks>
        /// Email must be valid. See <see cref="IsValidEmail(string)"/>
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <param name='email'>
        /// User's Email.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public static void FindUserByEmail(string email, KiiUserCallback callback)
        {
            if (!IsValidEmail(email))
            {
                ArgumentException e = new ArgumentException(ErrorInfo.KIIUSER_EMAIL_FORMAT_INVALID);
                callback(null, e);
                return;
            }

            ExecFindUser("EMAIL:" + EscapeEmailString(email), Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Finds the user by phone.
        /// </summary>
        /// <remarks>
        /// Phone must be valid. See <see cref="IsValidPhone(string)"/>
        /// </remarks>
        /// <returns>
        /// KiiUser instance
        /// </returns>
        /// <param name='phone'>
        /// User's Phone.
        /// </param>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        public static void FindUserByPhone(string phone, KiiUserCallback callback)
        {
            if (!IsValidPhone(phone))
            {
                ArgumentException e = new ArgumentException(ErrorInfo.KIIUSER_PHONE_FORMAT_INVALID);
                callback(null, e);
                return;
            }
            ExecFindUser("PHONE:" + phone, Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Returns the list of groups which the user is a member of.
        /// </summary>
        /// <remarks>
        /// Group item in the list does not contain all the property of group.
        /// To get all the property from cloud, a KiiGroup.Refresh() is necessary.
        /// </remarks>
        /// <param name='callback'>
        /// Callback.
        /// </param>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this instance doesn't have ID
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json
        /// </exception>
        public void MemberOfGroups(KiiGroupListCallback callback)
        {
            ExecMemberOfGroups(Kii.AsyncHttpClientFactory, callback);
        }

        /// <summary>
        /// Returns the groups owned by this user.
        /// </summary>
        /// <remarks>
        /// Group in the list does not contain all the property of group.
        /// To get all the property from cloud, a <see cref="KiiGroup.Refresh()"/> is necessary.
        /// </remarks>
        /// <param name="callback">Callback.</param>
        /// <exception cref='InvalidOperationException'>
        /// Is thrown when this instance doesn't have ID
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when server sends broken Json
        /// </exception>
        public void OwnerOfGroups(KiiGroupListCallback callback)
        {
            ExecOwnerOfGroups(Kii.AsyncHttpClientFactory, callback);
        }
        /// <summary>
        /// Asynchronous call for <see cref="ListTopics()"/>.
        /// </summary>
        /// <param name="callback">Callback.</param>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public void ListTopics(KiiGenericsCallback<KiiListResult<KiiTopic>> callback)
        {
            ListTopics(null, callback);
        }
        /// <summary>
        /// Asynchronous call for <see cref="ListTopics(string)"/>.
        /// </summary>
        /// <param name="paginationKey">
        /// Specifies the pagination key that is obtained by <see cref="KiiListResult{T}.PaginationKey"/>.
        /// If specified null or empty, it's same as the <see cref="ListTopics(KiiGenericsCallback{KiiListResult{KiiTopic}})"/>.
        /// </param>
        /// <param name="callback">Callback.</param>
        /// <exception cref='CloudException'>
        /// Is thrown when server sends error response.
        /// </exception>
        /// <remarks>
        /// This api sends a request to server.
        /// </remarks>
        public void ListTopics(string paginationKey, KiiGenericsCallback<KiiListResult<KiiTopic>> callback)
        {
            ExecListTopics(Kii.AsyncHttpClientFactory, paginationKey, callback);
        }
        #endregion

        private static JsonObject CreateLoginData(string userIdentifier, string password)
        {
            // UserIdentidier check
            if (!IsValidUserIdentifier(userIdentifier))
            {
                throw new ArgumentException("User identifier is invalid :"
                    + userIdentifier);
            }
            // Password check
            if (!IsValidPassword(password))
            {
                throw new ArgumentException(
                    ErrorInfo.KIIUSER_PASSWORD_INVALID + password);
            }

            // Prepare for login JSON data
            JsonObject loginData = new JsonObject();
            try
            {
                loginData.Put("username", userIdentifier);
                loginData.Put("password", password);
            }
            catch (JsonException)
            {
                // Won't happen.
                throw new ArgumentException(ErrorInfo.KIIUSER_DATA_PROBLEM);
            }
            return loginData;
        }

        private static JsonObject CreateLoginDataWithLocalPhone(
            String localPhoneNumber, String country, String password)
        {
            // LocalPhoneNumber check
            if (!IsValidLocalPhone(localPhoneNumber))
            {
                throw new ArgumentException("Local phone number is invalid : " + localPhoneNumber);
            }
            // Country check
            if (!IsValidCountry(country))
            {
                throw new ArgumentException("Country is invalid :" + country);
            }
            // Password check
            if (!IsValidPassword(password))
            {
                throw new ArgumentException(ErrorInfo.KIIUSER_PASSWORD_INVALID + password);
            }

            String userIdentifier = CreateLocalPhoneNumber(localPhoneNumber,
                country);

            // Prepare for login JSON data
            JsonObject loginData = new JsonObject();
            try
            {
                loginData.Put("username", userIdentifier);
                loginData.Put("password", password);
                //                Log.v(TAG, "request body: " + loginData.toString());
            }
            catch (JsonException)
            {
                // Won't happen.
                throw new ArgumentException(ErrorInfo.KIIUSER_DATA_PROBLEM);
            }
            return loginData;
        }

        private static string CreateLocalPhoneNumber(String localPhoneNumber, String country)
        {
            return "PHONE:" + country + "-" + localPhoneNumber;
        }

        #region Execution
        private static void ExecLogin(KiiUser user, string userIdentifier, string password, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            JsonObject loginData = null;
            try
            {
                loginData = CreateLoginData(userIdentifier, password);
            }
            catch (ArgumentException e)
            {
                if (callback != null)
                {
                    callback(null, e);
                }
                return;
            }
            ExecLogin(user, loginData, factory, callback);
        }

        private static void ExecLogInWithLocalPhone(KiiUser user, string localPhoneNumber, string password, string country, 
            KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            JsonObject loginData = null;
            try
            {
                loginData = CreateLoginDataWithLocalPhone(localPhoneNumber, country, password);
            }
            catch (ArgumentException e)
            {
                if (callback != null)
                {
                    callback(null, e);
                }
                return;
            }
            ExecLogin(user, loginData, factory, callback);
        }

        private static long SafeCalculateExpiresAt(long expirationInSeconds, DateTime time) {
            DateTime expireDate = SafeCalculateExpireDate(expirationInSeconds, time);
            DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan elapsedTime = expireDate - UNIX_EPOCH;
            return (long)elapsedTime.TotalMilliseconds;
        }

        private static DateTime SafeCalculateExpireDate(long expirationInSeconds, DateTime time) {
            DateTime expireDate;
            try {
                expireDate = time.AddSeconds(expirationInSeconds);
            } catch(ArgumentOutOfRangeException e){
                if(expirationInSeconds < 0){
                    expireDate = DateTime.MinValue;
                } else {
                    expireDate = DateTime.MaxValue;
                }
            }
            return expireDate;
        }

        private static void ExecLogin(KiiUser user, JsonObject loginData, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            if (Kii.CurrentUser != null)
            {
                LogOut();
            }

            // add expiresAt
            if(Kii.AccessTokenExpiration > 0) {
                long expiresAt = SafeCalculateExpiresAt(Kii.AccessTokenExpiration,  DateTime.UtcNow);
                loginData.Put("expiresAt", expiresAt);
            }

            string postUri = Utils.Path(Kii.BaseUrl, "oauth2", "token");

            KiiHttpClient client = factory.Create(postUri, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            client.ContentType = "application/vnd.kii.OauthTokenRequest+json";

            client.SendRequest(loginData.ToString(), (ApiResponse response, Exception e) => 
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                // Retrieve user info from server.
                JsonObject respObj = new JsonObject(response.Body);
                String token = respObj.GetString("access_token");
                String userId = respObj.GetString("id");
                long expiresIn = respObj.GetLong("expires_in");
                DateTime expireDate = SafeCalculateExpireDate(expiresIn, DateTime.UtcNow);
                KiiCloudEngine.UpdateAccessToken(token);

                user.mId = userId;
                user.mAccessTokenExpiresAt = expireDate;
                Kii.CurrentUser = user;
                KiiCloudEngine.UpdateAccessToken(token);

                callback(user, null);
            });
        }

        private static void ExecLoginWithToken(string token, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentException("KiiUserCallback must not be null");
            }
            if (Utils.IsEmpty(token))
            {
                callback(null, new ArgumentException("Specified token is null/empty"));
                return;
            }
            if (CurrentUser != null)
            {
                LogOut();
            }
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }

            KiiHttpClient client = factory.Create(MeUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            client.Headers["Authorization"] = "Bearer " + token;

            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                // parse response
                KiiUser me = null;
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    String userId = respObj.GetString("userID");
                    KiiCloudEngine.UpdateAccessToken(token);

                    me = new KiiUser();
                    me.mId = userId;
                    me.mJSON = respObj;
                    me.mAccessTokenExpiresAt = DateTime.MaxValue;
                    Kii.CurrentUser = me;
                }
                catch (JsonException e2)
                {
                    callback(null, new IllegalKiiBaseObjectFormatException(e2.Message));
                    return;
                }
                callback(me, null);
            });
        }
        private static void ExecLoginWithSocialNetwork(Dictionary<string, string> accessCredential, Provider provider, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            if (accessCredential == null || accessCredential.Count == 0)
            {
                callback(null, new ArgumentException("accessCredential should be not null or empty"));
                return;
            }
            try
            {
                ValidateSocialNetworkArguments(accessCredential, provider);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            string accessToken = Utils.GetDictionaryValue(accessCredential, "accessToken");
            string openId = Utils.GetDictionaryValue(accessCredential, "openID");
            string accessTokenSecret = Utils.GetDictionaryValue(accessCredential, "accessTokenSecret");
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }

            string providerName = null;
            string contentType = null;
            switch (provider)
            {
                case Provider.FACEBOOK:
                case Provider.GOOGLE:
                case Provider.TWITTER:
                case Provider.RENREN:
                case Provider.QQ:
                    providerName = provider.GetProviderName();
                    contentType = provider.GetTokenRequestContentType();
                    break;
                case Provider.GOOGLEPLUS:
                    providerName = Provider.GOOGLE.GetProviderName();
                    contentType = Provider.GOOGLE.GetTokenRequestContentType();
                    break;
                default:
                    callback(null, new ArgumentException(provider.GetProviderName() + " is not supported"));
                    return;
            }

            string postUri = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "integration", providerName);

            KiiHttpClient client = factory.Create(postUri, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            client.ContentType = contentType;
            // Prepare for login JSON data
            JsonObject loginData = new JsonObject();

            try {
                loginData.Put("accessToken", accessToken);
                if (provider == Provider.QQ)
                {
                    loginData.Put("openID", openId);
                }
                else if (provider == Provider.TWITTER)
                {
                    loginData.Put("accessTokenSecret", accessTokenSecret);
                }
                // add expiresAt
                if(Kii.AccessTokenExpiration > 0) {
                    long expiresAt = SafeCalculateExpiresAt(Kii.AccessTokenExpiration, DateTime.UtcNow);
                    loginData.Put("expiresAt", expiresAt);
                }
            } catch (JsonException) {
                // Won't happen.
                throw new ArgumentException(ErrorInfo.KIIUSER_DATA_PROBLEM);
            }

            client.SendRequest(loginData.ToString(), (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                // parse response
                KiiUser me = null;
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    String token = respObj.GetString("access_token");
                    String userId = respObj.GetString("id");
                    long expiresIn = respObj.GetLong("expires_in");
                    DateTime expireDate = SafeCalculateExpireDate(expiresIn, DateTime.UtcNow);

                    me = new KiiUser();
                    me.mId = userId;
                    me.mJSON = respObj;

                    me.mAccessTokenExpiresAt = expireDate;
                    if(CurrentUser != null)
                    {
                        LogOut();
                    }
                    Kii.CurrentUser = me;
                    KiiCloudEngine.UpdateAccessToken(token);
                }
                catch (JsonException e2)
                {
                    callback(null, new IllegalKiiBaseObjectFormatException(e2.Message));
                    return;
                }
                callback(me, null);
            });
        }
        private void ExecLinkWithSocialNetwork(Dictionary<string, string> accessCredential, Provider provider, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            try
            {
                ValidateSocialNetworkArguments(accessCredential, provider);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            string accessToken = Utils.GetDictionaryValue(accessCredential, "accessToken");
            string openId = Utils.GetDictionaryValue(accessCredential, "openID");
            string accessTokenSecret = Utils.GetDictionaryValue(accessCredential, "accessTokenSecret");
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(this, e);
                return;
            }

            string providerName = null;
            string contentType = null;
            switch (provider)
            {
                case Provider.FACEBOOK:
                case Provider.GOOGLE:
                case Provider.TWITTER:
                case Provider.RENREN:
                case Provider.QQ:
                    providerName = provider.GetProviderName();
                    contentType = provider.GetLinkRequestContentType();
                    break;
                case Provider.GOOGLEPLUS:
                    providerName = Provider.GOOGLE.GetProviderName();
                    contentType = Provider.GOOGLE.GetLinkRequestContentType();
                    break;
                default:
                    callback(this, new ArgumentException(provider.GetProviderName() + " is not supported"));
                    return;
            }

            //String postUrl = Utils.path(Kii.getBaseURL(), "apps", Kii.getAppId(), "users", "me", getSocialNetworkName(), "link");
            string postUri = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", providerName, "link");
            KiiHttpClient client = factory.Create(postUri, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = contentType;
            // Prepare for login JSON data
            JsonObject linkData = new JsonObject();

            try {
                linkData.Put("accessToken", accessToken);
                if (provider == Provider.QQ)
                {
                    linkData.Put("openID", openId);
                }
                else if (provider == Provider.TWITTER)
                {
                    linkData.Put("accessTokenSecret", accessTokenSecret);
                }
            } catch (JsonException) {
                // Won't happen.
                throw new ArgumentException(ErrorInfo.KIIUSER_DATA_PROBLEM);
            }
            client.SendRequest(linkData.ToString(), (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(this, e);
                    return;
                }
                callback(this, null);
            });
        }
        private void ExecUnLinkWithSocialNetwork(Provider provider, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            try
            {
                ValidateSocialNetworkArguments(null, provider);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(this, e);
                return;
            }

            string providerName = null;
            switch (provider)
            {
                case Provider.FACEBOOK:
                case Provider.GOOGLE:
                case Provider.TWITTER:
                case Provider.RENREN:
                case Provider.QQ:
                    providerName = provider.GetProviderName();
                    break;
                case Provider.GOOGLEPLUS:
                    providerName = Provider.GOOGLE.GetProviderName();
                    break;
                default:
                    callback(this, new ArgumentException(provider.GetProviderName() + " is not supported"));
                    return;
            }

            string postUri = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me", providerName, "unlink");
            KiiHttpClient client = factory.Create(postUri, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);

            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(this, e);
                    return;
                }
                callback(this, null);
            });
        }
        private void ExecRegister(string password, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }

            string username = Username;
            if (!IsValidPassword(password))
            {
                callback(null, new ArgumentException(ErrorInfo.KIIUSER_PASSWORD_INVALID + password));
                return;
            }
            string phone = Phone;
            if (!Utils.IsEmpty(phone) && !Utils.IsGlobalPhoneNumber(phone) && Utils.IsEmpty(Country))
            {
                callback(null, new ArgumentException(ErrorInfo.KIIUSER_LOCAL_PHONE_INVALID));
                return;
            }

            JsonObject obj = null;
            try
            {
                obj = new JsonObject(mJSON.ToString());
                obj.Put(PROPERTY_USERNAME, username);
                obj.Put("password", password);
                //                Log.v(TAG, "request body:" + object.toString(2));
            }
            catch (JsonException)
            {
                callback(this, new ArgumentException(ErrorInfo.KIIUSER_DATA_PROBLEM));
            }

            string postUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users");

            KiiHttpClient client = factory.Create(postUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            client.ContentType = "application/vnd.kii.RegistrationRequest+json";

            client.SendRequest(obj.ToString(), (ApiResponse response, Exception e) => 
            {
                if (e != null)
                {
                    callback(this, e);
                    return;
                }
                // parse response
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    mJSON = respObj;
                }
                catch (JsonException e2)
                {
                    callback(this, new IllegalKiiBaseObjectFormatException(e2.Message));
                    return;
                }
                callback(this, null);
            });
        }

        private static void ExecRegisterAsPseudoUser(UserFields userFields, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }

            JsonObject obj = null;
            try
            {
                if (userFields == null)
                {
                    obj = new JsonObject();
                }
                else
                {
                    obj = new JsonObject(userFields.mJSON.ToString());
                }
            }
            catch (JsonException)
            {
                callback(null, new ArgumentException(ErrorInfo.KIIUSER_DATA_PROBLEM));
                return;
            }

            string postUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users");
            KiiHttpClient client = factory.Create(postUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            client.ContentType = "application/vnd.kii.RegistrationAndAuthorizationRequest+json";

            client.SendRequest(obj.ToString(), (ApiResponse response, Exception e) => 
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                // parse response
                try
                {
                    JsonObject json = new JsonObject(response.Body);
                    string accessToken = json.GetString("_accessToken");
                    KiiCloudEngine.UpdateAccessToken(accessToken);
                    KiiUser me = new KiiUser();
                    me.mId = json.GetString("userID");
                    me.mJSON = json;
                    me.mAccessTokenExpiresAt = DateTime.MaxValue;
                    Kii.CurrentUser = me;
                    callback(me, null);
                }
                catch (JsonException e2)
                {
                    callback(null, new IllegalKiiBaseObjectFormatException(e2.Message));
                }
            });
        }
        private void ExecPutIdentity(IdentityData identityData, UserFields userFields, string password, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }

            string uuid = ID;
            if (uuid == null)
            {
                callback(null, new InvalidOperationException(ErrorInfo.KIIUSER_NO_ID));
                return;
            }
            if (uuid != Kii.CurrentUser.ID)
            {
                callback(null, new InvalidOperationException(ErrorInfo.KIIUSER_NOT_MODIFY));
                return;
            }
            if (identityData == null)
            {
                callback(null, new ArgumentNullException("IdentityData is null"));
                return;
            }
            if (!Utils.IsEmpty(identityData.Phone) && !Utils.IsGlobalPhoneNumber(identityData.Phone) && (userFields == null || !IsValidCountry(userFields.Country)))
            {
                callback(null, new ArgumentException(ErrorInfo.KIIUSER_LOCAL_PHONE_INVALID));
                return;
            }
            if (!IsValidPassword(password))
            {
                callback(null, new ArgumentException(ErrorInfo.KIIUSER_PASSWORD_INVALID + password));
                return;
            }
            if (!this.IsPseudoUser)
            {
                callback(this, new AlreadyHasIdentityException());
                return;
            }

            ExecRefresh(factory, (KiiUser user, Exception e1) =>
            {
                if (e1 != null)
                {
                    callback(user, e1);
                    return;
                }
                if (!user.IsPseudoUser)
                {
                    callback(user, new AlreadyHasIdentityException());
                    return;
                }
                KiiHttpClient client = factory.Create(MeUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
                KiiCloudEngine.SetAuthBearer(client);
                client.ContentType = "application/vnd.kii.UserUpdateRequest+json";

                JsonObject request = new JsonObject(user.mJSON.ToString());
                request.Put(PROPERTY_PASSWORD, password);
                if (!Utils.IsEmpty(identityData.UserName))
                {
                    request.Put(PROPERTY_USERNAME, identityData.UserName);
                }
                if (!Utils.IsEmpty(identityData.Email))
                {
                    request.Put(PROPERTY_EMAIL, identityData.Email);
                }
                if (!Utils.IsEmpty(identityData.Phone))
                {
                    request.Put(PROPERTY_PHONE, identityData.Phone);
                }
                if (userFields != null)
                {
                    if (!Utils.IsEmpty(userFields.Displayname))
                    {
                        request.Put(PROPERTY_DISPLAYNAME, userFields.Displayname);
                    }
                    if (!Utils.IsEmpty(userFields.Country))
                    {
                        request.Put(PROPERTY_COUNTRY, userFields.Country);
                    }
                    if (!Utils.IsEmpty(userFields.LocaleString))
                    {
                        request.Put(PROPERTY_LOCALE, userFields.LocaleString);
                    }
                    // overwrite existing fields by custom fieleds
                    foreach (string key in userFields.Keys())
                    {
                        request.Put(key, userFields[key]);
                    }
                    // remove existing fields
                    foreach (string key in userFields.RemovedFields)
                    {
                        request.Remove(key);
                    }
                }

                client.SendRequest(request.ToString(), (ApiResponse response, Exception e2) =>
                {
                    if (e2 != null)
                    {
                        callback(user, e2);
                        return;
                    }
                    RefreshWithIdentityData(identityData);
                    RefreshWithUserFields(userFields);
                    mJSON.Put(PROPERTY_HAS_PASSWORD, true);
                    callback(this, null);
                });
            });
        }
        private void ExecRefresh(KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            string id = ID;
            if (Utils.IsEmpty(id))
            {
                callback(this, new InvalidOperationException("User does not exist in the cloud."));
                return;
            }
            string getUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", id);

            KiiHttpClient client = factory.Create(getUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.Headers["userID"] = id;

            //ApiResponse response = client.sendRequest();
            client.SendRequest((ApiResponse response, Exception e) => 
            {
                if (e != null)
                {
                    callback(this, e);
                    return;
                }
                // parse response
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    mJSON = respObj;
                }
                catch (JsonException e2)
                {
                    callback(this, new IllegalKiiBaseObjectFormatException(e2.Message));
                    return;
                }
                callback(this, null);
            });
        }
        private void ExecOwnerOfGroups(KiiHttpClientFactory factory, KiiGroupListCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiGroupListCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            string id = ID;
            if (Utils.IsEmpty(id))
            {
                callback(null, new InvalidOperationException("User does not exist in the cloud."));
                return;
            }
            string getUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups");
            getUrl = getUrl + "?owner=" + id;

            KiiHttpClient client = factory.Create(getUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.Accept = "application/vnd.kii.GroupsRetrievalResponse+json";
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                // parse response
                List<KiiGroup> groups = new List<KiiGroup>();
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    JsonArray array = respObj.GetJsonArray("groups");
                    if (array == null)
                    {
                        callback(groups, null);
                        return;
                    }
                    for (int i = 0; i < array.Length(); i++)
                    {
                        JsonObject obj = array.GetJsonObject(i);
                        string groupId = obj.GetString("groupID");
                        string ownerId = obj.OptString("owner");
                        string name = obj.GetString("name");

                        KiiGroup group = KiiGroup.GroupWithID(groupId);
                        group.Name = name;
                        group.OwnerID = ownerId;
                        groups.Add(group);
                    }
                }
                catch (JsonException)
                {
                    callback(null, new IllegalKiiBaseObjectFormatException(response.Body));
                    return;
                }
                callback(groups, null);
            });
        }
        private void ExecUpdate(KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }

            string uuid = ID;
            if (uuid == null)
            {
                callback(null, new InvalidOperationException(ErrorInfo.KIIUSER_NO_ID));
                return;
            }

            KiiUser user = Kii.CurrentUser;
            if (uuid != user.ID)
            {
                callback(null, new InvalidOperationException(ErrorInfo.KIIUSER_NOT_MODIFY));
                return;
            }

            string postUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", uuid);

            KiiHttpClient client = factory.Create(postUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            client.Headers["userID"] = uuid;
            client.ContentType = "application/vnd.kii.UserUpdateRequest+json";

            client.SendRequest(mJSON.ToString(), (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                Kii.CurrentUser = this;
                callback(this, null);
            });
        }

        private void ExecUpdate(IdentityData identityData, UserFields userFields, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            if (identityData == null && (userFields == null || userFields.IsEmpty))
            {
                callback(null, new ArgumentNullException("At least one of identityData, or userFields must be specified."));
                return;
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }

            string uuid = ID;
            if (uuid == null)
            {
                callback(null, new InvalidOperationException(ErrorInfo.KIIUSER_NO_ID));
                return;
            }

            KiiUser user = Kii.CurrentUser;
            if (uuid != user.ID)
            {
                callback(null, new InvalidOperationException(ErrorInfo.KIIUSER_NOT_MODIFY));
                return;
            }
            if ((identityData != null && !Utils.IsEmpty(identityData.Phone) && !Utils.IsGlobalPhoneNumber(identityData.Phone)) && (userFields == null || !IsValidCountry(userFields.Country)))
            {
                callback(null, new ArgumentException(ErrorInfo.KIIUSER_PHONE_FORMAT_INVALID));
                return;
            }

            ExecRefresh(factory, (KiiUser refreshedUser, Exception e1) =>
            {
                if (e1 != null)
                {
                    callback(this, e1);
                    return;
                }
                if (identityData != null && refreshedUser.IsPseudoUser)
                {
                    callback(this, new ArgumentException("Pseudo user can not update identity with this method."));
                    return;
                }
                string postUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", uuid);

                KiiHttpClient client = factory.Create(postUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
                KiiCloudEngine.SetAuthBearer(client);
                client.ContentType = "application/vnd.kii.UserUpdateRequest+json";

                JsonObject request = new JsonObject(mJSON.ToString());
                if (identityData != null)
                {
                    if (!Utils.IsEmpty(identityData.UserName))
                    {
                        request.Put(PROPERTY_USERNAME, identityData.UserName);
                    }
                    if (!Utils.IsEmpty(identityData.Email))
                    {
                        request.Put(PROPERTY_EMAIL, identityData.Email);
                    }
                    if (!Utils.IsEmpty(identityData.Phone))
                    {
                        request.Put(PROPERTY_PHONE, identityData.Phone);
                    }
                }
                if (userFields != null)
                {
                    if (!Utils.IsEmpty(userFields.Displayname))
                    {
                        request.Put(PROPERTY_DISPLAYNAME, userFields.Displayname);
                    }
                    if (!Utils.IsEmpty(userFields.Country))
                    {
                        request.Put(PROPERTY_COUNTRY, userFields.Country);
                    }
                    if (!Utils.IsEmpty(userFields.LocaleString))
                    {
                        request.Put(PROPERTY_LOCALE, userFields.LocaleString);
                    }
                    // overwrite existing fields by custom fieleds
                    foreach (string key in userFields.Keys())
                    {
                        request.Put(key, userFields[key]);
                    }
                    // remove existing fields
                    foreach (string key in userFields.RemovedFields)
                    {
                        request.Remove(key);
                    }
                }
                client.SendRequest(request.ToString(), (ApiResponse response, Exception e2) =>
                {
                    if (e2 != null)
                    {
                        callback(user, e2);
                        return;
                    }
                    RefreshWithIdentityData(identityData);
                    RefreshWithUserFields(userFields);
                    callback(this, null);
                });
            });
        }

        private void ExecDelete(KiiHttpClientFactory factory, KiiCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(e);
                return;
            }

            String id = ID;
            if (Utils.IsEmpty(id))
            {
                callback(new InvalidOperationException("User does not exist in the cloud."));
                return;
            }
            bool isCurrentUser = (id == Kii.CurrentUser.ID);

            string deleteUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", id);

            KiiHttpClient client = factory.Create(deleteUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.DELETE);
            KiiCloudEngine.SetAuthBearer(client);
            client.Headers["userID"] = id;

            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(e);
                    return;
                }
                if (isCurrentUser)
                {
                    Kii.LogOut();
                }
                mId = null;
                mJSON = new JsonObject();
                callback(null);
            });

        }

        private static void ExecResetPassword(string identifier, NotificationMethod noticicationMean, KiiHttpClientFactory factory, KiiCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCallback must not be null");
            }
            if (Utils.IsEmpty(identifier))
            {
                callback(new ArgumentException("identifier is null or empty"));
                return;
            }
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(e);
                return;
            }
            string qualifiedID = Utils.EscapeUriString(GetQualifiedID(identifier));
            string resetUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", qualifiedID, "password", "request-reset");

            KiiHttpClient client = factory.Create(resetUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            client.ContentType = "application/vnd.kii.ResetPasswordRequest+json";
            JsonObject requestBody = new JsonObject();
            requestBody.Put("notificationMethod", noticicationMean.GetNotificationMethod());
            if (!String.IsNullOrEmpty(noticicationMean.GetSmsResetMethod()))
            {
                requestBody.Put("smsResetMethod", noticicationMean.GetSmsResetMethod());
            }
            client.SendRequest(requestBody.ToString(), (ApiResponse response, Exception e) => 
            {
                if (e != null)
                {
                    callback(e);
                    return;
                }
                callback(null);
            });

        }
        private static void ExecCompleteResetPassword(string identifier, string pinCode, string newPassword, KiiHttpClientFactory factory, KiiCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCallback must not be null");
            }
            if (Utils.IsEmpty(identifier))
            {
                callback(new ArgumentException("identifier is null or empty"));
                return;
            }
            if (Utils.IsEmpty(pinCode))
            {
                callback(new ArgumentException("pinCode is null or empty"));
                return;
            }
            string qualifiedID = Utils.EscapeUriString(GetQualifiedID(identifier));
            string resetUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", qualifiedID, "password", "complete-reset");

            KiiHttpClient client = factory.Create(resetUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            client.ContentType = "application/vnd.kii.CompletePasswordResetRequest+json";
            JsonObject requestBody = new JsonObject();
            if (!String.IsNullOrEmpty(newPassword))
            {
                requestBody.Put("newPassword", newPassword);
            }
            requestBody.Put("pinCode", pinCode);
            client.SendRequest(requestBody.ToString(), (ApiResponse response, Exception e) => 
            {
                if (e != null)
                {
                    callback(e);
                    return;
                }
                callback(null);
            });
        }
        private static void ExecResetPassword(string identifier, KiiHttpClientFactory factory, KiiCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(e);
                return;
            }

            string accountType = null;
            if (IsValidEmail(identifier))
            {
                accountType = "EMAIL";
            }
            else
            {
                callback(new ArgumentException("Identifier is not valid email address." + identifier));
                return;
            }
            string resetUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", accountType + ":" + identifier, "password", "request-reset");

            KiiHttpClient client = factory.Create(resetUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);

            client.SendRequest((ApiResponse response, Exception e) => 
            {
                if (e != null)
                {
                    callback(e);
                    return;
                }
                callback(null);
            });
        }

        private static void ExecChangePassword(string newPassword, string oldPassword, KiiHttpClientFactory factory, KiiCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(e);
                return;
            }

            if (!IsValidPassword(newPassword))
            {
                callback(new ArgumentException(ErrorInfo.KIIUSER_NEW_PASSWORD_INVALID + newPassword));
                return;
            }

            if (!IsValidPassword(oldPassword))
            {
                callback(new ArgumentException(ErrorInfo.KIIUSER_OLD_PASSWORD_INVALID + oldPassword));
                return;
            }

            String uuid = Kii.CurrentUser.ID;
            if (Utils.IsEmpty(uuid))
            {
                callback(new InvalidOperationException(ErrorInfo.KIIUSER_NO_ID));
                return;
            }

            JsonObject password = new JsonObject();
            try
            {
                password.Put("newPassword", newPassword);
                password.Put("oldPassword", oldPassword);
                //                Log.v(TAG, "request body: " + password.toString(2));
            }
            catch (JsonException)
            {
                // Won't happen.
            }

            string passwdUrl = Utils.Path(MeUrl, "password");
            KiiHttpClient client = factory.Create(passwdUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            KiiCloudEngine.SetAuthBearer(client);
            client.Headers["userID"] = uuid;
            client.ContentType = "application/vnd.kii.ChangePasswordRequest+json";

            client.SendRequest(password.ToString(), (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(e);
                    return;
                }
                callback(null);
            });
        }

        private static void ExecChangeEmail(string email, KiiHttpClientFactory factory, KiiCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(e);
                return;
            }
            if (Utils.IsEmpty(email))
            {
                callback(new ArgumentException(ErrorInfo.KIIUSER_EMAIL_EMPTY));
                return;
            }
            if (!IsValidEmail(email))
            {
                callback(new ArgumentException(ErrorInfo.KIIUSER_EMAIL_FORMAT_INVALID + email));
                return;
            }

            JsonObject requestBody = new JsonObject();
            try
            {
                requestBody.Put("emailAddress", email);
            }
            catch (JsonException)
            {
                // Won't happen.
                throw new ArgumentException(ErrorInfo.KIIUSER_EMAIL_FORMAT_INVALID);
            }

            string url = Utils.Path(MeUrl, "email-address");

            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = "application/vnd.kii.EmailAddressModificationRequest+json";

            // send request
            client.SendRequest(requestBody.ToString(), (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(e);
                    return;
                }
                callback(null);
            });
        }

        private static void ExecChangePhone(string phoneNumber, KiiHttpClientFactory factory, KiiCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(e);
                return;
            }
            if (Utils.IsEmpty(phoneNumber))
            {
                callback(new ArgumentException(ErrorInfo.KIIUSER_PHONE_EMPTY));
                return;
            }
            if (!IsValidPhone(phoneNumber))
            {
                callback(new ArgumentException(ErrorInfo.KIIUSER_PHONE_FORMAT_INVALID + phoneNumber));
                return;
            }

            JsonObject requestBody = new JsonObject();
            try
            {
                requestBody.Put("phoneNumber", phoneNumber);
                //                Log.v(TAG, "request body: " + requestBody.toString());
            }
            catch (JsonException)
            {
                // Won't happen.
                callback(new ArgumentException(ErrorInfo.KIIUSER_PHONE_FORMAT_INVALID));
                return;
            }

            string url = Utils.Path(MeUrl, "phone-number");

            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.PUT);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = "application/vnd.kii.PhoneNumberModificationRequest+json";

            // send request
            client.SendRequest(requestBody.ToString(), (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(e);
                    return;
                }
                callback(null);
            });

        }

        private static void ExecRequestResendEmailVerification(KiiHttpClientFactory factory, KiiCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(e);
                return;
            }

            string postUrl = Utils.Path(MeUrl, "email-address", "resend-verification");

            KiiHttpClient client = factory.Create(postUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(e);
                    return;
                }
                callback(null);
            });
        }

        private static void ExecRequestResendPhoneVerificationCode(KiiHttpClientFactory factory, KiiCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(e);
                return;
            }
            string postUrl = Utils.Path(MeUrl, "phone-number", "resend-verification");

            KiiHttpClient client = factory.Create(postUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(e);
                    return;
                }
                callback(null);
            });
        }

        private void ExecVerifyPhone(string code, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }

            if (Utils.IsEmpty(code))
            {
                callback(null, new ArgumentException(ErrorInfo.KIIUSER_REGISTER_EMPTY));
                return;
            }

            // build JSON string
            JsonObject jsonObject = new JsonObject();
            string jsonString = "";
            try
            {
                jsonObject.Put("verificationCode", code);
                jsonString = jsonObject.ToString();
            }
            catch (JsonException)
            {
                callback(null, new ArgumentException(ErrorInfo.KIIUSER_DATA_PROBLEM));
                return;
            }

            string postUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId,
                "users", ID, "phone-number", "verify");

            KiiHttpClient client = factory.Create(postUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.POST);
            KiiCloudEngine.SetAuthBearer(client);
            client.ContentType = "application/vnd.kii.AddressVerificationRequest+json";

            client.SendRequest(jsonString, (ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                // parse response
                try
                {
                    mJSON.Put(PROPERTY_PHONE_NUMBER_VERIFIED, true);
                }
                catch (JsonException e2)
                {
                    callback(null, new IllegalKiiBaseObjectFormatException(e2.Message));
                    return;
                }
                callback(this, null);
            });
        }

        private static void ExecFindUser(string lastSegment, KiiHttpClientFactory factory, KiiUserCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiUserCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            string url = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId,
                "users", lastSegment);

            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                // parse response
                KiiUser found = null;
                try
                {
                    found = new KiiUser();
                    found.mJSON = new JsonObject(response.Body);
                }
                catch (JsonException)
                {
                    callback(null, new IllegalKiiBaseObjectFormatException("Unexpected response."));
                    return;
                }
                callback(found, null);
            });
        }

        private void ExecMemberOfGroups(KiiHttpClientFactory factory, KiiGroupListCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiGroupListCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(false);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            string id = ID;
            if (Utils.IsEmpty(id))
            {
                callback(null, new InvalidOperationException("User does not exist in the cloud."));
                return;
            }

            string getUrl = Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "groups");
            getUrl = getUrl + "?is_member=" + id;

            KiiHttpClient client = factory.Create(getUrl, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.Accept = "application/vnd.kii.GroupsRetrievalResponse+json";

            // send request
            client.SendRequest((ApiResponse response, Exception e) =>
            {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                // parse response
                List<KiiGroup> groups = new List<KiiGroup>();
                try
                {
                    JsonObject respObj = new JsonObject(response.Body);
                    JsonArray array = respObj.GetJsonArray("groups");
                    if (array == null)
                    {
                        callback(groups, null);
                        return;
                    }
                    for (int i = 0; i < array.Length(); i++)
                    {
                        JsonObject obj = array.GetJsonObject(i);
                        string groupId = obj.GetString("groupID");
                        string ownerId = obj.OptString("owner");
                        string name = obj.GetString("name");

                        KiiGroup group = KiiGroup.GroupWithID(groupId);
                        group.Name = name;
                        group.OwnerID = ownerId;
                        groups.Add(group);
                    }
                }
                catch (JsonException)
                {
                    callback(null, new IllegalKiiBaseObjectFormatException(response.Body));
                    return;
                }
                callback(groups, null);
            });
        }

        private void ExecListTopics(KiiHttpClientFactory factory, string paginationKey, KiiGenericsCallback<KiiListResult<KiiTopic>> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("KiiGenericsCallback must not be null");
            }
            try
            {
                Utils.CheckInitialize(true);
            }
            catch (Exception e)
            {
                callback(null, e);
                return;
            }
            if (Utils.IsEmpty(ID))
            {
                callback(null, new InvalidOperationException(ErrorInfo.KIIUSER_NO_ID));
                return;
            }
            String url = Utils.Path(Url, "topics");
            if (!String.IsNullOrEmpty(paginationKey))
            {
                url = url + "?paginationKey=" + Uri.EscapeUriString(paginationKey);
            }
            KiiHttpClient client = factory.Create(url, Kii.AppId, Kii.AppKey, KiiHttpMethod.GET);
            KiiCloudEngine.SetAuthBearer(client);
            client.SendRequest((ApiResponse response, Exception e) => {
                if (e != null)
                {
                    callback(null, e);
                    return;
                }
                JsonObject json = new JsonObject(response.Body);
                String newPaginationKey = json.OptString("paginationKey", null);
                JsonArray array = json.GetJsonArray("topics");
                List<KiiTopic> topics = new List<KiiTopic>();
                for (int i = 0; i < array.Length(); i++)
                {
                    topics.Add(this.Topic(array.GetJsonObject(i).GetString("topicID")));
                }
                callback(new KiiListResult<KiiTopic>(topics, newPaginationKey), null);
            });
        }
        #endregion

        private static void ValidateSocialNetworkArguments(Dictionary<string, string> accessCredential, Provider provider)
        {
            switch (provider)
            {
                case Provider.FACEBOOK:
                case Provider.GOOGLE:
                case Provider.GOOGLEPLUS:
                case Provider.RENREN:
                    if (accessCredential != null)
                    {
                        if (!accessCredential.ContainsKey("accessToken") || String.IsNullOrEmpty(accessCredential["accessToken"]))
                        {
                            throw new ArgumentException("accessCredential should contain accessToken");
                        }
                    }
                    break;
                case Provider.TWITTER:
                    if (accessCredential != null)
                    {
                        if (!accessCredential.ContainsKey("accessToken") || String.IsNullOrEmpty(accessCredential["accessToken"]))
                        {
                            throw new ArgumentException("accessCredential should contain accessToken");
                        }
                        if (!accessCredential.ContainsKey("accessTokenSecret") || String.IsNullOrEmpty(accessCredential["accessTokenSecret"]))
                        {
                            throw new ArgumentException("accessCredential should contain accessTokenSecret");
                        }
                    }
                    break;
                case Provider.QQ:
                    if (accessCredential != null)
                    {
                        if (!accessCredential.ContainsKey("accessToken") || String.IsNullOrEmpty(accessCredential["accessToken"]))
                        {
                            throw new ArgumentException("accessCredential should contain accessToken");
                        }
                        if (!accessCredential.ContainsKey("openID") || String.IsNullOrEmpty(accessCredential["openID"]))
                        {
                            throw new ArgumentException("accessCredential should contain openID");
                        }
                    }
                    break;
                default:
                    throw new ArgumentException(provider.GetProviderName() + " is not supported");
            }
        }
        private void RefreshWithIdentityData(IdentityData identityData)
        {
            if (identityData != null)
            {
                if (!Utils.IsEmpty(identityData.UserName))
                {
                    mJSON.Put(PROPERTY_USERNAME, identityData.UserName);
                }
                if (!Utils.IsEmpty(identityData.Email))
                {
                    mJSON.Put(PROPERTY_EMAIL, identityData.Email);
                }
                if (!Utils.IsEmpty(identityData.Phone))
                {
                    mJSON.Put(PROPERTY_PHONE, identityData.Phone);
                }
            }
        }
        private void RefreshWithUserFields(UserFields userFields)
        {
            if (userFields != null)
            {
                if (!Utils.IsEmpty(userFields.Displayname))
                {
                    mJSON.Put(PROPERTY_DISPLAYNAME, userFields.Displayname);
                }
                if (!Utils.IsEmpty(userFields.Country))
                {
                    mJSON.Put(PROPERTY_COUNTRY, userFields.Country);
                }
                // overwrite existing fields by custom fieleds
                foreach (string key in userFields.Keys())
                {
                    mJSON.Put(key, userFields[key]);
                }
                // remove existing fields
                foreach (string key in userFields.RemovedFields)
                {
                    mJSON.Remove(key);
                }
            }
        }
        /// <summary><see cref="Phone"/>
        /// Instantiate KiiUser that refers to existing user which has specified ID.
        /// </summary>
        /// <remarks>
        /// You have to specify the ID of existing KiiUser. Unlike KiiObject,
        /// you can not assign ID in the client side.
        /// This API does not access to the server.
        /// After instantiation, call <see cref="Refresh()"/> to fetch the properties.
        /// </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Is thrown when specified userID is null or empty.
        /// </exception>
        /// <param name='userID'>
        /// ID of the user to instantiate.
        /// </param>
        public static KiiUser UserWithID(string userID)
        {
            if (Utils.IsEmpty(userID)) {
                throw new ArgumentException("Specified userID is null or empty");
            }
            KiiUser user = new KiiUser();
            user.mId = userID;
            return user;
        }


        /// <summary>
        /// Creates KiiUser instance by URI.
        /// </summary>
        /// <remarks>URI format is kiiuser: </remarks>
        /// <returns>
        /// KiiUser instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Is thrown when an argument is invalid.
        /// </exception>
        /// <param name='uri'>
        /// User's URI.
        /// </param>
        public static KiiUser CreateByUri(Uri uri)
        {
            string id = ParseIdFromUri(uri);
            return UserWithID(id);
        }

        /// <summary>
        /// Check format and return ID
        /// </summary>
        /// <remarks>The format of URI must be kiicloud://users/(userID) </remarks>
        /// <returns>
        /// User ID
        /// </returns>
        /// <param name='uri'>
        /// URI(kiicloud://users/(userID).
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when URI is invalid
        /// </exception>
        private static string ParseIdFromUri(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentException(ErrorInfo.KIIUSER_URI_IS_NULL);
            }
            string scheme = uri.Scheme;

            string[] segments = uri.Segments;
            if (scheme != ConstantValues.URI_SCHEME)
            {
                throw new ArgumentException(ErrorInfo.KIIUSER_URI_NO_SUPPORT + uri);
            }

            string authority = uri.Authority;
            if (authority != "users")
            {
                throw new ArgumentException(ErrorInfo.KIIUSER_URI_NO_SUPPORT + uri);
            }
            if (segments.Length != 2)
            {
                throw new ArgumentException(ErrorInfo.KIIUSER_URI_NO_SUPPORT + uri);
            }
            string id = segments[1];
            if (id.EndsWith("/"))
            {
                id = id.Substring(0, id.Length - 1);
            }
            if (id.Length == 0)
            {
                throw new ArgumentException(ErrorInfo.KIIUSER_URI_NO_SUPPORT + uri);
            }
            return id;
        }

        // Note : 
        // WWW client in Unity (iOS) regards email part in the finding user URL as authority because its URL has "@" string.
        // To avoid this issue, we need to escape "@" to "%40".
        // For details, please see KiiCorp/kanban#268
        private static string EscapeEmailString(string email)
        {
            if (email == null)
            {
                return email;
            }
            return email.Replace("@", "%40");
        }

        internal static KiiUser GetById(string id)
        {
            KiiUser user = new KiiUser();
            user.mId = id;
            return user;
        }

        /// <summary>
        /// Returns bucket belongs to this user.
        /// </summary>
        /// <remarks>
        /// Returns bucket belongs to this user.
        /// </remarks>
        /// <param name='bucketName'>
        /// Bucket name.
        /// </param>
        /// <returns>
        /// User scope KiiBucket instance.
        /// </returns>
        /// <exception cref='ArgumentException'>
        /// Is throwns when bucket name is invalid.
        /// </exception>
        public KiiBucket Bucket(string bucketName)
        {
            return new KiiBucket(this, bucketName);
        }

        /// <summary>
        /// Get instance of user scope topic.
        /// The topic bound to this user.
        /// </summary>
        /// <param name="name">Name of topic.</param>
        /// <returns>KiiTopic bound to this user.</returns>
        public KiiTopic Topic(string name)
        {
            Utils.CheckInitialize(true);
            if (Utils.IsEmpty(ID))
            {
                throw new InvalidOperationException("KiiUser has deleted or not registered in KiiCloud yet.");
            }
            return new KiiTopic(this, name);
        }

        /// <summary>
        /// Determines whether this instance is linked with specified social provider.
        /// </summary>
        /// <returns><c>true</c> if this instance is linked social provider the specified provider; otherwise, <c>false</c>.</returns>
        /// <param name="provider">Provider.</param>
        /// <remarks>
        /// Before call this method, calling the <see cref="Refresh()"/> method is necessary to get the correct value.
        /// </remarks>
        public bool IsLinkedWithSocialProvider(Provider provider)
        {
            JsonObject thirdPartyAccounts = mJSON.OptJsonObject("_thirdPartyAccounts");
            if (thirdPartyAccounts == null)
            {
                return false;
            }
            if (provider == Provider.GOOGLE || provider == Provider.GOOGLEPLUS)
            {
                return thirdPartyAccounts.Has(Provider.GOOGLE.GetLinkedProviderSocialNetworkName()) ||
                    thirdPartyAccounts.Has(Provider.GOOGLEPLUS.GetLinkedProviderSocialNetworkName());
            }
            return thirdPartyAccounts.Has(provider.GetLinkedProviderSocialNetworkName());
        }

        /// <summary>
        /// Logout Current User
        /// </summary>
        /// <remarks>Logout Current User</remarks>
        public static void LogOut()
        {
            Utils.CheckInitialize(false);
            Kii.LogOut();
        }

        /// <summary>
        /// Create Builder with user name.
        /// </summary>
        /// <remarks>Username must be valid. See <see cref="IsValidUserName(string)"/> </remarks>
        /// <returns>
        /// KiiUser builder instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">Username is not valid</exception>
        /// <param name='userName'>
        /// Username.
        /// </param>
        public static Builder BuilderWithName(string userName)
        {
            return new Builder().WithName(userName);
        }

        /// <summary>
        /// Create Builder with phone.
        /// </summary>
        /// <remarks>Phone must be valid. See <see cref="IsValidPhone(string)"/> </remarks>
        /// <returns>
        /// KiiUser builder instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">Phone is not valid</exception>
        /// <param name='phone'>
        /// Phone.
        /// </param>
        public static Builder BuilderWithPhone(string phone)
        {
            return new Builder().WithPhone(phone);
        }

        /// <summary>
        ///   Create Builder with local phone.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     Phone and country must be valid. For phone, See <see
        ///     cref="IsValidLocalPhone(string)"/>. For country, See
        ///     <see cref="IsValidCountry(string)"/>.
        ///   </para>
        /// </remarks>
        /// <returns>
        ///   KiiUser builder instance.
        /// </returns>
        /// <param name="phone">
        /// Local phone.
        /// </param>
        /// <param name="country">
        /// Country.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// One of phone and country is not valid or both of phone and
        /// country are not valid.
        /// </exception>
        public static Builder BuilderWithLocalPhone(
                string phone,
                string country)
        {
            return new Builder().SetLocalPhone(phone, country);
        }

        /// <summary>
        /// Create Builder with email.
        /// </summary>
        /// <remarks>Email must be valid. See <see cref="IsValidEmail(string)"/> </remarks>
        /// <returns>
        /// KiiUser builder instance.
        /// </returns>
        /// <exception cref="System.ArgumentException">Email is not valid</exception>
        /// <param name='email'>
        /// Email.
        /// </param>
        public static Builder BuilderWithEmail(string email)
        {
            return new Builder().WithEmail(email);
        }

        /// <summary>
        /// Create Builder with identifier.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This factory method receives identifier. The identifier
        ///     is one of use name, email address or phone
        ///     number. This constructor automatically identities what
        ///     identifier is.
        ///   </para>
        ///   <para>
        ///      Some Strings can be accepted as both user name and
        ///      phone number. If such string is passed to this
        ///      constructor as identifier, then phone number is prior
        ///      to user name. String of email address is in different
        ///      class against user name and phone number. So email
        ///      address is always identified correctly.
        ///   </para>
        ///   <para>
        ///     This method accepts local phone number as identifier
        ///     but if You use local phone number as identifier, You
        ///     need to set country code to KiiUser instance built by
        ///     <see cref="KiiCorp.Cloud.Storage.KiiUser.Builder.Build()" />.
        ///   </para>
        /// </remarks>
        /// <param name="identifier">
        /// The user's user name, email address or phone number.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Identifier is not user name, email address or phone number.
        /// </exception>
        public static Builder BuilderWithIdentifier(string identifier)
        {
            Builder builder = new Builder();
            if (KiiUser.IsValidPhone(identifier))
            {
                builder.WithPhone(identifier);
            }
            else if (KiiUser.IsValidEmail(identifier))
            {
                builder.SetEmail(identifier);
            }
            else if (KiiUser.IsValidUserName(identifier))
            {
                builder.SetName(identifier);
            }
            else
            {
                throw new ArgumentException(
                    ErrorInfo.KIIUSER_UNSPECIFIED_IDENTIFIER + identifier);
            }
            return builder;
        }

        #region properties
        /// <summary>
        /// Returns the ID of this user.
        /// </summary>
        /// <remarks>
        /// If the user has not saved to the cloud, returns null.
        /// </remarks>
        /// <value>
        /// The user ID.
        /// </value>
        public string ID
        {
            get
            {
                if (mId == null)
                {
                    mId = mJSON.OptString(PROPERTY_ID, null);
                }
                return mId;
            }
        }

        /// <summary>
        /// URI of this user.
        /// </summary>
        /// <remarks>
        /// To get KiiUser instance, call <see cref="CreateByUri(Uri)"/>.
        /// </remarks>
        /// <value>
        /// URI of this user.
        /// </value>
        public Uri Uri
        {
            get
            {
                Utils.CheckInitialize(false);

                string uuid = ID;
                if (Utils.IsEmpty(uuid))
                {
                    return null;
                }

                String url = Utils.Path(ConstantValues.URI_HEADER, "users", uuid);
                return new Uri(url);
            }
        }

        internal string KiiCloudAuthorityAndSegments
        {
            get
            {
                return Utils.Path("users", ID);
            }
        }

        internal static string MeUrl
        {
            get
            {
                return Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, "users", "me");
            }
        }

        internal string Url
        {
            get
            {
                return Utils.Path(Kii.BaseUrl, "apps", Kii.AppId, KiiCloudAuthorityAndSegments);
            }
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <remarks>
        /// If user is not logged in, return null.
        /// </remarks>
        /// <value>
        /// The access token.
        /// </value>
        public static string AccessToken
        {
            get
            {
                return KiiCloudEngine.AccessToken;
            }
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <remarks>
        /// If username is not set, return null.
        /// </remarks>
        /// <value>
        /// The username.
        /// </value>
        public string Username
        {
            get
            {
                return mJSON.OptString(PROPERTY_USERNAME, null);
            }
        }

        /// <summary>
        /// Return true if the user is disabled, false otherwise.
        /// </summary>
        /// <remarks>
        /// Call <see cref="Refresh()"/>, or <see cref="Refresh(KiiUserCallback)"/>
        /// prior calling this method to get correct status.
        /// </remarks>
        public Boolean Disabled
        {
            get
            {
                return mJSON.OptBoolean(PROPERTY_DISABLED);
            }
        }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <remarks>
        /// If email is not set, return null.
        /// </remarks>
        /// <value>
        /// The email.
        /// </value>
        public string Email
        {
            get
            {
                return GetString(PROPERTY_EMAIL, null);
            }
        }
        /// <summary>
        /// Get the email of this user that has not been verified.
        /// When the user's email has been changed and email verification is required in you app configuration,
        /// New email is stored as pending email.
        /// After the new email has been verified, the address can be obtained by <see cref="Email"/>
        /// </summary>
        /// <value>The pending email.</value>
        public string PendingEmail
        {
            get
            {
                return GetString(PROPERTY_PENDING_EMAIL, null);
            }
        }

        /// <summary>
        /// Gets the phone number.
        /// </summary>
        /// <remarks>
        /// If phone is not set, return null.
        /// </remarks>
        /// <value>
        /// The phone.
        /// </value>
        public string Phone
        {
            get
            {
                return GetString(PROPERTY_PHONE, null);
            }
        }

        /// <summary>
        /// Get the phone of this user that has not been verified.
        /// When the user's phone has been changed and phone verification is required in you app configuration,
        /// New phone is stored as pending phone.
        /// After the new phone has been verified, the number can be obtained by <see cref="Phone"/>
        /// </summary>
        /// <value>The pending phone.</value>
        public string PendingPhone
        {
            get
            {
                return GetString(PROPERTY_PENDING_PHONE, null);
            }
        }

        /// <summary>
        /// Gets or sets the displayname.
        /// </summary>
        /// <remarks>
        /// Display name must be valid.
        /// </remarks>
        /// <value>
        /// The displayname.
        /// </value>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        public string Displayname
        {
            get
            {
                return GetString(PROPERTY_DISPLAYNAME, null);
            }
            set
            {
                if (IsValidDisplayName(value))
                {
                    SetReserveProperty(PROPERTY_DISPLAYNAME, value);
                }
                else
                {
                    throw new ArgumentException(ErrorInfo.KIIUSER_DISPLAYNAME_INVALID + value);
                }
            }

        }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <remarks>
        /// Country must be valid.
        /// </remarks>
        /// <value>
        /// The country.
        /// </value>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid.
        /// </exception>
        public string Country
        {
            get
            {
                return GetString(PROPERTY_COUNTRY, null);
            }
            set
            {
                if (IsValidCountry(value))
                {
                    SetReserveProperty(PROPERTY_COUNTRY, value);
                }
                else
                {
                    throw new ArgumentException(ErrorInfo.KIIUSER_COUNTRY_INVALID + value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        /// <remarks>
        /// locale must be not null.
        /// </remarks>
        /// <value>
        /// The locale.
        /// </value>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is null.
        /// </exception>
        public LocaleContainer Locale {
            get
            {
                String localeString = GetString(KiiUser.PROPERTY_LOCALE, null);
                if (localeString == null)
                {
                    return null;
                }
                return LocaleContainer.FromBcp47Tag(localeString);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentException(ErrorInfo.KIIUSER_COUNTRY_INVALID + value);
                }
                SetReserveProperty(KiiUser.PROPERTY_LOCALE, value.LocaleString);
            }
        }

        /// <summary>
        /// Gets a value whether email verified on KiiCloud.
        /// </summary>
        /// <remarks>
        /// Email verification is done from the verification email sent to the registered email.
        /// </remarks>
        /// <value>
        /// <c>true</c> if email verified; otherwise, <c>false</c>.
        /// </value>
        public bool EmailVerified
        {
            get
            {
                return mJSON.OptBoolean(PROPERTY_EMAIL_ADDRESS_VERIFIED, false);
            }
        }

        /// <summary>
        /// Gets a value whether phone verified on KiiCloud.
        /// </summary>
        /// <remarks>
        /// Phone verification is done when VerifyPhone(string) is called successfully.
        /// </remarks>
        /// <value>
        /// <c>true</c> if phone verified; otherwise, <c>false</c>.
        /// </value>
        public bool PhoneVerified
        {
            get
            {
                return mJSON.OptBoolean(PROPERTY_PHONE_NUMBER_VERIFIED, false);
            }
        }

        private string Identifier
        {
            get
            {
                string identifier = null;
                identifier = Username;
                if (Utils.IsEmpty(identifier) && EmailVerified)
                {
                    identifier = Email;
                }
                if (Utils.IsEmpty(identifier) && PhoneVerified)
                {
                    identifier = Phone;
                }
                return identifier;
            }
        }

        /// <summary>
        /// Gets a value whether user is pseudo.
        /// </summary>
        /// <remarks>
        /// If this method is not called for current login user, calling the Refresh() method is necessary to get a correct value.
        /// </remarks>
        /// <value>
        /// <c>true</c> if user is pseudo; otherwise, <c>false</c>.
        /// </value>
        public bool IsPseudoUser
        {
            get
            {
                if (Has(PROPERTY_HAS_PASSWORD))
                {
                    return !GetBoolean(PROPERTY_HAS_PASSWORD);
                }
                else
                {
                    if (!Utils.IsEmpty(this.Username) || !Utils.IsEmpty(this.Phone) || !Utils.IsEmpty(this.Email))
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
        /// <summary>
        /// Gets the linked social accounts.
        /// </summary>
        /// <value> <![CDATA[ Dictionary<Provider,SocialAccountInfo> ]]> Provider as key and SocialAccountInfo as value</value>
        /// <remarks>
        /// Before call this method, calling the <see cref="Refresh()"/> method is necessary.
        /// Otherwise, empty list will be returned.
        /// </remarks>
        public Dictionary<Provider,SocialAccountInfo> LinkedSocialAccounts
        {
            get
            {
                JsonObject thirdPartyAccounts = mJSON.OptJsonObject("_thirdPartyAccounts");
                Dictionary<Provider,SocialAccountInfo> socialAccountInfoDict = new Dictionary<Provider, SocialAccountInfo>();
                if (thirdPartyAccounts == null || thirdPartyAccounts.Length() < 1)
                    return socialAccountInfoDict;

                IEnumerator<string> en = thirdPartyAccounts.Keys();
                while(en.MoveNext()) {
                    JsonObject info = thirdPartyAccounts.GetJsonObject(en.Current);
                    string providerName  = ProviderExtensions.GetProviderNameFromLinkedSocialNetworkName(en.Current);
                    Provider p = (Provider)Enum.Parse(typeof(Provider), providerName.ToUpper());
                    if (p == Provider.GOOGLE)
                    {
                        p = Provider.GOOGLEPLUS;
                    }
                    socialAccountInfoDict.Add(p, 
                                          new SocialAccountInfo(p, info.GetString("id"),
                                          info.GetLong("createdAt"))
                                        );
                }
                return socialAccountInfoDict;
            }
        }
        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <remarks>
        /// Current user is set when login is done.
        /// </remarks>
        /// <value>
        /// The current user.
        /// </value>
        public static KiiUser CurrentUser
        {
            get
            {
                Utils.CheckInitialize(false);
                return Kii.CurrentUser;
            }
        }

        #endregion

        #region KiiSubject

        /// <summary>
        /// Gets the subject string.
        /// </summary>
        /// <remarks>
        /// Developers don't need to use this property in their apps.
        /// </remarks>
        /// <value>
        /// The subject string.
        /// </value>
        public string Subject
        {
            get
            {
                return "UserID:" + ID;
            }
        }
        /// <summary>
        /// Get KiiPushSubscription instance of the user.
        /// The user will subscribe to the push service by using the returned instance.
        /// </summary>
        /// <remarks></remarks>
        /// <value>KiiPushSubscription Instance of KiiPushSubscription.</value>
        public  KiiPushSubscription PushSubscription
        {
            get
            {
                Utils.CheckInitialize(true);
                return new KiiPushSubscription(this);
            }
        }
        #endregion

        #region Inner Classes
        /// <summary>
        /// Builds KiiUser instance.
        /// </summary>
        /// <remarks>
        /// To create this instance. Please use
        /// <see cref="BuilderWithName(string)"/>,
        /// <see cref="BuilderWithEmail(string)"/>,
        /// <see cref="BuilderWithLocalPhone(string, string)"/> or
        /// <see cref="BuilderWithIdentifier(string)"/>.
        /// </remarks>
        public class Builder
        {
            private string phone;
            private string email;
            private string userName;
            private string country;

            internal Builder()
            {
                this.phone = null;
                this.email = null;
                this.userName = null;
                this.country = null;
            }

            /// <summary>
            /// Set phone
            /// </summary>
            /// <remarks>Phone must be valid.</remarks>
            /// <returns>
            /// Itself.
            /// </returns>
            /// <param name='phone'>
            /// Phone.
            /// </param>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid.
            /// </exception>
            [Obsolete("WithPhone() is deprecated, please use SetGlobalPhone(string) or SetLocalPhone(string, string) instead", false)]
            public Builder WithPhone(string phone)
            {
                if (!KiiUser.IsValidPhone(phone))
                {
                    throw new ArgumentException(
                        ErrorInfo.KIIUSER_PHONE_FORMAT_INVALID + phone);
                }
                this.phone = phone;
                return this;
            }

            /// <summary>
            /// Set global phone
            /// </summary>
            /// <remarks>Phone must be valid.</remarks>
            /// <returns>
            /// Itself.
            /// </returns>
            /// <param name='phone'>
            /// Phone.
            /// </param>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid.
            /// </exception>
            public Builder SetGlobalPhone(string phone)
            {
                if (!KiiUser.IsValidGlobalPhone(phone))
                {
                    throw new ArgumentException(
                        ErrorInfo.KIIUSER_PHONE_FORMAT_INVALID + phone);
                }
                this.phone = phone;
                this.country = null;
                return this;
            }

            /// <summary>
            ///   Set local phone.
            /// </summary>
            /// <remarks>
            ///   <para>
            ///     Phone and country must be valid.
            ///   </para>
            /// </remarks>
            /// <returns>
            ///   Itself.
            /// </returns>
            /// <param name="phone">
            /// Local phone.
            /// </param>
            /// <param name="country">
            /// Country.
            /// </param>
            /// <exception cref="System.ArgumentException">
            /// One of phone and country is not valid or both of phone
            /// and country are not valid.
            /// </exception>
            public Builder SetLocalPhone(string phone, string country)
            {
                if (!KiiUser.IsValidLocalPhone(phone))
                {
                    throw new ArgumentException(
                        ErrorInfo.KIIUSER_PHONE_FORMAT_INVALID + phone);
                }
                if (!KiiUser.IsValidCountry(country))
                {
                    throw new ArgumentException(
                        ErrorInfo.KIIUSER_COUNTRY_INVALID + country);
                }
                this.phone = phone;
                this.country = country;
                return this;
            }

            /// <summary>
            /// Set email
            /// </summary>
            /// <remarks>Email must be valid.</remarks>
            /// <returns>
            /// Itself
            /// </returns>
            /// <param name='email'>
            /// Email.
            /// </param>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid.
            /// </exception>
            [Obsolete("WithEmail() is deprecated, please use SetEmail(string) instead", false)]
            public Builder WithEmail(string email)
            {
                if (!KiiUser.IsValidEmail(email))
                {
                    throw new ArgumentException(
                        ErrorInfo.KIIUSER_EMAIL_FORMAT_INVALID + email);
                }
                this.email = email;
                return this;
            }

            /// <summary>
            /// Set email
            /// </summary>
            /// <remarks>Email must be valid.</remarks>
            /// <returns>
            /// Itself
            /// </returns>
            /// <param name='email'>
            /// Email.
            /// </param>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid.
            /// </exception>
            public Builder SetEmail(string email)
            {
                if (!KiiUser.IsValidEmail(email))
                {
                    throw new ArgumentException(
                        ErrorInfo.KIIUSER_EMAIL_FORMAT_INVALID + email);
                }
                this.email = email;
                return this;
            }

            /// <summary>
            /// Set username
            /// </summary>
            /// <remarks>Username must be valid.</remarks>
            /// <returns>
            /// Itself
            /// </returns>
            /// <param name='userName'>
            /// Username.
            /// </param>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid.
            /// </exception>
            [Obsolete("WithName() is deprecated, please use SetName(string) instead", false)]
            public Builder WithName(string userName)
            {
                if (!KiiUser.IsValidUserName(userName))
                {
                    throw new ArgumentException(
                        ErrorInfo.KIIUSER_USERNAME_INVALID + userName);
                }
                this.userName = userName;
                return this;
            }

            /// <summary>
            /// Set username
            /// </summary>
            /// <remarks>Username must be valid.</remarks>
            /// <returns>
            /// Itself
            /// </returns>
            /// <param name='userName'>
            /// Username.
            /// </param>
            /// <exception cref='ArgumentException'>
            /// Is thrown when an argument is invalid.
            /// </exception>
            public Builder SetName(string userName)
            {
                if (!KiiUser.IsValidUserName(userName))
                {
                    throw new ArgumentException(
                        ErrorInfo.KIIUSER_USERNAME_INVALID + userName);
                }
                this.userName = userName;
                return this;
            }

            /// <summary>
            /// Build KiiUser instance.
            /// </summary>
            /// <remarks>Build KiiUser instance with username, email and phone if they're set.</remarks>
            /// <returns>
            /// KiiUser instance.
            /// </returns>
            /// <exception cref='InvalidOperationException'>
            /// Is thrown when username, email and phone are null.
            /// </exception>
            public KiiUser Build()
            {
                if (this.userName == null && this.email == null
                    && this.phone == null)
                {
                    throw new InvalidOperationException(
                        "At least one of userName, phone, or email must be specified.");
                }
                KiiUser user = new KiiUser();
                if (this.userName != null)
                {
                    user.SetReserveProperty(PROPERTY_USERNAME, this.userName);
                }
                if (this.email != null)
                {
                    user.SetReserveProperty(PROPERTY_EMAIL, this.email);
                }
                if (this.phone != null)
                {
                    user.SetReserveProperty(PROPERTY_PHONE, this.phone);
                }
                if (this.country != null)
                {
                    user.SetReserveProperty(PROPERTY_COUNTRY, this.country);
                }
                return user;
            }
        }
        /// <summary>
        /// Social result.
        /// </summary>
        public static class SocialResultParams
        {
            /// <summary>
            /// User's access token
            /// </summary>
            public const string KII_NEW_USER = "kii_new_user";
            /// <summary>
            /// OAuth Token (shown if is returned by social provider)
            /// </summary>
            public const string OAUTH_TOKEN = "oauth_token";
            /// <summary>
            /// OAuth Token Secret (shown if is returned by social provider)
            /// </summary>
            public const string OAUTH_TOKEN_SECRET = "oauth_token_secret";
            /// <summary>
            /// The ID that identifies the social user running the application.
            /// </summary>
            public const string PROVIDER_USER_ID = "provider_user_id";
            /// <summary>
            /// Provider name
            /// </summary>
            public const string PROVIDER = "provider";
            /// <summary>
            /// Open ID for QQ
            /// </summary>
            public const string OPEN_ID = "openID";
        }
        #endregion

        #region KiiUser.Equals

        /// <summary>
        /// Serves as a hash function for a <see cref="KiiCorp.Cloud.Storage.KiiUser"/> object.
        /// </summary>
        /// <remarks>
        /// If this KiiUser instance has user ID, hash code is based on its ID.
        /// Otherwise, hash codes is based on an object's reference.
        /// </remarks>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                if (Utils.IsEmpty(this.ID))
                {
                    return base.GetHashCode();
                }
                else
                {
                    return this.ID.GetHashCode();
                }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="KiiCorp.Cloud.Storage.KiiUser"/> is equal to the current <see cref="KiiCorp.Cloud.Storage.KiiUser"/>.
        /// </summary>
        /// <remarks>
        /// Return true when the specified <see cref="KiiCorp.Cloud.Storage.KiiUser"/> is in following conditions.
        /// <list type="bullet">
        ///   <item><term>KiiUser ID is equal to this one.</term></item>
        ///   <item><term>(If KiiUser does not have ID) KiiUser instance is equal to this one.</term></item>
        /// </list>
        /// </remarks>
        /// <param name="other">The <see cref="KiiCorp.Cloud.Storage.KiiUser"/> to compare with the current <see cref="KiiCorp.Cloud.Storage.KiiUser"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="KiiCorp.Cloud.Storage.KiiUser"/> is equal to the current
        /// <see cref="KiiCorp.Cloud.Storage.KiiUser"/>; otherwise, <c>false</c>.</returns>
        bool IEquatable<KiiUser>.Equals(KiiUser other)
        {
            if (other == null)
            {
                return false;
            }

            string userID = this.ID;
            if (Utils.IsEmpty(userID))
            {
                return base.Equals(other);
            }
            else
            {
                return this.ID == other.ID;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="KiiCorp.Cloud.Storage.KiiUser"/>.
        /// </summary>
        /// <remarks>
        /// Return true when the specified <see cref="KiiCorp.Cloud.Storage.KiiUser"/> is in following conditions.
        /// <list type="bullet">
        ///   <item><term>KiiUser ID is equal to this one.</term></item>
        ///   <item><term>(If KiiUser does not have ID) KiiUser instance is equal to this one.</term></item>
        /// </list>
        /// </remarks>
        /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="KiiCorp.Cloud.Storage.KiiUser"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
        /// <see cref="KiiCorp.Cloud.Storage.KiiUser"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            KiiUser other = (KiiUser)obj;
            string userID = this.ID;
            if (Utils.IsEmpty(userID))
            {
                return base.Equals(other);
            }
            else
            {
                return this.ID == other.ID;
            }
        }

        #endregion

        #region GetAccessTokenDictionary
        /// <summary>
        /// Return the access tokens in a dictionary.
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///   <listheader>
        ///     <term>Key</term>
        ///     <description>Type</description>
        ///     <description>Value</description>
        ///   </listheader>
        ///   <item>
        ///     <term>access_token</term>
        ///     <description>string</description>
        ///     <description>required for accessing KiiCloud.
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <term>expires_at</term>
        ///     <description>DateTime</description>
        ///     <description>
        ///         Access token expiration time.
        ///     </description>
        ///   </item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// Access token dictionary or null if user is not logged-in.
        /// </returns>

        public Dictionary<string, object> GetAccessTokenDictionary () {
            if (KiiUser.CurrentUser == null)
                return null;
            Dictionary<string, object> dictionary = new Dictionary<string, object >();
            dictionary.Add("access_token", AccessToken);
            dictionary.Add("expires_at", mAccessTokenExpiresAt);
            return dictionary;
        }

        #endregion

        #region GetSocialAccessTokenDictionary
        /// <summary>
        /// Sets the social access token dictionary.
        /// </summary>
        /// <param name="socialAccessTokenDictionary">Social access token dictionary.</param>
        internal void SetSocialAccessTokenDictionary(Dictionary<string, object> socialAccessTokenDictionary) {
            mSocialAccessTokenDictionary = socialAccessTokenDictionary;
            if (mSocialAccessTokenDictionary.ContainsKey("kii_expires_in"))
            {
                long expiresIn = Convert.ToInt64(
                        mSocialAccessTokenDictionary["kii_expires_in"]);
                mAccessTokenExpiresAt = SafeCalculateExpireDate(expiresIn,
                        DateTime.UtcNow);
                mSocialAccessTokenDictionary.Remove("kii_expires_in");
            }
        }
        /// <summary>
        /// Return the access token and related information of access token
        /// for logged-in social network in a dictionary.
        /// </summary>
        /// <remarks>
        /// <list type="table">
        ///   <listheader>
        ///     <term>Key</term>
        ///     <description>Type</description>
        ///     <description>Value</description>
        ///   </listheader>
        ///   <item>
        ///     <term>oauth_token</term>
        ///     <description>string</description>
        ///     <description>access token for the provider.
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <term>oauth_token_secret</term>
        ///     <description>string</description>
        ///     <description>access token secret for the provider.
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <term>provider_user_id</term>
        ///     <description>string</description>
        ///     <description>
        ///         user id of the provider
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <term>provider</term>
        ///     <description>KiiCorp.Cloud.Storage.Connector.Provider</description>
        ///     <description>
        ///         value to identity the provider.
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <term>kii_new_user</term>
        ///     <description>bool</description>
        ///     <description>
        ///         Indicates if user was created during connection.
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <term>oauth_token_expires_in</term>
        ///     <description>long</description>
        ///     <description>
        ///         Social provider's token expiration time.
        ///     </description>
        ///   </item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// Access token and related information dictionary for the provider
        /// or null if user is not logged-in a social network.
        /// </returns>
        public Dictionary<string, object> GetSocialAccessTokenDictionary() {
            return mSocialAccessTokenDictionary;
        }
        #endregion

        private static string GetQualifiedID(string identifier)
        {
            if (IsValidEmail(identifier))
            {
                return "EMAIL:" + identifier;
            }
            else if (Utils.IsGlobalPhoneNumber(identifier))
            {
                return "PHONE:" + identifier;
            }
            else
            {
                return identifier;
            }
        }
    }
    internal static class NotificationMethodExtensions
    {
        internal static string GetNotificationMethod(this KiiUser.NotificationMethod m)
        {
            switch (m)
            {
                case KiiUser.NotificationMethod.EMAIL:
                    return "EMAIL";
                case KiiUser.NotificationMethod.SMS:
                    return "SMS";
                case KiiUser.NotificationMethod.SMS_PIN:
                    return "SMS";
            }
            return null;
        }
        internal static string GetSmsResetMethod(this KiiUser.NotificationMethod m)
        {
            switch (m)
            {
                case KiiUser.NotificationMethod.EMAIL:
                    return null;
                case KiiUser.NotificationMethod.SMS:
                    return "URL";
                case KiiUser.NotificationMethod.SMS_PIN:
                    return "PIN";
            }
            return null;
        }
    }
}
