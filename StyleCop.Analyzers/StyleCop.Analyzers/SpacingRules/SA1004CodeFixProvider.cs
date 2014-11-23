namespace StyleCop.Analyzers.SpacingRules
{
    using System.Collections.Immutable;
    using System.Composition;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;

    /// <summary>
    /// Implements a code fix for <see cref="SA1004DocumentationLinesMustBeginWithSingleSpace"/>.
    /// </summary>
    /// <remarks>
    /// <para>To fix a violation of this rule, ensure that the header line begins with a single space.</para>
    /// </remarks>
    [ExportCodeFixProvider(nameof(SA1004CodeFixProvider), LanguageNames.CSharp)]
    [Shared]
    public class SA1004CodeFixProvider : CodeFixProvider
    {
        private static readonly ImmutableArray<string> _fixableDiagnostics =
            ImmutableArray.Create(SA1004DocumentationLinesMustBeginWithSingleSpace.DiagnosticId);

        /// <inheritdoc/>
        public override ImmutableArray<string> GetFixableDiagnosticIds()
        {
            return _fixableDiagnostics;
        }

        /// <inheritdoc/>
        public override FixAllProvider GetFixAllProvider()
        {
            return WellKnownFixAllProviders.BatchFixer;
        }

        /// <inheritdoc/>
        public override async Task ComputeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics)
            {
                if (!diagnostic.Id.Equals(SA1004DocumentationLinesMustBeginWithSingleSpace.DiagnosticId))
                    continue;

#warning not yet implemented
            }
        }
    }
}
