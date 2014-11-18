namespace StyleCop.Analyzers
{
    using System;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    public struct SyntaxTokenAnalysisContext
    {
        private readonly Action<Diagnostic> _reportDiagnostic;

        public SyntaxTokenAnalysisContext(SyntaxToken token, AnalyzerOptions options, Action<Diagnostic> reportDiagnostic, CancellationToken cancellationToken)
        {
            Token = token;
            Options = options;
            _reportDiagnostic = reportDiagnostic;
            CancellationToken = cancellationToken;
        }

        public CancellationToken CancellationToken
        {
            get;
        }

        public SyntaxToken Token
        {
            get;
        }

        public AnalyzerOptions Options
        {
            get;
        }

        public void ReportDiagnostic(Diagnostic diagnostic)
        {
            _reportDiagnostic?.Invoke(diagnostic);
        }
    }
}
