namespace StyleCop.Analyzers.NamingRules
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using System;
    using System.Collections.Immutable;


    /// <summary>
    /// The name of a C# interface does not begin with the capital letter I.
    /// </summary>
    /// <remarks>
    /// <para>A violation of this rule occurs when the name of an interface does not begin with the capital letter I. 
    /// Interface names should always begin with I. For example, <c>ICustomer</c>.</para>
    /// <para>If the field or variable name is intended to match the name of an item associated with Win32 or COM, 
    /// and thus cannot begin with the letter I, place the field or variable within a special <c>NativeMethods</c> 
    /// class. A NativeMethods class is any class which contains a name ending in NativeMethods, and is intended as 
    /// a placeholder for Win32 or COM wrappers. StyleCop will ignore this violation if the item is placed within a 
    /// NativeMethods class.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1302InterfaceNamesMustBeginWithI : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "SA1300";
        internal const string Title = "Element Must Begin With Upper Case Letter";
        internal const string MessageFormat = "TODO: Message format";
        internal const string Category = "StyleCop.CSharp.Naming";
        internal const string Description = "The name of a C# element does not begin with an upper-case letter.";
        internal const string HelpLink = "http://www.stylecop.com/docs/SA1300.html";

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void Initialize(AnalysisContext context)
        {
            throw new NotImplementedException();
        }
    }
}
