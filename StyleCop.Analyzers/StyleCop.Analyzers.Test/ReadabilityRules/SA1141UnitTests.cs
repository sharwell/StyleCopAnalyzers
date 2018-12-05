// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.ReadabilityRules
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.ReadabilityRules.SA1141UseTupleSyntax>;

    /// <summary>
    /// This class contains unit tests for SA1141.
    /// </summary>
    /// <seealso cref="SA1141UseTupleSyntax" />
    public class SA1141UnitTests
    {
        [Fact]
        public async Task TestTupleSyntaxNotSupportedAsync()
        {
            string source = @"
using System;

class MyClass {
    ValueTuple<string, string> Method1() {
        return new ValueTuple<string, string>(""key"", ""value"");
    }

    ValueTuple<string, string> Method2() {
        return ValueTuple.Create(""key"", ""value"");
    }
}
";

            await VerifyCSharpDiagnosticAsync(source, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
