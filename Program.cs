using System;
using Microsoft.Extensions.CommandLineUtils;

namespace DotnetPackages
{
    public class Program
    {
        public static int Main(string[] args)
        {
             var app = new CommandLineApplication{
                Name="dotnet packages",
                FullName="",
                Description=""
            };
            var projectFile = app.Option("--projectFile", "Path to the project.json file for your project, defaults to project.json in the current directory.", CommandOptionType.SingleValue);
            var framework = app.Option("--framework", "The framework you wish to example. If not specified the fist framework is chosen.", CommandOptionType.SingleValue);
            var packageId = app.Argument("PackageId", "Package id to examine", false);
            app.HelpOption("-h|--help");
            
            var installCommand = app.Command("install", config => {
                config.OnExecute(() => {
                    Console.WriteLine("NotImplementedException"); 
                    return 1;
                });
            });

            app.OnExecute(() => {
                var command = new PackagesCommand(projectFile.Value(), packageId.Value, framework.Value());
                command.Execute();
                return 0;
            });

            try
            {
                return app.Execute(args);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"An unexpected error occured. Exception: {ex.ToString()}");
                return 500;
            }
        }
    }
}
