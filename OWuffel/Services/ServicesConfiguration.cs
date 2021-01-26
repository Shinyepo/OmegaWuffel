﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using OWuffel.util;
using Victoria;
using Victoria.EventArgs;
using System.Linq;
using Microsoft.Extensions.Configuration;
using OWuffel.events;
using Npgsql;
using NLog;
using OWuffel.Services.Config;
using OWuffel.Events;

namespace OWuffel.Services
{
    public class ServicesConfiguration
    {

        // declare the fields used later in this class
        private readonly MainConfig Config;
        private readonly Logger _logger;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly LavaNode _instanceOfLavaNode;
        private readonly IServiceProvider _services;
        private readonly MessageEvents _msgevents;
        private readonly GuildEvents _guildevents;
        private readonly VoiceChannelEvents _voiceevents;
        private readonly UserEvents _userevents;

        public ServicesConfiguration(IServiceProvider services)
        {
            // get the services we need via DI, and assign the fields declared above to them
            Config = services.GetRequiredService<MainConfig>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _commands = services.GetRequiredService<CommandService>();
            _instanceOfLavaNode = services.GetRequiredService<LavaNode>();
            _msgevents = services.GetService<MessageEvents>();
            _guildevents = services.GetService<GuildEvents>();
            _voiceevents = services.GetService<VoiceChannelEvents>();
            _userevents = services.GetService<UserEvents>();
            _services = services;
            _logger = LogManager.GetCurrentClassLogger();
            _instanceOfLavaNode.OnTrackEnded += OnTrackEnded;

            //_discord.Log += OnLogAsync;
            //_commands.Log += OnLogAsync;
            _instanceOfLavaNode.OnLog += OnLogAsync;
        }

        private async Task OnTrackEnded(TrackEndedEventArgs args)
        {
            if (!args.Reason.ShouldPlayNext())
            {
                return;
            }

            var player = args.Player;
            if (!player.Queue.TryDequeue(out var queueable))
            {
                await player.TextChannel.SendMessageAsync("Queue completed!");
                await _instanceOfLavaNode.LeaveAsync(player.VoiceChannel);
                return;
            }

            if (!(queueable is LavaTrack track))
            {
                await player.TextChannel.SendMessageAsync("Next item in queue is not a track.");
                return;
            }

            await args.Player.PlayAsync(track);
            await args.Player.TextChannel.SendMessageAsync(
                $"{args.Reason}: {args.Track.Title}\nNow playing: {track.Title}");
        }
        public virtual async Task AllShardsReadyAsync()
        {
            await _discord.SetActivityAsync(new Game("ARK: Survival Evolved"));
            _discord.MessageUpdated += _msgevents.MessageUpdated;
            _discord.MessageDeleted += _msgevents.MessageDelete;
            //_discord.MessagesBulkDeleted += _msgevents.MessageBulkDelete;
            _discord.UserVoiceStateUpdated += _voiceevents.UserVoiceStateUpdate;
            _discord.JoinedGuild += _guildevents.JoinedGuild;
            _discord.LeftGuild += _guildevents.LeftGuild;
            _discord.GuildUpdated += _guildevents.GuildUpdated;
            _discord.UserJoined += _userevents.UserJoined;
            _discord.UserLeft += _userevents.UserLeft;
            _discord.UserUnbanned += _userevents.UserUnbanned;
            _discord.UserUpdated += _userevents.UserUpdated;
            if (!_instanceOfLavaNode.IsConnected)
            {
                await _instanceOfLavaNode.ConnectAsync();
            }
        }

        // this method switches out the severity level from Discord.Net's API, and logs appropriately
        public Task OnLogAsync(LogMessage msg)
        {
            string logText = $"{msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
            switch (msg.Severity.ToString())
            {
                case "Critical":
                    {
                        _logger.Error(logText);
                        break;
                    }
                case "Warning":
                    {
                        _logger.Warn(logText);
                        break;
                    }
                case "Info":
                    {
                        _logger.Info(logText);
                        break;
                    }
                case "Verbose":
                    {
                        _logger.Fatal(logText);
                        break;
                    }
                case "Debug":
                    {
                        _logger.Debug(logText);
                        break;
                    }
                case "Error":
                    {
                        _logger.Error(logText);
                        break;
                    }
            }

            return Task.CompletedTask;

        }

        
    }
}