namespace Extenso.Windows.Forms;

/// <summary>
/// Provides extension methods for tree views.
/// </summary>
public static class TreeViewExtensions
{
    extension(TreeView treeView)
    {
        /// <summary>
        /// Gets the node by full path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The node by full path.</returns>
        public TreeNode GetNodeByFullPath(string path) =>
            treeView.Nodes.OfType<TreeNode>().Where(x => x.FullPath == path).SingleOrDefault();

        /// <summary>
        /// Gets the node by name.
        /// </summary>
        /// <param name="nodeName">The node name.</param>
        /// <returns>The node by name.</returns>
        public TreeNode GetNodeByName(string nodeName) =>
            treeView.Nodes.OfType<TreeNode>().Where(x => x.Name == nodeName).SingleOrDefault();

        /// <summary>
        /// Gets the node by text.
        /// </summary>
        /// <param name="nodeText">The node text.</param>
        /// <returns>The node by text.</returns>
        public TreeNode GetNodeByText(string nodeText) =>
            treeView.Nodes.OfType<TreeNode>().Where(x => x.Text == nodeText).SingleOrDefault();
    }

    extension(TreeNode treeNode)
    {
        /// <summary>
        /// Gets the node by text.
        /// </summary>
        /// <param name="nodeText">The node text.</param>
        /// <returns>The node by text.</returns>
        public TreeNode GetNodeByText(string nodeText) =>
            treeNode.Nodes.OfType<TreeNode>().Where(x => x.Text == nodeText).SingleOrDefault();
    }
}