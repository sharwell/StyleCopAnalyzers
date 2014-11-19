namespace StyleCop.Analyzers
{
    using System;
    using Microsoft.CodeAnalysis.Diagnostics;

    public static class TokenAnalysisExtensions
    {
        public static void RegisterSyntaxTokenAction<TLanguageEnumKind>(this AnalysisContext context, Action<SyntaxTokenAnalysisContext> action, params TLanguageEnumKind[] syntaxKinds)
        {
            foreach (var syntaxKind in syntaxKinds)
                TokenAnalysisDiagnosticAnalyzer.RegisterSyntaxTokenAction(Convert.ToInt32(syntaxKind), action);
        }
    }
}
