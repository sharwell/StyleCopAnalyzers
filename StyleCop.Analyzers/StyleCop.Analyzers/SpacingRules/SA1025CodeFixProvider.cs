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
    /// Implements a code fix for <see cref="SA1025CodeMustNotContainMultipleWhitespaceInARow"/>.
    /// </summary>
    /// <remarks>
    /// <para>To fix a violation of this rule, remove the extra whitespace characters and leave only a single
    /// space.</para>
    /// </remarks>
    [ExportCodeFixProvider(nameof(SA1025CodeFixProvider), LanguageNames.CSharp)]
    [Shared]
    public class SA1025CodeFixProvider : CodeFixProvider
    {
        private static readonly ImmutableArray<string> _fixableDiagnostics =
            ImmutableArray.Create(SA1025CodeMustNotContainMultipleWhitespaceInARow.DiagnosticId);

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
                if (!diagnostic.Id.Equals(SA1025CodeMustNotContainMultipleWhitespaceInARow.DiagnosticId))
                    continue;

#warning not yet implemented
            }
        }
    }
}
