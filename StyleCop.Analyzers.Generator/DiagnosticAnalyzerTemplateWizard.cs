namespace StyleCop.Analyzers.Templates.Wizard
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using EnvDTE;
    using Microsoft.VisualStudio.TemplateWizard;

    /// <summary>
    /// Retrieves and formats the data for the DiagnosticAnalyzer Template.
    /// </summary>
    public class DiagnosticAnalyzerTemplateWizard : IWizard
    {
        /// <inheritdoc/>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
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

        /// <inheritdoc/>
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }
    }
}
