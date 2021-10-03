using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OWuffel.Services;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace OWuffel
{
    class Program
    {
        public static async Task Main(string[] args)
        {
#if DEBUG
            Console.WriteLine("Debug Breakpoint friendly mode.");
#endif
            Console.WriteLine("Process ID: {0}", Process.GetCurrentProcess().Id);

            if (args.Length == 2
                && int.TryParse(args[0], out int shardId)
                && int.TryParse(args[1], out int parentProcessId))
            {
                await new OmegaWuffelBot(shardId, parentProcessId)
                    .RunAndBlockAsync();
            }
            else
            {
                //CreateHostBuilder(args).Build().RunAsync().ConfigureAwait(false);
                await new ShardsMother()
                    .RunAsync()
                    .ConfigureAwait(false);

#if DEBUG
                await new OmegaWuffelBot(0, Process.GetCurrentProcess().Id)
                    .RunAndBlockAsync();
#else
                await Task.Delay(-1);
#endif
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.ConfigureKestrel(serverOptions =>
               {
                   serverOptions.ListenLocalhost(2167);
               })
               .UseStartup<Startup>();
           });
    }
}
