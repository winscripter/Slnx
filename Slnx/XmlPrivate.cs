using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Slnx
{
    internal static class XmlPrivate
    {
        public static XmlWriterSettings DefaultSettings => new()
        {
            Indent = true,
            IndentChars = "    ", // 4 spaces
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace,
            Encoding = new UTF8Encoding(false),
            OmitXmlDeclaration = true
        };

        internal static string FormatIndented(this XNode node, XmlWriterSettings settings)
        {
            using var sw = new StringWriter();

            using (var xmlWriter = XmlWriter.Create(sw, settings))
            {
                node.WriteTo(xmlWriter);
            }

            return sw.ToString();
        }
    }
}
