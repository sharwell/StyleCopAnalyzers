namespace StyleCop.Analyzers
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Provides extension methods to simplify the analysis of syntax tokens within a code document.
    /// </summary>
    public static class TokenAnalysisExtensions
    {
        /// <summary>
        /// Register an action to be executed at completion of parsing a code document. A syntax token action reports
        /// <see cref="Diagnostic"/>s about the <see cref="SyntaxToken"/>s within a document.
        /// </summary>
        /// <typeparam name="TLanguageEnumKind">Enum type giving the syntax node kinds of the source language for which
        /// the action applies.</typeparam>
        /// <param name="context">The analysis context.</param>
        /// <param name="action">Action to be executed at the end of semantic analysis of a code block.</param>
        /// <param name="syntaxKinds">Action will only be execute if a <see cref="SyntaxToken"/>'s Kind matches one of
        /// the syntax kind values.</param>
        public static void RegisterSyntaxTokenAction<TLanguageEnumKind>(this AnalysisContext context, Action<SyntaxTokenAnalysisContext> action, params TLanguageEnumKind[] syntaxKinds)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (syntaxKinds == null)
                throw new ArgumentNullException(nameof(syntaxKinds));
            if (syntaxKinds.Length == 0)
                throw new ArgumentException("syntaxKinds cannot be empty", nameof(syntaxKinds));

            foreach (var syntaxKind in syntaxKinds)
                TokenAnalysisDiagnosticAnalyzer.RegisterSyntaxTokenAction(Convert.ToInt32(syntaxKind), action);
        }
    }
}
