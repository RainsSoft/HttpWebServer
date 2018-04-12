using System.IO;
using System.Text;

namespace HttpServer.Mvc.Views.TinyEngine
{
    internal class TinyParser
    {
        private readonly TextReader _reader;
        private StringBuilder _code = new StringBuilder();
        private StringBuilder _currentData = new StringBuilder();
        private ParserState _state;

        public TinyParser(TextReader reader)
        {
            _reader = reader;
        }


        public bool EOF
        {
            get { return _reader.Peek() == -1; }
        }

        public char Peek
        {
            get { return EOF ? char.MinValue : (char) _reader.Peek(); }
        }

        private void FindTags()
        {
        }

        public void Parse()
        {
            ParseMethod method = FindTags;
            while (!EOF)
            {
                var ch = (char) _reader.Read();

                if (ch == '\\')
                {
                }
                if (ch == '"')
                {
                }

                if (ch == '<' && Peek == '%' && _state != ParserState.InTag)
                {
                    if (_state != ParserState.InTag)
                        _state = ParserState.InTag;
                }
            }
        }

        #region Nested type: ParseMethod

        private delegate void ParseMethod();

        #endregion

        #region Nested type: ParserState

        private enum ParserState
        {
            InTag,
            InTagText,
            Outside
        }

        #endregion
    }
}