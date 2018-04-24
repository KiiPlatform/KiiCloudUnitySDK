using System;
using System.Globalization;
using System.Text;

namespace JsonOrg
{
    internal class JsonTokener {

        // for parsing double
        private static CultureInfo NUMBER_CULTURE = CultureInfo.GetCultureInfo("en-US");
        
        /** The input JSON. */
        private string input;

        /**
         * The index of the next character to be returned by {@link #next}. When
         * the input is exhausted, this equals the input's length.
         */
        private int pos;
    
        /**
         * @param in JSON encoded string. Null is not permitted and will yield a
         *     tokener that throws {@code NullPointerExceptions} when methods are
         *     called.
         */
        public JsonTokener(string input) {
            // consume an optional byte order mark (BOM) if it exists
            if (input != null)
            {
                if (input.Length == 0)
                {
                    throw new JsonException("empty string is given");
                }
                if (input[0] == 0xfeff) 
                {
                    input = input.Substring(1);
                }
            }
            this.input = input;
        }
    
        /**
         * Returns the next value from the input.
         *
         * @return a {@link JSONObject}, {@link JSONArray}, String, Boolean,
         *     Integer, Long, Double or {@link JSONObject#NULL}.
         * @throws JSONException if the input is malformed.
         */
        public object NextValue() {
            int c = NextCleanInternal();
            switch (c) {
                case -1:
                    throw SyntaxError("End of input");
    
                case '{':
                    return ReadObject();
    
                case '[':
                    return ReadArray();
    
                case '\'':
                    goto case '"';
                case '"':
                    return NextString((char) c);
    
                default:
                    pos--;
                    return ReadLiteral();
            }
        }
    
        private int NextCleanInternal() {
            while (pos < input.Length) {
                int c = input[pos];
                ++pos;
                switch (c) {
                    case '\t':
                        continue;
                    case ' ':
                        continue;
                    case '\n':
                        continue;
                    case '\r':
                        continue;
    
                    case '/':
                        if (pos == input.Length) {
                            return c;
                        }
    
                        char peek = input[pos];
                        switch (peek) {
                            case '*':
                                // skip a /* c-style comment */
                                pos++;
                                int commentEnd = input.IndexOf("*/", pos);
                                if (commentEnd == -1) {
                                    throw SyntaxError("Unterminated comment");
                                }
                                pos = commentEnd + 2;
                                continue;
    
                            case '/':
                                // skip a // end-of-line comment
                                pos++;
                                SkipToEndOfLine();
                                continue;
    
                            default:
                                return c;
                        }
    
                    case '#':
                        /*
                         * Skip a # hash end-of-line comment. The JSON RFC doesn't
                         * specify this behavior, but it's required to parse
                         * existing documents. See http://b/2571423.
                         */
                        SkipToEndOfLine();
                        continue;
    
                    default:
                        return c;
                }
            }
    
            return -1;
        }
    
        /**
         * Advances the position until after the next newline character. If the line
         * is terminated by "\r\n", the '\n' must be consumed as whitespace by the
         * caller.
         */
        private void SkipToEndOfLine() {
            for (; pos < input.Length; pos++) {
                char c = input[pos];
                if (c == '\r' || c == '\n') {
                    pos++;
                    break;
                }
            }
        }
    
        /**
         * Returns the string up to but not including {@code quote}, unescaping any
         * character escape sequences encountered along the way. The opening quote
         * should have already been read. This consumes the closing quote, but does
         * not include it in the returned string.
         *
         * @param quote either ' or ".
         * @throws NumberFormatException if any unicode escape sequences are
         *     malformed.
         */
        public string NextString(char quote) {
            /*
             * For strings that are free of escape sequences, we can just extract
             * the result as a substring of the input. But if we encounter an escape
             * sequence, we need to use a StringBuilder to compose the result.
             */
            StringBuilder builder = null;
    
            /* the index of the first character not yet appended to the builder. */
            int start = pos;
    
            while (pos < input.Length) {
                int c = input[pos++];
                if (c == quote) {
                    if (builder == null) {
                        // a new string avoids leaking memory
                        return input.Substring(start, pos - 1 - start);
                    } else {
                        builder.Append(input, start, pos - 1 - start);
                        return builder.ToString();
                    }
                }
    
                if (c == '\\') {
                    if (pos == input.Length) {
                        throw SyntaxError("Unterminated escape sequence");
                    }
                    if (builder == null) {
                        builder = new StringBuilder();
                    }
                    builder.Append(input, start, pos - 1 - start);
                    builder.Append(ReadEscapeCharacter());
                    start = pos;
                }
            }
    
            throw SyntaxError("Unterminated string");
        }
    
        /**
         * Unescapes the character identified by the character or characters that
         * immediately follow a backslash. The backslash '\' should have already
         * been read. This supports both unicode escapes "u000A" and two-character
         * escapes "\n".
         *
         * @throws NumberFormatException if any unicode escape sequences are
         *     malformed.
         */
        private char ReadEscapeCharacter() {
            char escaped = input[pos++];
            switch (escaped) {
                case 'u':
                    if (pos + 4 > input.Length) {
                        throw SyntaxError("Unterminated escape sequence");
                    }
                    String hex = input.Substring(pos, pos + 4 - pos);
                    pos += 4;
                    return (char)int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                case 't':
                    return '\t';
    
                case 'b':
                    return '\b';
    
                case 'n':
                    return '\n';
    
                case 'r':
                    return '\r';
    
                case 'f':
                    return '\f';
    
                case '\'':
                    return escaped;
                case '"':
                    return escaped;
                case '\\':
                    return escaped;
                default:
                    return escaped;
            }
        }
    
        /**
         * Reads a null, boolean, numeric or unquoted string literal value. Numeric
         * values will be returned as an Integer, Long, or Double, in that order of
         * preference.
         */
        private object ReadLiteral() {
            string literal = NextToInternal("{}[]/\\:,=;# \t\f");
    
            if (literal.Length == 0) {
                throw SyntaxError("Expected literal value");
            } else if (string.Compare(literal, "null", true) == 0) {
                return JsonObject.NULL;
            } else if (string.Compare(literal, "true", true) == 0) {
                return true;
            } else if (string.Compare(literal, "false", true) == 0){
                return false;
            }
    
            /* try to parse as an integral type... */
            if (literal.IndexOf('.') == -1) {
                int baseType = 10;
                String number = literal;
                if (number.StartsWith("0x") || number.StartsWith("0X")) {
                    number = number.Substring(2);
                    baseType = 16;
                } else if (number.StartsWith("0") && number.Length > 1) {
                    number = number.Substring(1);
                    baseType = 8;
                }
                try {
                    long longValue = Convert.ToInt64(number, baseType);
                    if (longValue <= int.MaxValue && longValue >= int.MinValue) {
                        return (int) longValue;
                    } else {
                        return longValue;
                    }
                } catch (FormatException) {
                    /*
                     * This only happens for integral numbers greater than
                     * Long.MAX_VALUE, numbers in exponential form (5e-10) and
                     * unquoted strings. Fall through to try floating point.
                     */
                }
            }
    
            /* ...next try to parse as a floating point... */
            try {
                return double.Parse(literal, NUMBER_CULTURE);
            } catch (FormatException) {
            }
    
            /* ... finally give up. We have an unquoted string */
            return literal; // a new string avoids leaking memory
        }
    
        /**
         * Returns the string up to but not including any of the given characters or
         * a newline character. This does not consume the excluded character.
         */
        private String NextToInternal(String excluded) {
            int start = pos;
            for (; pos < input.Length; pos++) {
                char c = input[pos];
                if (c == '\r' || c == '\n' || excluded.IndexOf(c) != -1) {
                    return input.Substring(start, pos - start);
                }
            }
            return input.Substring(start);
        }
    
        /**
         * Reads a sequence of key/value pairs and the trailing closing brace '}' of
         * an object. The opening brace '{' should have already been read.
         */
        private JsonObject ReadObject() {
            JsonObject result = new JsonObject();
    
            /* Peek to see if this is the empty object. */
            int first = NextCleanInternal();
            if (first == '}') {
                return result;
            } else if (first != -1) {
                pos--;
            }

            while (true) {
                object name = NextValue();
                if (!(name is String)) {
                    if (name == null) {
                        throw SyntaxError("Names cannot be null");
                    } else {
                        throw SyntaxError("Names must be strings, but " + name
                                + " is of type " + name.GetType().ToString());
                    }
                }
    
                /*
                 * Expect the name/value separator to be either a colon ':', an
                 * equals sign '=', or an arrow "=>". The last two are bogus but we
                 * include them because that's what the original implementation did.
                 */
                int separator = NextCleanInternal();
                if (separator != ':' && separator != '=') {
                    throw SyntaxError("Expected ':' after " + name);
                }
                if (pos < input.Length && input[pos] == '>') {
                    pos++;
                }
    
                result.Put((String) name, NextValue());
    
                switch (NextCleanInternal()) {
                    case '}':
                        return result;
                    case ';':
                    case ',':
                        continue;
                    default:
                        throw SyntaxError("Unterminated object");
                }
            }
        }
        internal string Input
        {
            get
            {
                return this.input;
            }
        }
        /**
         * Reads a sequence of values and the trailing closing brace ']' of an
         * array. The opening brace '[' should have already been read. Note that
         * "[]" yields an empty array, but "[,]" returns a two-element array
         * equivalent to "[null,null]".
         */
        private JsonArray ReadArray() {
            JsonArray result = new JsonArray();
    
            /* to cover input that ends with ",]". */
            bool hasTrailingSeparator = false;

            while (true) {
                switch (NextCleanInternal()) {
                    case -1:
                        throw SyntaxError("Unterminated array");
                    case ']':
                        if (hasTrailingSeparator) {
                            result.Put(null);
                        }
                        return result;
                    case ',':
                        goto case ';';
                    case ';':
                        /* A separator without a value first means "null". */
                        result.Put(null);
                        hasTrailingSeparator = true;
                        continue;
                    default:
                        pos--;
                        break;
                }
    
                result.Put(NextValue());
    
                switch (NextCleanInternal()) {
                    case ']':
                        return result;
                    case ',':
                        goto case ';';
                    case ';':
                        hasTrailingSeparator = true;
                        continue;
                    default:
                        throw SyntaxError("Unterminated array");
                }
            }
        }
    
        /**
         * Returns an exception containing the given message plus the current
         * position and the entire input string.
         */
        public JsonException SyntaxError(String message) {
            return new JsonException(message + this);
        }
    
        /**
         * Returns the current position and the entire input string.
         */
        public override string ToString() {
            // consistent with the original implementation
            return " at character " + pos + " of " + input;
        }
    
        /*
         * Legacy APIs.
         *
         * None of the methods below are on the critical path of parsing JSON
         * documents. They exist only because they were exposed by the original
         * implementation and may be used by some clients.
         */
    
        /**
         * Returns true until the input has been exhausted.
         */
        public bool More() {
            return pos < input.Length;
        }
    
        /**
         * Returns the next available character, or the null character '\0' if all
         * input has been exhausted. The return value of this method is ambiguous
         * for JSON strings that contain the character '\0'.
         */
        public char Next() {
            return pos < input.Length ? input[pos++] : '\0';
        }
    
        /**
         * Returns the next available character if it equals {@code c}. Otherwise an
         * exception is thrown.
         */
        public char Next(char c) {
            char result = Next();
            if (result != c) {
                throw SyntaxError("Expected " + c + " but was " + result);
            }
            return result;
        }
    
        /**
         * Returns the Next character that is not whitespace and does not belong to
         * a comment. If the input is exhausted before such a character can be
         * found, the null character '\0' is returned. The return value of this
         * method is ambiguous for JSON strings that contain the character '\0'.
         */
        public char NextClean() {
            int nextCleanInt = NextCleanInternal();
            return nextCleanInt == -1 ? '\0' : (char) nextCleanInt;
        }
    
        /**
         * Returns the next {@code length} characters of the input.
         *
         * <p>The returned string shares its backing character array with this
         * tokener's input string. If a reference to the returned string may be held
         * indefinitely, you should use {@code new String(result)} to copy it first
         * to avoid memory leaks.
         *
         * @throws JSONException if the remaining input is not long enough to
         *     satisfy this request.
         */
        public String Next(int length) {
            if (pos + length > input.Length) {
                throw SyntaxError(length + " is out of bounds");
            }
            String result = input.Substring(pos, pos + length - pos);
            pos += length;
            return result;
        }
    
        /**
         * Returns the {@link String#trim trimmed} string holding the characters up
         * to but not including the first of:
         * <ul>
         *   <li>any character in {@code excluded}
         *   <li>a newline character '\n'
         *   <li>a carriage return '\r'
         * </ul>
         *
         * <p>The returned string shares its backing character array with this
         * tokener's input string. If a reference to the returned string may be held
         * indefinitely, you should use {@code new String(result)} to copy it first
         * to avoid memory leaks.
         *
         * @return a possibly-empty string
         */
        public String NextTo(String excluded) {
            if (excluded == null) {
                throw new NullReferenceException();
            }
            return NextToInternal(excluded).Trim();
        }
    
        /**
         * Equivalent to {@code NextTo(String.valueOf(excluded))}.
         */
        public String NextTo(char excluded) {
            return NextToInternal(Convert.ToString(excluded)).Trim();
        }
    
        /**
         * Advances past all input up to and including the Next occurrence of
         * {@code thru}. If the remaining input doesn't contain {@code thru}, the
         * input is exhausted.
         */
        public void SkipPast(String thru) {
            int thruStart = input.IndexOf(thru, pos);
            pos = thruStart == -1 ? input.Length : (thruStart + thru.Length);
        }
    
        /**
         * Advances past all input up to but not including the next occurrence of
         * {@code to}. If the remaining input doesn't contain {@code to}, the input
         * is unchanged.
         */
        public char SkipTo(char to) {
            int index = input.IndexOf(to, pos);
            if (index != -1) {
                pos = index;
                return to;
            } else {
                return '\0';
            }
        }
    
        /**
         * Unreads the most recent character of input. If no input characters have
         * been read, the input is unchanged.
         */
        public void Back() {
            if (--pos == -1) {
                pos = 0;
            }
        }
    
        /**
         * Returns the integer [0..15] value for the given hex character, or -1
         * for non-hex input.
         *
         * @param hex a character in the ranges [0-9], [A-F] or [a-f]. Any other
         *     character will yield a -1 result.
         */
        public static int Dehexchar(char hex) {
            if (hex >= '0' && hex <= '9') {
                return hex - '0';
            } else if (hex >= 'A' && hex <= 'F') {
                return hex - 'A' + 10;
            } else if (hex >= 'a' && hex <= 'f') {
                return hex - 'a' + 10;
            } else {
                return -1;
            }
        }
    }
}

