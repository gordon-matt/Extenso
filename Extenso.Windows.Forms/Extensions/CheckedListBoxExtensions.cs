namespace Extenso.Windows.Forms;

public static class CheckedListBoxExtensions
{
    extension(CheckedListBox checkedListBox)
    {
        public void SetItemsChecked(bool value)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, value);
            }
        }

        public void SetItemsCheckState(CheckState value)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemCheckState(i, value);
            }
        }
    }
}