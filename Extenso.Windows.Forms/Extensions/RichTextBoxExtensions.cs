using System.Text.RegularExpressions;

namespace Extenso.Windows.Forms;

public static class RichTextBoxExtensions
{
    extension(RichTextBox richTextBox)
    {
        public void Highlight(int startIndex, int endIndex) =>
            richTextBox.Highlight(startIndex, endIndex, Color.Yellow);

        public void Highlight(int startIndex, int endIndex, Color color)
        {
            richTextBox.Select(startIndex, endIndex);
            richTextBox.SelectionBackColor = color;
        }

        public void HighlightAll(string text) =>
            richTextBox.HighlightAll(text, Color.Yellow);

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