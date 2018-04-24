using System;
using JsonOrg;

namespace KiiCorp.Cloud.Storage
{
    /// <summary>
    /// Represents supported field type for hasFieldClause.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public enum FieldType
    {
        /// <summary>
        /// String Field Type.
        /// </summary>
        STRING,
        /// <summary>
        /// Integer Field Type.
        /// </summary>
        INTEGER,
        /// <summary>
        /// Decimal Field Type.
        /// </summary>
        DECIMAL,
        /// <summary>
        /// Boolean Field Type.
        /// </summary>
        BOOLEAN
    }
    /// <summary>
    /// Provides APIs to construct query condition.
    /// </summary>
    /// <remarks>
    /// This class is used for building <see cref="KiiQuery"/>
    /// <code>
    /// // example 1 : I want to get object whose name is "John"
    /// KiiQuery q = new KiiQuery(KiiClause.Equals("name", "John"));
    ///
    /// // example 2 : I want to get objects whose age >= 18 and score > 80
    /// KiiQuery q = new KiiQuery(
    ///     KiiClause.And(
    ///         KiiClause.GreaterThanOrEqual("age", 18),
    ///         KiiClause.GreaterThan("score", 80))
    ///     );
    /// </code>
    /// </remarks>
    public class KiiClause
    {
        private JsonObject mJson = null;

        private KiiClause() {
        }

        internal JsonObject ToJson(){
            return mJson;
        }

        #region Equals
        /// <summary>
        /// Create a clause of equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause Equals(string key, int value)
        {
            return EqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause Equals(string key, long value)
        {
            return EqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause Equals(string key, double value)
        {
            return EqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause Equals(string key, bool value)
        {
            return EqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause Equals(string key, string value)
        {
            return EqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause Equals(string key, object value)
        {
            CheckInstance1(value);
            return EqualsInternal(key, value);
        }

        #endregion

        #region NotEquals
    
        /// <summary>
        /// Create a clause of not equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause NotEquals(string key, int value)
        {
            return NotEqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of not equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause NotEquals(string key, long value)
        {
            return NotEqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of not equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause NotEquals(string key, double value)
        {
            return NotEqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of not equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause NotEquals(string key, bool value)
        {
            return NotEqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of not equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause NotEquals(string key, string value)
        {
            return NotEqualsInternal(key, value);
        }

        /// <summary>
        /// Create a clause of not equals condition.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared.
        /// </param>
        public static KiiClause NotEquals(string key, object value)
        {
            return NotEqualsInternal(key, value);
        }

        #endregion

        #region
        /// <summary>
        /// Concatenate <see cref="KiiClause"/> with NOT operator.
        /// </summary>
        /// <remarks>
        /// Query performance will be worse as the number of objects in bucket increases, so we recommend you avoid the OR clause if possible.
        /// </remarks>
        /// <param name="clause">Clause.</param>
        /// <returns>
        /// KiiClanse instance.
        /// </returns>
        public static KiiClause Not(KiiClause clause)
        {
            if (clause == null)
            {
                throw new ArgumentException("clause can not be null");
            }
            KiiClause cls = new KiiClause();
            try {
                JsonObject json = new JsonObject();
                json.Put("type", "not");
                json.Put("clause", clause.ToJson());
                cls.mJson = json;
            } catch (JsonException jse) {
                throw new ArgumentException("Invalid arugment.", jse);
            }
            return cls;
        }
        #endregion

        #region Or

        /// <summary>
        /// Concatenate <see cref="KiiClause"/> with OR operator.
        /// </summary>
        /// <remarks>
        /// Clauses must not be null or empty array.
        /// Query performance will be worse as the number of objects in bucket increases, so we recommend you avoid the OR clause if possible.
        /// </remarks>
        /// <returns>
        /// KiiClanse instance.
        /// </returns>
        /// <param name='clauses'>
        /// Clauses.
        /// </param>
        public static KiiClause Or(params KiiClause[] clauses) {
            if(clauses == null)
                throw new ArgumentException("Clause can not be null");
            else if(clauses.Length <= 0 )
                throw new ArgumentException("No clause found");
            else if(clauses.Length == 1)
                return clauses[0];
            KiiClause cls = new KiiClause();
            try {
                JsonObject json = new JsonObject();
                json.Put("type", "or");
                JsonArray array = new JsonArray();
                for (int i = 0; i < clauses.Length; i++) {
                    KiiClause clause = clauses[i];
                    if (clause == null)
                    {
                        throw new ArgumentException(ErrorInfo.KIICLAUSE_CLAUSE_NULL);
                    }
                    array.Put(clause.ToJson());
                }
                json.Put("clauses", array);
                cls.mJson = json;
            } catch (JsonException jse) {
                throw new ArgumentException("Invalid arugment.", jse);
            }
            return cls;
        }

        #endregion

        #region And

        /// <summary>
        /// Concatenate <see cref="KiiClause"/> with AND operator.
        /// </summary>
        /// <remarks>
        /// Clauses must not be null or empty array.
        /// </remarks>
        /// <returns>
        /// KiiClanse instance.
        /// </returns>
        /// <param name='clauses'>
        /// Clauses.
        /// </param>
        public static KiiClause And(params KiiClause[] clauses) {
            if(clauses == null)
                throw new ArgumentException("Clause can not be null");
            else if(clauses.Length <= 0 )
                throw new ArgumentException("No clause found");
            else if(clauses.Length == 1)
                return clauses[0];
            KiiClause cls = new KiiClause();
            try {
                JsonObject json = new JsonObject();
                json.Put("type", "and");
                JsonArray array = new JsonArray();
                for (int i = 0; i < clauses.Length; i++) {
                    KiiClause clause = clauses[i];
                    if (clause == null)
                    {
                        throw new ArgumentException(ErrorInfo.KIICLAUSE_CLAUSE_NULL);
                    }
                    array.Put(clause.ToJson());
                }
                json.Put("clauses", array);
                cls.mJson = json;
            } catch (JsonException jse) {
                // Wont happen.
                throw new ArgumentException("Invalid arugment.", jse);
            }
            return cls;
        }

        #endregion
    
        #region In

        /// <summary>
        /// Create a clause of in condition.
        /// </summary>
        /// <remarks>
        /// Query records matches with key-value specified by argument.
        /// More efficient than using combination of "equals" and "or"
        /// When querying the multiple records with specific key.
        /// </remarks>
        /// <returns>
        /// KiiClause instance
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Values to be compared. The length of values array should be less than or equals to 200.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown in the following cases:
        /// <list type="bullet">
        /// <item>
        /// <term>key is null.</term>
        /// </item>
        /// <item>
        /// <term>value is null or length of value is 0.</term>
        /// </item>
        /// <item>
        /// <term>length of value is more than 200.</term>
        /// </item>
        /// </list>
        /// </exception>
        /// <exception cref='JsonException'>
        /// Is thrown when the unexpected invalid params given. 
        /// </exception>
        public static KiiClause InWithIntValue(string key, params int[] value) {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            if (value == null || value.Length == 0)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_NULL);
            }
            if (value.Length > 200)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_EXCEED_MAX_LENGTH);
            }
            KiiClause clause = new KiiClause();
            try {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "in");
                clause.mJson.Put("field", key);
                JsonArray elements = new JsonArray();
                foreach (int v in value) {
                    elements.Put(v);
                }
                clause.mJson.Put("values", elements);
                return clause;
            } catch (JsonException ex) {
                throw new SystemException("Invalid param given!", ex);
            }
        }

        /// <summary>
        /// Create a clause of in condition.
        /// </summary>
        /// <remarks>
        /// Query records matches with key-value specified by argument.
        /// More efficient than using combination of "equals" and "or"
        /// When querying the multiple records with specific key.
        /// </remarks>
        /// <returns>
        /// KiiClause instance
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Values to be compared. The length of values array should be less than or equals to 200.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown in the following cases:
        /// <list type="bullet">
        /// <item>
        /// <term>key is null.</term>
        /// </item>
        /// <item>
        /// <term>value is null or length of value is 0.</term>
        /// </item>
        /// <item>
        /// <term>length of value is more than 200.</term>
        /// </item>
        /// </list>
        /// </exception>
        /// <exception cref='JsonException'>
        /// Is thrown when the unexpected invalid params given. 
        /// </exception>
        public static KiiClause InWithLongValue(string key, params long[] value) {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            if (value == null || value.Length == 0)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_NULL);
            }
            if (value.Length > 200)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_EXCEED_MAX_LENGTH);
            }
            KiiClause clause = new KiiClause();
            try {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "in");
                clause.mJson.Put("field", key);
                JsonArray elements = new JsonArray();
                foreach (long v in value) {
                    elements.Put(v);
                }
                clause.mJson.Put("values", elements);
                return clause;
            } catch (JsonException ex) {
                throw new SystemException("Invalid param given!", ex);
            }
        }
    
        /// <summary>
        /// Create a clause of in condition.
        /// </summary>
        /// <remarks>
        /// Query records matches with key-value specified by argument.
        /// More efficient than using combination of "equals" and "or"
        /// When querying the multiple records with specific key.
        /// </remarks>
        /// <returns>
        /// KiiClause instance
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Values to be compared. The length of values array should be less than or equals to 200.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown in the following cases:
        /// <list type="bullet">
        /// <item>
        /// <term>key is null.</term>
        /// </item>
        /// <item>
        /// <term>value is null or length of value is 0.</term>
        /// </item>
        /// <item>
        /// <term>length of value is more than 200.</term>
        /// </item>
        /// </list>
        /// </exception>
        /// <exception cref='JsonException'>
        /// Is thrown when the unexpected invalid params given. 
        /// </exception>
        public static KiiClause InWithDoubleValue(string key, params double[] value) {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            if (value == null || value.Length == 0)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_NULL);
            }
            if (value.Length > 200)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_EXCEED_MAX_LENGTH);
            }
            KiiClause clause = new KiiClause();
            try {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "in");
                clause.mJson.Put("field", key);
                JsonArray elements = new JsonArray();
                foreach (double v in value) {
                    elements.Put(v);
                }
                clause.mJson.Put("values", elements);
                return clause;
            } catch (JsonException ex) {
                throw new SystemException("Invalid param given!", ex);
            }
        }
    
        /// <summary>
        /// Create a clause of in condition.
        /// </summary>
        /// <remarks>
        /// Query records matches with key-value specified by argument.
        /// More efficient than using combination of "equals" and "or"
        /// When querying the multiple records with specific key.
        /// </remarks>
        /// <returns>
        /// KiiClause instance
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Values to be compared. The length of values array should be less than or equals to 200.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown in the following cases:
        /// <list type="bullet">
        /// <item>
        /// <term>key is null.</term>
        /// </item>
        /// <item>
        /// <term>value is null or length of value is 0.</term>
        /// </item>
        /// <item>
        /// <term>length of value is more than 200.</term>
        /// </item>
        /// </list>
        /// </exception>
        /// <exception cref='JsonException'>
        /// Is thrown when the unexpected invalid params given. 
        /// </exception>
        public static KiiClause InWithStringValue(string key, params string[] value) {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            if (value == null || value.Length == 0)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_NULL);
            }
            if (value.Length > 200)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_EXCEED_MAX_LENGTH);
            }
            KiiClause clause = new KiiClause();
            try {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "in");
                clause.mJson.Put("field", key);
                JsonArray elements = new JsonArray();
                foreach (string v in value) {
                    if (v == null)
                    {
                        throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_NULL);
                    }
                    elements.Put(v);
                }
                clause.mJson.Put("values", elements);
                return clause;
            } catch (JsonException ex) {
                throw new SystemException("Invalid param given!", ex);
            }
        }

        #endregion

        #region GreaterThan

        /// <summary>
        /// Create a clause of greater than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThan(string key, object value)
        {
            CheckInstance2(value);
            return GreaterThanInternal(key, value, false);
        }

        /// <summary>
        /// Create a clause of greater than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThan(string key, int value)
        {
            return GreaterThanInternal(key, value, false);
        }

        /// <summary>
        /// Create a clause of greater than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThan(string key, long value)
        {
            return GreaterThanInternal(key, value, false);
        }

        /// <summary>
        /// Create a clause of greater than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThan(string key, double value)
        {
            return GreaterThanInternal(key, value, false);
        }

        /// <summary>
        /// Create a clause of greater than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThan(string key, string value)
        {
            return GreaterThanInternal(key, value, false);
        }

        #endregion

        #region GreaterThanOrEqual
        /// <summary>
        /// Create a clause of greater than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThanOrEqual(string key, object value)
        {
            CheckInstance2(value);
            return GreaterThanInternal(key, value, true);
        }

        /// <summary>
        /// Create a clause of greater than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThanOrEqual(string key, int value)
        {
            return GreaterThanInternal(key, value, true);
        }

        /// <summary>
        /// Create a clause of greater than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThanOrEqual(string key, long value)
        {
            return GreaterThanInternal(key, value, true);
        }

        /// <summary>
        /// Create a clause of greater than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThanOrEqual(string key, double value)
        {
            return GreaterThanInternal(key, value, true);
        }

        /// <summary>
        /// Create a clause of greater than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause GreaterThanOrEqual(string key, string value)
        {
            return GreaterThanInternal(key, value, true);
        }

        #endregion

        #region LessThan

        /// <summary>
        /// Create a clause of less than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThan(String key, object value)
        {
            CheckInstance2(value);
            return LessThanInternal(key, value, false);
        }

        /// <summary>
        /// Create a clause of less than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThan(string key, int value)
        {
            return LessThanInternal(key, value, false);
        }

        /// <summary>
        /// Create a clause of less than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThan(string key, long value)
        {
            return LessThanInternal(key, value, false);
        }
    
        /// <summary>
        /// Create a clause of less than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThan(string key, double value)
        {
            return LessThanInternal(key, value, false);
        }

        /// <summary>
        /// Create a clause of less than.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThan(string key, string value)
        {
            return LessThanInternal(key, value, false);
        }

        #endregion

        #region LessThanOrEqual

        /// <summary>
        /// Create a clause of less than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThanOrEqual(string key, Object value)
        {
            CheckInstance2(value);
            return LessThanInternal(key, value, true);
        }

        /// <summary>
        /// Create a clause of less than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThanOrEqual(string key, int value)
        {
            return LessThanInternal(key, value, true);
        }

        /// <summary>
        /// Create a clause of less than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThanOrEqual(string key, long value)
        {
            return LessThanInternal(key, value, true);
        }

        /// <summary>
        /// Create a clause of less than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThanOrEqual(string key, double value)
        {
            return LessThanInternal(key, value, true);
        }

        /// <summary>
        /// Create a clause of less than or equal.
        /// </summary>
        /// <remarks>
        /// If type of value in Object is different, it won't be included in a result.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause LessThanOrEqual(string key, string value)
        {
            return LessThanInternal(key, value, true);
        }
        #endregion

        #region Internal methods

        private static KiiClause GeoBoxInternal(string key, JsonObject box)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            KiiClause clause = new KiiClause();
            try
            {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "geobox");
                clause.mJson.Put("field", key);
                clause.mJson.Put("box", box);
                return clause;
            }
            catch (JsonException ex)
            {
                throw new SystemException("Invalid param given!", ex);
            }
        }

        private static KiiClause GeoDistanceInternal(string key, JsonObject geodistance,double radius,string putDistanceInto)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            KiiClause clause = new KiiClause();
            try
            {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "geodistance");
                clause.mJson.Put("field", key);
                clause.mJson.Put("center", geodistance);
                clause.mJson.Put("radius",radius);
                if(!Utils.IsEmpty(putDistanceInto))
                {
                    clause.mJson.Put("putDistanceInto",putDistanceInto);
                }
                return clause;
            }
            catch (JsonException ex)
            {
                throw new SystemException("Invalid param given!", ex);
            }
        }

        private static KiiClause EqualsInternal<T>(string key, T value)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            KiiClause clause = new KiiClause();
            try
            {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "eq");
                clause.mJson.Put("field", key);
                clause.mJson.Put("value", value);
                return clause;
            }
            catch (JsonException ex)
            {
                throw new SystemException("Invalid param given!", ex);
            }
        }

        private static KiiClause NotEqualsInternal<T>(string key, T value)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            CheckInstance1(value);
            KiiClause clause = new KiiClause();
            try
            {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "not");
                clause.mJson.Put("clause", Equals(key, value).ToJson());
                return clause;
            }
            catch (JsonException ex)
            {
                throw new SystemException("Invalid param given!", ex);
            }
        }

        private static KiiClause GreaterThanInternal<T>(string key, T value, bool included)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            KiiClause clause = new KiiClause();
            try
            {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "range");
                clause.mJson.Put("field", key);
                clause.mJson.Put("lowerLimit", value);
                clause.mJson.Put("lowerIncluded", included);
                return clause;
            }
            catch (JsonException ex)
            {
                throw new SystemException("Invalid param given!", ex);
            }
        }

        private static KiiClause LessThanInternal<T>(string key, T value, bool included)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            KiiClause clause = new KiiClause();
            try
            {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "range");
                clause.mJson.Put("field", key);
                clause.mJson.Put("upperLimit", value);
                clause.mJson.Put("upperIncluded", included);
                return clause;
            }
            catch (JsonException ex)
            {
                throw new SystemException("Invalid param given!", ex);
            }
        }

        #endregion

        #region StartsWith
    
        /// <summary>
        /// Create a clause with the prefix condition.
        /// </summary>
        /// <remarks>
        /// It matches the specified key's value to be starts with the specified value.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <param name='value'>
        /// Value to be compared
        /// </param>
        public static KiiClause StartsWith(string key, string value)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }
            if (value == null)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_NULL);
            }
            KiiClause clause = new KiiClause();
            try {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "prefix");
                clause.mJson.Put("field", key);
                clause.mJson.Put("prefix", value);
                return clause;
            } catch (JsonException ex) {
                throw new SystemException("Invalid param given!", ex);
            }
        }

        #endregion

        #region Geo Query

        /// <summary>
        /// Create a clause of geo box.
        /// </summary>
        /// <remarks>
        /// This clause inquires objects in the specified rectangle.
        /// Rectangle would be placed parallel to the equator with specified coordinates of the corner.
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid. Please see parameter explanation.
        /// </exception>
        /// <param name='key'>
        /// Name of the key to inquire, which holds geo point. Must not null or empty string.
        /// </param>
        /// <param name='northEast'>
        /// North east corner of the rectangle. Must not null.
        /// </param>
        /// <param name='southWest'>
        /// South west corner of the rectangle. Must not null.
        /// </param>
        public static KiiClause GeoBox(string key, KiiGeoPoint northEast, KiiGeoPoint southWest)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }

            JsonObject box = new JsonObject();
            box.Put("ne",northEast.ToJson());
            box.Put("sw",southWest.ToJson());

            return GeoBoxInternal(key,box);
        }

        /// <summary>
        /// Create a clause of geo distance.
        /// </summary>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument is invalid. Please see parameter explanation.
        /// </exception>
        /// <remarks>
        /// This clause inquires objects in the specified circle.<br/>
        /// <b>Note:</b> You can get the results in ascending order of distances from center.
        /// To do so, build the orderBy field  by "_calculated.{specified value of calculatedDistance}" and pass it in <see cref="KiiQuery.SortByAsc()"/>.<br/>
        /// Note that, descending order of distances is not supported. The unit of distance is meter.
        /// <see cref="KiiObject"/>
        /// <code>
        /// // example
        /// string calculatedDistance = "distanceFromCurrentLoc";
        /// KiiGeoPoint currentLoc = KiiGeoPoint(120.000,77.000); //dummy location
        /// KiiClause geoDist = KiiClause("location",currentLoc, 7.0,calculatedDistance);
        /// KiiQuery query = KiiQuery.QueryWithClause(geoDist);
        /// // sort distance ny ascending order.
        /// string orderByKey = "_calculated."+ calculatedDistance;
        /// query.SortByAsc(orderByKey);
        /// KiiBucket bucket = Kii.Bucket("MyBucket");
        /// KiiQueryResult&lt;KiiObject&gt; result = bucket.Query(query);
        /// if(result.Size &gt; 0)
        /// {
        ///     KiiObject object = result[0];
        ///     double distanceInMeter = object.GetJsonObject("_calculated").GetDouble(calculatedDistance);
        /// }
        /// </code>
        /// </remarks>
        /// <param name='key'>
        /// Name of the key to inquire, which holds geo point. Must not null or empty string.
        /// </param>
        /// <param name='center'>
        /// Geo point which specify center of the circle. Mus not null.
        /// </param>
        /// <param name='radius'>
        /// Radius of the circle. unit is meter. value should be in range of 0-20000000.
        /// </param>
        /// <param name='calculatedDistance'>
        /// Calculated distance is used for retrieve distance from the center from the query result. If the specified value is null, query result will not contain the distance.
        /// </param>
        ///
        public static KiiClause GeoDistance(string key, KiiGeoPoint center, double radius, string calculatedDistance)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }

            if(radius <= 0 || radius > 20000000)
            {
                throw new ArgumentException("radius value should be in range of 0-20000000");
            }

            return GeoDistanceInternal(key,center.ToJson(),radius,calculatedDistance);
        }

        /// <summary>
        /// Create a clause to return all entities that have a specified field.
        /// </summary>
        /// <remarks>
        /// The type of the content of the field must be provided, possible values are "STRING", "INTEGER", "DECIMAL" and "BOOLEAN".
        /// </remarks>
        /// <returns>
        /// KiiClause instance.
        /// </returns>
        /// <param name='key'>
        /// Name of the specified field.
        /// </param>
        /// <param name='fieldType'>
        /// The type of the content of the field.
        /// </param>
        public static KiiClause HasField(string key, FieldType fieldType)
        {
            if (Utils.IsEmpty(key))
            {
                throw new ArgumentException("key must not null or empty string");
            }

            KiiClause clause = new KiiClause();
            try {
                clause.mJson = new JsonObject();
                clause.mJson.Put("type", "hasField");
                clause.mJson.Put("field", key);
                clause.mJson.Put("fieldType", fieldType.ToString());
                return clause;
            } catch (JsonException ex) {
                throw new SystemException("Invalid param given!", ex);
            }
        }
        #endregion

        /**
         * Check instance for =, !=
         * @param object
         */
        private static void CheckInstance1(object obj) {
            if (obj == null)
            {
                throw new ArgumentException(ErrorInfo.KIICLAUSE_VALUE_NULL);
            }
            if ((!(obj is int)) &&
               (!(obj is long)) &&
               (!(obj is double)) &&
               (!(obj is bool)) &&
               (!(obj is string))) {
                    throw new ArgumentException("Unsupported type: " + obj.GetType().ToString());
               }
        }
    
        /// <summary>
        /// Checks the instance.
        /// </summary>
        /// <remarks>
        /// Check instance for &lt;, &gt;, &lt;=, &gt;=, in
        /// </remarks>
        /// <param name='obj'>
        /// Object.
        /// </param>
        /// <exception cref='ArgumentException'>
        /// Is thrown when an argument passed to a method is invalid.
        /// </exception>
        private static void CheckInstance2(object obj) {
            if ((!(obj is int)) &&
               (!(obj is long)) &&
               (!(obj is double)) &&
               (!(obj is string))) {
                    throw new ArgumentException("Unsupported type: " + obj.GetType().ToString());
               }
        }

    }
}

