namespace StyleCop.Analyzers
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    public static class TokenAnalysisExtensions
    {
        public static void RegisterSyntaxTokenAction(this AnalysisContext context, Action<SyntaxTokenAnalysisContext> action, params SyntaxKind[] syntaxKinds)
        {
            foreach (var syntaxKind in syntaxKinds)
                TokenAnalysisDiagnosticAnalyzer.RegisterSyntaxTokenAction(Convert.ToInt32(syntaxKind), action);
        }
    }
}
