﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Helpers
{
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Settings.ObjectModel;
    using StyleCop.Analyzers.Helpers.ObjectPools;

    /// <summary>
    /// Provides helper methods to work with indentation.
    /// </summary>
    internal static class IndentationHelper
    {
        /// <summary>
        /// Gets the first token on the textline that the given token is on.
        /// </summary>
        /// <param name="token">The token used to determine the textline.</param>
        /// <returns>The first token on the textline of the given token.</returns>
        public static SyntaxToken GetFirstTokenOnTextLine(SyntaxToken token)
        {
            while (true)
            {
                var precedingToken = token.GetPreviousToken();
                if (precedingToken.IsKind(SyntaxKind.None))
                {
                    return token;
                }

                if (precedingToken.GetLine() < token.GetLine())
                {
                    return token;
                }

                token = precedingToken;
            }
        }

        /// <summary>
        /// Gets the number of steps that the given node is indented.
        /// </summary>
        /// <param name="indentationSettings">The indentation settings to use.</param>
        /// <param name="node">The node to inspect.</param>
        /// <returns>The number of steps that the node is indented.</returns>
        public static int GetIndentationSteps(IndentationSettings indentationSettings, SyntaxNode node)
        {
            return GetIndentationSteps(indentationSettings, node.SyntaxTree, node.GetLeadingTrivia());
        }

        /// <summary>
        /// Gets the number of steps that the given token is indented.
        /// </summary>
        /// <param name="indentationSettings">The indentation settings to use.</param>
        /// <param name="token">The token to inspect.</param>
        /// <returns>The number of steps that the token is indented.</returns>
        public static int GetIndentationSteps(IndentationSettings indentationSettings, SyntaxToken token)
        {
            return GetIndentationSteps(indentationSettings, token.SyntaxTree, token.LeadingTrivia);
        }

        /// <summary>
        /// Generate a new indentation string.
        /// </summary>
        /// <param name="indentationSettings">The indentation settings to use.</param>
        /// <param name="indentationSteps">The number of indentation steps.</param>
        /// <returns>A string containing the amount of whitespace needed for the given indentation steps.</returns>
        public static string GenerateIndentationStringForSteps(IndentationSettings indentationSettings, int indentationSteps)
        {
            string result;
            var indentationCount = indentationSteps * indentationSettings.IndentationSize;
            if (indentationSettings.UseTabs)
            {
                var tabCount = indentationCount / indentationSettings.TabSize;
                var spaceCount = indentationCount % indentationSettings.TabSize;
                result = new string('\t', tabCount) + new string(' ', spaceCount);
            }
            else
            {
                result = new string(' ', indentationCount);
            }

            return result;
        }

        /// <summary>
        /// Generate a new indentation string for the given indentation width.
        /// </summary>
        /// <param name="indentationSettings">The indentation settings to use.</param>
        /// <param name="indentationWidth">The width of the indentation string.</param>
        /// <returns>A string containing the whitespace needed to indent code to the specified width.</returns>
        public static string GenerateIndentationString(IndentationSettings indentationSettings, int indentationWidth) =>
            GenerateIndentationString(indentationSettings, indentationWidth, 0);

        /// <summary>
        /// Generate a new indentation string for the given indentation width.
        /// </summary>
        /// <param name="indentationSettings">The indentation settings to use.</param>
        /// <param name="indentationWidth">The width of the indentation string.</param>
        /// <param name="startColumn">The starting column for the indentation.</param>
        /// <returns>A string containing the whitespace needed to indent code to the specified width.</returns>
        public static string GenerateIndentationString(IndentationSettings indentationSettings, int indentationWidth, int startColumn)
        {
            if (!indentationSettings.UseTabs)
            {
                return new string(' ', indentationWidth);
            }

            // Adjust the indentation width so a narrower first tab doesn't affect the outcome
            indentationWidth += startColumn % indentationSettings.TabSize;

            int tabCount = indentationWidth / indentationSettings.TabSize;
            int spaceCount = indentationWidth - (tabCount * indentationSettings.TabSize);

            StringBuilder builder = StringBuilderPool.Allocate();
            builder.EnsureCapacity(tabCount + spaceCount);
            builder.Append('\t', tabCount);
            builder.Append(' ', spaceCount);
            return StringBuilderPool.ReturnAndFree(builder);
        }

        /// <summary>
        /// Generates a whitespace trivia with the requested indentation.
        /// </summary>
        /// <param name="indentationSettings">The indentation settings to use.</param>
        /// <param name="indentationSteps">The amount of indentation steps.</param>
        /// <returns>A <see cref="SyntaxTrivia"/> containing the indentation whitespace.</returns>
        public static SyntaxTrivia GenerateWhitespaceTrivia(IndentationSettings indentationSettings, int indentationSteps)
        {
            return SyntaxFactory.Whitespace(GenerateIndentationStringForSteps(indentationSettings, indentationSteps));
        }

        private static int GetIndentationSteps(IndentationSettings indentationSettings, SyntaxTree syntaxTree, SyntaxTriviaList leadingTrivia)
        {
            var triviaSpan = syntaxTree.GetLineSpan(leadingTrivia.FullSpan);

            // There is no indentation when the leading trivia doesn't begin at the start of the line.
            if ((triviaSpan.StartLinePosition == triviaSpan.EndLinePosition) && (triviaSpan.StartLinePosition.Character > 0))
            {
                return 0;
            }

            var builder = StringBuilderPool.Allocate();

            foreach (SyntaxTrivia trivia in leadingTrivia.Reverse())
            {
                if (!trivia.IsKind(SyntaxKind.WhitespaceTrivia))
                {
                    break;
                }

                builder.Insert(0, trivia.ToFullString());
            }

            var tabSize = indentationSettings.TabSize;
            var indentationCount = 0;
            for (var i = 0; i < builder.Length; i++)
            {
                indentationCount += builder[i] == '\t' ? tabSize - (indentationCount % tabSize) : 1;
            }

            StringBuilderPool.ReturnAndFree(builder);

            return (indentationCount + (indentationSettings.IndentationSize / 2)) / indentationSettings.IndentationSize;
        }
    }
}
