namespace StyleCop.Analyzers
{
    using System;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Context for a syntax token action. A syntax token action can use a <see cref="SyntaxTokenAnalysisContext"/> to
    /// report <see cref="Diagnostic"/>s about a <see cref="SyntaxToken"/> within a code document.
    /// </summary>
    public struct SyntaxTokenAnalysisContext
    {
        /// <summary>
        /// This is the backing field for the <see cref="ReportDiagnostic"/> method.
        /// </summary>
        private readonly Action<Diagnostic> _reportDiagnostic;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyntaxTokenAnalysisContext"/>.
        /// </summary>
        /// <param name="token">The <see cref="SyntaxToken"/> to analyze.</param>
        /// <param name="options">Options specified for the analysis.</param>
        /// <param name="reportDiagnostic">A callback method used for reporting <see cref="Diagnostic"/>s.</param>
        /// <param name="cancellationToken">The cancellation token that the analyzer will observe.</param>
        internal SyntaxTokenAnalysisContext(SyntaxToken token, AnalyzerOptions options, Action<Diagnostic> reportDiagnostic, CancellationToken cancellationToken)
        {
            Token = token;
            Options = options;
            _reportDiagnostic = reportDiagnostic;
            CancellationToken = cancellationToken;
        }

        /// <summary>
        /// Gets the cancellation token that the task will observe.
        /// </summary>
        public CancellationToken CancellationToken
        {
            get;
        }

        /// <summary>
        /// Gets the syntax token to analyze.
        /// </summary>
        public SyntaxToken Token
        {
            get;
        }

        /// <summary>
        /// Gets the options specified for the analysis.
        /// </summary>
        public AnalyzerOptions Options
        {
            get;
        }

        /// <summary>
        /// Report a diagnostic message identified during the analysis.
        /// </summary>
        /// <param name="diagnostic">A <see cref="Diagnostic"/> instance describing the identified issue or
        /// message.</param>
        public void ReportDiagnostic(Diagnostic diagnostic)
        {
            _reportDiagnostic?.Invoke(diagnostic);
        }
    }
}
