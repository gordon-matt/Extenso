namespace Extenso.Windows.Forms;

public static class TreeViewExtensions
{
    extension(TreeView treeView)
    {
        public TreeNode GetNodeByFullPath(string path) =>
            treeView.Nodes.OfType<TreeNode>().Where(x => x.FullPath == path).SingleOrDefault();

        public TreeNode GetNodeByName(string nodeName) =>
            treeView.Nodes.OfType<TreeNode>().Where(x => x.Name == nodeName).SingleOrDefault();

        public TreeNode GetNodeByText(string nodeText) =>
            treeView.Nodes.OfType<TreeNode>().Where(x => x.Text == nodeText).SingleOrDefault();
    }

    extension(TreeNode treeNode)
    {
        public TreeNode GetNodeByText(string nodeText) =>
            treeNode.Nodes.OfType<TreeNode>().Where(x => x.Text == nodeText).SingleOrDefault();
    }
}