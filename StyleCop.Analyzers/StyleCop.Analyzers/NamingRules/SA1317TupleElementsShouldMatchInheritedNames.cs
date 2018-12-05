// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.NamingRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;
    using StyleCop.Analyzers.Lightup;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1317TupleElementsShouldMatchInheritedNames : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1317TupleElementsShouldMatchInheritedNames"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1317";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(NamingResources.SA1317Title), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(NamingResources.SA1317MessageFormat), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(NamingResources.SA1317Description), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1317.md";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.NamingRules, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledNoTests, Description, HelpLink);

        private static readonly Action<SyntaxNodeAnalysisContext> TupleElementAction = HandleTupleElement;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(TupleElementAction, SyntaxKindEx.TupleElement);
        }

        private static void HandleTupleElement(SyntaxNodeAnalysisContext context)
        {
            var tupleElement = (TupleElementSyntaxWrapper)context.Node;
            var identifier = tupleElement.Identifier;
            if (identifier.IsMissingOrDefault())
            {
                return;
            }

            if (IsInOriginalDefinition(identifier, context.SemanticModel))
            {
                return;
            }

            var name = identifier.ValueText ?? "<unnamed>";
            var expected = "<expected>";
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, identifier.GetLocation(), name, expected));
        }

        private static bool IsInOriginalDefinition(SyntaxToken tupleElementName, SemanticModel semanticModel)
        {
            // TODO: Filter cases where a tuple type appears in an overriding or implementing signature
            return true;
        }
    }
}
