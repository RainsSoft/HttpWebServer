using System;
using SharpWeb.Tools;

namespace SharpWeb.Headers.Parsers
{
    /// <summary>
    /// Parses "Date" header.
    /// </summary>
    [ParserFor(DateHeader.NAME)]
    [ParserFor("If-Modified-Since")]
    internal class DateParser : IHeaderParser
    {
        #region IHeaderParser Members

        /// <summary>
        /// Parse a header
        /// </summary>
        /// <param name="name">Name of header.</param>
        /// <param name="reader">Reader containing value.</param>
        /// <returns>HTTP Header</returns>
        /// <exception cref="FormatException">Header value is not of the expected format.</exception>
        public IHeader Parse(string name, ITextReader reader)
        {
            string value = reader.ReadToEnd();

            try
            {
                return new DateHeader(name, DateTime.Parse(value));
            }
            catch (FormatException err)
            {
                throw new FormatException("Failed to parse 'Date' header.", err);
            }
        }

        #endregion
    }
}