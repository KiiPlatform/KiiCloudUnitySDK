using System;
using System.Collections.Generic;
using System.Globalization;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// The purpose of this class is to support the BCP 47 format.
    /// Some language code is not supported by System.Globalization.CultureInfo
    /// </summary>
    public class LocaleContainer
    {
        /// <summary>
        /// zh-CHS and zh-CHT are legacy language code for Chinese.
        /// If client sends these language codes to the server, UFE converts language code to 'chs' and 'cht'.
        /// CultureInfo class cannot handle'chs' and 'cht'.
        /// Probably these language codes are not available on the real device.
        /// But LocaleContainer handles these language codes just in case.
        /// </summary>
        private static Dictionary<string, string> Name2IetfLanguageTag = new Dictionary<string, string>();

        static LocaleContainer()
        {
            Name2IetfLanguageTag.Add("zh-CHS", "zh-CN");
            Name2IetfLanguageTag.Add("zh-CHT", "zh-HK");
        }

        /// <summary>
        /// Create Bcp47Locale instance by specified BPC47 tag.
        /// </summary>
        /// <returns>The bcp47 tag.</returns>
        /// <param name="bcp47Tag">Bcp47 tag.</param>
        public static LocaleContainer FromBcp47Tag(string bcp47Tag)
        {
            if (String.IsNullOrEmpty(bcp47Tag))
            {
                throw new ArgumentNullException("bcp47Tag is null or empty");
            }
            CultureInfo cultureInfo = null;
            try
            {
                cultureInfo = CultureInfo.CreateSpecificCulture(bcp47Tag);
            }
            catch
            {
                try 
                {
                    // Try to parse the BPC47 tag using only language sub tag.
                    cultureInfo = CultureInfo.CreateSpecificCulture(bcp47Tag.Split('-')[0]);
                }
                catch
                {
                }
            }
            return new LocaleContainer(cultureInfo, bcp47Tag);
        }
        
        private CultureInfo cultureInfo;
        private string localeString;

        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.LocaleContainer"/> class using current CultureInfo.
        /// </summary>
        public LocaleContainer() : this(CultureInfo.CurrentCulture)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.LocaleContainer"/> class.
        /// </summary>
        /// <param name="cultureInfo">Culture info.</param>
        public LocaleContainer(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                throw new ArgumentNullException("cultureInfo is null");
            }
            this.cultureInfo = cultureInfo;
            this.localeString = null;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="KiiCorp.Cloud.Storage.LocaleContainer"/> class.
        /// </summary>
        /// <param name="cultureInfo">Culture info.</param>
        /// <param name="localeString">Locale string.</param>
        internal LocaleContainer(CultureInfo cultureInfo, string localeString)
        {
            this.cultureInfo = cultureInfo;
            this.localeString = localeString;
        }

        /// <summary>
        /// Get the locale as System.Globalization.CultureInfo class.
        /// This property returns null if CultureInfo does not support the BCP 47 tag which server returned.
        /// </summary>
        /// <value>The culture info.</value>
        public CultureInfo CultureInfo
        {
            get
            {
                return this.cultureInfo;
            }
        }
        /// <summary>
        /// Get the BCP47 Tag string which server returned.
        /// </summary>
        /// <value>The locale string.</value>
        public string LocaleString
        {
            get
            {
                if (!String.IsNullOrEmpty(this.localeString))
                {
                    return this.localeString;
                }
                if (Name2IetfLanguageTag.ContainsKey(this.cultureInfo.Name))
                {
                    return Name2IetfLanguageTag[this.cultureInfo.Name];
                }
                return this.cultureInfo.Name;
            }
        }
    }
}

