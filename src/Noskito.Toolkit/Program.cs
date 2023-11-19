using System.Diagnostics;
using System.IO;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Noskito.Common.Extension;
using Noskito.Database;
using Noskito.Database.Extension;
using Noskito.Logging;
using Noskito.Toolkit.Command;
using Noskito.Toolkit.Generator;
using Noskito.Toolkit.Parser;
using Noskito.Toolkit.Serialization;

namespace Noskito.Toolkit
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();

            services.UseLoggingModule();
            services.AddDatabase();
            services.AddTransient<ISerialization, Yaml>();
            services.AddImplementingTypes<IGenerator>();
            services.AddImplementingTypes<IParser>();

            var provider = services.BuildServiceProvider();

            var contextFactory = provider.GetRequiredService<DbContextFactory>();
            
            CommandLine.Parser.Default.ParseArguments<ParseCommand, GenerateCommand>(args)
                .WithParsed<ParseCommand>(command =>
                {
                    var stopwatch = new Stopwatch();
                    
                    var directory = new DirectoryInfo(command.Path);
                    if (!directory.Exists)
                    {
                        Log.Warn($"Can't found directory: {command.Path}");
                        return;
                    }

                    stopwatch.Start();
                    using (var context = contextFactory.CreateContext())
                    {
                        Log.Info("Clearing database");
                        context.Database.EnsureDeleted();

                        Log.Info("Migrating database");
                        context.Database.EnsureCreated();
                    }
                    
                    var parsers = provider.GetServices<IParser>();
                    foreach (var parser in parsers)
                    {
                        parser.Parse(directory).GetAwaiter().GetResult();
                    }
                    stopwatch.Stop();

                    Log.Info($"Parsing completed in {stopwatch.Elapsed:mm\\:ss}");
                })
                .WithParsed<GenerateCommand>(command =>
                {
                    var path = new DirectoryInfo(command.Path);
                    
                    if (!path.Exists)
                    {
                        Log.Warn($"Can't found directory: {command.Path}");
                        return;
                    }

                    var generators = provider.GetServices<IGenerator>();
                    foreach (var generator in generators)
                    {
                        generator.Generate(path);
                    }
                });
        }
    }
}