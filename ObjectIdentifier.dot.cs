#if NET20 || NET35
using System.Collections.Generic;
#else
using System.Linq;
#endif

namespace SabreTools.ASN1
{
    /// <summary>
    /// Methods related to Object Identifiers (OID) and dot notation
    /// </summary>
    public static partial class ObjectIdentifier
    {
        /// <summary>
        /// Parse an OID in separated-value notation into dot notation
        /// </summary>
        /// <param name="values">List of values to check against</param>
        /// <returns>List of values representing the dot notation</returns>
        public static string? ParseOIDToDotNotation(ulong[]? values)
        {
            // If we have an invalid set of values, we can't do anything
            if (values == null || values.Length == 0)
                return null;

#if NET20 || NET35
            var stringValues = new List<string>();
            foreach (ulong value in values)
            {
                stringValues.Add(value.ToString());
            }

            return string.Join(".", stringValues.ToArray());
#else
            return string.Join(".", values.Select(v => v.ToString()).ToArray());
#endif
        }
    }
}