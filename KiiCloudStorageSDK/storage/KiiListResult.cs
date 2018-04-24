using System;
using System.Collections.Generic;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Kii list result.
    /// </summary>
    public class KiiListResult<T>
    {
        private List<T> result; 
        private string paginationKey;

        internal KiiListResult(List<T> result) : this(result, null)
        {
        }
        internal KiiListResult(List<T> result, string paginationKey)
        {
            this.result = result;
            this.paginationKey = paginationKey;
        }
        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <value>Returns empty list if there's no results.</value>
        public List<T> Result
        {
            get
            {
                return this.result;
            }
        }
        /// <summary>
        /// Gets the pagination key to retrieve the pending results.
        /// </summary>
        /// <value>Returns null if HasNext returns false.</value>
        public string PaginationKey
        {
            get
            {
                return this.paginationKey;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this result has more data.
        /// </summary>
        /// <value><c>true</c> if there are pending results; otherwise, <c>false</c>.</value>
        public bool HasNext
        {
            get
            {
                return this.paginationKey != null;
            }
        }
    }
}

