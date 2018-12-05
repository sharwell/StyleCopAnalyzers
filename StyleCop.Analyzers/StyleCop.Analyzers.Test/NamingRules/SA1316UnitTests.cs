// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.NamingRules.SA1316TupleElementNamesShouldBeginWithLowerCaseLetter>;

    public class SA1316UnitTests
    {
        [Fact]
        public async Task TestTupleSyntaxNotSupportedAsync()
        {
            string source = @"
using System;

class MyClass {
    ValueTuple<string, string> Method() { throw null; }
}
";

            await VerifyCSharpDiagnosticAsync(source, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
