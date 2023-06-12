namespace Extenso.Windows.Forms;

public static class DirectoryInfoExtensions
{
    /// <summary>
    /// Creates the directory, if it does not already exist
    /// </summary>
    /// <param name="directoryInfo"></param>
    public static void EnsureExists(this DirectoryInfo directoryInfo)
    {
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }
    }
}