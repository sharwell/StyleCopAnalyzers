// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp7.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.MaintainabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.MaintainabilityRules.SA1414NameTupleElementsInSignatures>;

    public class SA1414CSharp7UnitTests : SA1414UnitTests
    {
        [Fact]
        public async Task TestGenericTypeConstraintAsync()
        {
            string source = @"
using System;
using System.Collections.Generic;

internal class MyClass<T>
    where T : List<(int, int)>
{
    void Method<U>()
        where U : List<(int, int)>
    {
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(6, 21),
                Diagnostic().WithLocation(6, 26),
                Diagnostic().WithLocation(9, 25),
                Diagnostic().WithLocation(9, 30),
            };
            await VerifyCSharpDiagnosticAsync(source, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestLocalsNotInSignaturesAsync()
        {
            string source = @"
using System;

internal class MyClass
{
    void Method()
    {
        (int, int) a;
        Action<(int, int)> b;
    }
}
";

            await VerifyCSharpDiagnosticAsync(source, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestPartialMethodAsync()
        {
            string source = @"
using System;

internal partial class MyClass
{
    partial void Method((int, int) args);

    // No diagnostic is reported for a direct reference to ValueTuple<T1, T2>
    partial void Method2(ValueTuple<int, int> args);

    // No diagnostic is reported for an implementing method
    partial void Method((int, int) args) { }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(6, 26),
                Diagnostic().WithLocation(6, 31),
            };
            await VerifyCSharpDiagnosticAsync(source, expected, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
