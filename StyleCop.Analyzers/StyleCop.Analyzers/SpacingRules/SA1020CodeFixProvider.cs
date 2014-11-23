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
    /// Implements a code fix for <see cref="SA1020IncrementDecrementSymbolsMustBeSpacedCorrectly"/>.
    /// </summary>
    /// <remarks>To fix a violation of this rule, ensure that there is no whitespace between the increment or decrement
    /// symbol and the item that is being incremented or decremented.</remarks>
    [ExportCodeFixProvider(nameof(SA1020CodeFixProvider), LanguageNames.CSharp)]
    [Shared]
    public class SA1020CodeFixProvider : CodeFixProvider
    {
        private static readonly ImmutableArray<string> _fixableDiagnostics =
            ImmutableArray.Create(SA1020IncrementDecrementSymbolsMustBeSpacedCorrectly.DiagnosticId);

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
                if (!diagnostic.Id.Equals(SA1020IncrementDecrementSymbolsMustBeSpacedCorrectly.DiagnosticId))
                    continue;

#warning not yet implemented
            }
        }
    }
}
