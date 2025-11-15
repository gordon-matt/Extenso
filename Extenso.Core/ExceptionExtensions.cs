using System.Text;

namespace Extenso;

/// <summary>
/// Provides a set of static methods for working with exceptions.
/// </summary>
public static class ExceptionExtensions
{
    extension(Exception source)
    {
        /// <summary>
        /// Gets the messages from the given System.Exception and every inner exception thereof.
        /// </summary>
        /// <returns>A System.String with the given exception's message and the messages of all inner exceptions on separate lines.</returns>
        public string GetMessageStack()
        {
            if (source is null)
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.AppendLine(source.Message);

            while (source.InnerException is not null)
            {
                source = source.InnerException;
                sb.Append("--> ");
                sb.AppendLine(source.Message);
            }

            return sb.ToString();
        }
    }
}