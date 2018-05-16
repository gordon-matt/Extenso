using System;
using System.IO;
using System.Text;

namespace Extenso.IO
{
    public class CustomEncodingStringWriter : StringWriter
    {
        private Encoding encoding;

        public override Encoding Encoding
        {
            get { return encoding ?? base.Encoding; }
        }

        public CustomEncodingStringWriter(Encoding encoding)
            : base()
        {
            this.encoding = encoding;
        }

        public CustomEncodingStringWriter(Encoding encoding, IFormatProvider formatProvider)
            : base(formatProvider)
        {
            this.encoding = encoding;
        }

        public CustomEncodingStringWriter(Encoding encoding, StringBuilder sb)
            : base(sb)
        {
            this.encoding = encoding;
        }

        public CustomEncodingStringWriter(Encoding encoding, StringBuilder sb, IFormatProvider formatProvider)
            : base(sb, formatProvider)
        {
            this.encoding = encoding;
        }
    }
}