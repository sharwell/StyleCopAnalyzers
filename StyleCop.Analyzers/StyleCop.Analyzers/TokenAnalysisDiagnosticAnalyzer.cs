namespace StyleCop.Analyzers
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class TokenAnalysisDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private static ImmutableArray<Action<SyntaxTokenAnalysisContext>> _handlers =
            ImmutableArray<Action<SyntaxTokenAnalysisContext>>.Empty;

        /// <remarks>
        /// <para>The <see cref="DiagnosticSeverity"/> must be set to <see cref="DiagnosticSeverity.Info"/> or higher,
        /// or the analyzer will be suppressed for all files that are not currently open (even if the IDE is configured
        /// to show messages from all files).</para>
        ///
        /// <para>The <see cref="WellKnownDiagnosticTags.NotConfigurable"/> tag prevents this diagnostic from appearing
        /// in the rule editor, which is important since this analyzer implements functionality that is required by
        /// (potentially) many other analyzers.</para>
        /// </remarks>
        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor("Internal:TokenAnalysisHelper", "TokenAnalysisHelper", string.Empty, string.Empty, DiagnosticSeverity.Info, true, customTags: WellKnownDiagnosticTags.NotConfigurable);

        private static readonly ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics =
            ImmutableArray.Create(Descriptor);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return _supportedDiagnostics;
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(HandleSyntaxTree);
        }

        private void HandleSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            if (_handlers.IsDefaultOrEmpty)
                return;

            Action<Diagnostic> reportDiagnostic = context.ReportDiagnostic;
            SyntaxNode rootNode = context.Tree.GetRoot(context.CancellationToken);
            foreach (var token in rootNode.DescendantTokens())
            {
                int tokenKind = token.RawKind;
                if (tokenKind < 0 || tokenKind >= _handlers.Length)
                    continue;

                _handlers[tokenKind]?.Invoke(new SyntaxTokenAnalysisContext(token, context.Options, reportDiagnostic, context.CancellationToken));
            }
        }

        internal static void RegisterSyntaxTokenAction(int syntaxKind, Action<SyntaxTokenAnalysisContext> action)
        {
            int gap = syntaxKind + 1 - _handlers.Length;
            if (gap > 0)
                _handlers = _handlers.AddRange(ImmutableArray.Create(new Action<SyntaxTokenAnalysisContext>[gap]));

            _handlers = _handlers.SetItem(syntaxKind, _handlers[syntaxKind] - action + action);
        }
    }
}
