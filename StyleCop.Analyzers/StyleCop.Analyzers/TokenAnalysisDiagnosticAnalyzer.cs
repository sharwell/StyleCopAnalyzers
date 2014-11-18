namespace StyleCop.Analyzers
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class TokenAnalysisDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private static ImmutableArray<Action<SyntaxTokenAnalysisContext>> _handlers =
            ImmutableArray<Action<SyntaxTokenAnalysisContext>>.Empty;

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor("Internal:TokenAnalysisHelper", "TokenAnalysisHelper", string.Empty, string.Empty, DiagnosticSeverity.Info, true);

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

            SyntaxTokenAnalyzer analyzer = new SyntaxTokenAnalyzer(context);
            analyzer.DefaultVisit(context.Tree.GetRoot(context.CancellationToken));
        }

        internal static void RegisterSyntaxTokenAction(int syntaxKind, Action<SyntaxTokenAnalysisContext> action)
        {
            int gap = syntaxKind + 1 - _handlers.Length;
            if (gap > 0)
                _handlers = _handlers.AddRange(ImmutableArray.Create(new Action<SyntaxTokenAnalysisContext>[gap]));

            _handlers = _handlers.SetItem(syntaxKind, _handlers[syntaxKind] - action + action);
        }

        private class SyntaxTokenAnalyzer : CSharpSyntaxWalker
        {
            private readonly SyntaxTreeAnalysisContext _context;
            private readonly Action<Diagnostic> _reportDiagnostic;

            public SyntaxTokenAnalyzer(SyntaxTreeAnalysisContext context)
                : base(SyntaxWalkerDepth.Token)
            {
                _context = context;
                _reportDiagnostic = context.ReportDiagnostic;
            }

            public override void VisitToken(SyntaxToken token)
            {
                int tokenKind = token.RawKind;
                if (tokenKind < 0 || tokenKind >= _handlers.Length)
                    return;

                _handlers[tokenKind]?.Invoke(new SyntaxTokenAnalysisContext(token, _context.Options, _reportDiagnostic, _context.CancellationToken));
            }
        }
    }
}
