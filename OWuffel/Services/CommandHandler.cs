using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using NLog;
using OWuffel.Extensions.Database;
using Microsoft.AspNetCore.Mvc;
using OWuffel.Services.Config;
using OWuffel.Util;

namespace OWuffel.Services
{
    public class CommandHandler
    {
        // setup fields to be set later in the constructor
        private readonly MainConfig Config;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly Logger _logger;

        public CommandHandler(IServiceProvider services)
        {
            // juice up the fields with these services
            // since we passed the services in, we can use GetRequiredService to pass them into the fields set earlier
            Config = services.GetRequiredService<MainConfig>();
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _logger = LogManager.GetCurrentClassLogger();
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

            if (message.Source != MessageSource.User)
            {
                return;
            }

            // sets the argument position away from the prefix we set
            var argPos = 0;

            // get prefix from the configuration file
            char prefix = Char.Parse(Config.Prefix);

            // determine if the message has a valid prefix, and adjust argPos based on prefix
            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.HasCharPrefix(prefix, ref argPos)))
            {
                return;
            }


            var context = new Cipska(_client, message, _services);
            
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
                _logger.Error($"Shard {_client.ShardId} Command failed to execute for [{context.User.Username}] <-> [{result.ErrorReason}]!");
                return;
            }


            // log success to the console and exit this method
            if (result.IsSuccess)
            {
                
                _logger.Info($"Shard {_client.ShardId} Command [{command.Value.Name}] executed for [{context.User.Username}] on [{context.Guild.Name}]");
                return;
            }

            // failure scenario, let's let the user know
            await context.Channel.SendMessageAsync($"Sorry, {context.User.Username}... something went wrong -> [{result}]!");
        }
    }

    public class Cipska : SocketCommandContext
    {
        public Settings Settings { get; }
        private readonly DatabaseUtilities _db;

        public Cipska(DiscordSocketClient client, SocketUserMessage ms) : base(client, ms) {  }
        public Cipska(DiscordSocketClient client, SocketUserMessage ms, IServiceProvider services) : base(client, ms)
        {
            _db = services.GetRequiredService<DatabaseUtilities>();
            var chnl = ms.Channel as SocketGuildChannel;
            var Guild = chnl.Guild.Id;
            Settings = _db.GetGuildSettingsAsync(Guild).GetAwaiter().GetResult();
        }
    }
}