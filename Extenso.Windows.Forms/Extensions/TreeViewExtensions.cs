namespace Extenso.Windows.Forms;

public static class TreeViewExtensions
{
    #region TreeView

    public static TreeNode GetNodeByFullPath(this TreeView treeView, string path) =>
        treeView.Nodes.Cast<TreeNode>().Where(x => x.FullPath == path).SingleOrDefault();

    public static TreeNode GetNodeByName(this TreeView treeView, string nodeName) =>
        treeView.Nodes.Cast<TreeNode>().Where(x => x.Name == nodeName).SingleOrDefault();

    public static TreeNode GetNodeByText(this TreeView treeView, string nodeText) =>
        treeView.Nodes.Cast<TreeNode>().Where(x => x.Text == nodeText).SingleOrDefault();

    #endregion TreeView

    #region TreeNode

    public static TreeNode GetNodeByText(this TreeNode treeNode, string nodeText) =>
        treeNode.Nodes.Cast<TreeNode>().Where(x => x.Text == nodeText).SingleOrDefault();

    #endregion TreeNode
}