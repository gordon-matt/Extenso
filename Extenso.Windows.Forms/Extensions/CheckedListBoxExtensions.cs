namespace Extenso.Windows.Forms;

/// <summary>
/// Provides extension methods for checked list boxs.
/// </summary>
public static class CheckedListBoxExtensions
{
    extension(CheckedListBox checkedListBox)
    {
        /// <summary>
        /// Sets the items checked.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetItemsChecked(bool value)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, value);
            }
        }

        /// <summary>
        /// Sets the items check state.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetItemsCheckState(CheckState value)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemCheckState(i, value);
            }
        }
    }
}