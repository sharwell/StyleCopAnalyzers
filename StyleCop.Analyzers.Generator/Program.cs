namespace StyleCop.Analyzers.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using File = System.IO.File;
    using Path = System.IO.Path;

    internal class Program
    {
        public static void Main(string[] args)
        {
            List<int> ids = new List<int>();
            ids.AddRange(Enumerable.Range(1000, 28));
            ids.AddRange(Enumerable.Range(1100, 27));
            ids.AddRange(Enumerable.Range(1200, 16));
            ids.AddRange(Enumerable.Range(1300, 12));
            ids.AddRange(Enumerable.Range(1400, 12));
            ids.AddRange(Enumerable.Range(1500, 19));
            ids.AddRange(Enumerable.Range(1600, 51));

            foreach (int id in ids)
            {
                IDescriptionService descriptionService = new StyleCopSiteDescriptionService();
                if (!descriptionService.RuleExists(id).Result)
                    continue;

                PageInfo pageInfo = descriptionService.GetPageInfo(id).Result;

                string className = pageInfo.CheckId + pageInfo.TypeName;
                string category = pageInfo.Category.Replace(" ", string.Empty);
                string fileName = Path.Combine(category, className + ".cs");

                Console.Write("Generating {0}... ", fileName);

                if (File.Exists(fileName))
                {
                    Console.WriteLine("already exists.");
                    continue;
                }

                Dictionary<string, string> replacementsDictionary = new Dictionary<string, string>();
                replacementsDictionary.Add("$SA$", pageInfo.CheckId);
                replacementsDictionary.Add("$classSummary$", FormatClassRemarks(pageInfo.Cause));
                replacementsDictionary.Add("$classRemarks$", FormatClassRemarks(pageInfo.RuleDescription));
                replacementsDictionary.Add("$title$", string.Join(Environment.NewLine, pageInfo.Cause).TrimEnd(Environment.NewLine.ToCharArray()));
                replacementsDictionary.Add("$helpLink$", pageInfo.Link);
                replacementsDictionary.Add("$category$", category);
                replacementsDictionary.Add("$className$", pageInfo.CheckId + pageInfo.TypeName);
                replacementsDictionary.Add("$rootnamespace$", "StyleCop.Analyzers." + category);

                string result = Resources.DiagnosticAnalyzerTemplate;
                foreach (var pair in replacementsDictionary)
                {
                    result = result.Replace(pair.Key, pair.Value);
                }

                // Normalize newlines
                result = result.Replace("\r\n", "\n");
                result = result.Replace("\r", "\n");
                result = result.Replace("\n", Environment.NewLine);

                File.WriteAllText(fileName, result, new UTF8Encoding(true));

                Console.WriteLine("done.");
            }
        }

        /// <summary>
        /// Formats the text with a particular style.
        /// </summary>
        /// <param name="remarks">Text to be formated.</param>
        /// <returns>Returns a formated string that will represent a part of the class remark.</returns>
        private static string FormatClassRemarks(List<Line> remarks)
        {
            const int maxLineLength = 130;
            List<string> formatedLines = new List<string>();
            StringBuilder text = new StringBuilder();

            // Substitute all of the web formating with a correct XmlDocumentatin elements
            foreach (Line line in remarks)
            {
                // In case we should treat this line as a paragraph, decorate with the right elements
                if (line.IsParagraph)
                {
                    line.Format(@"<para>{0}</para>");
                }

                line.Replace(@"<EM>", @"<c>");
                line.Replace(@"</EM>", @"</c>");

                // in case the lines are longer than 130 char, split them opportunely
                if (line.Length > maxLineLength)
                {
                    formatedLines.AddRange(SplitAt(line.Text, maxLineLength));
                }
                else
                {
                    formatedLines.Add(line.Text);
                }
            }

            // Decorate every line with the leading spaces (4) and the slash signs
            foreach (string line in formatedLines)
            {
                text.AppendLine(string.Format(@"    /// {0}", line));
            }

            return text.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }

        /// <summary>
        /// Splits a string in an array of strings of a given length.
        /// </summary>
        /// <param name="text">String that should be split.</param>
        /// <param name="lenght">Maximum length of each substring.</param>
        /// <returns>An array of strings where each line represents a substring of a given text of specified length.</returns>
        /// <remarks>
        /// In case the character at the split position is not a space character, the last occurrence
        /// of the space character will be searched and the string will be split only at that point.
        /// </remarks>
        private static string[] SplitAt(string text, int lenght)
        {
            List<string> splitted = new List<string>();

            if (text.Length > lenght)
            {
                int trunkateAt = text.Substring(0, lenght).LastIndexOf(" ");

                if (trunkateAt == 0)
                {
                    trunkateAt = lenght;
                }

                splitted.Add(text.Substring(0, trunkateAt));

                string rest = text.Substring(trunkateAt + 1);

                if (rest.Length > lenght)
                {
                    string[] subSplit = SplitAt(rest, lenght);

                    splitted.AddRange(subSplit);
                }
                else
                {
                    splitted.Add(rest);
                }
            }

            return splitted.ToArray();
        }
    }
}
