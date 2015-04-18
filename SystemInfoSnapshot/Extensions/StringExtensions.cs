/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */
using System;

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
    }
}
