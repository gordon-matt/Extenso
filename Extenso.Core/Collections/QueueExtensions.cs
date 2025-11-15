namespace Extenso.Collections;

/// <summary>
/// Provides a set of static methods for querying and manipulating objects of type System.Collections.Generic.Queue`1.
/// </summary>
public static class QueueExtensions
{
    extension<T>(Queue<T> source)
    {
        /// <summary>
        /// Dequeues the specified number of objects from the beginning of the System.Collections.Generic.Queue`1
        /// </summary>
        /// <param name="chunkSize">The number of objects to dequeue</param>
        /// <returns>The objects that are removed from the beginning of the System.Collections.Generic.Queue`1.</returns>
        public IEnumerable<T> DequeueChunk(int chunkSize)
        {
            for (int i = 0; i < chunkSize && source.Count > 0; i++)
            {
                yield return source.Dequeue();
            }
        }
    }
}