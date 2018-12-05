// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.NamingRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;
    using StyleCop.Analyzers.Lightup;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1316TupleElementNamesShouldBeginWithLowerCaseLetter : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1316TupleElementNamesShouldBeginWithLowerCaseLetter"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1316";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(NamingResources.SA1316Title), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(NamingResources.SA1316MessageFormat), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(NamingResources.SA1316Description), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1316.md";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.NamingRules, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledNoTests, Description, HelpLink);

        private static readonly Action<SyntaxNodeAnalysisContext> ArgumentAction = HandleArgument;
        private static readonly Action<SyntaxNodeAnalysisContext> TupleElementAction = HandleTupleElement;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(ArgumentAction, SyntaxKind.Argument);
            context.RegisterSyntaxNodeAction(TupleElementAction, SyntaxKindEx.TupleElement);
        }

        private static void HandleArgument(SyntaxNodeAnalysisContext context)
        {
            var argument = (ArgumentSyntax)context.Node;
            if (argument.NameColon?.Name is null)
            {
                return;
            }

            CheckTupleElementName(context, argument.NameColon.Name.Identifier);
        }

        private static void HandleTupleElement(SyntaxNodeAnalysisContext context)
        {
            var tupleElement = (TupleElementSyntaxWrapper)context.Node;
            CheckTupleElementName(context, tupleElement.Identifier);
        }

        private static void CheckTupleElementName(SyntaxNodeAnalysisContext context, SyntaxToken identifier)
        {
            if (identifier.IsMissingOrDefault())
            {
                return;
            }

            string name = identifier.ValueText;
            if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
            {
                return;
            }

            if (!IsInOriginalDefinition(identifier, context.SemanticModel))
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Descriptor, identifier.GetLocation(), name));
        }

        private static bool IsInOriginalDefinition(SyntaxToken tupleElementName, SemanticModel semanticModel)
        {
            if (!tupleElementName.Parent.IsKind(SyntaxKindEx.TupleElement))
            {
                return true;
            }

            // TODO: Filter cases where a tuple type appears in an overriding or implementing signature
            return true;
        }
    }
}
