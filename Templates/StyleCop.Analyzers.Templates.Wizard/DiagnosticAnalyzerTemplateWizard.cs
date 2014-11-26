using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StyleCop.Analyzers.Templates.Wizard
{
    public class DiagnosticAnalyzerTemplateWizard : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

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

                replacementsDictionary.Add("$SA$", form.PageInfo.CheckId);
                replacementsDictionary.Add("$classSummary$", FormatClassRemarks(form.PageInfo.Cause));
                replacementsDictionary.Add("$classRemarks$", FormatClassRemarks(form.PageInfo.RuleDescription));
                replacementsDictionary.Add("$title$", form.PageInfo.Cause.TrimEnd(Environment.NewLine.ToCharArray()));
                replacementsDictionary.Add("$helpLink$", form.PageInfo.Link);
                replacementsDictionary.Add("$category$", form.PageInfo.Category);
                replacementsDictionary.Add("$HasExamples$", form.PageInfo.HasExample.ToString());
                replacementsDictionary.Add("$example$", FormatClassRemarks(form.PageInfo.Examples, false));
            }
            catch (WizardCancelledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                throw;
            }
        }

        private string FormatClassRemarks(string remarks, bool treatLineAsPara = true)
        {
            string[] lines = remarks.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                if (treatLineAsPara)
                {
                    lines[i] = string.Format(@"<para>{0}</para>", lines[i]);
                }

                lines[i] = lines[i].Replace(@"<EM>", @"<c>");
                lines[i] = lines[i].Replace(@"<em>", @"<c>");
                lines[i] = lines[i].Replace(@"</EM>", @"</c>");
                lines[i] = lines[i].Replace(@"</em>", @"</c>");
            }

            List<string> formatedLines = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Length > 130)
                {
                    formatedLines.AddRange(SplitAt(lines[i], 130));
                }
                else
                {
                    formatedLines.Add(lines[i]);
                }
            }

            StringBuilder text = new StringBuilder();

            for (int i = 0; i < formatedLines.Count; i++)
            {
                text.AppendLine(string.Format(@"    /// {0}", formatedLines[i]));
            }

            return text.ToString().TrimEnd(Environment.NewLine.ToCharArray());
        }
        private string[] SplitAt(string text, int lenght)
        {
            List<string> splitted = new List<string>();

            if (text.Length > lenght)
            {
                int trunkateAt = text.Substring(0, lenght).LastIndexOf(" ");

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

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
