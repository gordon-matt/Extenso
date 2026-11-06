using System.Diagnostics;

namespace Extenso.KendoGridBinder.Validations;

/// <summary>
/// Provides guard.
/// </summary>
[DebuggerStepThrough]
public static class Guard
{
    /// <summary>
    /// Gets the not null.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value.</param>
    /// <param name="argumentName">The argument name.</param>
    /// <returns>The not null.</returns>
    public static T NotNull<T>(T value, string argumentName)
    {
        if (value is null)
        {
            NotNullOrEmpty(argumentName, nameof(argumentName));

            throw new ArgumentNullException(argumentName);
        }

        return value;
    }

    /// <summary>
    /// Gets the not null or empty.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="argumentName">The argument name.</param>
    /// <returns>The not null or empty.</returns>
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