using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OWuffel.Extensions.Database;
using OWuffel.Services.Config;
using OWuffel.Util;
using System.Text.RegularExpressions;

namespace OWuffel.Services
{
    public class CommandHandler
    {
        // setup fields to be set later in the constructor
        private readonly MainConfig Config;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private DatabaseUtilities _db;
        private string Prefix;
        private ulong GuildId = 812328100988977162;
        private ulong ChannelId = 821288092853862461;

        public CommandHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            Config = services.GetRequiredService<MainConfig>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _db = services.GetRequiredService<DatabaseUtilities>();
            _services = services;

            // take action when we execute a command
            _commands.CommandExecuted += CommandExecutedAsync;


            // take action when we receive a message (so we can process it, and see if it is a valid command)
            _client.MessageReceived += MessageReceivedAsync;
        }
        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // ensures we don't process system/other bot messages
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User || message.Author.IsBot)
            {
                return;
            }

            // sets the argument position away from the prefix we set
            var argPos = 0;

            // get prefix from the configuration file
            var guildchannel = message.Channel as SocketGuildChannel;
            var settings = await _db.GetGuildSettingsAsync(guildchannel.Guild);
            Prefix = settings.botPrefix;
            if (Prefix == null)
            {
                await _db.SetSettingsValueAsync(guildchannel.Guild, "botPrefix", "+");
                settings = await _db.GetGuildSettingsAsync(guildchannel.Guild);
                Prefix = settings.botPrefix;
            }
            if (message.Content.Contains(_client.CurrentUser.Id.ToString()))
            {
                var cut = message.Content.Replace("<@!" + _client.CurrentUser.Id + ">", "").Trim();
                if (cut.Length == 0)
                {
                    await message.Channel.SendMessageAsync($"{message.Author.Mention}, my prefix for this server is \"**{Prefix}**\"");
                    return;
                }
            }
            // determine if the message has a valid prefix, and adjust argPos based on prefix

            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasStringPrefix(Prefix.ToString(), ref argPos)))
            {
                return;
            }
            var context = new Cipska(_client, message, settings);
            // execute command if one is found that matches
            await _commands.ExecuteAsync(context, argPos, _services);
        }

        public async Task InitializeAsync()
        {
            // register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        // this class is where the magic starts, and takes actions upon receiving messages

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if a command isn't found, log that info to console and exit this method
            if (!command.IsSpecified)
            {
                Log.Error($"Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                return;
            }


            // log success to the console and exit this method
            if (result.IsSuccess)
            {

                Log.Info($"Command [{command.Value.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
                return;
            }

            // failure scenario, let's let the user know
            try
            {
                //if (command.Value.Name == "channel")
                //{
                //    var embed = new EmbedBuilder()
                //        .WithCurrentTimestamp()
                //        .WithColor(Color.Green)
                //        .WithTitle("Instrukcja obsługi Channel Check'a")
                //        .WithDescription($"**+check [id embedu] [kanal] [status]** - Służy do wypełnienia dużego embeda z komendy **+find nick** w celu znalezienia frajera z wojny.\n\n**id embedu** - To ta cyferka w stópce dużego embeda który sie pojawia po wywołaniu komendy \"**find**\"\n\n**kanal** - Przyjmuje krótkie wersje nazw kanałow(1 literowa, 3 literowa i pełna nazwa + cyferka z numerem kanału) np. ser3, c4, cal6 itd.\n\n**status** - Cokolwiek wpiszesz wyświetli sie w górnym embedzie, żeby oznaczyć znalezienie kogoś wpisz \"**bingo**\", \"**znaleziony**\" lub \"**tak**\". Po znalezieniu nie mozna korzystac juz z tego samego embeda. ");
                //    await context.Channel.SendMessageAsync(embed: embed.Build());
                //    return;
                //}
                var guild = await context.Client.GetGuildAsync(GuildId);
                var chnl = await guild.GetChannelAsync(ChannelId) as ITextChannel;
                var em = new EmbedBuilder()
                    .WithAuthor(context.User)
                    .WithCurrentTimestamp()
                    .WithColor(Color.Red)
                    .WithTitle($"Caught an exception while executing **{command.Value.Name}**")
                    .WithDescription("```" + result + "```\n\nThis exception was caught in CommandExecutedAsync()")
                    .AddField("Command Executed by", context.User.Username + "(" + context.User.Id + ")", false)
                    .AddField("Command Executed in", context.Guild.Name + "(" + context.Guild.Id + ")", false);
                await chnl.SendMessageAsync(embed: em.Build());
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            //await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
        }
    }
    public class Cipska : SocketCommandContext
    {
        public Settings Settings { get; }

        public Cipska(DiscordSocketClient client, SocketUserMessage ms) : base(client, ms) { }
        public Cipska(DiscordSocketClient client, SocketUserMessage ms, Settings settings) : base(client, ms)
        {
            Settings = settings;
        }
    }
}