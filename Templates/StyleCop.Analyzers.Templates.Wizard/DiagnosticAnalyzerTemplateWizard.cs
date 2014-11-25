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
            throw new NotImplementedException();
        }

        public void ProjectFinishedGenerating(Project project)
        {
            throw new NotImplementedException();
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            throw new NotImplementedException();
        }

        public void RunFinished()
        {
            throw new NotImplementedException();
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            string destinationDirectory = replacementsDictionary["$destinationdirectory$"];

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
                replacementsDictionary.Add("$title$", form.PageInfo.Cause);
                replacementsDictionary.Add("$helpLink$", form.PageInfo.Link);


            }
            catch (WizardCancelledException)
            {
                RemoveFolder(destinationDirectory);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                RemoveFolder(destinationDirectory);

                throw;
            }
        }

        private string FormatClassRemarks(string remarks)
        {
            string[] lines = remarks.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = string.Format(@"<para>{0}</para>", lines[i]);

                lines[i] = lines[i].Replace(@"<EM>", @"<c>");
                lines[i] = lines[i].Replace(@"<em>", @"<c>");
                lines[i] = lines[i].Replace(@"</EM>", @"</c>");
                lines[i] = lines[i].Replace(@"</em>", @"</c>");
            }

            List<string> formatedLines = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                formatedLines.AddRange(SplitAt(lines[i], 140));
            }

            StringBuilder text = new StringBuilder();

            for (int i = 0; i < formatedLines.Count; i++)
            {
                text.AppendLine(string.Format(@"    /// {0}", formatedLines[i]));
            }

            return text.ToString();
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

        private void RemoveFolder(string folderToRemove)
        {
            // Clean up the template that was written to disk
            if (Directory.Exists(folderToRemove))
            {
                Directory.Delete(folderToRemove, true);
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
