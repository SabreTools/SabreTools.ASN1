using System;

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

            var stringValues = Array.ConvertAll(values, v => v.ToString());
            return string.Join(".", [.. stringValues]);
        }
    }
}