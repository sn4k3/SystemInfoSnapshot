using System.Web.Script.Serialization;

namespace SystemInfoSnapshot.Components
{
    /// <summary>
    /// Javascript utility class
    /// </summary>
    public static class JavaScript
    {
        /*public static string Encode(string value, bool addDoubleQuotes)
        {
            return System.Web.JavaScriptStringEncode
        }*/
        /// <summary>
        /// Encode a object into javascript code.
        /// </summary>
        /// <param name="obj">Object to encode.</param>
        /// <param name="assingVar">Assign the serialized object to an variable. Null for no variable</param>
        /// <returns>A valid javascript string.</returns>
        public static string Encode(object obj, string assingVar = null)
        {
            var js = new JavaScriptSerializer();
            return !string.IsNullOrEmpty(assingVar) ? 
                string.Format("var {0} = {1};", assingVar, js.Serialize(obj)) 
                : 
                js.Serialize(obj);
        }
    }
}
