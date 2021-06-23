using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OWuffel.events;
using OWuffel.Extensions.Database;
using OWuffel.Services.Config;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Victoria;
using Microsoft.EntityFrameworkCore;
using OWuffel.Events;
using OWuffel.Util;
using System.IO;
using Serilog.Context;
using System.Runtime.CompilerServices;
using Serilog.Sinks.SystemConsole.Themes;
using Discord.Addons.Interactive;
using Interactivity;
using OWuffel.Modules.Commands;

namespace OWuffel.Services
{

    public static class LoggerCallerEnrichmentConfiguration
    {
        public static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;
    }
    public class OmegaWuffelBot
    {
        private DiscordSocketClient Client { get; }
        public TaskCompletionSource<bool> Ready { get; private set; } = new TaskCompletionSource<bool>();
        public CommandService CommandService { get; }
        private MainConfig Config { get; }
        private ServicesConfiguration _sp;
        private LavaConfig LavaConfig;



        public OmegaWuffelBot(int shardId, int parentPorcessId)
        {
            if (shardId < 0)
                throw new ArgumentOutOfRangeException(nameof(shardId));
            Config = new MainConfig();

            var path = Path.Combine("logs", $"Shard{shardId}_Logfile_.log");
            var OutputTemplate = "Shard #" + shardId + ": [{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: OutputTemplate, theme: AnsiConsoleTheme.Literate)
                .WriteTo.File(path, rollingInterval: RollingInterval.Day, outputTemplate: OutputTemplate)
                .CreateLogger();


            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 100,
                LogLevel = LogSeverity.Debug,
                ConnectionTimeout = int.MaxValue,
                TotalShards = Config.TotalShards,
                ShardId = shardId,
                AlwaysDownloadUsers = true,
                //ExclusiveBulkDelete = true,

            });

            CommandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
            });

            LavaConfig = new LavaConfig
            {
                SelfDeaf = true,
                LogSeverity = LogSeverity.Debug,
            };


            SetupShard(parentPorcessId);
        }
        
        private static void SetupShard(int parentProcessId)
        {
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    var p = Process.GetProcessById(parentProcessId);
                    if (p == null)
                        return;
                    p.WaitForExit();
                }
                finally
                {
                    Environment.Exit(10);
                }
            })).Start();
        }
        private void StartSendingData()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var data = new ShardComMessage()
                    {
                        ConnectionState = Client.ConnectionState,
                        Guilds = Client.ConnectionState == ConnectionState.Connected ? Client.Guilds.Count : 0,
                        ShardId = Client.ShardId,
                        Time = DateTime.UtcNow,
                    };

                    await Task.Delay(7500).ConfigureAwait(false);
                }
            });
        }

        public async Task RunAndBlockAsync()
        {
            await RunAsync().ConfigureAwait(false);
            await Task.Delay(-1).ConfigureAwait(false);
        }
        public async Task RunAsync()
        {
            var sw = Stopwatch.StartNew();




            await LoginAsync(Config.Token).ConfigureAwait(false);

            Log.Info("Starting loading services procedure...");
            var services = AddServices();

            _sp = services.GetRequiredService<ServicesConfiguration>();
            await _sp.AllShardsReadyAsync();
            var commandHandler = services.GetService<CommandHandler>().InitializeAsync();
            var CommandService = services.GetService<CommandService>();
            //var _ = await CommandService.AddModulesAsync(this.GetType().GetTypeInfo().Assembly, services)
            // .ConfigureAwait(false);





            sw.Stop();
            Log.Info($"Connected in {sw.Elapsed.TotalSeconds:F2}s");

            //unload modules which are not available on the public bot




            StartSendingData();
            Ready.TrySetResult(true);

            Log.Info($"Ready!");
        }
        private List<ulong> GetCurrentGuildIds()
        {
            return Client.Guilds.Select(x => x.Id).ToList();
        }
        private ServiceProvider AddServices()
        {
            var startingGuildIdList = GetCurrentGuildIds();

            //this unit of work will be used for initialization of all modules too, to prevent multiple queries from running
            var sw = Stopwatch.StartNew();

            var _bot = Client.CurrentUser;
            var conf = new InteractivityConfig()
            {
                DefaultTimeout = TimeSpan.FromMinutes(5) 
            };

            var s = new ServiceCollection()
                .AddSingleton(Config)
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddSingleton<Settings>()
                .AddSingleton<CommandService>()
                .AddSingleton<NpgsqlConnection>()
                .AddSingleton<ServicesConfiguration>()
                .AddSingleton<LavaNode>()
                .AddSingleton(LavaConfig)
                .AddSingleton<CommandHandler>()
                .AddSingleton<MessageEvents>()
                .AddSingleton<GuildEvents>()
                .AddSingleton<UserEvents>()
                .AddSingleton<ReactionEvents>()
                .AddSingleton<DatabaseUtilities>()
                .AddSingleton<VoiceChannelEvents>()
                .AddSingleton(new InteractivityService(Client, conf))
                .AddSingleton<InteractiveService>();



            s.AddHttpClient();
            //s.AddDbContext<WuffelDBContext>(option => option.UseSqlServer(Config.DefaultConnectionString));
            //s.AddDbContext<WuffelDBContext>(ServiceLifetime.Transient);
            s.AddTransient<WuffelDBContext>();



            //initialize Services
            var Services = s.BuildServiceProvider();

            sw.Stop();
            Log.Info($"All services loaded in {sw.Elapsed.TotalSeconds:F2}s");

            return Services;
        }
        private async Task LoginAsync(string token)
        {
            var clientReady = new TaskCompletionSource<bool>();

            Task SetClientReady()
            {
                var _ = Task.Run(async () =>
                {
                    clientReady.TrySetResult(true);
                    try
                    {
                        foreach (var chan in (await Client.GetDMChannelsAsync().ConfigureAwait(false)))
                        {
                            await chan.CloseAsync().ConfigureAwait(false);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                    finally
                    {

                    }
                });
                return Task.CompletedTask;
            }

            //connect
            Log.Info("Starting logging procedure ...");
            await Client.LoginAsync(TokenType.Bot, token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            Client.Ready += SetClientReady;
            await clientReady.Task.ConfigureAwait(false);
            Client.Ready -= SetClientReady;
            Log.Info("Logged in!");
        }
    }
}
