using System.Text.RegularExpressions;

namespace Extenso.Windows.Forms;

/// <summary>
/// Provides extension methods for rich text boxs.
/// </summary>
public static class RichTextBoxExtensions
{
    extension(RichTextBox richTextBox)
    {
        /// <summary>
        /// Performs the highlight operation.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">The end index.</param>
        public void Highlight(int startIndex, int endIndex) =>
            richTextBox.Highlight(startIndex, endIndex, Color.Yellow);

        /// <summary>
        /// Performs the highlight operation.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="endIndex">The end index.</param>
        /// <param name="color">The color.</param>
        public void Highlight(int startIndex, int endIndex, Color color)
        {
            richTextBox.Select(startIndex, endIndex);
            richTextBox.SelectionBackColor = color;
        }

        /// <summary>
        /// Performs the highlight all operation.
        /// </summary>
        /// <param name="text">The text.</param>
        public void HighlightAll(string text) =>
            richTextBox.HighlightAll(text, Color.Yellow);

        /// <summary>
        /// Performs the highlight all operation.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        public void HighlightAll(string text, Color color)
        {
            var regex = new Regex(text, RegexOptions.Compiled);

            foreach (Match match in regex.Matches(richTextBox.Text))
            {
                richTextBox.Highlight(match.Index, match.Length, color);
            }
        }
    }
}