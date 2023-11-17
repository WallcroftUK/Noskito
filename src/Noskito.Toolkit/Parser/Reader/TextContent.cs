using System.Collections.Generic;

namespace Noskito.Toolkit.Parser.Reader
{
    public class TextContent : TextRegion
    {
        public TextContent(IEnumerable<TextLine> lines) : base(lines)
        {
        }
    }
}