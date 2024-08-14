using System.Text;

namespace Slnx
{
    /// <summary>
    /// Internal class used by Slnx to write text with indentation.
    /// </summary>
    internal class TextBuilderInternal
    {
        private readonly StringBuilder _sb;
        private int _indent = 0;
        private const int IndentBy = 4;

        public TextBuilderInternal(StringBuilder sb)
        {
            _sb = sb;
        }

        public void IndentUp()
        {
            _indent += IndentBy;
        }

        public void IndentDown()
        {
            _indent -= IndentBy;

            if (_indent < 0) _indent = 0;
        }

        public void Write(string text)
        {
            _sb.Append($"{text}");
        }

        public void WriteLine(string text)
        {
            _sb.AppendLine($"{text}");
        }

        public void WriteIndented(string text)
        {
            _sb.Append($"{new string(' ', _indent)}{text}");
        }

        public void WriteLineIndented(string text)
        {
            _sb.AppendLine($"{new string(' ', _indent)}{text}");
        }

        public StringBuilder GetBuilder()
        {
            return _sb;
        }

        public void SetContent(string text)
        {
            _sb.Clear();
            _sb.Append(text);
        }

        public static TextBuilderInternal CreateNew()
        {
            return new TextBuilderInternal(new StringBuilder());
        }
    }
}
