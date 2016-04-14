using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.DotNet.ProjectModel;
using Microsoft.Extensions.CommandLineUtils;
using NuGet.Configuration;
using NuGet.Frameworks;

namespace DotnetPackages
{
    public class PackagesCommand
    {
        private string _projectFile;
        private string _packageId;

        private string _framework;

        private List<string> _colorCodes = new List<string>{"39","31","32","33","34","35","36","37","90","91","92","93","94","95","96","97"};

        private static string _seperator = new String('-', 110);
        static string _resetCode = "\x1b[39m";

        public PackagesCommand(string projectFile, string packageId, string framework)
        {
            _projectFile = projectFile ?? Path.Combine(Directory.GetCurrentDirectory(), "project.json");
            _packageId = packageId;
            _framework = framework;
        }

        public int Execute()
        {
            if (!File.Exists(_projectFile))
            {
                Console.WriteLine($"Unable to find project.json at {_projectFile}");
                return 1;
            }


            ProjectContext projectContext;
            if(_framework == null)
            {
                projectContext = ProjectContext.CreateContextForEachTarget(_projectFile).First();
            }
            else
            {
                //TODO: Might be easier to just create for all frameworks and then pick the right one or error. This doesn't seem like the right way to go.
                var nuGetFramework = NuGetFramework.Parse(_framework);
                if(nuGetFramework == null)
                {
                    Console.WriteLine("Unable to parse framework name {_framework}.");
                    return 1;
                }
                projectContext = ProjectContext.Create(_projectFile, nuGetFramework);
                
                if(!projectContext.ProjectFile.GetTargetFrameworks().Any(x=>x.FrameworkName.Equals(nuGetFramework)))
                {
                    Console.WriteLine($"This project doesn't support framework {_framework}.");
                    return 1;
                }
            }

            Console.WriteLine($"{projectContext.ProjectFile.Name} Dependency Information");
            Console.WriteLine($"Framework: {projectContext.TargetFramework.GetFrameworkString()}");
            Console.WriteLine(_seperator);

            if (_packageId == null)
            {
                return RenderDependencyTable(projectContext);
            }
            else
            {
                return RenderDependencyInfo(_packageId, projectContext);
            }
        }

        private int RenderDependencyTable(ProjectContext projectContext)
        {
            var console = AnsiConsole.GetOutput(false);
            Console.WriteLine($"{"DependencyName",-55} | {"Type",-10} | {"Requested Version",-20} | {"Resolved Version",-20}");
            Console.WriteLine(_seperator);

            var resolvedDependencies = projectContext.LibraryManager.GetLibraries();
            var diagnostics = projectContext.LibraryManager.GetAllDiagnostics();

            foreach (var dependency in projectContext.RootProject.Dependencies)
            {
                var resolvedDependency = resolvedDependencies.First(x => x.Identity.Name.Equals(dependency.Name));
                var colorCode = string.Empty;

                var diagnosticMessageForPackage = diagnostics.FirstOrDefault(x=>x.Source.Identity.Name == resolvedDependency.Identity.Name);
                if(diagnosticMessageForPackage != null)
                {
                    colorCode = GetColorForSeverity(diagnosticMessageForPackage.Severity);
                }

                Console.WriteLine($"{colorCode}{dependency.Name,-55} | {dependency.Type.Value,-10} | {dependency.VersionRange.OriginalString,-20} | {resolvedDependency.Identity.Version.ToFullString(),-20}{_resetCode}");
            }

            Console.WriteLine(" ");
            Console.WriteLine("Diagnostics");
            Console.WriteLine(" ");
            RenderDiagnostics(DiagnosticMessageSeverity.Error, diagnostics);
            RenderDiagnostics(DiagnosticMessageSeverity.Warning, diagnostics);
            RenderDiagnostics(DiagnosticMessageSeverity.Info, diagnostics);
            Console.WriteLine(" ");
            Console.WriteLine("Configured NuGet Feeds:");
            Console.WriteLine(_seperator);
            var rootPath = Path.GetDirectoryName(_projectFile);
            var nugetSettings = Settings.LoadDefaultSettings(rootPath, null, null);
            Console.WriteLine($"{"Feed", -60} | {"From Config", -50}");
            Console.WriteLine(_seperator);
            
            foreach(var source in nugetSettings.GetSettingValues("packageSources"))
            {
                Console.WriteLine($"{source.Value, -60} | {Path.Combine(source.Origin.Root, source.Origin.FileName)}");
            }
            return 0;
        }

        public void RenderDiagnostics(DiagnosticMessageSeverity severity, IList<DiagnosticMessage> diagnostics)
        {
            var console = AnsiConsole.GetOutput(false);
            Console.WriteLine(Enum.GetName(typeof(DiagnosticMessageSeverity), severity));
            Console.WriteLine(_seperator);
            if(diagnostics.Any(x=>x.Severity == severity))
            {
                foreach(var message in diagnostics.Where(x=>x.Severity == severity))
                {
                    console.WriteLine($"{GetColorForSeverity(message.Severity)}{message.Message}{_resetCode}");
                }
            }
            else
            {
                Console.WriteLine("NONE");
            }
            Console.WriteLine(_seperator);
            Console.WriteLine(" ");
        }

        public int RenderDependencyInfo(string pattern, ProjectContext context)
        {
            //TODO: fix on windows. Need to be true on windows false on OS X and Linux apparently...
            var ansiConsole = AnsiConsole.GetOutput(false);
            var resolvedDependencies = context.LibraryManager.GetLibraries();
            var resolvedDep = resolvedDependencies.FirstOrDefault(x => x.Identity.Name.IndexOf(_packageId, 0, StringComparison.OrdinalIgnoreCase) > 0);

            if (resolvedDep == null)
            {
                ansiConsole.WriteLine($"Unknown dependency: {pattern}");
                return 1;
            }

            ansiConsole.WriteLine($"Dependency: {resolvedDep.Identity.Name}");
            ansiConsole.WriteLine($"Selected Version: {resolvedDep.Identity.Version.ToFullString()}");
            ansiConsole.WriteLine($"Expected Location: {resolvedDep.Path}");
            ansiConsole.WriteLine($"Resolved: {CheckMark(resolvedDep.Resolved)}");
            ansiConsole.WriteLine(" ");
            ansiConsole.WriteLine("Depended on by:");

            ansiConsole.WriteLine(new String('-', 100));
            if(resolvedDep.Identity.Name == context.RootProject.Project.Name)
            {
                ansiConsole.WriteLine("NONE (this is the root application)");
            }
            else
            {
                ansiConsole.WriteLine($"{"Top Level", -10} | {"Parent",-60} | {"Requested Version",-25}");
                ansiConsole.WriteLine(new String('-', 100));
                var selectedColorCodes = new Dictionary<string, string>();
                int nextColorCodeIndex = 0;
                var colorcode = "";
                foreach (var dep in resolvedDependencies)
                {
                    foreach (var subDep in dep.Dependencies)
                    {

                        if (subDep.Name == resolvedDep.Identity.Name)
                        {
                            if (!selectedColorCodes.ContainsKey(subDep.VersionRange.OriginalString))
                            {
                                selectedColorCodes.Add(subDep.VersionRange.OriginalString, _colorCodes[nextColorCodeIndex]);
                                nextColorCodeIndex++;
                                if (nextColorCodeIndex > _colorCodes.Count())
                                {
                                    //TODO: This was originally the plan, but the limit on color groups and many of them being
                                    //bad for some theme or other might mean we just make the ones that don't match the
                                    //the resolved version be yellow.
                                    ansiConsole.WriteLine("To many requested versions, run out of console colors.");
                                    return 1;
                                }
                            }
                            colorcode = $"\x1b[{selectedColorCodes[subDep.VersionRange.OriginalString]}m";
                            var isTopLevel = context.RootProject.Dependencies.Any(x=>x.Name == dep.Identity.Name);
                            //TODO: Need to factor out table layout and make sure it is better at handline smaller consoles. Lining up the columns with two magic hard coded numbers is already getting difficult.
                            //TODO: This is gross.
                            var marker = isTopLevel ? "     *    " : "";
                            if (dep.Identity.Name == context.ProjectFile.Name)
                            {
                                marker = "    \uD83D\uDC51     ";
                            }
                            ansiConsole.WriteLine($"{marker, -10} | {dep.Identity.Name,-60} | {colorcode}{subDep.VersionRange.OriginalString,-25}{_resetCode} ");
                        }

                    }
                }
            }

            ansiConsole.WriteLine(new String('-', 95));
            ansiConsole.WriteLine(" ");

            ansiConsole.WriteLine("Depends on:");
            ansiConsole.WriteLine(new String('-', 95));
            if (resolvedDep.Dependencies.Any())
            {
                ansiConsole.WriteLine($"{"Dependency",-55} | {"Version",-25} | {"Resolved",-5}");
                ansiConsole.WriteLine(new String('-', 95));
                foreach (var dep in resolvedDep.Dependencies)
                {
                    var resolvedSubDep = resolvedDependencies.First(x => x.Identity.Name.Equals(dep.Name));
                    var tick = CheckMark(resolvedSubDep.Resolved);
                    ansiConsole.WriteLine($"{dep.Name,-55} | {dep.VersionRange.OriginalString,-25} |  {tick,-5}");
                }
            }
            else
            {
                ansiConsole.WriteLine("NONE");
            }
            ansiConsole.WriteLine(new String('-', 95));
            return 0;
        }

        private string GetColorForSeverity(DiagnosticMessageSeverity severity)
        {
            switch(severity)
            {
                case DiagnosticMessageSeverity.Error:
                    return "\x1b[31m";
                case DiagnosticMessageSeverity.Warning:
                    return "\x1b[33m";
                default:
                    return string.Empty;
            }
        }

        private string CheckMark(bool resolved)
        {
            return resolved ? "\u2713" : "\u274C";
        }
    }
}