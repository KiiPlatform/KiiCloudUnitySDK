using System;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Kii http header.
    /// </summary>
    public interface KiiHttpHeaderList
    {
        /// <summary>
        /// Sets the Http header with the specified name.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <remarks></remarks>
        string this[string key] { set;}
    }
}

