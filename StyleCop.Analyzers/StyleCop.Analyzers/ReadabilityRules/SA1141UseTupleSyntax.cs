// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.ReadabilityRules
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
    internal class SA1141UseTupleSyntax : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1141UseTupleSyntax"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1141";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(ReadabilityResources.SA1141Title), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(ReadabilityResources.SA1141MessageFormat), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(ReadabilityResources.SA1141Description), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1141.md";

        private static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.ReadabilityRules, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledNoTests, Description, HelpLink);

        private static readonly Action<SyntaxNodeAnalysisContext> InvocationExpressionAction = HandleInvocationExpression;
        private static readonly Action<SyntaxNodeAnalysisContext> ObjectCreationExpressionAction = HandleObjectCreationExpression;
        private static readonly Action<SyntaxNodeAnalysisContext> GenericNameAction = HandleGenericName;
        private static readonly Action<SyntaxNodeAnalysisContext> QualifiedNameAction = HandleQualifiedName;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(InvocationExpressionAction, SyntaxKind.InvocationExpression);
            context.RegisterSyntaxNodeAction(ObjectCreationExpressionAction, SyntaxKind.ObjectCreationExpression);
            context.RegisterSyntaxNodeAction(GenericNameAction, SyntaxKind.GenericName);
            context.RegisterSyntaxNodeAction(QualifiedNameAction, SyntaxKind.QualifiedName);
        }

        private static void HandleInvocationExpression(SyntaxNodeAnalysisContext context)
        {
            throw new NotImplementedException();
        }

        private static void HandleObjectCreationExpression(SyntaxNodeAnalysisContext context)
        {
            throw new NotImplementedException();
        }

        private static void HandleGenericName(SyntaxNodeAnalysisContext context)
        {
            throw new NotImplementedException();
        }

        private static void HandleQualifiedName(SyntaxNodeAnalysisContext context)
        {
            throw new NotImplementedException();
        }
    }
}
