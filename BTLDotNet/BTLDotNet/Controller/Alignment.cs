using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLDotNet.Controller
{
    class Alignment
    {
        public string JustifyLines(string _text, Font _font, int _ctl_width)
        {
            string _result = "";

            // Split text in paragraphs
            List<string> _paragraphs = new List<string>();
            _paragraphs.AddRange(_text.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList());

            // Justify each paragraph and re-insert a linefeed
            foreach (string _paragraph in _paragraphs)
            {
                _result += Justify(_paragraph, _font, _ctl_width) + "\r\n";
            }

            return _result.Substring(0, _result.Length - 2); // Cuts the last linefeed
        }
        public static string JustifyParagraph(string _text, Font _font, int _ctl_width)
        {
            string _result = String.Empty;

            // Split text in paragraphs
            List<string> _paragraphs = new List<string>();
            _paragraphs.AddRange(_text.Split(new[] { "\r\n" }, StringSplitOptions.None).ToList());

            // Justify each paragraph and re-insert a linefeed
            foreach (string _paragraph in _paragraphs)
            {
                string _line = string.Empty;
                int _par_width = TextRenderer.MeasureText(_paragraph, _font).Width;

                if (_par_width > _ctl_width)
                {
                    // Get all paragraph words, add a normal space and calculate
                    // when their sum exceeds the constraints
                    string[] _Words = _paragraph.Split(' ');
                    _line = _Words[0] + (char)32;
                    for (int x = 1; x < _Words.Length; x++)
                    {
                        string _tmpline = _line + (_Words[x] + (char)32);
                        if (TextRenderer.MeasureText(_tmpline, _font).Width > _ctl_width)
                        {
                            // Max lenght reached. Justify the line and step back
                            _result += Justify(_line.TrimEnd(), _font, _ctl_width) + "\r\n";
                            _line = string.Empty;
                            --x;
                        }
                        else
                        {
                            // Some capacity still left
                            _line += (_Words[x] + (char)32);
                        }
                    }
                    // Adds the remainder if any
                    if (_line.Length > 0)
                        _result += _line + "\r\n";
                }
                else
                {
                    // This line is less than its constraint
                    _result += _paragraph + "\r\n";
                }
            }
            return _result.Substring(0, _result.Length - 2); // Cuts the last linefeed
        }
        private static string Justify(string _text, Font _font, int _width)
        {
            char _spacechar = (char)0x200A;
            // Extract all text words.
            List<string> _Words = _text.Split(' ').ToList();
            if (_Words.Capacity < 2)
                return _text;

            int _num_words = _Words.Capacity - 1;

            // Overall width of words and width of hairspace
            int _WordsWidth = TextRenderer.MeasureText(_text.Replace(" ", ""), _font).Width;
            int _spacecharwidth = TextRenderer.MeasureText(_Words[0] + _spacechar, _font).Width
                                - TextRenderer.MeasureText(_Words[0], _font).Width;

            // Calculate the average spacing between each word minus the last one 
            int _avgspace = ((_width - _WordsWidth) / _num_words) / _spacecharwidth;
            //Remainder 
            float _adjustspace = (_width - (_WordsWidth + (_avgspace * _num_words * _spacecharwidth)));

            // Add spaces to all words
            return ((Func<string>)(() =>
            {
                string _spaces = "";
                string _adjustedwords = "";

                // Builds a "Spacer" string using "_spacechar"
                for (int h = 0; h < _avgspace; h++)
                    _spaces += _spacechar;

                foreach (string _word in _Words)
                {
                    _adjustedwords += _word + _spaces;
                    //Adjust the spacing if there's a reminder
                    if (_adjustspace > 0)
                    {
                        _adjustedwords += _spacechar;
                        _adjustspace -= _spacecharwidth;
                    }
                }
                return _adjustedwords.TrimEnd();
            }))();

        }
    }
}
