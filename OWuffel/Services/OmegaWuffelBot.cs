using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
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

namespace OWuffel.Services
{
    public class OmegaWuffelBot
    {
        private DiscordSocketClient Client { get; }
        public TaskCompletionSource<bool> Ready { get; private set; } = new TaskCompletionSource<bool>();
        public CommandService CommandService { get; }
        private MainConfig Config { get; }
        private Logger _logger;
        private ServicesConfiguration _sp;
        private LavaConfig LavaConfig;

        public OmegaWuffelBot(int shardId, int parentPorcessId)
        {
            if (shardId < 0)
                throw new ArgumentOutOfRangeException(nameof(shardId));
            Config = new MainConfig();
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.File("logs/owuffel.log", rollingInterval: RollingInterval.Day)
                 .WriteTo.Console()
                 .CreateLogger();

            LogInit.SetupLogger(shardId);
            _logger = LogManager.GetCurrentClassLogger();

            

            Client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 100,
                LogLevel = LogSeverity.Warning,
                ConnectionTimeout = int.MaxValue,
                TotalShards = Config.TotalShards,
                ShardId = shardId,
                AlwaysDownloadUsers = false,
                //ExclusiveBulkDelete = true,

            });

            CommandService = new CommandService(new CommandServiceConfig()
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async,
            });

            LavaConfig = new LavaConfig{
                SelfDeaf = true,
                LogSeverity = LogSeverity.Verbose,
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
            
            _logger.Info($"Starting loading services procedure...");
            var services = AddServices();

            _sp = services.GetRequiredService<ServicesConfiguration>();
            await _sp.AllShardsReadyAsync();
            var commandHandler = services.GetService<CommandHandler>().InitializeAsync();
            var CommandService = services.GetService<CommandService>();
            var _ = await CommandService.AddModulesAsync(this.GetType().GetTypeInfo().Assembly, services)
             .ConfigureAwait(false);




            sw.Stop();
            _logger.Info($"Connected in {sw.Elapsed.TotalSeconds:F2}s");

            //unload modules which are not available on the public bot




            StartSendingData();
            Ready.TrySetResult(true);

            _logger.Info($"Ready!");
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
                .AddSingleton<DatabaseUtilities>()
                .AddSingleton<VoiceChannelEvents>();

            

            s.AddHttpClient();
            //s.AddDbContext<WuffelDBContext>(option => option.UseSqlServer(Config.DefaultConnectionString));
            s.AddDbContext<WuffelDBContext>();
            

            
            //initialize Services
            var Services = s.BuildServiceProvider();

            sw.Stop();
            _logger.Info($"All services loaded in {sw.Elapsed.TotalSeconds:F2}s");

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
            _logger.Info($"Starting logging procedure ...");
            await Client.LoginAsync(TokenType.Bot, token).ConfigureAwait(false);
            await Client.StartAsync().ConfigureAwait(false);
            Client.Ready += SetClientReady;
            await clientReady.Task.ConfigureAwait(false);
            Client.Ready -= SetClientReady;
            _logger.Info($"Logged in!");
        }
    }
}
