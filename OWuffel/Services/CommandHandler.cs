using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OWuffel.Extensions.Database;
using OWuffel.Util;
using System.Text.RegularExpressions;
using OWuffel.Models;
using System.Linq;
using OWuffel.Extensions;

namespace OWuffel.Services
{
    public class CommandHandler
    {
        // setup fields to be set later in the constructor
        private readonly CommandService Commands;
        private readonly DiscordSocketClient Client;
        private readonly IServiceProvider Services;
        private DatabaseUtilities DbUtilities;

        private ulong GuildId = 812328100988977162;
        private ulong ChannelId = 821288092853862461;

        public CommandHandler(CommandService commandService,
                              DiscordSocketClient client,
                              IServiceProvider services,
                              DatabaseUtilities dbUtilities)
        {
            Commands = commandService;
            Client = client;
            DbUtilities = dbUtilities;
            Services = services;

            Commands.CommandExecuted += CommandExecutedAsync;
            Client.MessageReceived += MessageReceivedAsync;
        }
        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message))
            {
                return;
            }

            if (message.Source != MessageSource.User || message.Author.IsBot)
            {
                return;
            }

            var argPos = 0;

            var guildChannel = message.Channel as SocketGuildChannel;
            var settings = await DbUtilities.GetGuildSettingsAsync(guildChannel.Guild);
            var prefix = settings.BotPrefix;
            if (prefix == null)
            {
                await DbUtilities.SetSettingsValueAsync(guildChannel.Guild, "BotPrefix", "+");
                settings = await DbUtilities.GetGuildSettingsAsync(guildChannel.Guild);
                prefix = settings.BotPrefix;
            }
            var mentions = message.MentionedUsers;
            if (mentions.Count == 1 && mentions.First().Id == Client.CurrentUser.Id)
            {
                var content = message.Content;
                int mentionEnd = content.IndexOf('>');
                if (!(content.Length > mentionEnd + 2))
                {
                    int mentionStart = content.IndexOf("@");
                    var subString = content.Substring(mentionStart+2, mentionEnd - 3);
                    if (subString == Client.CurrentUser.Id.ToString())
                    {
                        await message.Channel.SendMessageAsync($"{message.Author.Mention}, my prefix for this server is \"**{prefix}**\"");
                        return;
                    }

                }
            }

            // determine if the message has a valid prefix, and adjust argPos based on prefix
            var d = message.Stickers;
            if (!(message.HasMentionPrefix(Client.CurrentUser, ref argPos) || message.HasStringPrefix(prefix.ToString(), ref argPos)))
            {
                return;
            }
            var context = new SocketCommandContext(Client, message);
            // execute command if one is found that matches
            await Commands.ExecuteAsync(context, argPos, Services);
        }

        public async Task InitializeAsync()
        {
            // register modules that are public and inherit ModuleBase<T>.
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
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
}