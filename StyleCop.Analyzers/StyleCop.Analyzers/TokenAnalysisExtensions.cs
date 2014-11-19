namespace StyleCop.Analyzers
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using System.Reflection;
    using TypeInfo = System.Reflection.TypeInfo;
    using System.Collections.Immutable;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provides extension methods to simplify the analysis of syntax tokens within a code document.
    /// </summary>
    public static class TokenAnalysisExtensions
    {
        private static readonly ConditionalWeakTable<object, ExtendedHostAnalysisScope> _analysisScopeData =
            new ConditionalWeakTable<object, ExtendedHostAnalysisScope>();

        /// <summary>
        /// Register an action to be executed at completion of parsing a code document. A syntax token action reports
        /// <see cref="Diagnostic"/>s about the <see cref="SyntaxToken"/>s within a document.
        /// </summary>
        /// <typeparam name="TLanguageEnumKind">Enum type giving the syntax node kinds of the source language for which
        /// the action applies.</typeparam>
        /// <param name="context">The analysis context.</param>
        /// <param name="action">Action to be executed at the end of semantic analysis of a code block.</param>
        /// <param name="syntaxKinds">Action will only be execute if a <see cref="SyntaxToken"/>'s Kind matches one of
        /// the syntax kind values.</param>
        public static void RegisterSyntaxTokenAction<TLanguageEnumKind>(this AnalysisContext context, Action<SyntaxTokenAnalysisContext> action, params TLanguageEnumKind[] syntaxKinds)
            where TLanguageEnumKind : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (syntaxKinds == null)
                throw new ArgumentNullException(nameof(syntaxKinds));
            if (syntaxKinds.Length == 0)
                throw new ArgumentException("syntaxKinds cannot be empty", nameof(syntaxKinds));

            // get the innermost scope object
            object scope = GetInnermostScope(context);
            DiagnosticAnalyzer analyzer = GetAnalyzer(context);
            var scopeData = (ExtendedHostSessionStartAnalysisScope)_analysisScopeData.GetValue(scope, CreateSessionStartScope);
            scopeData.RegisterSyntaxTokenAction(analyzer, action, syntaxKinds);
        }

        public static void RegisterSyntaxTokenAction<TLanguageEnumKind>(this CompilationStartAnalysisContext context, Action<SyntaxTokenAnalysisContext> action, params TLanguageEnumKind[] syntaxKinds)
            where TLanguageEnumKind : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (syntaxKinds == null)
                throw new ArgumentNullException(nameof(syntaxKinds));
            if (syntaxKinds.Length == 0)
                throw new ArgumentException("syntaxKinds cannot be empty", nameof(syntaxKinds));

            // get the innermost scope object
            object scope = GetInnermostScope(context);
            DiagnosticAnalyzer analyzer = GetAnalyzer(context);
            var scopeData = (ExtendedCompilationStartAnalysisScope)_analysisScopeData.GetValue(scope, CreateCompilationStartScope);
            scopeData.RegisterSyntaxTokenAction(analyzer, action, syntaxKinds);
        }

        private static object GetInnermostScope(AnalysisContext context)
        {
            throw new NotImplementedException();
        }

        private static object GetInnermostScope(CompilationStartAnalysisContext context)
        {
            throw new NotImplementedException();
        }

        private static DiagnosticAnalyzer GetAnalyzer(AnalysisContext context)
        {
            throw new NotImplementedException();
        }

        private static DiagnosticAnalyzer GetAnalyzer(CompilationStartAnalysisContext context)
        {
            throw new NotImplementedException();
        }

        private static ExtendedHostAnalysisScope CreateSessionStartScope(object key)
        {
            return new ExtendedHostSessionStartAnalysisScope();
        }

        private static ExtendedHostAnalysisScope CreateCompilationStartScope(object key)
        {
            throw new NotImplementedException();
        }
    }

    internal abstract class ExtendedHostAnalysisScope
    {
        private ImmutableArray<AnalyzerAction> syntaxTokenActions = ImmutableArray<AnalyzerAction>.Empty;
        private readonly Dictionary<DiagnosticAnalyzer, ExtendedAnalyzerActions> analyzerActions =
            new Dictionary<DiagnosticAnalyzer, ExtendedAnalyzerActions>();

        public virtual ImmutableArray<SyntaxTokenAnalyzerAction<TLanguageKindEnum>> GetSyntaxTokenActions<TLanguageKindEnum>()
            where TLanguageKindEnum : struct
        {
            return syntaxTokenActions.OfType<SyntaxTokenAnalyzerAction<TLanguageKindEnum>>().AsImmutable();
        }

        public virtual ExtendedAnalyzerActions GetAnalyzerActions(DiagnosticAnalyzer analyzer)
        {
            ExtendedAnalyzerActions actions;
            analyzerActions.TryGetValue(analyzer, out actions);
            return actions;
        }

        public void RegisterSyntaxTokenAction<TLanguageKindEnum>(DiagnosticAnalyzer analyzer, Action<SyntaxTokenAnalysisContext> action, params TLanguageKindEnum[] syntaxKinds)
            where TLanguageKindEnum : struct
        {
            SyntaxTokenAnalyzerAction<TLanguageKindEnum> analyzerAction = new SyntaxTokenAnalyzerAction<TLanguageKindEnum>(action, ImmutableArray.Create(syntaxKinds), analyzer);
            GetOrCreateExtendedAnalyzerActions(analyzer).AddSyntaxTokenAction(analyzerAction);
            syntaxTokenActions = syntaxTokenActions.Add(analyzerAction);
        }

        protected ExtendedAnalyzerActions GetOrCreateExtendedAnalyzerActions(DiagnosticAnalyzer analyzer)
        {
            ExtendedAnalyzerActions actions;
            if (!analyzerActions.TryGetValue(analyzer, out actions))
            {
                actions = new ExtendedAnalyzerActions();
                analyzerActions[analyzer] = actions;
            }

            return actions;
        }
    }

    internal sealed class ExtendedHostSessionStartAnalysisScope : ExtendedHostAnalysisScope
    {
    }

    internal sealed class ExtendedCompilationStartAnalysisScope : ExtendedHostAnalysisScope
    {
        private readonly ExtendedHostSessionStartAnalysisScope sessionScope;

        public ExtendedCompilationStartAnalysisScope(ExtendedHostSessionStartAnalysisScope sessionScope)
        {
            this.sessionScope = sessionScope;
        }

        public override ImmutableArray<SyntaxTokenAnalyzerAction<TLanguageKindEnum>> GetSyntaxTokenActions<TLanguageKindEnum>()
        {
            return base.GetSyntaxTokenActions<TLanguageKindEnum>().AddRange(sessionScope.GetSyntaxTokenActions<TLanguageKindEnum>());
        }

        public override ExtendedAnalyzerActions GetAnalyzerActions(DiagnosticAnalyzer analyzer)
        {
            ExtendedAnalyzerActions compilationActions = base.GetAnalyzerActions(analyzer);
            ExtendedAnalyzerActions sessionActions = sessionScope.GetAnalyzerActions(analyzer);

            if (sessionActions == null)
                return compilationActions;
            if (compilationActions == null)
                return sessionActions;

            return compilationActions.Append(sessionActions);
        }
    }

    internal sealed class ExtendedAnalyzerActions
    {
        private ImmutableArray<AnalyzerAction> syntaxTokenActions;

        internal int SyntaxTokenActionsCount
        {
            get
            {
                return syntaxTokenActions.Length;
            }
        }

        internal ImmutableArray<AnalyzerAction> SyntaxTokenActions
        {
            get
            {
                return syntaxTokenActions;
            }
        }

        internal void AddSyntaxTokenAction<TLanguageEnumKind>(SyntaxTokenAnalyzerAction<TLanguageEnumKind> action)
            where TLanguageEnumKind : struct
        {
            syntaxTokenActions = syntaxTokenActions.Add(action);
        }

        public ExtendedAnalyzerActions Append(ExtendedAnalyzerActions otherActions)
        {
            if (otherActions == null)
                throw new ArgumentNullException("otherActions");

            ExtendedAnalyzerActions actions = new ExtendedAnalyzerActions();
            actions.syntaxTokenActions = syntaxTokenActions.AddRange(otherActions.syntaxTokenActions);

            return actions;
        }
    }

    internal abstract class AnalyzerAction
    {
        internal AnalyzerAction(DiagnosticAnalyzer analyzer)
        {
            Analyzer = analyzer;
        }

        internal DiagnosticAnalyzer Analyzer
        {
            get;
        }
    }

    internal sealed class SyntaxTokenAnalyzerAction<TLanguageEnumKind> : AnalyzerAction
        where TLanguageEnumKind : struct
    {
        public SyntaxTokenAnalyzerAction(Action<SyntaxTokenAnalysisContext> action, ImmutableArray<TLanguageEnumKind> kinds, DiagnosticAnalyzer analyzer)
            : base(analyzer)
        {
            Action = action;
            Kinds = kinds;
        }

        public Action<SyntaxTokenAnalysisContext> Action
        {
            get;
        }

        public ImmutableArray<TLanguageEnumKind> Kinds
        {
            get;
        }
    }
}
