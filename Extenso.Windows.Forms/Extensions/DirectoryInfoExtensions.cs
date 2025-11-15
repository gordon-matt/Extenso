namespace Extenso.Windows.Forms;

public static class DirectoryInfoExtensions
{
    extension(DirectoryInfo directoryInfo)
    {
        /// <summary>
        /// Creates the directory, if it does not already exist
        /// </summary>
        /// <param name="directoryInfo"></param>
        public void EnsureExists()
        {
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }
    }
}