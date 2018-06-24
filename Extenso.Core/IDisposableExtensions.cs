using System;

namespace Extenso
{
    /// <summary>
    /// Provides a set of static methods for manipulating objects that inherit from System.IDisposable.
    /// </summary>
    public static class DisposableExtensions
    {
        /// <summary>
        /// Calls the Dispose() method on obj if obj is not null
        /// </summary>
        /// <param name="obj">The object to dispose of.</param>
        public static void DisposeIfNotNull(this IDisposable obj)
        {
            if (obj != null)
            {
                obj.Dispose();
            }
        }
    }
}