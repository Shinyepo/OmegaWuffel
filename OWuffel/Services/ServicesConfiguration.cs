using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using Victoria;
using Victoria.EventArgs;
//using OWuffel.events;
using OWuffel.Services.Config;
using OWuffel.Events;
using System.Threading;
using OWuffel.Util;
using OWuffel.Modules.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using OWuffel.Extensions;

namespace OWuffel.Services
{
    public class ServicesConfiguration
    {

        // declare the fields used later in this class
        private readonly MainConfig Config;
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly LavaNode _instanceOfLavaNode;
        private readonly IServiceProvider _services;
        //private readonly MessageEvents _msgevents;
        //private readonly GuildEvents _guildevents;
        //private readonly VoiceChannelEvents _voiceevents;
        //private readonly UserEvents _userevents;
        //private readonly ReactionEvents _reactionevets;

        private Timer timer;
        private DatabaseUtilities _db;
        private DiscordSocketClient _client;

        public ServicesConfiguration(IServiceProvider services)
        {
            // get the services we need via DI, and assign the fields declared above to them
            Config = services.GetRequiredService<MainConfig>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _commands = services.GetRequiredService<CommandService>();
            _instanceOfLavaNode = services.GetRequiredService<LavaNode>();
            //_msgevents = services.GetService<MessageEvents>();
            //_guildevents = services.GetService<GuildEvents>();
            //_voiceevents = services.GetService<VoiceChannelEvents>();
            //_userevents = services.GetService<UserEvents>();
            //_reactionevets = services.GetService<ReactionEvents>();
            _db = services.GetRequiredService<DatabaseUtilities>();
            _services = services;
            //_instanceOfLavaNode.OnTrackEnded += OnTrackEnded;

            //_discord.Log += OnLogAsync;
            //_commands.Log += OnLogAsync;
            _instanceOfLavaNode.OnLog += OnLogAsync;
        }

        //private async Task OnTrackEnded(TrackEndedEventArgs args)
        //{
        //    if (!args.Reason.ShouldPlayNext())
        //    {
        //        return;
        //    }

        //    var player = args.Player;
        //    if (!player.Queue.TryDequeue(out var queueable))
        //    {
        //        await player.TextChannel.SendMessageAsync("Queue completed!");
        //        await _instanceOfLavaNode.LeaveAsync(player.VoiceChannel);
        //        return;
        //    }

        //    if (!(queueable is LavaTrack track))
        //    {
        //        await player.TextChannel.SendMessageAsync("Next item in queue is not a track.");
        //        return;
        //    }

        //    await args.Player.PlayAsync(track);
        //    await args.Player.TextChannel.SendMessageAsync(
        //        $"{args.Reason}: {args.Track.Title}\nNow playing: {track.Title}");
        //}
        public virtual async Task AllShardsReadyAsync()
        {
            await _discord.SetActivityAsync(new Game("Spierdalaj"));
            //_discord.MessageUpdated += _msgevents.MessageUpdated;
            //_discord.MessageDeleted += _msgevents.MessageDelete;
            //_discord.MessagesBulkDeleted += _msgevents.MessageBulkDelete;
            //_discord.UserVoiceStateUpdated += _voiceevents.UserVoiceStateUpdate;
            //_discord.JoinedGuild += _guildevents.JoinedGuild;
            //_discord.LeftGuild += _guildevents.LeftGuild;
            //_discord.GuildUpdated += _guildevents.GuildUpdated;
            //_discord.RoleCreated += _guildevents.RoleCreated;
            //_discord.RoleDeleted += _guildevents.RoleDeleted;
            //_discord.RoleUpdated += _guildevents.RoleUpdated;
            //_discord.UserJoined += _userevents.UserJoined;
            //_discord.UserLeft += _userevents.UserLeft;
            //_discord.UserUnbanned += _userevents.UserUnbanned;
            //_discord.UserBanned += _userevents.UserBanned;
            //_discord.UserUpdated += _userevents.UserUpdated;
            //_discord.GuildMemberUpdated += _userevents.GuildMemberUpdated;
            //_discord.ReactionAdded += _reactionevets.ReactionAdded;
            //_discord.ReactionRemoved += _reactionevets.ReactionRemoved;
            //_discord.ReactionsCleared += _reactionevets.ReactionCleared;
            //_discord.ChannelUpdated += _guildevents.ChannelUpdated;

            //Log.Info("Setting up ranking loop.");
            //SetUpTimer(new TimeSpan(12, 06, 00));
            //Log.Info("Finished setting up loop.");

            //if (!_instanceOfLavaNode.IsConnected)
            //{
            //    await _instanceOfLavaNode.ConnectAsync();
            //}
        }

        // this method switches out the severity level from Discord.Net's API, and logs appropriately
        public Task OnLogAsync(LogMessage msg)
        {
            string logText = $"{msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
            switch (msg.Severity.ToString())
            {
                case "Critical":
                    {
                        Log.Error(logText);
                        break;
                    }
                case "Warning":
                    {
                        Log.Warn(logText);
                        break;
                    }
                case "Info":
                    {
                        Log.Info(logText);
                        break;
                    }
                case "Verbose":
                    {
                        Log.Fatal(logText);
                        break;
                    }
                case "Debug":
                    {
                        Log.Debug(logText);
                        break;
                    }
                case "Error":
                    {
                        Log.Error(logText);
                        break;
                    }
            }

            return Task.CompletedTask;

        }
        private void SetUpTimer(TimeSpan alertTime)
        {
            DateTime current = DateTime.UtcNow;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                return;//time already passed
            }
            this.timer = new Timer(x =>
            {
                //this.SomeMethodRunsAt1600();
            }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }

        //private void SomeMethodRunsAt1600()
        //{
        //    var _ = Task.Run(async () =>
        //    {
        //        try
        //        {
        //            Log.Info("Started the daily message ranking loop.");
        //            var configlist = await _db.GetAllDailyRankingsConfigruations();

        //            foreach (var guild in _discord.Guilds)
        //            {
        //                var config = configlist.SingleOrDefault(g => g.GuildId == guild.Id);
        //                if (config != null)
        //                {
        //                    var channels = guild.TextChannels.ToList();
        //                    var done = false;
        //                    var top = new List<UserRanking>();
        //                    var currdate = DateTime.UtcNow.Day;
        //                    var sw = Stopwatch.StartNew();
        //                    ulong total = 0;
        //                    while (!done)
        //                    {
        //                        foreach (var item in channels)
        //                        {
        //                            var list = await item.GetMessagesAsync(100).FlattenAsync();
        //                            int count = list.Count();
        //                            if (count < 1) continue;
        //                            var fulllist = false;
        //                            IMessage lastmsg = list.Last();
        //                            while (!fulllist)
        //                            {
        //                                foreach (var msg in list)
        //                                {
        //                                    lastmsg = msg;

        //                                    if (msg.CreatedAt.Day == currdate)
        //                                    {
        //                                        total += 1;
        //                                        if (msg.Author.IsBot) continue;
        //                                        var isin = top.FirstOrDefault(e => e.AuthorId == msg.Author.Id);
        //                                        if (isin != null)
        //                                        {
        //                                            var entry = top.FirstOrDefault(e => e.AuthorId == msg.Author.Id);
        //                                            entry.Count += 1;
        //                                        }
        //                                        else
        //                                        {
        //                                            var entry = new UserRanking();
        //                                            entry.AuthorId = msg.Author.Id;
        //                                            entry.Count = 1;
        //                                            top.Add(entry);
        //                                        }
        //                                    }
        //                                    else if (msg.CreatedAt.Day != currdate)
        //                                    {
        //                                        fulllist = true;
        //                                        break;
        //                                    }

        //                                }
        //                                if (!fulllist)
        //                                {
        //                                    list = await item.GetMessagesAsync(lastmsg, Direction.Before, 100).FlattenAsync();
        //                                }
        //                            }
        //                        }
        //                        done = true;
        //                    }
        //                    top.Sort(delegate (UserRanking x, UserRanking y)
        //                    {
        //                        if (x.Count == 0 && y.Count == 0) return 0;
        //                        else if (x.Count == 0) return -1;
        //                        else if (y.Count == 0) return 1;
        //                        else return y.Count.CompareTo(x.Count);
        //                    });
        //                    sw.Stop();
        //                    var a = "🥇";
        //                    var b = "🥈";
        //                    var c = "🥉";
        //                    var d = "<:medal4:831979903049007114>";
        //                    var e = "<:medal5:831979917230735381>";
        //                    var emoji = new List<string> { a, b, c, d, e };
        //                    var em = new EmbedBuilder()
        //                       .WithTitle("Daily ranking")
        //                       .WithColor(Color.Gold)
        //                       .WithThumbnailUrl("https://cdn.discordapp.com/attachments/812328100988977166/831968859488911410/trophy.png")
        //                       .WithDescription($"**Today's top spammers are:**\n\n")
        //                       .WithFooter($"It took me {sw.Elapsed.TotalSeconds:F0}s to gather this data.");
        //                    for (int i = 0; i < top.Count; i++)
        //                    {
        //                        if (i == 5) break;
        //                        var user = guild.GetUser(top[i].AuthorId).Username;
        //                        var value = ((double)top[i].Count / total) * 100;
        //                        var percent = Convert.ToInt32(Math.Round(value, 0));
        //                        em.Description += emoji[i] + $" **{user}** - **{top[i].Count}**({percent}% of total) messages\n";

        //                    }
        //                    em.Description += $"\nTotal number of messages sent today - **{total}**";
        //                    await guild.GetTextChannel(config.ChannelId).SendMessageAsync(embed: em.Build());


        //                }
        //            }
        //            SetUpTimer(new TimeSpan(12, 08, 00));
        //            Log.Info("Finished daily message ranking loop.");

        //        }
        //        catch (Exception er)
        //        {
        //            Log.Error(er);
        //        }
        //    });

        //}


    }
}