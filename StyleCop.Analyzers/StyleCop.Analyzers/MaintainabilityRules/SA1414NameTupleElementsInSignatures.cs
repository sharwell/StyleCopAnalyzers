// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.MaintainabilityRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;
    using StyleCop.Analyzers.Lightup;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1414NameTupleElementsInSignatures : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1414NameTupleElementsInSignatures"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1414";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(MaintainabilityResources.SA1414Title), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(MaintainabilityResources.SA1414MessageFormat), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(MaintainabilityResources.SA1414Description), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));
        private static readonly string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1414.md";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.ReadabilityRules, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledNoTests, Description, HelpLink);

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
            if (!tupleElement.Identifier.IsMissingOrDefault())
            {
                return;
            }

            if (!IsInOriginalDefinition(tupleElement, context.SemanticModel))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Descriptor, tupleElement.SyntaxNode.GetLocation()));
        }

        private static bool IsInOriginalDefinition(TupleElementSyntaxWrapper tupleElement, SemanticModel semanticModel)
        {
            // TODO: Filter cases where a tuple type appears in an overriding or implementing signature
            return true;
        }
    }
}
