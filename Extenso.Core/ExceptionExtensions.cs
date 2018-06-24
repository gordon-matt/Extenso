using System;
using System.Text;

namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for working with exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Gets the messages from the given System.Exception and every inner exception thereof.
        /// </summary>
        /// <param name="exception">The System.Exception from which to extract the error messages.</param>
        /// <returns>A System.String with the given exception's message and the messages of all inner exceptions on separate lines.</returns>
        public static string GetMessageStack(this Exception exception)
        {
            if (exception == null)
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.AppendLine(exception.Message);

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                sb.Append("--> ");
                sb.AppendLine(exception.Message);
            }

            return sb.ToString();
        }
    }
}