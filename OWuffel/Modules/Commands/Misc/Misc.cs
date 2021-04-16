using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OWuffel.Modules.Commands
{
    public class Misc : ModuleBase<Cipska>
    {
        private Timer timer;
        private DatabaseUtilities _db;
        private DiscordSocketClient _client;

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
                this.SomeMethodRunsAt1600();
            }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }

        private void SomeMethodRunsAt1600()
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    Log.Info("Started the daily message ranking loop.");
                    var configlist = await _db.GetAllDailyRankingsConfigruations();
                    
                    foreach (var guild in _client.Guilds)
                    {
                        var config = configlist.SingleOrDefault(g => g.GuildId == guild.Id);
                        if (config != null)
                        {
                            var channels = guild.TextChannels.ToList();
                            var done = false;
                            var top = new List<UserRanking>();
                            var currdate = DateTime.UtcNow.Day;
                            var sw = Stopwatch.StartNew();
                            ulong total = 0;
                            while (!done)
                            {
                                foreach (var item in channels)
                                {
                                    var list = await item.GetMessagesAsync(100).FlattenAsync();
                                    int count = list.Count();
                                    if (count < 1) continue;
                                    var fulllist = false;
                                    IMessage lastmsg = list.Last();
                                    while (!fulllist)
                                    {
                                        foreach (var msg in list)
                                        {
                                            lastmsg = msg;

                                            if (msg.CreatedAt.Day == currdate)
                                            {
                                                total += 1;
                                                if (msg.Author.IsBot) continue;
                                                var isin = top.FirstOrDefault(e => e.AuthorId == msg.Author.Id);
                                                if (isin != null)
                                                {
                                                    var entry = top.FirstOrDefault(e => e.AuthorId == msg.Author.Id);
                                                    entry.Count += 1;
                                                }
                                                else
                                                {
                                                    var entry = new UserRanking();
                                                    entry.AuthorId = msg.Author.Id;
                                                    entry.Count = 1;
                                                    top.Add(entry);
                                                }
                                            }
                                            else if (msg.CreatedAt.Day != currdate)
                                            {
                                                fulllist = true;
                                                break;
                                            }

                                        }
                                        if (!fulllist)
                                        {
                                            list = await item.GetMessagesAsync(lastmsg, Direction.Before, 100).FlattenAsync();
                                        }
                                    }
                                }
                                done = true;
                            }
                            top.Sort(delegate (UserRanking x, UserRanking y)
                            {
                                if (x.Count == 0 && y.Count == 0) return 0;
                                else if (x.Count == 0) return -1;
                                else if (y.Count == 0) return 1;
                                else return y.Count.CompareTo(x.Count);
                            });
                            sw.Stop();
                            var userone = guild.GetUser(top[0].AuthorId).Username;
                            var usertwo = guild.GetUser(top[1].AuthorId).Username;
                            var userthree = guild.GetUser(top[2].AuthorId).Username;
                            var userfour = guild.GetUser(top[3].AuthorId).Username;
                            var userfive = guild.GetUser(top[4].AuthorId).Username;
                            var value = ((double)top[0].Count / total) * 100;
                            var perone = Convert.ToInt32(Math.Round(value, 0));
                            value = ((double)top[1].Count / total) * 100;
                            var pertwo = Convert.ToInt32(Math.Round(value, 0));
                            value = ((double)top[2].Count / total) * 100;
                            var perthree = Convert.ToInt32(Math.Round(value, 0));
                            value = ((double)top[3].Count / total) * 100;
                            var perfour = Convert.ToInt32(Math.Round(value, 0));
                            value = ((double)top[4].Count / total) * 100;
                            var perfive = Convert.ToInt32(Math.Round(value, 0));
                            var em = new EmbedBuilder()
                                .WithTitle("Daily ranking")
                                .WithColor(Color.Gold)
                                .WithThumbnailUrl("https://cdn.discordapp.com/attachments/812328100988977166/831968859488911410/trophy.png")
                                .WithDescription($"**Today's top spammers are:**\n\n🥇 **{userone}** - **{top[0].Count}**({perone}% of total) messages\n🥈 **{usertwo}** - **{top[1].Count}**({pertwo}% of total) messages\n🥉 **{userthree}** - **{top[2].Count}**({perthree}% of total) messages\n<:medal4:831979903049007114> **{userfour}** - **{top[3].Count}**({perfour}% of total) messages\n<:medal5:831979917230735381> **{userfive}** - **{top[4].Count}**({perfive}% of total) messages\n\nTotal number of messages sent today - **{total}**")
                                .WithFooter($"It took me {sw.Elapsed.TotalSeconds:F0}s to gather this data.");
                            await guild.GetTextChannel(config.ChannelId).SendMessageAsync(embed: em.Build());
                        }
                    }
                    Log.Info("Finished daily message ranking loop.");

                }
                catch (Exception)
                {

                    throw;
                }
            });

        }
        public Misc(DatabaseUtilities db, DiscordSocketClient client)
        {
            _db = db;
            _client = client;
            Log.Info("Setting up ranking loop.");
            SetUpTimer(new TimeSpan(23,55,00));
            Log.Info("Finished setting up loop.");
        }
        [Command("Avatar")]
        public async Task avatar([Optional] SocketGuildUser user)
        {
            EmbedBuilder em = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithCurrentTimestamp();

            user = user == null ? Context.User as SocketGuildUser : user;
            var avatar = user.GetAvatarUrl() == null ? user.GetDefaultAvatarUrl() : user.GetAvatarUrl();
            avatar = avatar.Split("?")[0] + "?size=1024";

            var name = user.Nickname == null ? user.Username : user.Nickname;
            em.WithAuthor(user)
                .WithTitle($"{name}'s avatar.")
                .WithImageUrl(avatar);

            await ReplyAsync(embed: em.Build());
        }

        [Command("Ping")]
        public async Task ping()
        {
            EmbedBuilder em = new EmbedBuilder()
                .WithDescription($"🏓 Pong! {Context.Client.Latency}ms")
                .WithColor(Color.Blue);

            await ReplyAsync(embed: em.Build());
        }

        [Command("say")]
        public async Task say([Remainder] string message)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(message);
        }
        [Command("say")]
        public async Task saychnl(SocketGuildChannel channel, [Remainder] string message)
        {
            await Context.Message.DeleteAsync();
            var chnl = channel as ITextChannel;
            await chnl.SendMessageAsync(message);
        }
        [Command("say")]
        public async Task saydm(SocketUser user, [Remainder] string message)
        {
            await Context.Message.DeleteAsync();
            await user.SendMessageAsync(message);
        }
        [Command("say")]
        public async Task sayguild(ulong guild, ulong channel, [Remainder] string message)
        {
            await Context.Message.DeleteAsync();
            var foundguild = Context.Client.Guilds.FirstOrDefault(g => g.Id == guild);
            if (foundguild == null) await ReplyAsync("I could not find provided guild.");
            var foundchannel = foundguild.GetTextChannel(channel) as ITextChannel;
            if (foundchannel == null) await ReplyAsync("I could not find provided channel.");
            await foundchannel.SendMessageAsync(message);
        }

        [Command("Stats")]
        public async Task stats()
        {
            EmbedBuilder em = new EmbedBuilder();
            var bot = Context.Client;
            var thisProcess = Process.GetCurrentProcess();
            var sum = 0;
            foreach (var item in bot.Guilds)
            {
                sum += item.Users.Count;
            }
            var timespan = DateTime.Now - thisProcess.StartTime;
            var formateddate = timespan.ToString("%d") + " Day(s) " + timespan.ToString(@"hh\:mm\:ss");

            em.WithAuthor(Context.Client.CurrentUser)
                .WithColor(Color.Blue)
                .WithCurrentTimestamp()
                .WithDescription("Statistics for the shard this server is on.")
                .AddField("<:server:762736900812832768> Shard's ID:", bot.ShardId, false)
                .AddField("<:discord:818478882000076850> Number of guilds:", bot.Guilds.Count, true)
                .AddField("👥 Number of users:", sum.ToString(), true)
                .AddField("<:rmemory:762736008210546708> Ram usage:", thisProcess.WorkingSet64 / 1024 / 1024 + " MB", true)
                .AddField("🏓 Latency:", bot.Latency, true)
                .AddField("🕥 Uptime:", formateddate, true);


            await ReplyAsync(embed: em.Build());

        }
        [Command("feedback")]
        public async Task feedback([Remainder] string message)
        {
            ulong guildid = 812328100988977162;
            ulong channelid = 821293930623664148;
            var guild = Context.Client.GetGuild(guildid);
            var chnl = guild.GetTextChannel(channelid) as ITextChannel;

            var em = new EmbedBuilder()
                .WithAuthor(Context.User)
                .WithCurrentTimestamp()
                .WithColor(Color.Blue)
                .WithTitle("A feedback from user")
                .WithDescription("```" + message + "```")
                .AddField("Author details", Context.User.Username + "(" + Context.User.Id + ")", false)
                .AddField("Guild details", Context.Guild.Name + "(" + Context.Guild.Id + ")", false);
            await chnl.SendMessageAsync(embed: em.Build());
            await ReplyAsync("Thank you for your feedback! 👍");
        }
        [Command("topmsg")]
        public async Task SetupTopMsgAsync(SocketTextChannel channel)
        {
            var config = new DailyRanking();
            config.ChannelId = channel.Id;
            config.GuildId = Context.Guild.Id;
            config.IgnoreId = "";
            await _db.CreateDailyRankingConfigAsync(config);
            await ReplyAsync($"Subscribed daily message ranking to **{channel.Mention}**.");
        }
        [Command("topmsg", RunMode = RunMode.Async)]
        public async Task TopMsgAsync([Optional] string arg)
        {
            try
            {
                var channels = Context.Guild.TextChannels.ToList();
                var done = false;
                var top = new List<UserRanking>();
                var currdate = DateTime.UtcNow.Day;
                var sw = Stopwatch.StartNew();
                ulong total = 0;
                while (!done)
                {
                    foreach (var item in channels)
                    {
                        var list = await item.GetMessagesAsync(100).FlattenAsync();
                        int count = list.Count();
                        if (count < 1) continue;
                        var fulllist = false;
                        IMessage lastmsg = list.Last();
                        while (!fulllist)
                        {
                            foreach (var msg in list)
                            {
                                lastmsg = msg;

                                if (msg.CreatedAt.Day == currdate)
                                {
                                    total += 1;
                                    if (msg.Author.IsBot) continue;
                                    var isin = top.FirstOrDefault(e => e.AuthorId == msg.Author.Id);
                                    if (isin != null)
                                    {
                                        var entry = top.FirstOrDefault(e => e.AuthorId == msg.Author.Id);
                                        entry.Count += 1;
                                    }
                                    else
                                    {
                                        var entry = new UserRanking();
                                        entry.AuthorId = msg.Author.Id;
                                        entry.Count = 1;
                                        top.Add(entry);
                                    }
                                }
                                else if (msg.CreatedAt.Day != currdate)
                                {
                                    fulllist = true;
                                    break;
                                }

                            }
                            if (!fulllist)
                            {
                                list = await item.GetMessagesAsync(lastmsg, Direction.Before, 100).FlattenAsync();
                            }
                        }
                    }
                    done = true;
                }
                top.Sort(delegate (UserRanking x, UserRanking y)
                {
                    if (x.Count == 0 && y.Count == 0) return 0;
                    else if (x.Count == 0) return -1;
                    else if (y.Count == 0) return 1;
                    else return y.Count.CompareTo(x.Count);
                });
                sw.Stop();
                var userone = Context.Guild.GetUser(top[0].AuthorId).Username;
                var usertwo = Context.Guild.GetUser(top[1].AuthorId).Username;
                var userthree = Context.Guild.GetUser(top[2].AuthorId).Username;
                var userfour = Context.Guild.GetUser(top[3].AuthorId).Username;
                var userfive = Context.Guild.GetUser(top[4].AuthorId).Username;
                var value = ((double)top[0].Count / total) * 100;
                var perone = Convert.ToInt32(Math.Round(value, 0));
                value = ((double)top[1].Count / total) * 100;
                var pertwo = Convert.ToInt32(Math.Round(value, 0));
                value = ((double)top[2].Count / total) * 100;
                var perthree = Convert.ToInt32(Math.Round(value, 0));
                value = ((double)top[3].Count / total) * 100;
                var perfour = Convert.ToInt32(Math.Round(value, 0));
                value = ((double)top[4].Count / total) * 100;
                var perfive = Convert.ToInt32(Math.Round(value, 0));
                var em = new EmbedBuilder()
                    .WithTitle("Daily ranking")
                    .WithColor(Color.Gold)
                    .WithThumbnailUrl("https://cdn.discordapp.com/attachments/812328100988977166/831968859488911410/trophy.png")
                    .WithDescription($"**Today's top spammers are:**\n\n🥇 **{userone}** - **{top[0].Count}**({perone}% of total) messages\n🥈 **{usertwo}** - **{top[1].Count}**({pertwo}% of total) messages\n🥉 **{userthree}** - **{top[2].Count}**({perthree}% of total) messages\n<:medal4:831979903049007114> **{userfour}** - **{top[3].Count}**({perfour}% of total) messages\n<:medal5:831979917230735381> **{userfive}** - **{top[4].Count}**({perfive}% of total) messages\n\nTotal number of messages sent today - **{total}**")
                    .WithFooter($"It took me {sw.Elapsed.TotalSeconds:F0}s to gather this data.");
                await ReplyAsync(embed: em.Build());
            }
            catch (Exception)
            {

                throw;
            }
            //var a = await Context.Channel.GetMessagesAsync(last, Direction.Before, 100).FlattenAsync();
        }
    }

    public class UserRanking
    {
        public ulong AuthorId { get; set; }
        public ulong Count { get; set; }
    }
}
