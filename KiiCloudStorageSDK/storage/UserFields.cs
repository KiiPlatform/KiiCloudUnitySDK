using System;
using System.Globalization;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The User Information except for the Identity Data.
    /// </summary>
    public class UserFields : KiiBaseObject
    {
        private List<string> removedFields = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.UserFields"/> class.
        /// </summary>
        public UserFields() : base(KiiUser.ReservedKeys)
        {
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
                return GetString(KiiUser.PROPERTY_DISPLAYNAME, null);
            }
            set
            {
                if (KiiUser.IsValidDisplayName(value))
                {
                    SetReserveProperty(KiiUser.PROPERTY_DISPLAYNAME, value);
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
                return GetString(KiiUser.PROPERTY_COUNTRY, null);
            }
            set
            {
                if (KiiUser.IsValidCountry(value))
                {
                    SetReserveProperty(KiiUser.PROPERTY_COUNTRY, value);
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
        internal String LocaleString
        {
            get
            {
                return GetString(KiiUser.PROPERTY_LOCALE, null);
            }
        }
        /// <summary>
        /// Gets the remove fields.
        /// </summary>
        /// <value>The remove fields.</value>
        internal string[] RemovedFields
        {
            get
            {
                return this.removedFields.ToArray();
            }
        }
        /// <summary>
        /// Remove a pair of key/value from UserFields.
        /// This pair is also removed from server when <see cref="KiiUser.Update(IdentityData, UserFields, KiiUserCallback)"/> is succeeded.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <remarks>
        /// <see cref="UserFields"/> has a similar method which is <see cref="UserFields.Remove"/>
        /// If you remove fields by <see cref="UserFields.Remove"/> , pairs are not removed from server.
        /// So you can use <see cref="UserFields.Remove"/> if you want to cancel to update fields which are set in <see cref="UserFields"/>.
        /// </remarks>
        public void RemoveFromServer(string key)
        {
            base.Remove(key);
            if (!this.removedFields.Contains(key))
            {
                this.removedFields.Add(key);
            }
        }
        /// <summary>
        /// Remove the specified key and value.
        /// </summary>
        /// <remarks>
        /// Key must be valid and should not be reserved.
        /// </remarks>
        /// <param name='key'>
        /// Key you want to remove
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when key is empty or reserved.
        /// </exception>
        public override void Remove(string key)
        {
            base.Remove(key);
            this.removedFields.Remove(key);
        }
        /// <summary>
        /// Removes the displayname from this instance.
        /// </summary>
        /// <remarks>
        /// Even if you use this method, display name is not removed from server.
        /// </remarks>
        public void RemoveDisplayname()
        {
            if (mJSON.Has(KiiUser.PROPERTY_DISPLAYNAME))
            {
                mJSON.Remove(KiiUser.PROPERTY_DISPLAYNAME);
            }
            if (mJSONPatch.Has(KiiUser.PROPERTY_DISPLAYNAME))
            {
                mJSONPatch.Remove(KiiUser.PROPERTY_DISPLAYNAME);
            }
        }
        /// <summary>
        /// Removes the country from this instance.
        /// </summary>
        /// <remarks>
        /// Even if you use this method, country is not removed from server.
        /// </remarks>
        public void RemoveCountry()
        {
            if (mJSON.Has(KiiUser.PROPERTY_COUNTRY))
            {
                mJSON.Remove(KiiUser.PROPERTY_COUNTRY);
            }
            if (mJSONPatch.Has(KiiUser.PROPERTY_COUNTRY))
            {
                mJSONPatch.Remove(KiiUser.PROPERTY_COUNTRY);
            }
        }
        /// <summary>
        /// Removes the locale from this instance.
        /// </summary>
        /// <remarks>
        /// Even if you use this method, locale is not removed from server.
        /// </remarks>
        public void RemoveLocale()
        {
            if (mJSON.Has(KiiUser.PROPERTY_LOCALE))
            {
                mJSON.Remove(KiiUser.PROPERTY_LOCALE);
            }
            if (mJSONPatch.Has(KiiUser.PROPERTY_LOCALE))
            {
                mJSONPatch.Remove(KiiUser.PROPERTY_LOCALE);
            }
        }
        /// <summary>
        /// Gets or sets the object with the specified key.
        /// </summary>
        /// <remarks>
        /// As the type of value is <see cref="object"/>, developers need to cast value.
        /// If you want to suppress IllegalKiiBaseObjectFormatException, use GetXxx(key, fallback)
        /// </remarks>
        /// <param name="key">Key.</param>
        /// <value>
        /// Value associated with the key.
        /// </value>
        /// <exception cref='ArgumentNullException'>
        /// Is thrown when key is null.
        /// </exception>
        /// <exception cref='IllegalKiiBaseObjectFormatException'>
        /// Is thrown when specified key is not existed.
        /// </exception>
        public override object this[string key]
        {
            get
            {
                return base[key];
            }
            set
            {
                base[key] = value;
                this.removedFields.Remove(key);
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get
            {
                if (this.removedFields.Count > 0)
                {
                    return false;
                }
                if (mJSON.Keys().MoveNext())
                {
                    return false;
                }
                if (mJSONPatch.Keys().MoveNext())
                {
                    return false;
                }
                return true;
            }
        }
    }
}

