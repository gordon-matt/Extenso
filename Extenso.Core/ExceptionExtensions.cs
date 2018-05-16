using System;
using System.Text;

namespace Extenso
{
    public static class ExceptionExtensions
    {
        public static string GetMessageStack(this Exception x)
        {
            if (x == null)
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.AppendLine(x.Message);

            while (x.InnerException != null)
            {
                x = x.InnerException;
                sb.Append("--> ");
                sb.AppendLine(x.Message);
            }

            return sb.ToString();
        }
    }
}