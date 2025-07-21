using System.Diagnostics;

namespace Extenso.KendoGridBinder.Validations;

[DebuggerStepThrough]
public static class Guard
{
    public static T NotNull<T>(T value, string argumentName)
    {
        if (value is null)
        {
            NotNullOrEmpty(argumentName, nameof(argumentName));

            throw new ArgumentNullException(argumentName);
        }

        return value;
    }

    public static string NotNullOrEmpty(string value, string argumentName)
    {
        Exception e = null;
        if (value is null)
        {
            e = new ArgumentNullException(argumentName);
        }
        else if (value.Trim().Length == 0)
        {
            e = new ArgumentException($"The string argument '{argumentName}' cannot be empty.");
        }

        if (e != null)
        {
            NotNullOrEmpty(argumentName, nameof(argumentName));

            throw e;
        }

        return value;
    }
}