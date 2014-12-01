namespace StyleCop.Analyzers.Generator
{
    using System.Text.RegularExpressions;

    public class Line
    {
        private string text;
        private bool isParagraph;

        public Line(string text)
        {
            this.text = text;
            this.isParagraph = false;
        }

        public Line(string text, bool isParagraph)
        {
            this.text = text;
            this.isParagraph = isParagraph;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public bool IsParagraph
        {
            get { return isParagraph; }
            set { isParagraph = value; }
        }

        public int Length
        {
            get { return text.Length; }
        }

        public void Replace(string oldValue, string newValue)
        {
            text = Regex.Replace(text, Regex.Escape(oldValue), newValue, RegexOptions.IgnoreCase);
        }

        public void Format(string format)
        {
            text = string.Format(format, text);
        }

        public override string ToString()
        {
            return text;
        }
    }
}
