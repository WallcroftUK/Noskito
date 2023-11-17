using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CommandLine;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Noskito.Common.Extension;
using Noskito.Common.Logging;
using Noskito.Database;
using Noskito.Database.Extension;
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
            
            services.AddLogger();
            services.AddDatabase();
            services.AddTransient<ISerialization, Yaml>();
            services.AddImplementingTypes<IGenerator>();
            services.AddImplementingTypes<IParser>();

            var provider = services.BuildServiceProvider();

            var logger = provider.GetRequiredService<ILogger>();
            var contextFactory = provider.GetRequiredService<DbContextFactory>();
            
            CommandLine.Parser.Default.ParseArguments<ParseCommand, GenerateCommand>(args)
                .WithParsed<ParseCommand>(command =>
                {
                    var stopwatch = new Stopwatch();
                    
                    var directory = new DirectoryInfo(command.Path);
                    if (!directory.Exists)
                    {
                        logger.Error($"Can't found directory: {command.Path}");
                        return;
                    }

                    stopwatch.Start();
                    using (var context = contextFactory.CreateContext())
                    {
                        logger.Information("Clearing database");
                        context.Database.EnsureDeleted();
                        
                        logger.Information("Migrating database");
                        context.Database.EnsureCreated();
                    }
                    
                    var parsers = provider.GetServices<IParser>();
                    foreach (var parser in parsers)
                    {
                        parser.Parse(directory).GetAwaiter().GetResult();
                    }
                    stopwatch.Stop();
                    
                    logger.Information($"Parsing completed in {stopwatch.Elapsed:mm\\:ss}");
                })
                .WithParsed<GenerateCommand>(command =>
                {
                    var input = new DirectoryInfo(command.Input);
                    var output = new DirectoryInfo(command.Output);
                    
                    if (!input.Exists)
                    {
                        logger.Error($"Can't found directory: {command.Input}");
                        return;
                    }

                    if (!output.Exists)
                    {
                        logger.Error($"Can't found directory: {command.Output}");
                        return;
                    }

                    var generators = provider.GetServices<IGenerator>();
                    foreach (var generator in generators)
                    {
                        generator.Generate(input, output);
                    }
                });
        }
    }
}