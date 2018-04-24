using System;
using System.Collections.Generic;
using System.Globalization;


namespace JsonOrg
{
    public class JsonObject {

        private const double NEGATIVE_ZERO = -0d;
        
        private static CultureInfo NUMBER_CULTURE = CultureInfo.GetCultureInfo("en-US");

        internal static object NULL = new JsonNull();

/*
            @Override public boolean equals(Object o) {
                return o == this || o == null; // API specifies this broken equals implementation
            }
            @Override public String toString() {
                return "null";
            }
        };
*/
        private Dictionary<string, object> nameValuePairs;

        /**
         * Creates a {@code JSONObject} with no name/value mappings.
         */
        public JsonObject() {
            nameValuePairs = new Dictionary<string, object>();
        }
    
        /**
         * Creates a new {@code JSONObject} by copying all name/value mappings from
         * the given map.
         *
         * @param copyFrom a map whose keys are of type {@link String} and whose
         *     values are of supported types.
         * @throws NullPointerException if any of the map's keys are null.
         */
        /* (accept a raw type for API compatibility) */
        public JsonObject(Dictionary<string, object> copyFrom) : this() {
            Dictionary<string, object> contentsTyped = copyFrom;
            foreach (KeyValuePair<string, object> entry in contentsTyped) {
                /*
                 * Deviate from the original by checking that keys are non-null and
                 * of the proper type. (We still defer validating the values).
                 */
                string key = entry.Key;
                if (key == null) {
                    throw new NullReferenceException();
                }
                nameValuePairs[key] = entry.Value;
//                nameValuePairs.put(key, entry.getValue());
            }
        }
    
        /**
         * Creates a new {@code JSONObject} with name/value mappings from the next
         * object in the tokener.
         *
         * @param readFrom a tokener whose nextValue() method will yield a
         *     {@code JSONObject}.
         * @throws JSONException if the parse fails or doesn't yield a
         *     {@code JSONObject}.
         */
        internal JsonObject(JsonTokener readFrom) {
            /*
             * Getting the parser to populate this could get tricky. Instead, just
             * parse to temporary JSONObject and then steal the data from that.
             */
            object obj = readFrom.NextValue();
            if (obj is JsonObject) {
                this.nameValuePairs = ((JsonObject) obj).nameValuePairs;
            } else {
                throw new JsonException("Parse Error : " + readFrom.Input);
            }
        }
    
        /**
         * Creates a new {@code JSONObject} with name/value mappings from the JSON
         * string.
         *
         * @param json a JSON-encoded string containing an object.
         * @throws JSONException if the parse fails or doesn't yield a {@code
         *     JSONObject}.
         */
        public JsonObject(string json) : this(new JsonTokener(json)){
        }
    
        /**
         * Creates a new {@code JSONObject} by copying mappings for the listed names
         * from the given object. Names that aren't present in {@code copyFrom} will
         * be skipped.
         */
        public JsonObject(JsonObject copyFrom, string[] names) : this() {
            foreach (string name in names) {
                object value = copyFrom.Opt(name);
                if (value != null) {
                    nameValuePairs[name] = value;
                }
            }
        }
    
        /**
         * Returns the number of name/value mappings in this object.
         */
        public int Length() {
            return nameValuePairs.Count;
        }
    
        /**
         * Maps {@code name} to {@code value}, clobbering any existing name/value
         * mapping with the same name.
         *
         * @return this object.
         */
        public JsonObject Put(string name, bool value) {
            nameValuePairs[checkName(name)] = value;
            return this;
        }
    
        /**
         * Maps {@code name} to {@code value}, clobbering any existing name/value
         * mapping with the same name.
         *
         * @param value a finite value. May not be {@link Double#isNaN() NaNs} or
         *     {@link Double#isInfinite() infinities}.
         * @return this object.
         */
        public JsonObject Put(string name, double value) {
            nameValuePairs[checkName(name)] = Json.CheckDouble(value);
            return this;
        }
    
        /**
         * Maps {@code name} to {@code value}, clobbering any existing name/value
         * mapping with the same name.
         *
         * @return this object.
         */
        public JsonObject Put(string name, int value) {
            nameValuePairs[checkName(name)] = value;
            return this;
        }
    
        /**
         * Maps {@code name} to {@code value}, clobbering any existing name/value
         * mapping with the same name.
         *
         * @return this object.
         */
        public JsonObject Put(string name, long value) {
            nameValuePairs[checkName(name)] = value;
            return this;
        }
    
        /**
         * Maps {@code name} to {@code value}, clobbering any existing name/value
         * mapping with the same name. If the value is {@code null}, any existing
         * mapping for {@code name} is removed.
         *
         * @param value a {@link JSONObject}, {@link JSONArray}, String, Boolean,
         *     Integer, Long, Double, {@link #NULL}, or {@code null}. May not be
         *     {@link Double#isNaN() NaNs} or {@link Double#isInfinite()
         *     infinities}.
         * @return this object.
         */
        public JsonObject Put(string name, object value) {
            if (value == null) {
                nameValuePairs.Remove(name);
                return this;
            }
            if (Json.IsNumber(value)) {
                // deviate from the original by checking all Numbers, not just floats & doubles
                Json.CheckDouble(Convert.ToDouble(value));
            }
            nameValuePairs[checkName(name)] = value;
            return this;
        }
    
        /**
         * Equivalent to {@code put(name, value)} when both parameters are non-null;
         * does nothing otherwise.
         */
        public JsonObject PutOpt(string name, Object value) {
            if (name == null || value == null) {
                return this;
            }
            return Put(name, value);
        }
    
        /**
         * Appends {@code value} to the array already mapped to {@code name}. If
         * this object has no mapping for {@code name}, this inserts a new mapping.
         * If the mapping exists but its value is not an array, the existing
         * and new values are inserted in order into a new array which is itself
         * mapped to {@code name}. In aggregate, this allows values to be added to a
         * mapping one at a time.
         *
         * @param value a {@link JSONObject}, {@link JSONArray}, String, Boolean,
         *     Integer, Long, Double, {@link #NULL} or null. May not be {@link
         *     Double#isNaN() NaNs} or {@link Double#isInfinite() infinities}.
         */
        public JsonObject Accumulate(string name, object value) {
            object current = nameValuePairs[checkName(name)];
            if (current == null) {
                return Put(name, value);
            }
    
            // check in accumulate, since array.put(Object) doesn't do any checking
            if (Json.IsNumber(value)) {
                Json.CheckDouble((double)value);
            }
    
            if (current is JsonArray) {
                JsonArray array = (JsonArray) current;
                array.Put(value);
            } else {
                JsonArray array = new JsonArray();
                array.Put(current);
                array.Put(value);
                nameValuePairs[name] = array;
            }
            return this;
        }
    
        string checkName(string name) {
            if (name == null) {
                throw new JsonException("Names must be non-null");
            }
            return name;
        }
    
        /**
         * Removes the named mapping if it exists; does nothing otherwise.
         *
         * @return the value previously mapped by {@code name}, or null if there was
         *     no such mapping.
         */
        public object Remove(string name) {
            object prev = nameValuePairs[name];
            nameValuePairs.Remove(name);
            return prev;
        }
    
        /**
         * Returns true if this object has no mapping for {@code name} or if it has
         * a mapping whose value is {@link #NULL}.
         */
        public bool IsNull(string name) {
            object value = nameValuePairs[name];
            return value == null || value == NULL;
        }
    
        /**
         * Returns true if this object has a mapping for {@code name}. The mapping
         * may be {@link #NULL}.
         */
        public bool Has(string name) {
            return nameValuePairs.ContainsKey(name);
        }
    
        /**
         * Returns the value mapped by {@code name}.
         *
         * @throws JSONException if no such mapping exists.
         */
        public object Get(string name) {
            if (!nameValuePairs.ContainsKey(name)) {
                throw new JsonException("No value for " + name);
            }
            object result = nameValuePairs[name];
            return result;
        }

        /**
         * Returns the value mapped by {@code name}, or null if no such mapping
         * exists.
         */
        public object Opt(string name) {
            if (!nameValuePairs.ContainsKey(name)) {
                return null;
            }
            return nameValuePairs[name];
        }

        /**
         * Returns the value mapped by {@code name} if it exists and is a boolean or
         * can be coerced to a boolean.
         *
         * @throws JSONException if the mapping doesn't exist or cannot be coerced
         *     to a boolean.
         */
        public bool GetBoolean(string name) {
            object obj = Get(name);
            try {
                return Json.ToBoolean(obj);
            } catch {
                throw Json.TypeMismatch(name, obj, "boolean");
            }
        }

        /**
         * Returns the value mapped by {@code name} if it exists and is a boolean or
         * can be coerced to a boolean. Returns false otherwise.
         */
        public bool OptBoolean(string name) {
            return OptBoolean(name, false);
        }

        /**
         * Returns the value mapped by {@code name} if it exists and is a boolean or
         * can be coerced to a boolean. Returns {@code fallback} otherwise.
         */
        public bool OptBoolean(string name, bool fallback) {
            object obj = Opt(name);
            try {
                return Json.ToBoolean(obj);
            } catch {
                return fallback;
            }
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is a double or
         * can be coerced to a double.
         *
         * @throws JSONException if the mapping doesn't exist or cannot be coerced
         *     to a double.
         */
        public double GetDouble(String name) {
            object obj = Get(name);
            try {
                return Json.ToDouble(obj);
            } catch {
                throw Json.TypeMismatch(name, obj, "double");
            }
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is a double or
         * can be coerced to a double. Returns {@code NaN} otherwise.
         */
        public double OptDouble(String name) {
            return OptDouble(name, Double.NaN);
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is a double or
         * can be coerced to a double. Returns {@code fallback} otherwise.
         */
        public double OptDouble(string name, double fallback) {
            object obj = Opt(name);
            try {
                return Json.ToDouble(obj);
            } catch {
                return fallback;
            }
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is an int or
         * can be coerced to an int.
         *
         * @throws JSONException if the mapping doesn't exist or cannot be coerced
         *     to an int.
         */
        public int GetInt(string name) {
            object obj = Get(name);
            int result;
            try {
                result = Json.ToInteger(obj);
            } catch {
                throw Json.TypeMismatch(name, obj, "int");
            }
            return result;
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is an int or
         * can be coerced to an int. Returns 0 otherwise.
         */
        public int OptInt(string name) {
            return OptInt(name, 0);
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is an int or
         * can be coerced to an int. Returns {@code fallback} otherwise.
         */
        public int OptInt(string name, int fallback) {
            object obj = Opt(name);
            try {
                return Json.ToInteger(obj);
            } catch {
                return fallback;
            }
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is a long or
         * can be coerced to a long.
         *
         * @throws JSONException if the mapping doesn't exist or cannot be coerced
         *     to a long.
         */
        public long GetLong(string name) {
            object obj = Get(name);
            try {
                return Json.ToLong(obj);
            } catch {
                throw Json.TypeMismatch(name, obj, "long");
            }
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is a long or
         * can be coerced to a long. Returns 0 otherwise.
         */
        public long OptLong(string name) {
            return OptLong(name, 0L);
        }

        /**
         * Returns the value mapped by {@code name} if it exists and is a long or
         * can be coerced to a long. Returns {@code fallback} otherwise.
         */
        public long OptLong(string name, long fallback) {
            object obj = Opt(name);
            try {
                return Json.ToLong(obj);
            } catch {
                return fallback;
            }
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists, coercing it if
         * necessary.
         *
         * @throws JSONException if no such mapping exists.
         */
        public string GetString(string name) {
            object obj = Get(name);
            string result = Json.ToString(obj);
            if (result == null) {
                throw Json.TypeMismatch(name, obj, "String");
            }
            return result;
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists, coercing it if
         * necessary. Returns the empty string if no such mapping exists.
         */
        public string OptString(string name) {
            return OptString(name, "");
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists, coercing it if
         * necessary. Returns {@code fallback} if no such mapping exists.
         */
        public string OptString(string name, string fallback) {
            object obj = Opt(name);
            string result = Json.ToString(obj);
            return result != null ? result : fallback;
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is a {@code
         * JSONArray}.
         *
         * @throws JSONException if the mapping doesn't exist or is not a {@code
         *     JSONArray}.
         */
        public JsonArray GetJsonArray(string name) {
            object obj = Get(name);
            if (obj is JsonArray) {
                return (JsonArray) obj;
            } else {
                throw Json.TypeMismatch(name, obj, "JSONArray");
            }
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is a {@code
         * JSONArray}. Returns null otherwise.
         */
        public JsonArray OptJsonArray(string name) {
            object obj = Opt(name);
            return obj is JsonArray ? (JsonArray) obj : null;
        }

        /**
         * Returns the value mapped by {@code name} if it exists and is a {@code
         * JSONObject}.
         *
         * @throws JSONException if the mapping doesn't exist or is not a {@code
         *     JSONObject}.
         */
        public JsonObject GetJsonObject(string name) {
            object obj = Get(name);
            if (obj is JsonObject) {
                return (JsonObject) obj;
            } else {
                throw Json.TypeMismatch(name, obj, "JSONObject");
            }
        }
    
        /**
         * Returns the value mapped by {@code name} if it exists and is a {@code
         * JSONObject}. Returns null otherwise.
         */
        public JsonObject OptJsonObject(string name) {
            object obj = Opt(name);
            return obj is JsonObject ? (JsonObject) obj : null;
        }
    
        /**
         * Returns an array with the values corresponding to {@code names}. The
         * array contains null for names that aren't mapped. This method returns
         * null if {@code names} is either null or empty.
         */
        public JsonArray ToJSONArray(JsonArray names) {
            JsonArray result = new JsonArray();
            if (names == null) {
                return null;
            }
            int length = names.Length();
            if (length == 0) {
                return null;
            }
            for (int i = 0; i < length; i++) {
                string name = Json.ToString(names.Opt(i));
                result.Put(Opt(name));
            }
            return result;
        }

        /**
         * Returns an iterator of the {@code String} names in this object. The
         * returned iterator supports {@link Iterator#remove() remove}, which will
         * remove the corresponding mapping from this object. If this object is
         * modified after the iterator is returned, the iterator's behavior is
         * undefined. The order of the keys is undefined.
         */
        /* Return a raw type for API compatibility */
        public Dictionary<string, object>.KeyCollection.Enumerator Keys() {
            return nameValuePairs.Keys.GetEnumerator();
//            return nameValuePairs.keySet().iterator();
        }

        /**
         * Returns an array containing the string names in this object. This method
         * returns null if this object contains no mappings.
         */
        public JsonArray Names() {
            return nameValuePairs.Count == 0
                    ? null
                    : JsonArray.FromCollection(nameValuePairs.Keys);
        }

        /**
         * Encodes this object as a compact JSON string, such as:
         * <pre>{"query":"Pizza","locations":[94043,90210]}</pre>
         */
        public override string ToString() {
            try {
                JsonStringer stringer = new JsonStringer();
                WriteTo(stringer);
                return stringer.ToString();
            } catch (JsonException) {
                return null;
            }
        }
    
        /**
         * Encodes this object as a human readable JSON string for debugging, such
         * as:
         * <pre>
         * {
         *     "query": "Pizza",
         *     "locations": [
         *         94043,
         *         90210
         *     ]
         * }</pre>
         *
         * @param indentSpaces the number of spaces to indent for each level of
         *     nesting.
         */
        public string ToString(int indentSpaces) {
            JsonStringer stringer = new JsonStringer(indentSpaces);
            WriteTo(stringer);
            return stringer.ToString();
        }

        internal void WriteTo(JsonStringer stringer) {
            stringer.Obj();
            foreach (KeyValuePair<string, object> entry in nameValuePairs) {
                stringer.Key(entry.Key).Value(entry.Value);
            }
            stringer.EndObject();
        }
    
        /**
         * Encodes the number as a JSON string.
         *
         * @param number a finite value. May not be {@link Double#isNaN() NaNs} or
         *     {@link Double#isInfinite() infinities}.
         */
        public static string NumberToString(object number) {
            if (number == null) {
                throw new JsonException("Number must be non-null");
            }
            double doubleValue = Convert.ToDouble(number);
            Json.CheckDouble(doubleValue);
    
            // the original returns "-0" instead of "-0.0" for negative zero
            if (doubleValue == -0d) {
                return "-0";
            }
            
            // check max value of long
            if (doubleValue < (double)long.MinValue || (double)long.MaxValue < doubleValue)
            {
                return doubleValue.ToString("r", NUMBER_CULTURE);
            }
    
            long longValue = Convert.ToInt64(number);
            if (doubleValue == (double) longValue) {
                return longValue.ToString();
            }
            
            if (number is float)
            {
                return ((float)number).ToString(NUMBER_CULTURE);                
            }
            return doubleValue.ToString("r", NUMBER_CULTURE);
        }
    
        /**
         * Encodes {@code data} as a JSON string. This applies quotes and any
         * necessary character escaping.
         *
         * @param data the string to encode. Null will be interpreted as an empty
         *     string.
         */
        public static string Quote(String data) {
            if (data == null) {
                return "\"\"";
            }
            try {
                JsonStringer stringer = new JsonStringer();
                stringer.Open(JsonStringer.Scope.NULL, "");
                stringer.Value(data);
                stringer.Close(JsonStringer.Scope.NULL, JsonStringer.Scope.NULL, "");
                return stringer.ToString();
            } catch (JsonException) {
                throw new ApplicationException();
            }
        }
    }
}

