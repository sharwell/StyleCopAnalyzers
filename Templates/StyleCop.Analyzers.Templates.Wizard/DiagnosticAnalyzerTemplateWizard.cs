namespace StyleCop.Analyzers.Templates.Wizard
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Windows.Forms;
    using EnvDTE;
    using Microsoft.VisualStudio.TemplateWizard;

    /// <summary>
    /// Retrieves and formats the data for the DiagnosticAnalyzer Template.
    /// </summary>
    public class DiagnosticAnalyzerTemplateWizard : IWizard
    {
        protected string addedTemplateName;

        /// <inheritdoc/>
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            try
            {
                projectItem.Name = string.Format("{0}.cs", addedTemplateName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Can't rename the item template: {0}", ex.Message);
            }
        }

        /// <inheritdoc/>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                IDescriptionService descriptionService = new StyleCopSiteDescriptionService();
                ChooseSaId form = new ChooseSaId(descriptionService);

                DialogResult result = form.ShowDialog();

                if (result != DialogResult.OK)
                {
                    throw new WizardCancelledException();
                }

                addedTemplateName = form.PageInfo.CheckId + form.PageInfo.TypeName;

                replacementsDictionary.Add("$SA$", form.PageInfo.CheckId);
                replacementsDictionary.Add("$classSummary$", FormatClassRemarks(form.PageInfo.Cause));
                replacementsDictionary.Add("$classRemarks$", FormatClassRemarks(form.PageInfo.RuleDescription));
                replacementsDictionary.Add("$title$", String.Join<Line>(Environment.NewLine, form.PageInfo.Cause).TrimEnd(Environment.NewLine.ToCharArray()));
                replacementsDictionary.Add("$helpLink$", form.PageInfo.Link);
                replacementsDictionary.Add("$category$", form.PageInfo.Category);
                replacementsDictionary.Add("$className$", addedTemplateName);
            }
            catch (RuleNotFoundException notFound)
            {
                MessageBox.Show(notFound.Message);

                throw new WizardCancelledException(notFound.Message, notFound);
            }
            catch (WizardCancelledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                throw new WizardCancelledException(ex.Message, ex);
            }
        }

        /// <inheritdoc/>
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        /// <inheritdoc/>
        public void RunFinished()
        {
        }

        /// <inheritdoc/>
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        /// <inheritdoc/>
        public void ProjectFinishedGenerating(Project project)
        {
        }

        /// <summary>
        /// Formats the text with a particular style.
        /// </summary>
        /// <param name="remarks">Text to be formated.</param>
        /// <returns>Returns a formated string that will represent a part of the class remark.</returns>
        private string FormatClassRemarks(List<Line> remarks)
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
        protected string[] SplitAt(string text, int lenght)
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
