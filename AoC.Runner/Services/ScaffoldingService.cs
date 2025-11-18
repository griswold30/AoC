using AoC.Shared;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace AoC.Server.Services
{
    public class ScaffoldingService
    {
        public ScaffoldingService()
        {
        }

        public async Task ScaffoldDayAsync(int year, int day, string language)
        {
            var yearFolder = $"AoC.{year}";
            var dayFolder = $"Days";
            var className = $"Day{day:D2}";
            var testClassName = $"{className}Tests";

            // Solution directory structure
            var solutionDir = UtilityFunctions.FindSolutionDirectory();
            var testDir = Path.Combine(solutionDir, "AoC.Tests");

            // Create day folders
            //../../../AOC.2023/chsarp/
            var targetLanguageFolder = Path.Combine(solutionDir, yearFolder, language);
            var targetClassFolder = Path.Combine(targetLanguageFolder, dayFolder);

            //../../../AOC.Tests/csharp/AoC.2023/
            var testProjectFolder = Path.Combine(testDir, language);
            var testClassFolder = Path.Combine(testProjectFolder, yearFolder);

            if (!Directory.Exists(targetClassFolder)) Directory.CreateDirectory(targetClassFolder);
            if (!Directory.Exists(testClassFolder)) Directory.CreateDirectory(testClassFolder);

            // File paths
            var filePath = Path.Combine(targetClassFolder, $"{className}.cs");
            var testFilePath = Path.Combine(testClassFolder, $"{testClassName}.cs");

            // Namespaces
            string ns = $"AoC._{year}.Days";
            string testNs = $"AoC.Tests._{year}.Days";

            var csprojPath = Path.Combine(targetLanguageFolder, $"AoC.{year}.csproj");
            var testCsProjPath = Path.Combine(testProjectFolder, $"AoC.Tests.csproj");
            // 1. Create project if missing
            AddSharedProjectIfNeeded(solutionDir, csprojPath, testCsProjPath, year);
            await TryAddDayClass(filePath, ns, className);
            await TryAddTestClass(testFilePath, testNs, testClassName, className, ns);
        }

        private void AddSharedProjectIfNeeded(string solutionDirectory, string problemProjectPath, string testProjectPath, int year)
        {

            if (!File.Exists(problemProjectPath))
            {
                var csproj = new XElement("Project",
                    new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                    new XElement("PropertyGroup",
                    new XElement("TargetFramework", "net8.0"),
                    new XElement("ImplicitUsings", "enable"),
                    new XElement("Nullable", "enable"),
                    new XElement("LangVersion", "latest"),
                    new XElement("RootNamespace", $"AoC._{year}")),
                    new XElement("ItemGroup",
                    new XElement("ProjectReference",
                    new XAttribute("Include", @"..\..\AoC.Shared\CSharp\AoC.Shared.csproj"))));
                
                csproj.Save(problemProjectPath);
                RunDotNetSlnAdd(Path.Combine(solutionDirectory, "AoC.sln").ToString(), problemProjectPath); 
            }
           
            // Ensure AoC.Shared ref exists
            using (var fs = File.Open(problemProjectPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var doc = XDocument.Load(fs);
                var hasShared = doc.Descendants("ProjectReference")
                .Any(x => x.Attribute("Include")?.Value.Contains("AoC.Shared") == true);
                if (!hasShared)
                {
                    doc.Root?.Add(new XElement("ItemGroup",
                        new XElement("ProjectReference", new XAttribute("Include", @"..\..\AoC.Shared\CSharp\AoC.Shared.csproj"))));
                    doc.Save(problemProjectPath);
                }
            }

            // Ensure AoC.Shared ref exists
            using (var fs = File.Open(testProjectPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var doc = XDocument.Load(fs);


                var hasShared = doc.Descendants("ProjectReference").Any(x => x.Attribute("Include")?.Value.Contains("AoC.Shared") == true);
                var hasYear = doc.Descendants("ProjectReference").Any(x => x.Attribute("Include")?.Value.Contains($"Aoc.{year}.csproj") == true);

                if (!hasShared)
                {
                    doc.Root?.Add(new XElement("ItemGroup",
                        new XElement("ProjectReference", new XAttribute("Include", @"..\..\AoC.Shared\CSharp\AoC.Shared.csproj"))));
                }

                if (!hasYear)
                {
                    doc.Root?.Add(new XElement("ItemGroup",
                        new XElement("ProjectReference", new XAttribute("Include", $@"..\..\AoC.{year}\CSharp\Aoc.{year}.csproj"))));
                }

                    doc.Save(testProjectPath);
              }
        }

        private async Task TryAddDayClass(string filePath, string @namespace, string className)
        {
            if (!File.Exists(filePath))
            {
                var classContent = new StringBuilder($$"""
                    using System;
                    using AoC.Shared;

                    namespace {{@namespace}}
                    {
                        public class {{className}} : IAoCDay
                        {
                            public string Part1(string input)
                            {
                                throw new NotImplementedException();
                            }

                            public string Part2(string input)
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }
                    """);

                await File.WriteAllTextAsync(filePath, classContent.ToString(), Encoding.UTF8);
            }
        }

        private async Task TryAddTestClass(string testFilePath, string @namespace, string className, string day, string dayNamespace)
        {
            if (!File.Exists(testFilePath))
            {
                var testContent = new StringBuilder($$"""
                    using Xunit;
                    using {{dayNamespace}};

                    namespace {{@namespace}}
                    {
                        public class {{className}}
                        {
                            private readonly {{day}} _day = new();

                            [Fact]
                            public void Part1_ExampleInput_ReturnsExpected()
                            {
                                string input = "";
                                var result = _day.Part1(input);
                                Assert.Equal("", result);
                            }

                            [Fact]
                            public void Part2_ExampleInput_ReturnsExpected()
                            {
                                string input = "";
                                var result = _day.Part2(input);
                                Assert.Equal("", result);
                            }
                        }
                    }
                    """);

                await File.WriteAllTextAsync(testFilePath, testContent.ToString(), Encoding.UTF8);
            }
        }

        private static void RunDotNetSlnAdd(string slnFile, string projPath)
        {
            var psi = new ProcessStartInfo("dotnet", $"sln \"{slnFile}\" add \"{projPath}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var proc = Process.Start(psi)!;

            string stdOut = proc.StandardOutput.ReadToEnd();
            string stdErr = proc.StandardError.ReadToEnd();

            proc.WaitForExit();

            if (proc.ExitCode == 0)
                Console.WriteLine($"Added project to solution: {projPath}");
            else
                Console.WriteLine($"⚠️ Failed to add project to solution: {projPath}\n{stdErr}");
        }
    }
}