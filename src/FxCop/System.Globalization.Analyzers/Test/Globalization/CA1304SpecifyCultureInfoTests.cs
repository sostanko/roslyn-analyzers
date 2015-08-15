// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;   

namespace System.Globalization.Analyzers.UnitTests
{
    [TestClass]
    public sealed class CA1304SpecifyCultureInfoTests : DiagnosticVerifier
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
        public void CA1304ShoudUseOverloadsWithExplicitCultureInfoParameterTests_String_CS()
        {
            string source = @"
using System;
using System.Globalization;

sealed class C
{
    void M(string strA, string strB)
    {
        string strAld = strA.ToLower();
        string strBud = strB.ToUpper();

        // The following calls are okay as far as CA1304 is concerned
        string strAlc = strA.ToLower(CultureInfo.CurrentCulture);
        string strBuc = strB.ToUpper(CultureInfo.InvariantCulture);
        if ((strAld == strAlc) != (strBud == strBuc))
        {
            Console.WriteLine(""Oops"");
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 25)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 25)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304NoCultureInfoWithListCollectionInitializerShouldGenerateDiagnostic()
        {
            string source = @"
using System.Collections.Generic;

sealed class C
{
    void TestMethod(string strA, string strB)
    {
        List<string> strAld = new List<string>() { strA.ToLower() };
        List<string> strBud = new List<string>() { strB.ToUpper() };
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 8, 52)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 52)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304NoCultureInfoWithArrayCollectionInitializerShouldGenerateDiagnostic()
        {
            string source = @"
using System.Collections.Generic;

sealed class C
{
    void TestMethod(string strA, string strB)
    {
        string[] strAld = new string[] { strA.ToLower() };
        string[] strBud = new string[] { strB.ToUpper() };
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 8, 42)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 42)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304NoCultureInfoWithDictionaryCollectionInitializerShouldGenerateDiagnostic()
        {
            string source = @"
using System.Collections.Generic;

sealed class C
{
    void TestMethod(string strA, string strB)
    {
        Dictionary<int, string> strAld = new Dictionary<int, string> { { 1, strA.ToLower() } };
        Dictionary<int, string> strBud = new Dictionary<int, string> { { 1, strA.ToUpper() } };
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 8, 77)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 9, 77)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304NoCultureInfoInTryBlockShouldGenerateDiagnostic()
        {
            string source = @"
using System;

sealed class C
{
    void TestMethod(string strA, string strB)
    {
        try
        {
            string strAld = strA.ToLower();
            string strBud = strB.ToUpper();
        }
        catch (Exception) { throw; }
        finally { }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 29)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 29)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304NoCultureInfoInCatchBlockShouldGenerateDiagnostic()
        {
            string source = @"
using System;

sealed class C
{
    void TestMethod(string strA, string strB)
    {
        try { }
        catch (Exception) 
        { 
            string strAld = strA.ToLower();
            string strBud = strB.ToUpper();
        }
        finally { }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 29)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 29)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304NoCultureInfoInFinallyBlockShouldGenerateDiagnostic()
        {
            string source = @"
using System;

sealed class C
{
    void TestMethod(string strA, string strB)
    {
        try { }
        catch (Exception) { }
        finally 
        {
            string strAld = strA.ToLower();
            string strBud = strB.ToUpper(); 
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 29)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 29)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304NoCultureInfoAsyncWaitShouldGenerateDiagnostic()
        {
            string source = @"
using System.Threading.Tasks;

class TestClass
{
    private async Task TestMethod(string strA, string strB)
    {
        await Task.Run(() =>
        {
            string strAld = strA.ToLower();
            string strBud = strB.ToUpper();
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
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 29)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 29)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304NoCultureInfoDelegateShouldGenerateDiagnostic()
        {
            string source = @"
using System.Security.Cryptography;

class TestClass
{
    delegate void Del(string strA, string strB);

    Del d = delegate (string strA, string strB)
    {
        string strAld = strA.ToLower();
        string strBud = strB.ToUpper();
    };
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 10, 25)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 11, 25)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304ShoudUseOverloadsWithExplicitCultureInfoParameterTests_Misc_CS()
        {
            string source = @"
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

static class C
{
    static void T(string f, string s)
    {
        CultureInfoOverloads.M1(f, s.ToLower(CultureInfo.InvariantCulture));
        CultureInfoOverloads.M2(f, s.ToUpper(CultureInfo.InvariantCulture));

        // The following calls are okay as far as CA1304 is concerned
        CultureInfoOverloads.M1(f, s.ToLower(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
        CultureInfoOverloads.M2(CultureInfo.CurrentCulture, f, s.ToUpper(CultureInfo.CurrentCulture));
        CultureInfoOverloads.M3(f, s);
        CultureInfoOverloads.M1cc(f, s);

        char[] characters = { 'a', 'b', 'c', 'd', 'e', 'f' };
        object[][] arguments = new object[3][] { new object[] { characters },
                                                 new object[] { characters, 1, 4 },
                                                 new object[] { characters[1], 20 } };
        for (int ctr = 0; ctr <= arguments.GetUpperBound(0); ctr++) {
            object[] args = arguments[ctr];
            object result = Activator.CreateInstance(typeof(String), args);
            Console.WriteLine(""{0}: {1}"", result.GetType().Name, result);
        }

        var crm = new System.ComponentModel.ComponentResourceManager(typeof(C));
        crm.ApplyResources(f, s);
        Console.WriteLine(crm.GetString(s));
        Console.WriteLine(crm.GetObject(s).ToString());
        Console.WriteLine(crm.GetStream(s).ToString());

        PropertyInfo pinfo = typeof(string).GetProperty(""Chars"");
        pinfo.SetValue(null, null);
        pinfo.SetValue(null, null, null);
        Console.WriteLine(pinfo.GetValue(null, null));
    }

    internal static class CultureInfoOverloads
    {
        internal static void M1(string format, string content)
        {
            M1(format, content, CultureInfo.CurrentCulture);
        }

        internal static void M1(string format, string content, CultureInfo provider)
        {
            Console.WriteLine(string.Format(provider, format, content));
        }

        // compilation error: default needs to be compile-time constant
        internal static void M1cc(string format, string content, CultureInfo provider = CultureInfo.CurrentCulture)
        {
            Console.WriteLine(string.Format(provider, format, content));
        }

        internal static void M2(string format, string content)
        {
            M2(CultureInfo.CurrentCulture, format, content);
        }

        internal static void M2(CultureInfo provider, string format, string content)
        {
            Console.WriteLine(string.Format(provider, format, content));
        }

        internal static void M3(string format, string content)
        {
            M3(format, CultureInfo.InvariantCulture, content);
        }

        private static void M3(string format, CultureInfo provider, string content)
        {
            Console.WriteLine(string.Format(provider, format, content));
        }
    }
}";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 12, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.cs", 13, 9)}
                }
            };

            VerifyCSharpDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304ShoudUseOverloadsWithExplicitCultureInfoParameterTests_String_VB()
        {
            string source = @"
Imports System
Imports System.Globalization

Public Module C
    Public Sub M(strA As String, strB As String)
        Dim strAld As String = strA.ToLower()
        Dim strBud As String = strB.ToUpper()

        ' The following calls are okay as far as CA1304 is concerned
        Dim strAlc As String = strA.ToLower(CultureInfo.CurrentCulture)
        Dim strBuc As String = strB.ToUpper(CultureInfo.InvariantCulture)
        If ((strAld = strAlc) <> (strBud = strBuc)) Then
            Console.WriteLine(""Oops"")
        End If
    End Sub
End Module";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 7, 32)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 8, 32)}
                }
            };

            VerifyBasicDiagnostic(source, expected);
        }

        [TestMethod]
        [TestCategory(TestCategories.Gated)]
        public void CA1304ShoudUseOverloadsWithExplicitCultureInfoParameterTests_Misc_VB()
        {
            string source = @"
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Reflection
Imports System.Resources

Public Module C
    Public Sub M(f As String, s As String)
        CultureInfoOverloads.M1(f, s.ToLower(CultureInfo.InvariantCulture))
        CultureInfoOverloads.M2(f, s.ToUpper(CultureInfo.InvariantCulture))

        ' The following calls are okay as far as CA1304 is concerned
        CultureInfoOverloads.M1(f, s.ToLower(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture)
        CultureInfoOverloads.M2(CultureInfo.CurrentCulture, f, s.ToUpper(CultureInfo.CurrentCulture))
        CultureInfoOverloads.M3(f, s)
        CultureInfoOverloads.M1cc(f, s)

        Dim characters() As Char = New Char() {""a""c, ""b""c, ""c""c, ""d""c, ""e""c, ""f""c}
        Dim arguments()() As Object = New Object(2)() {New Object() {characters},
                                                        New Object() {characters, 1, 4},
                                                        New Object() {characters(1), 20}}
        For ctr = 0 To arguments.GetUpperBound(0)
            Dim args As Object = arguments(ctr)
            Dim result As Object = Activator.CreateInstance(GetType(String), args)
            Console.WriteLine(""{0}: {1}"", result.GetType().Name, result)
        Next

        Dim crm = New System.ComponentModel.ComponentResourceManager(GetType(C))
        crm.ApplyResources(Nothing, s)
        Console.WriteLine(crm.GetString(s))
        Console.WriteLine(crm.GetObject(s).ToString())
        Console.WriteLine(crm.GetStream(s).ToString())

        Dim pinfo As PropertyInfo = f.GetType().GetProperty(""Chars"")
        pinfo.SetValue(Nothing, Nothing)
        pinfo.SetValue(Nothing, Nothing, Nothing)
        Console.WriteLine(pinfo.GetValue(Nothing, Nothing))
    End Sub
End Module

Friend Module CultureInfoOverloads
    Friend Sub M1(format As String, content As String)
        M1(format, content, CultureInfo.CurrentCulture)
    End Sub

    Friend Sub M1(format As String, content As String, provider As CultureInfo)
        Console.WriteLine(String.Format(provider, format, content))
    End Sub

    ' compilation error: default needs to be compile-time constant
    Friend Sub M1cc(format As String, content As String, Optional provider As CultureInfo = CultureInfo.CurrentCulture)
        Console.WriteLine(String.Format(provider, format, content))
    End Sub

    Friend Sub M2(format As String, content As String)
        M2(CultureInfo.CurrentCulture, format, content)
    End Sub

    Friend Sub M2(provider As CultureInfo, format As String, content As String)
        Console.WriteLine(String.Format(provider, format, content))
    End Sub

    Friend Sub M3(format As String, content As String)
        M3(format, CultureInfo.InvariantCulture, content)
    End Sub

    Private Sub M3(format As String, provider As CultureInfo, content As String)
        Console.WriteLine(String.Format(provider, format, content))
    End Sub
End Module";

            DiagnosticResult[] expected = {
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 10, 9)}
                },
                new DiagnosticResult
                {
                    Id = CA1304DiagnosticAnalyzer.RuleId1304,
                    Severity = CA1304DiagnosticAnalyzer.Rule1304Severity,
                    Locations = new[] { new DiagnosticResultLocation("SourceString0.vb", 11, 9)}
                }
            };

            VerifyBasicDiagnostic(source, expected);
        }
    }
}
