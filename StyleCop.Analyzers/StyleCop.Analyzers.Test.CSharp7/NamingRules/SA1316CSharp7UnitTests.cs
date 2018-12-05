// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp7.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.NamingRules;
    using StyleCop.Analyzers.Test.NamingRules;
    using StyleCop.Analyzers.Test.Verifiers;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.CustomDiagnosticVerifier<StyleCop.Analyzers.NamingRules.SA1316TupleElementNamesShouldBeginWithLowerCaseLetter>;

    public class SA1316CSharp7UnitTests : SA1316UnitTests
    {
        [Fact]
        public async Task TestTupleDefinedInInterfaceAsync()
        {
            string source = @"
using System;

interface IMyInterface {
    (int First, int second) Method();
}

class MyClass : IMyInterface {
    // No warning here since the method implements an interface method
    public (int First, int second) Method() => (0, 0);
}
";

            var expected = Diagnostic().WithLocation(5, 10).WithArguments("First");
            await VerifyCSharpDiagnosticAsync(LanguageVersion.CSharp7_1, source, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestTupleDefinedInInterfaceWithoutNameAsync()
        {
            string source = @"
interface IMyInterface {
    (int, int) Method();
}

class MyClass : IMyInterface {
    public (int First, int second) Method() => (0, 0);
}
";

            var expected = DiagnosticResult.CompilerError("CS8141").WithLocation(7, 37).WithMessage("The tuple element names in the signature of method 'MyClass.Method()' must match the tuple element names of interface method 'IMyInterface.Method()' (including on the return type).");
            await VerifyCSharpDiagnosticAsync(LanguageVersion.CSharp7_1, source, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestLocalVariableAsync()
        {
            string source = @"
class MyClass {
    void Method() {
        (int first, int Second) value = (first: 0, Second: 0);
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(4, 25).WithArguments("Second"),
                Diagnostic().WithLocation(4, 52).WithArguments("Second"),
            };
            await VerifyCSharpDiagnosticAsync(LanguageVersion.CSharp7_1, source, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestLocalVariableImplicitNameAsync()
        {
            string source = @"
using System;
class MyClass {
    void Method() {
        const int Second = 3;
        var tuple = (0, Second);

        // Access the tuple element to verify the implicit name
        Console.WriteLine(tuple.Second);
    }
}
";

            await VerifyCSharpDiagnosticAsync(LanguageVersion.CSharp7_1, source, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        private static Task VerifyCSharpDiagnosticAsync(LanguageVersion languageVersion, string source, DiagnosticResult expected, CancellationToken cancellationToken)
            => VerifyCSharpDiagnosticAsync(languageVersion, source, new[] { expected }, cancellationToken);

        private static Task VerifyCSharpDiagnosticAsync(LanguageVersion languageVersion, string source, DiagnosticResult[] expected, CancellationToken cancellationToken)
        {
            var test = new CSharpTest(languageVersion)
            {
                TestCode = source,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            return test.RunAsync(cancellationToken);
        }

        private class CSharpTest : StyleCopDiagnosticVerifier<SA1316TupleElementNamesShouldBeginWithLowerCaseLetter>.CSharpTest
        {
            public CSharpTest(LanguageVersion languageVersion)
            {
                this.SolutionTransforms.Add((solution, projectId) =>
                {
                    var parseOptions = (CSharpParseOptions)solution.GetProject(projectId).ParseOptions;
                    return solution.WithProjectParseOptions(projectId, parseOptions.WithLanguageVersion(languageVersion));
                });
            }
        }
    }
}
