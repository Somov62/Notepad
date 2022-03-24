using System.Windows.Controls;
using System.Windows.Documents;

namespace notepad
{
    public static class Extension
    {
        public static void SetText(this RichTextBox richTextBox, string text)
        {
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
        }
        public static void Clear(this RichTextBox richTextBox)
        {
            richTextBox.Document.Blocks.Clear();
        }

        public static string GetText(this RichTextBox richTextBox)
        {
            return new TextRange(richTextBox.Document.ContentStart,richTextBox.Document.ContentEnd).Text;
        }
        public static void AddLine(this RichTextBox richTextBox, string line)
        {
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(line)));
        }
    }
}
