// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;   

namespace System.Globalization.Analyzers.UnitTests
{
    [TestClass]
    public sealed class CA1305SpecifyIFormatProviderTests : DiagnosticVerifier
    {
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new CSharpCA1304DiagnosticAnalyzer();
        }

        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new VisualBasicCA1304DiagnosticAnalyzer();
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305ShouldUseOverloadsWithExplicitIFormatProviderParamTests_StringFormatting_CS()
        {
            string source = @"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {
        string str1 = string.Format(""Foo {0}"", strA);
        string str2 = string.Format(""Foo {0} {1}"", strA, strB);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2);
        IFormatProviderOverloads.LeadingIFormatProvider(str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2);

        // The following calls are okay as far as CA1305 is concerned
        str1 = string.Format(CultureInfo.InvariantCulture, ""Foo {0}"", strA);
        str2 = string.Format(CultureInfo.CurrentCulture, ""Foo {0} {1}"", strA, strB);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InvariantCulture, str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentCulture);
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InvariantCulture, str1);
        IFormatProviderOverloads.LeadingIFormatProvider2(str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentCulture);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2);
        IFormatProviderOverloads.TrailingIFormatProvider2(str2);
        strA = strA.ToString();
        strB = strB.ToString(CultureInfo.CurrentCulture);
        Console.WriteLine(true.ToString());
        Console.WriteLine('A'.ToString());
        Console.WriteLine(StringComparison.Ordinal.ToString());
        Console.WriteLine(new Guid().ToString());
    }

    internal static class IFormatProviderOverloads
    {
        public static void LeadingIFormatProvider(string s)
        {
            LeadingIFormatProvider(CultureInfo.CurrentCulture, s);
        }

        public static void LeadingIFormatProvider(IFormatProvider provider, string s)
        {
            Console.WriteLine(string.Format(provider, s));
        }

        public static void LeadingIFormatProvider2(string s)
        {
            LeadingIFormatProvider2(CultureInfo.CurrentCulture, s);
        }

        private static void LeadingIFormatProvider2(IFormatProvider provider, string s)
        {
            Console.WriteLine(string.Format(provider, s));
        }

        public static void TrailingIFormatProvider(string format)
        {
            TrailingIFormatProvider(format, CultureInfo.CurrentCulture);
        }

        public static void TrailingIFormatProvider(string format, IFormatProvider provider)
        {
            Console.WriteLine(string.Format(provider, format));
        }

        public static string LeadingIFormatProviderReturningString(string format)
        {
            return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format);
        }

        public static string LeadingIFormatProviderReturningString(IFormatProvider provider, string format)
        {
            return string.Format(provider, format);
        }

        public static string TrailingIFormatProviderReturningString(string format)
        {
            return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture);
        }

        public static string TrailingIFormatProviderReturningString(string format, IFormatProvider provider)
        {
            return string.Format(provider, format);
        }

        // compilation error: default needs to be compile-time constant
        public static void TrailingIFormatProvider2(string format, IFormatProvider provider = CultureInfo.CurrentCulture)
        {
            Console.WriteLine(string.Format(provider, format));
        }

        // compilation error: default needs to be compile-time constant
        public static string TrailingIFormatProviderReturningString2(string format, IFormatProvider provider = CultureInfo.CurrentCulture)
        {
            return string.Format(provider, format);
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 9)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305ShouldUseOverloadsWithExplicitIFormatProviderParamTests_ConvertingParsing_CS()
        {
            string source = @"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA)
    {
        int i0 =  Convert.ToInt32(strA);
        long l0 = Convert.ToInt64(strA);
        int i1 =  Int32.Parse(strA);
        long l1 = Int64.Parse(strA);
        int i2 =  Int32.Parse(strA, NumberStyles.HexNumber);
        long l2 = Int64.Parse(strA, NumberStyles.HexNumber);

        // The following calls are okay as far as CA1305 is concerned
        if ((i0 != Convert.ToInt32(strA, CultureInfo.CurrentCulture))
            || (l0 != Convert.ToInt64(strA, CultureInfo.CurrentCulture))
            || (i1 !=  Int32.Parse(strA, CultureInfo.CurrentCulture))
            || (l1 != Int64.Parse(strA, CultureInfo.CurrentCulture))
            || (i2 !=  Int32.Parse(strA, NumberStyles.HexNumber, CultureInfo.CurrentCulture))
            || (l2 != Int64.Parse(strA, NumberStyles.HexNumber, CultureInfo.CurrentCulture)))
        {
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 19)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 19)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 19)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 19)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 19)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 19)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305FormatProviderWithListCollectionInitializerShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Collections.Generic;
using System.Globalization;

class TestClass
{
    void TestMethod(string strA)
    {
        List<int> i0 = new List<int>() { Convert.ToInt32(strA) };
        List<long> l0 = new List<long>() { Convert.ToInt64(strA) };
        List<int>  i1 = new List<int>() { Int32.Parse(strA) };
        List<long> l1 = new List<long>() { Int64.Parse(strA) };
        List<int> i2 = new List<int>() { Int32.Parse(strA, NumberStyles.HexNumber) };
        List<long> l2 = new List<long>() { Int64.Parse(strA, NumberStyles.HexNumber) };
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 44)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 43)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 44)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 44)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305FormatProviderWithArrayCollectionInitializerShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Globalization;

class TestClass
{
    void TestMethod(string strA)
    {
        int[] i0 = new int[] { Convert.ToInt32(strA) };
        long[] l0 = new long[] { Convert.ToInt64(strA) };
        int[]  i1 = new int[] { Int32.Parse(strA) };
        long[] l1 = new long[] { Int64.Parse(strA) };
        int[] i2 = new int[] { Int32.Parse(strA, NumberStyles.HexNumber) };
        long[] l2 = new long[] { Int64.Parse(strA, NumberStyles.HexNumber) };
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 34)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 33)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 34)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 34)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305FormatProviderWithDictionaryCollectionInitializerShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Collections.Generic;
using System.Globalization;

class TestClass
{
    void TestMethod(string strA)
    {
        Dictionary<int,int> i0 = new Dictionary<int, int> { { 1, Convert.ToInt32(strA) } };
        Dictionary<int, long> l0 = new Dictionary<int, long> { { 1, Convert.ToInt64(strA) } };
        Dictionary<int, int> i1 = new Dictionary<int, int> { { 1, Int32.Parse(strA) } };
        Dictionary<int, long> l1 = new Dictionary<int, long> { { 1, Int64.Parse(strA) } };
        Dictionary<int, int> i2 = new Dictionary<int, int> { { 1, Int32.Parse(strA, NumberStyles.HexNumber) } };
        Dictionary<int, long> l2 = new Dictionary<int, long> { { 1, Int64.Parse(strA, NumberStyles.HexNumber) } };
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 66)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 69)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 69)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 69)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305FormatProviderInTryBlockShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Globalization;

class TestClass
{
    void TestMethod(string strA)
    {
        try {
            int i0 = Convert.ToInt32(strA);
            long l0 = Convert.ToInt64(strA);
            int i1 = Int32.Parse(strA);
            long l1 = Int64.Parse(strA);
            int i2 = Int32.Parse(strA, NumberStyles.HexNumber);
            long l2 = Int64.Parse(strA, NumberStyles.HexNumber);
        }
        catch (Exception) { throw; }
        finally { }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 23)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305FormatProviderInCatchBlockShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Globalization;

class TestClass
{
    void TestMethod(string strA)
    {
        try {  }
        catch (Exception) { 
            int i0 = Convert.ToInt32(strA);
            long l0 = Convert.ToInt64(strA);
            int i1 = Int32.Parse(strA);
            long l1 = Int64.Parse(strA);
            int i2 = Int32.Parse(strA, NumberStyles.HexNumber);
            long l2 = Int64.Parse(strA, NumberStyles.HexNumber);
        }
        finally { }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 23)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305FormatProviderInFinallyBlockShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Globalization;

class TestClass
{
    void TestMethod(string strA)
    {
        try {  }
        catch (Exception) { throw; }
        finally { 
            int i0 = Convert.ToInt32(strA);
            long l0 = Convert.ToInt64(strA);
            int i1 = Int32.Parse(strA);
            long l1 = Int64.Parse(strA);
            int i2 = Int32.Parse(strA, NumberStyles.HexNumber);
            long l2 = Int64.Parse(strA, NumberStyles.HexNumber);
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 23)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305FormatProviderInAsyncWaitShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Globalization;
using System.Threading.Tasks;

class TestClass
{
    private async Task TestMethod(string strA)
    {
        await Task.Run(() => {
            int i0 = Convert.ToInt32(strA);
            long l0 = Convert.ToInt64(strA);
            int i1 = Int32.Parse(strA);
            long l1 = Int64.Parse(strA);
            int i2 = Int32.Parse(strA, NumberStyles.HexNumber);
            long l2 = Int64.Parse(strA, NumberStyles.HexNumber);
        });
    }

    private async void TestMethod2()
    {
        await TestMethod(""test"");
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 22)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 23)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305FormatProviderInDelegateShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Globalization;

class TestClass10
{
    delegate void Del(string strA);
    Del d = delegate (string strA) {
        int i0 = Convert.ToInt32(strA);
        long l0 = Convert.ToInt64(strA);
        int i1 = Int32.Parse(strA);
        long l1 = Int64.Parse(strA);
        int i2 = Int32.Parse(strA, NumberStyles.HexNumber);
        long l2 = Int64.Parse(strA, NumberStyles.HexNumber);
    };
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 19)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 19)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 19)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305ShouldNotUseUICultureAsIFormatProviderParamTests_CS()
        {
            string source = @"using System;
using System.Globalization;
using System.Threading;

sealed class C
{
    void M(string strA, string strB)
    {
        string str1 = string.Format(Thread.CurrentThread.CurrentUICulture, ""Foo {0}"", strA);
        string str2 = string.Format(CultureInfo.InstalledUICulture, ""Foo {0} {1}"", strA, strB);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.CurrentUICulture, str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentUICulture);
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.CurrentUICulture, str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentUICulture);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(Thread.CurrentThread.CurrentUICulture, str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, Thread.CurrentThread.CurrentUICulture);
        IFormatProviderOverloads.LeadingIFormatProvider(Thread.CurrentThread.CurrentUICulture, str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, Thread.CurrentThread.CurrentUICulture);
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InstalledUICulture, str1);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.InstalledUICulture);
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InstalledUICulture, str1);
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.InstalledUICulture);
        IFormatProviderOverloads.TrailingIFormatProvider2(str2);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2);
    }

    internal static class IFormatProviderOverloads
    {
        public static void LeadingIFormatProvider(string s)
        {
            LeadingIFormatProvider(CultureInfo.CurrentCulture, s);
        }

        public static void LeadingIFormatProvider(IFormatProvider provider, string s)
        {
            Console.WriteLine(string.Format(provider, s));
        }

        public static void LeadingIFormatProvider2(string s)
        {
            LeadingIFormatProvider2(CultureInfo.CurrentCulture, s);
        }

        private static void LeadingIFormatProvider2(IFormatProvider provider, string s)
        {
            Console.WriteLine(string.Format(provider, s));
        }

        public static void TrailingIFormatProvider(string format)
        {
            TrailingIFormatProvider(format, CultureInfo.CurrentCulture);
        }

        public static void TrailingIFormatProvider(string format, IFormatProvider provider)
        {
            Console.WriteLine(string.Format(provider, format));
        }

        public static string LeadingIFormatProviderReturningString(string format)
        {
            return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format);
        }

        public static string LeadingIFormatProviderReturningString(IFormatProvider provider, string format)
        {
            return string.Format(provider, format);
        }

        public static string TrailingIFormatProviderReturningString(string format)
        {
            return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture);
        }

        public static string TrailingIFormatProviderReturningString(string format, IFormatProvider provider)
        {
            return string.Format(provider, format);
        }

        // compilation error: default needs to be compile-time constant
        public static void TrailingIFormatProvider2(string format, IFormatProvider provider = CultureInfo.InstalledUICulture)
        {
            Console.WriteLine(string.Format(provider, format));
        }

        // compilation error: default needs to be compile-time constant
        public static string TrailingIFormatProviderReturningString2(string format, IFormatProvider provider = Thread.CurrentThread.CurrentUICulture)
        {
            return string.Format(provider, format);
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 23)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 19, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 20, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 21, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 22, 9)}
                },
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305ShouldUseOverloadsWithExplicitIFormatProviderParamTests_StringFormatting_VB()
        {
            string source = @"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = string.Format(""Foo {0}"", strA)
        Dim str2 As String = string.Format(""Foo {0} {1}"", strA, strB)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2)
        IFormatProviderOverloads.LeadingIFormatProvider(str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2)

        ' The following calls are okay as far as CA1305 is concerned
        str1 = string.Format(CultureInfo.InvariantCulture, ""Foo {0}"", strA)
        str2 = string.Format(CultureInfo.CurrentCulture, ""Foo {0} {1}"", strA, strB)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InvariantCulture, str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentCulture)
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InvariantCulture, str1)
        IFormatProviderOverloads.LeadingIFormatProvider2(str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentCulture)
        IFormatProviderOverloads.TrailingIFormatProvider2(str2);
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2);
        strA = strA.ToString()
        strB = strB.ToString(CultureInfo.CurrentCulture)
        Console.WriteLine(true.ToString())
        Console.WriteLine('A'.ToString())
        Console.WriteLine(StringComparison.Ordinal.ToString())
        Console.WriteLine(new Guid().ToString())
    End Sub
End Module

Friend Module IFormatProviderOverloads
    Friend Sub LeadingIFormatProvider(s As String)
        LeadingIFormatProvider(CultureInfo.CurrentCulture, s);
    End Sub

    Friend Sub LeadingIFormatProvider(provider As IFormatProvider, s As String)
        Console.WriteLine(String.Format(provider, s))
    End Sub

    Friend Sub LeadingIFormatProvider2(s As String)
        LeadingIFormatProvider2(CultureInfo.CurrentCulture, s)
    End Sub

    Private Sub LeadingIFormatProvider2(provider As IFormatProvider, s As String)
        Console.WriteLine(String.Format(provider, s))
    End Sub

    Friend Sub TrailingIFormatProvider(format As String)
        TrailingIFormatProvider(format, CultureInfo.CurrentCulture)
    End Sub

    Friend Sub TrailingIFormatProvider(format As String, provider As IFormatProvider)
        Console.WriteLine(String.Format(provider, format))
    End Sub

    Friend Function LeadingIFormatProviderReturningString(format As String) As String
        Return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format)
    End Function

    Friend Function LeadingIFormatProviderReturningString(provider As IFormatProvider, format As String) As String
        Return String.Format(provider, format)
    End Function

    Friend Function TrailingIFormatProviderReturningString(format As String) As String
        Return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture)
    End Function

    Friend Function TrailingIFormatProviderReturningString(format As String, provider As IFormatProvider) As String
        Return String.Format(provider, format)
    End Function

    ' compilation error: default needs to be compile-time constant
    Friend Sub TrailingIFormatProvider2(format As String, Optional provider As IFormatProvider = CultureInfo.InvariantCulture)
        Console.WriteLine(String.Format(provider, format))
    End Sub

    ' compilation error: default needs to be compile-time constant
    Friend Function TrailingIFormatProviderReturningString2(format As String, Optional provider As IFormatProvider = CultureInfo.CurrentCulture) As String
        Return String.Format(provider, format)
    End Function
End Module";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 9, 30)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 10, 30)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 11, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 12, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 13, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 14, 9)}
                }
            };

            VerifyBasicDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305ShouldUseOverloadsWithExplicitIFormatProviderParamTests_ConvertingParsing_VB()
        {
            string source = @"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String)
        Dim i0 As Integer = Convert.ToInt32(strA)
        Dim l0 As Long =    Convert.ToInt64(strA)
        Dim i1 As Integer = Int32.Parse(strA)
        Dim l1 As Long =    Int64.Parse(strA)
        Dim i2 As Integer = Int32.Parse(strA, NumberStyles.HexNumber)
        Dim l2 As Long =    Int64.Parse(strA, NumberStyles.HexNumber)

        // The following calls are okay as far as CA1305 is concerned
        If ((i0 <> Convert.ToInt32(strA, CultureInfo.CurrentCulture)) _
            Or (l0 <> Convert.ToInt64(strA, CultureInfo.CurrentCulture)) _
            Or (i1 <> Int32.Parse(strA, CultureInfo.CurrentCulture)) _
            Or (l1 <> Int64.Parse(strA, CultureInfo.CurrentCulture)) _
            Or (i2 <> Int32.Parse(strA, NumberStyles.HexNumber, CultureInfo.CurrentCulture)) _
            Or (l2 <> Int64.Parse(strA, NumberStyles.HexNumber, CultureInfo.CurrentCulture))) Then
        End If
    End Sub
End Module";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 9, 29)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 10, 29)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 11, 29)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 12, 29)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 13, 29)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 14, 29)}
                }
            };

            VerifyBasicDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1305ShouldNotUseUICultureAsIFormatProviderParamTests_VB()
        {
            string source = @"
Imports System
Imports System.Globalization
Imports System.Threading


Public Module C
    Public Sub M(strA As String, strB As String)
        Dim str1 As String = string.Format(Thread.CurrentThread.CurrentUICulture, ""Foo {0}"", strA)
        Dim str2 As String = string.Format(CultureInfo.InstalledUICulture, ""Foo {0} {1}"", strA, strB)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.CurrentUICulture, str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.CurrentUICulture)
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.CurrentUICulture, str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.CurrentUICulture)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(Thread.CurrentThread.CurrentUICulture, str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, Thread.CurrentThread.CurrentUICulture)
        IFormatProviderOverloads.LeadingIFormatProvider(Thread.CurrentThread.CurrentUICulture, str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, Thread.CurrentThread.CurrentUICulture)
        str1 = IFormatProviderOverloads.LeadingIFormatProviderReturningString(CultureInfo.InstalledUICulture, str1)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString(str2, CultureInfo.InstalledUICulture)
        IFormatProviderOverloads.LeadingIFormatProvider(CultureInfo.InstalledUICulture, str1)
        IFormatProviderOverloads.TrailingIFormatProvider(str2, CultureInfo.InstalledUICulture)
        IFormatProviderOverloads.TrailingIFormatProvider2(str2)
        str2 = IFormatProviderOverloads.TrailingIFormatProviderReturningString2(str2)
    End Sub

    Friend Module IFormatProviderOverloads
        Friend Sub LeadingIFormatProvider(s As String)
            LeadingIFormatProvider(CultureInfo.CurrentCulture, s);
        End Sub

        Friend Sub LeadingIFormatProvider(provider As IFormatProvider, s As String)
            Console.WriteLine(string.Format(provider, s));
        End Sub

        Friend Sub LeadingIFormatProvider2(s As String)
            LeadingIFormatProvider2(CultureInfo.CurrentCulture, s);
        End Sub

        Private Sub LeadingIFormatProvider2(provider As IFormatProvider, s As String)
            Console.WriteLine(string.Format(provider, s))
        End Sub

        Friend Sub TrailingIFormatProvider(format As String)
            TrailingIFormatProvider(format, CultureInfo.CurrentCulture)
        End Sub

        Friend Sub TrailingIFormatProvider(format As String, provider As IFormatProvider)
            Console.WriteLine(string.Format(provider, format))
        End Sub

        Friend Function LeadingIFormatProviderReturningString(format As String) As String
            Return LeadingIFormatProviderReturningString(CultureInfo.CurrentCulture, format)
        End Function

        Friend Function LeadingIFormatProviderReturningString(provider As IFormatProvider, format As String) As String
            Return string.Format(provider, format)
        End Function

        Friend Function TrailingIFormatProviderReturningString(format As String) As String
            Return TrailingIFormatProviderReturningString(format, CultureInfo.CurrentCulture)
        End Function

        Friend Function TrailingIFormatProviderReturningString(format As String, provider As IFormatProvider) As String
            Return string.Format(provider, format)
        End Function

        ' compilation error: default needs to be compile-time constant
        Friend Sub TrailingIFormatProvider2(format As String, Optional provider As IFormatProvider = Thread.CurrentThread.CurrentUICulture)
            Console.WriteLine(string.Format(provider, format))
        End Sub

        ' compilation error: default needs to be compile-time constant
        Friend Function TrailingIFormatProviderReturningString2(format As String, Optional provider As IFormatProvider = CultureInfo.InstalledUICulture) As String
            Return string.Format(provider, format)
        End Function
    End Sub
End Module ' C";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 9, 30)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 10, 30)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 11, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 12, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 13, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 14, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 15, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 16, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 17, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 18, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 19, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 20, 16)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 21, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1305,
                    Severity = CA1304DiagnosticAnalyzer.Rule1305Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 22, 9)}
                },
            };

            VerifyBasicDiagnostic(source, expected);
        }
    }
}
