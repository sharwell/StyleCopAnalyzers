namespace $rootnamespace$
{
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
$classSummary$
    /// </summary>
    /// <remarks>
$classRemarks$
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class $className$ : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "$SA$";
        internal const string Title = "$title$";
        internal const string MessageFormat = "TODO: Message format";
        internal const string Category = "StyleCop.CSharp.$category$";
        internal const string Description = "$title$";
        internal const string HelpLink = "$helpLink$";

        public static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledNoTests, Description, HelpLink);

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
            // TODO: Implement analysis
        }
    }
}
