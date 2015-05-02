/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */
using System;
using System.Text.RegularExpressions;

namespace SystemInfoSnapshot.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static string GetNotNull(this string source, string defaultString = "")
        {
            if (!string.IsNullOrEmpty(source))
                return defaultString;

            return source;
        }

        /*public static bool SetIfNull(this string source, string value)
        {
            if (!string.IsNullOrEmpty(source))
                return false;
            source = value;
            return true;
        }*/

        public static string SpaceCamelCase(this string s)
        {
            return new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace).Replace(s, " ");
        }
    }
}
