// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;   

namespace System.Globalization.Analyzers.UnitTests
{
    [TestClass]
    public sealed class CA1307SpecifyStringComparisonTests : DiagnosticVerifier
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
        public void CA1307ShouldUseOverloadsWithExplicitStringComparisionParameterTests_CS()
        {
            string source = @"
using System;
using System.Globalization;

sealed class C //5
{
    void M(string strA, string strB)
    {
        bool b;
        int r; // 10
        r =  String.Compare(strA, strB);
        r += String.Compare(strA, 0, strB, 0, 1);
        r += strA.IndexOf(strB);
        r += strA.IndexOf(""abc"", 0);
        r += ""xyz"".IndexOf(strB, 0, 1); //15
        r += strA.CompareTo(strB);
        r += strA.CompareTo((object)strB);
        r += ""uvw"".CompareTo(strB);
        b =      strA.StartsWith(strB);
        b = b || strA.EndsWith(strB); // 20

        // The following calls are okay as far as CA1307 is concerned
        r += String.Compare(strA, strB, StringComparison.Ordinal);
        r += String.Compare(strA, strB, StringComparison.CurrentCulture);
        r += String.Compare(strA, 0, strB, 0, 1, StringComparison.CurrentCulture);
        r += String.Compare(strA, 0, strB, 0, 1, StringComparison.Ordinal);
        r += strA.IndexOf(strB, StringComparison.Ordinal);
        r += strA.IndexOf(strB, 0, StringComparison.CurrentCultureIgnoreCase);
        r += strA.IndexOf(strB, 0, 1, StringComparison.CurrentCulture);
        if ((r == 0) || b
            || String.Equals(strA, strB, StringComparison.CurrentCultureIgnoreCase)
            || strA.Equals(strB, StringComparison.Ordinal)
            || String.Equals(strA, strB)
            || strA.Equals(strB)
            || strA.StartsWith(strB, StringComparison.CurrentCulture)
            || strA.EndsWith(strB, StringComparison.CurrentCultureIgnoreCase)
        ) {
            Console.WriteLine(strA == strB);
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 19, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 20, 18)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1307NoStringComparisonWithListCollectionInitializerShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Collections.Generic;

class testClass
{
    void TestMethod(string strA, string strB)
    {
        List<int> r1 = new List<int>() { String.Compare(strA, strB) };
        List<int> r2 = new List<int>() { String.Compare(strA, 0, strB, 0, 1) };
        List<int> r3 = new List<int>() { strA.IndexOf(strB) };
        List<int> r4 = new List<int>() { strA.IndexOf(""abc"", 0) };
        List<int> r5 = new List<int>() { ""xyz"".IndexOf(strB, 0, 1) };
        List<int> r6 = new List<int>() { strA.CompareTo(strB) };
        List<int> r7 = new List<int>() { strA.CompareTo((object)strB) };
        List<int> r8 = new List<int>() { ""uvw"".CompareTo(strB) };
        List<bool> b1 = new List<bool>() { strA.StartsWith(strB) };
        List<bool> b2 = new List<bool>() { strA.EndsWith(strB) }; 
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 44)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 44)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1307NoStringComparisonWithArrayInitializerShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Collections.Generic;

class testClass
{
    void TestMethod(string strA, string strB)
    {
        int[] r1 = new int[] { String.Compare(strA, strB) };
        int[] r2 = new int[] { String.Compare(strA, 0, strB, 0, 1) };
        int[] r3 = new int[] { strA.IndexOf(strB) };
        int[] r4 = new int[] { strA.IndexOf(""abc"", 0) };
        int[] r5 = new int[] { ""xyz"".IndexOf(strB, 0, 1) };
        int[] r6 = new int[] { strA.CompareTo(strB) };
        int[] r7 = new int[] { strA.CompareTo((object)strB) };
        int[] r8 = new int[] { ""uvw"".CompareTo(strB) };
        bool[] b1 = new bool[] { strA.StartsWith(strB) };
        bool[] b2 = new bool[] { strA.EndsWith(strB) }; 
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 34)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 34)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1307NoStringComparisonWithDictionaryCollectionInitializerShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Collections.Generic;

class testClass
{
    void TestMethod(string strA, string strB)
    {
        Dictionary<int, int> r1 = new Dictionary<int, int> { { 1, String.Compare(strA, strB) } };
        Dictionary<int, int> r2 = new Dictionary<int, int> { { 1, String.Compare(strA, 0, strB, 0, 1) } };
        Dictionary<int, int> r3 = new Dictionary<int, int> { { 1, strA.IndexOf(strB) } };
        Dictionary<int, int> r4 = new Dictionary<int, int> { { 1, strA.IndexOf(""abc"", 0) } };
        Dictionary<int, int> r5 = new Dictionary<int, int> { { 1, ""xyz"".IndexOf(strB, 0, 1) } };
        Dictionary<int, int> r6 = new Dictionary<int, int> { { 1, strA.CompareTo(strB) } };
        Dictionary<int, int> r7 = new Dictionary<int, int> { { 1, strA.CompareTo((object)strB) } };
        Dictionary<int, int> r8 = new Dictionary<int, int> { { 1, ""uvw"".CompareTo(strB) } };
        Dictionary<int, bool> b1 = new Dictionary<int, bool> { { 1, strA.StartsWith(strB) } };
        Dictionary<int, bool> b2 = new Dictionary<int, bool> { { 1, strA.EndsWith(strB) } };  
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 67)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 69)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 69)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1307NoStringComparisonInTryBlockShouldGenerateDiagnostic()
        {
            string source = @"
using System;
class TestClass
{
    private void TestMethod(string strA, string strB)
    {
        int r;
        bool b;
        try
        {
            r = String.Compare(strA, strB);
            r = String.Compare(strA, 0, strB, 0, 1);
            r = strA.IndexOf(strB);
            r = strA.IndexOf(""abc"", 0);
            r = ""xyz"".IndexOf(strB, 0, 1);
            r = strA.CompareTo(strB);
            r = strA.CompareTo((object)strB);
            r = ""uvw"".CompareTo(strB);
            b = strA.StartsWith(strB);
            b = strA.EndsWith(strB);
        }
        catch (Exception) { throw; }
        finally { }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 19, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 20, 17)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1307NoStringComparisonInCatchBlockShouldGenerateDiagnostic()
        {
            string source = @"
using System;
class TestClass
{
    private void TestMethod(string strA, string strB)
    {
        int r;
        bool b;
        try {    }
        catch (Exception) 
        {
            r = String.Compare(strA, strB);
            r = String.Compare(strA, 0, strB, 0, 1);
            r = strA.IndexOf(strB);
            r = strA.IndexOf(""abc"", 0);
            r = ""xyz"".IndexOf(strB, 0, 1);
            r = strA.CompareTo(strB);
            r = strA.CompareTo((object)strB);
            r = ""uvw"".CompareTo(strB);
            b = strA.StartsWith(strB);
            b = strA.EndsWith(strB); 
        }
        finally { }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 21, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 19, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 20, 17)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1307NoStringComparisonInFinallyBlockShouldGenerateDiagnostic()
        {
            string source = @"
using System;
class TestClass
{
    private void TestMethod(string strA, string strB)
    {
        int r;
        bool b;
        try {    }
        catch (Exception) {  }
        finally 
        { 
            r = String.Compare(strA, strB);
            r = String.Compare(strA, 0, strB, 0, 1);
            r = strA.IndexOf(strB);
            r = strA.IndexOf(""abc"", 0);
            r = ""xyz"".IndexOf(strB, 0, 1);
            r = strA.CompareTo(strB);
            r = strA.CompareTo((object)strB);
            r = ""uvw"".CompareTo(strB);
            b = strA.StartsWith(strB);
            b = strA.EndsWith(strB); 
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 21, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 22, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 19, 17)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 20, 17)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1307NoStringComparisonAsyncWaitShouldGenerateDiagnostic()
        {
            string source = @"
using System;
using System.Threading.Tasks;

class TestClass
{
    private async Task TestMethod(string strA, string strB)
    {
        await Task.Run(() =>
        {
        int r;
        bool b;
        r = String.Compare(strA, strB);
        r = String.Compare(strA, 0, strB, 0, 1);
        r = strA.IndexOf(strB);
        r = strA.IndexOf(""abc"", 0);
        r = ""xyz"".IndexOf(strB, 0, 1);
        r = strA.CompareTo(strB);
        r = strA.CompareTo((object)strB);
        r = ""uvw"".CompareTo(strB);
        b = strA.StartsWith(strB);
        b = strA.EndsWith(strB);
        });
    }

    private async void TestMethod2()
    {
        await TestMethod(""test1"", ""test2"");
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 19, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 20, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 21, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 22, 13)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1307NoStringComparisonInDelegateShouldGenerateDiagnostic()
        {
            string source = @"
using System;

class TestClass
{
    delegate void Del(string strA, string strB);

    Del d = delegate (string strA, string strB) {
        int r;
        bool b;
        r = String.Compare(strA, strB);
        r = String.Compare(strA, 0, strB, 0, 1);
        r = strA.IndexOf(strB);
        r = strA.IndexOf(""abc"", 0);
        r = ""xyz"".IndexOf(strB, 0, 1);
        r = strA.CompareTo(strB);
        r = strA.CompareTo((object)strB);
        r = ""uvw"".CompareTo(strB);
        b = strA.StartsWith(strB);
        b = strA.EndsWith(strB);
    };
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 14, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 15, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 16, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 17, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 18, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 19, 13)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 20, 13)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1307ShouldUseOverloadsWithExplicitStringComparisionParameterTests_VB()
        {
            string source = @"
Imports System
Imports System.Globalization



Public Module C
    Public Sub M(strA As String, strB As String)
        Dim b As Boolean
        Dim r As Integer ' 10
        r =  String.Compare(strA, strB)
        r += String.Compare(strA, 0, strB, 0, 1)
        r += strA.IndexOf(strB)
        r += strA.IndexOf(""abc"", 0)
        r += ""xyz"".IndexOf(strB, 0, 1) ' 15
        r += strA.CompareTo(strB)
        r += strA.CompareTo(DirectCast(strB, Object))
        r += ""uvw"".CompareTo(strB)
        b =      strA.StartsWith(strB)
        b = b Or strA.EndsWith(strB) ' 20

        ' The following calls are okay as far as CA1307 is concerned
        r += String.Compare(strA, strB, StringComparison.Ordinal)
        r += String.Compare(strA, strB, StringComparison.CurrentCulture)
        ' Roslyn gives back wrong method info on the following two calls
        'r += String.Compare(strA, strB, 0, 1, StringComparison.CurrentCulture)
        'r += String.Compare(strA, strB, 0, 1, StringComparison.Ordinal)
        r += strA.IndexOf(strB, StringComparison.Ordinal)
        r += strA.IndexOf(strB, 0, StringComparison.CurrentCultureIgnoreCase)
        r += strA.IndexOf(strB, 0, 1, StringComparison.CurrentCulture)
        If ((r = 0) Or b _
            Or String.Equals(strA, strB, StringComparison.CurrentCultureIgnoreCase) _
            Or strA.Equals(strB, StringComparison.Ordinal) _
            Or String.Equals(strA, strB) _
            Or strA.Equals(strB) _
            Or strA.StartsWith(strB, StringComparison.CurrentCulture) _
            Or strA.EndsWith(strB, StringComparison.CurrentCultureIgnoreCase)) Then
                Console.WriteLine(strA == strB)
        End If
    End Sub
End Module";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 11, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 12, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 13, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 14, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 15, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 16, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 17, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 18, 14)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 19, 18)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1307,
                    Severity = CA1304DiagnosticAnalyzer.Rule1307Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 20, 18)}
                }
            };

            VerifyBasicDiagnostic(source, expected);
        }
    }
}
