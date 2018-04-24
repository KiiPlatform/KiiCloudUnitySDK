using System;

using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents query interface.
    /// </summary>
    /// <remarks>
    /// Developers can set the conditions with <see cref="KiiClause"/>
    /// <code>
    /// KiiQuery q = new KiiQuery(KiiClause.GreaterThan("score", 80));
    /// q.Limit = 10;
    /// q.SortByDesc("score");
    /// </code>
    /// </remarks>
    public class KiiQuery
    {

        private JsonObject mJson = null;
        private string nextPaginationKey = null;
        private int mLimit;

        private static JsonObject ALLCLAUSE = new JsonObject();

        static KiiQuery()
        {
            ALLCLAUSE.Put("type", "all");
        }

        /// <summary>
        /// Create new instance and copy all fields.
        /// </summary>
        /// <param name="query">Query.</param>
        internal static KiiQuery copy(KiiQuery query)
        {
            return new KiiQuery(query);
        }

        /// <summary>
        /// Create Query.
        /// </summary>
        /// <remarks>
        /// Matches all items in the bucket.
        /// </remarks>
        public KiiQuery() : this((KiiClause)null) {
        }

        /// <summary>
        /// Create Query from <see cref="KiiClause"/>
        /// </summary>
        /// <remarks>
        /// If clause is null, matches all items in the bucket.
        /// </remarks>
        /// <param name='clause'>
        /// Clause.
        /// </param>
        public KiiQuery(KiiClause clause)
        {
            mJson = new JsonObject();
            try
            {
                if(clause == null)
                {
                    mJson.Put("clause", ALLCLAUSE);
                }
                else
                {
                    mJson.Put("clause", clause.ToJson());
                }
            }
            catch (JsonException e)
            {
                throw new SystemException("Query clause generate error", e);
            }
        }
        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="query">Query.</param>
        private KiiQuery(KiiQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query is null");
            }
            mJson = new JsonObject(query.mJson.ToString());
            mLimit = query.mLimit;
        }

        /// <summary>
        /// Sort with the specified key in descending order.
        /// </summary>
        /// <remarks>
        /// The type of value associated with key must be number or string.
        /// </remarks>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when key is empty.
        /// </exception>
        public void SortByDesc(string key)
        {
            if(Utils.IsEmpty(key))
            {
                throw new ArgumentException("Empty key is not acceptable.");
            }
            try
            {
                mJson.Put("orderBy", key);
                mJson.Put("descending", true);
            }
            catch (JsonException jse)
            {
                throw new ArgumentException("Invalid arugment.", jse);
            }
        }

        /// <summary>
        /// Sort with the specified key in ascending order.
        /// </summary>
        /// <remarks>
        /// The type of value associated with key must be number or string.
        /// </remarks>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when key is empty.
        /// </exception>
        public void SortByAsc(string key)
        {
            if(Utils.IsEmpty(key))
            {
                throw new ArgumentException("Empty key is not acceptable.");
            }
            try
            {
                mJson.Put("orderBy", key);
                mJson.Put("descending", false);
            }
            catch (JsonException jse)
            {
                throw new ArgumentException("Invalid arugment.", jse);
            }
        }

        internal JsonObject toJson()
        {
            JsonObject query = new JsonObject();
            try {
                query.Put("bucketQuery", mJson);
                if(NextPaginationKey != null)
                    query.Put("paginationKey", NextPaginationKey);
                if(mLimit > 0)
                    query.Put("bestEffortLimit", mLimit);
            } catch (JsonException jse) {
                // Wont happen
                throw new SystemException("Unexpected error.", jse);
            }
            return query;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="KiiCorp.Cloud.Storage.KiiQuery"/>.
        /// </summary>
        /// <remarks>
        /// Developers don't need to use this API in their apps.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.String"/> that represents the current <see cref="KiiCorp.Cloud.Storage.KiiQuery"/>.
        /// </returns>
        public override string ToString()
        {
            return toJson().ToString();
        }


        #region properties
        internal string NextPaginationKey
        {
            get
            {
                return nextPaginationKey;
            }
            set
            {
                nextPaginationKey = value;
            }
        }

        /// <summary>
        /// Set the maximum number of results returned on response. 
        /// This limit behaves in a best effort way, actual number of 
        /// returned result could be smaller than requested number but never exceed the limit.
        /// </summary>
        /// <remarks>
        /// If specified limit is &lt;= 0, 0 will be applied.
        /// If the specified limit is greater than the limit of the server or limit is set to 0, limit defined in server will be applied.
        /// </remarks>
        /// <value>
        /// Maximum return items.
        /// </value>
        public int Limit
        {
            set
            {
                if (value > 0)
                {
                    mLimit = value;
                }
                else
                {
                    mLimit = 0;
                }
            }
        }
        #endregion

    }
}

