// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.ReadabilityRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1142ReferenceTupleElementsByName : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1142ReferenceTupleElementsByName"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1142";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(ReadabilityResources.SA1142Title), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(ReadabilityResources.SA1142MessageFormat), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(ReadabilityResources.SA1142Description), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1142.md";

        private static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.ReadabilityRules, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledNoTests, Description, HelpLink);

        private static readonly Action<SyntaxNodeAnalysisContext> SimpleMemberAccessExpressionAction = HandleSimpleMemberAccessExpression;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
        }

        private static void HandleSimpleMemberAccessExpression(SyntaxNodeAnalysisContext context)
        {
            var memberAccessExpression = (MemberAccessExpressionSyntax)context.Node;
            if (!(memberAccessExpression.Name is IdentifierNameSyntax identifierName))
            {
                return;
            }

            if (identifierName.Identifier.IsMissingOrDefault())
            {
                return;
            }

            var name = identifierName.Identifier.ValueText;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            if (name.Equals("Rest", StringComparison.Ordinal))
            {
            }
            else if (name.StartsWith("Item", StringComparison.Ordinal))
            {
                for (int i = "Item".Length; i < name.Length; i++)
                {
                    if (name[i] < '0' || name[i] > '9')
                    {
                        // Name doesn't match ItemXX for some integer XX
                        return;
                    }
                }
            }
            else
            {
                // Name doesn't match the known ValueTuple field names
                return;
            }
        }
    }
}
