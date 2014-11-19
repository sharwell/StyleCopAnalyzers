namespace StyleCop.Analyzers
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// A "helper" diagnostic analyzer which enables other analyzers to efficiently analyze syntax tokens within code
    /// documents.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class TokenAnalysisDiagnosticAnalyzer : DiagnosticAnalyzer
    {
        private static ImmutableArray<Action<SyntaxTokenAnalysisContext>> _handlers =
            ImmutableArray<Action<SyntaxTokenAnalysisContext>>.Empty;

        /// <summary>
        /// A <see cref="DiagnosticDescriptor"/> used for the sole purpose of ensuring the
        /// <see cref="SupportedDiagnostics"/> property does not return an empty collection, and cannot be
        /// unintentionally suppressed by users.
        /// </summary>
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

        /// <summary>
        /// This is the backing field for the <see cref="SupportedDiagnostics"/> property.
        /// </summary>
        private static readonly ImmutableArray<DiagnosticDescriptor> _supportedDiagnostics =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return _supportedDiagnostics;
            }
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Do not call directly. This method enables
        /// <see cref="TokenAnalysisExtensions.RegisterSyntaxTokenAction{TLanguageEnumKind}"/> to register actions for
        /// syntax tokens.
        /// </summary>
        /// <param name="syntaxKind">The kind of syntax token to analyze.</param>
        /// <param name="action">The action to take.</param>
        internal static void RegisterSyntaxTokenAction(int syntaxKind, Action<SyntaxTokenAnalysisContext> action)
        {
            if (syntaxKind < 0)
                throw new ArgumentException("Syntax kinds below 0 are not supported.", nameof(syntaxKind));

            int gap = syntaxKind + 1 - _handlers.Length;
            if (gap > 0)
                _handlers = _handlers.AddRange(ImmutableArray.Create(new Action<SyntaxTokenAnalysisContext>[gap]));

            _handlers = _handlers.SetItem(syntaxKind, _handlers[syntaxKind] - action + action);
        }
    }
}
