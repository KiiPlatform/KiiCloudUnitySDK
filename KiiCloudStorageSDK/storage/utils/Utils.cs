using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Contains utilities that the KiiSDK uses.
    /// </summary>
    /// <remarks></remarks>
    public class Utils
    {
        private static DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private const string USERNAME_PATTERN_REX = "^[a-zA-Z0-9-_\\.]{3,64}$";
        private const string PHONE_NUMBER_REX = "^[\\+]?[0-9.-]+$";
        private const string GLOBAL_PHONE_NUMBER_REX = "^\\+[0-9.-]{2,15}$";
        private const string LOCAL_PHONE_PATTERN_REX = "^[0-9]*$";
        private const string EMAIL_ADDRESS_REX = "^[a-zA-Z0-9\\+\\.\\%\\-\\+_]{1,256}" +
            "@" +
            "[a-zA-Z0-9][a-zA-Z0-9\\-]{0,64}" +
            "(" +
                "\\." +
                "[a-zA-Z0-9][a-zA-Z0-9\\-]{0,25}" +
            ")+$";
        private const string PASSWORD_PATTERN_REX = "^[\\u0020-\\u007E]{4,50}$";
        private const string COUNTRY_PATTERN_REX = "^[A-Z]{2}$";
        private const string BUCKET_NAME_PATTERN_REX = "^[a-zA-Z0-9-_]{2,64}$";
        private const string OBJECT_ID_PATTERN_REX = "^[a-zA-Z0-9-_\\.]{2,100}$";
        private const string GROUP_ID_PATTERN_REX = "^[a-z0-9._-]{1,30}$";


        /// <summary>
        /// Checks if a String contains text.
        /// </summary>
        /// <returns>true if the String is empty or null.</returns>
        /// <param name="value">Value.</param>
        /// <remarks></remarks>
        public static bool IsEmpty(string value) {
            if (value == null)
            {
                return true;
            }
            if (value.Length == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Validates the username.
        /// </summary>
        /// <returns><c>true</c>, if username was validated, <c>false</c> otherwise.</returns>
        /// <param name="username">Username.</param>
        /// <remarks></remarks>
        public static bool ValidateUsername(string username){
            return Regex.IsMatch(username, USERNAME_PATTERN_REX);
        }
        /// <summary>
        /// Validates the email.
        /// </summary>
        /// <returns><c>true</c>, if email was validated, <c>false</c> otherwise.</returns>
        /// <param name="email">Email.</param>
        /// <remarks></remarks>
        public static bool ValidateEmail (string email)
        {
            return Regex.IsMatch(email, EMAIL_ADDRESS_REX);
        }
        /// <summary>
        /// Determines if is global phone number the specified phone.
        /// </summary>
        /// <returns><c>true</c> if is global phone number the specified phone; otherwise, <c>false</c>.</returns>
        /// <param name="phone">Phone.</param>
        public static bool IsGlobalPhoneNumber(string phone)
        {
            if (String.IsNullOrEmpty(phone))
            {
                return false;
            }
            return Regex.IsMatch(phone, GLOBAL_PHONE_NUMBER_REX);
        }
        /// <summary>
        /// Validates the phone number.
        /// </summary>
        /// <returns><c>true</c>, if phone number was validated, <c>false</c> otherwise.</returns>
        /// <param name="phone">Phone.</param>
        /// <remarks></remarks>
        public static bool ValidatePhoneNumber (string phone)
        {
            return Regex.IsMatch(phone, PHONE_NUMBER_REX);
        }
        /// <summary>
        /// Validates the global phone number.
        /// </summary>
        /// <returns><c>true</c>, if global phone number was validated, <c>false</c> otherwise.</returns>
        /// <param name="number">Number.</param>
        /// <remarks></remarks>
        public static bool ValidateGlobalPhoneNumber(string number) {
            if (IsEmpty(number))
            {
                return false;
            }
            return Regex.IsMatch(number, GLOBAL_PHONE_NUMBER_REX);
        }
        /// <summary>
        /// Validates the local phone number.
        /// </summary>
        /// <returns><c>true</c>, if local phone number was validated, <c>false</c> otherwise.</returns>
        /// <param name="number">Number.</param>
        /// <remarks></remarks>
        public static bool ValidateLocalPhoneNumber(string number) {
            return Regex.IsMatch(number, LOCAL_PHONE_PATTERN_REX);
        }
        /// <summary>
        /// Validates the password.
        /// </summary>
        /// <returns><c>true</c>, if password was validated, <c>false</c> otherwise.</returns>
        /// <param name="pw">Pw.</param>
        /// <remarks></remarks>
        public static bool ValidatePassword(string pw)
        {
            return Regex.IsMatch(pw, PASSWORD_PATTERN_REX);
        }
        /// <summary>
        /// Validates the displayname.
        /// </summary>
        /// <returns><c>true</c>, if specified displayname length is 1-50 chars,
        /// <c>false</c> otherwise.</returns>
        /// <param name="displayname">Displayname.</param>
        /// <remarks></remarks>
        public static bool ValidateDisplayname(string displayname) {
            if (Utils.IsEmpty(displayname)) {
                return false;
            }
            return (displayname.Length >= 1) && (displayname.Length <= 50);
        }
        /// <summary>
        /// Validates the country.
        /// </summary>
        /// <returns><c>true</c>, if country was validated, <c>false</c> otherwise.</returns>
        /// <param name="country">Country.</param>
        /// <remarks></remarks>
        public static bool ValidateCountry(string country){
            return Regex.IsMatch(country, COUNTRY_PATTERN_REX);
        }
        /// <summary>
        /// Validates the name of the bucket.
        /// </summary>
        /// <returns><c>true</c>, if bucket name was validated, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        /// <remarks></remarks>
        public static bool ValidateBucketName(String name)
        {
            return Regex.IsMatch(name, BUCKET_NAME_PATTERN_REX);
        }
        /// <summary>
        /// Validates the given ID of the group.
        /// Alphanumeric string with less than 100 character, symbol _- are allowed. 
        /// </summary>
        /// <returns><c>true</c>, if group id is valid, <c>false</c> otherwise.</returns>
        /// <param name="groupID">Group ID to be checked.</param>
        /// <remarks></remarks>
        public static bool ValidateGroupID(string groupID)
        {
            return (groupID == null) ? false : Regex.IsMatch(groupID, GROUP_ID_PATTERN_REX);
        }
        /// <summary>
        /// Validates the given ID of the object.
        /// Valid pattern: ^[a-zA-Z0-9-_\\.]{2,100}$
        /// </summary>
        /// <returns><c>true</c>, if object id is valid, <c>false</c> otherwise.</returns>
        /// <param name="objectID">Object ID to be checked.</param>
        /// <remarks></remarks>
        public static bool ValidateObjectID(string objectID)
        {
            return (objectID == null) ? false : Regex.IsMatch(objectID, OBJECT_ID_PATTERN_REX);
        }
        /// <summary>
        /// Checks if the SDK is initialized.
        /// </summary>
        /// <param name="checkLogin">true if the SDK is initialized.</param>
        /// <remarks></remarks>
        public static void CheckInitialize(bool checkLogin)
        {
            if(Kii.AppId == null){
                throw new InvalidOperationException(ErrorInfo.UTILS_KIICLIENT_NULL);
            }

            if(checkLogin && Kii.CurrentUser == null){
                throw new InvalidOperationException(ErrorInfo.UTILS_NO_LOGIN);
            }
        }
        /// <summary>
        /// Combines some strings into a path.
        /// </summary>
        /// <param name="segments">Segments.</param>
        /// <remarks></remarks>
        public static string Path(params object[] segments) {
            string path = "";
            bool first = true;
            foreach (object segment in segments) {
                object s = segment;
                if (s is object[]) {
                    object[] v = (object[])segment;
                    s = Utils.Path(v);
                }
                if (!IsEmpty(s)) {
                    if (first) {
                        path = s.ToString();
                        first = false;
                    } else {
                        if ((!path.EndsWith("/") && (!segment.ToString().StartsWith("/")))) {
                            path += "/";
                        }
                        path += segment.ToString();
                    }
                }
            }
            return path;
        }

        /// <summary>
        /// Checks if a Object has value.
        /// </summary>
        /// <returns>true if the Object is empty or null.</returns>
        /// <param name="s">string.</param>
        /// <remarks></remarks>
        public static bool IsEmpty(object s) {
            if (s == null) {
                return true;
            }
            if ((s is String) && (((String) s).Trim().Length == 0)) {
                return true;
            }
            if (s is IList) {
                return ((IList)s).Count == 0;
            }
            if (s is IDictionary) {
                return ((IDictionary)s).Count == 0;
            }
            return false;
        }

        /// <summary>
        /// Removes the last slash if input string has.
        /// </summary>
        /// <returns>
        /// String whose last slash is removed
        /// </returns>
        /// <param name='input'>
        /// Input string.
        /// </param>
        public static string RemoveLastSlash(string input)
        {
            if (!input.EndsWith("/"))
            {
                return input;
            }
            return input.Substring(0, input.Length - 1);
        }
        /// <summary>
        /// Gets the current time mills.
        /// </summary>
        /// <value>The current time mills.</value>
        public static long CurrentTimeMills 
        {
            get
            {
                DateTime nowUTC = DateTime.Now.ToUniversalTime();
                TimeSpan elapsedTime = nowUTC - UNIX_EPOCH;
                return (long)elapsedTime.TotalMilliseconds;
            }
        }
        /// <summary>
        /// Unixs the time to date time.
        /// </summary>
        /// <returns>The time to date time.</returns>
        /// <param name="unixTimeMilliseconds">Unix time milliseconds.</param>
        public static DateTime UnixTimeToDateTime(long unixTimeMilliseconds)
        {
            return UNIX_EPOCH.AddMilliseconds(unixTimeMilliseconds);
        }
        /// <summary>
        /// Throws the exception.
        /// </summary>
        /// <param name="ex">Ex.</param>
        public static void ThrowException(GroupOperationException ex)
        {
/*
            Throwable exp =  e.getCause();
            if (exp instanceof BadRequestException)
                throw (BadRequestException) exp;
            if (exp instanceof ConflictException)
                throw (ConflictException) exp;
            if (exp instanceof ForbiddenException)
                throw (ForbiddenException) exp;
            if (exp instanceof NotFoundException)
                throw (NotFoundException) exp;
            if (exp instanceof UnauthorizedException)
                throw (UnauthorizedException) exp;
            if (exp instanceof UndefinedException)
                throw (UndefinedException) exp;
            if (exp instanceof IOException)
                throw (IOException) exp;
            throw new SystemException(
                    SystemException.Reason.__UNKNOWN__.toString(), e,
                    SystemException.Reason.__UNKNOWN__);
*/                    
        }
        public static string EscapeUriString(string str)
        {
            return Uri.EscapeUriString(str);
        }
        public static V GetDictionaryValue<K, V>(Dictionary<K, V> accessCredential, K key)
        {
            if (accessCredential == null || accessCredential.Count == 0)
            {
                return default(V);
            }
            try
            {
                return accessCredential[key];
            }
            catch (KeyNotFoundException e)
            {
                return default(V);
            }
        }
    }


}

