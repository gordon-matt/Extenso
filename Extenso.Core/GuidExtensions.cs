using System;

namespace Extenso
{
    public static class GuidExtensions
    {
        /// <summary>
        /// Indicates whether the specified System.Guid is null or empty
        /// </summary>
        /// <param name="guid">This instance of System.Guid.</param>
        /// <returns>true if the System.Guid is null or an empty Guid (00000000-0000-0000-0000-000000000000); otherwise, false.</returns>
        public static bool IsNullOrEmpty(this Guid guid)
        {
            return guid == null || guid == Guid.Empty;
        }
    }
}