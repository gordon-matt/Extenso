using System.Collections.Generic;

namespace Extenso.Collections
{
    /// <summary>
    /// Provides a set of static methods for querying and manipulating objects of type System.Collections.Generic.Queue`1.
    /// </summary>
    public static class QueueExtensions
    {
        /// <summary>
        /// Dequeues the specified number of objects from the beginning of the System.Collections.Generic.Queue`1
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">The System.Collections.Generic.Queue`1 to dequeue objects from.</param>
        /// <param name="chunkSize">The number of objects to dequeue</param>
        /// <returns>The objects that are removed from the beginning of the System.Collections.Generic.Queue`1.</returns>
        public static IEnumerable<T> DequeueChunk<T>(this Queue<T> source, int chunkSize)
        {
            for (int i = 0; i < chunkSize && source.Count > 0; i++)
            {
                yield return source.Dequeue();
            }
        }
    }
}