using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using OWuffel.Models;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Modules.Commands.OwnerCommands
{
    [RequireOwner]
    public class DumbCommandsForTests : ModuleBase<Cipska>
    {
        [Command("d")]
        public async Task d()
        {
            var model = new EmbedModel();
            //model.author = new JObject(Context.User.Username, Context.User.GetAvatarUrl());
            var Fields = new JArray("Fields");
            var obj = new JObject();
            obj.Add(new JProperty("name", "naame"));
            obj.Add(new JProperty("value", "naame"));
            obj.Add(new JProperty("inline", "false"));
            Fields.Add(obj);


            //model.fields = new JArray(new JObject("name", "value", "true"));
            Console.WriteLine(model.author.ToString());
        }
        [Command("kill")]
        public async Task kill(int processID)
        {
            Process.GetProcessById(processID).Kill();
            await ReplyAsync("Zajebałem shard #0. Dumny ty z siebie jestes?");
            Console.WriteLine($"Killing {processID}");
        }
        [Command("guildicon")]
        public async Task guildicon()
        {
            await ReplyAsync(Context.Guild.IconUrl);
        }
        [Command("dupa")]
        public async Task dupa()
        {
            string s = "sds";
            var a = Convert.ToInt32(s);
            var b = Context.User.PublicFlags.Value;
            await ReplyAsync(b.ToString().Replace("d", ""));
        }
        [Command("embed")]
        public async Task embed(string nick)
        {
            var channels = "```\n--------------\n1:\n2:\n3:\n4:\n5:\n6:```";
            var smallchannels = "```\n--------------\n1:\n2:\n3:\n4:```";
            var em = new EmbedBuilder()
                .WithAuthor(Context.User)
                .WithColor(Color.Blue)
                .WithDescription($"Poszukiwania frajera o nicku: **{nick}**.")
                .AddField("Balenos               ", channels, true)
                .AddField("Calpheon", channels, true)
                .AddField("Mediah", channels, true)
                .AddField("Serendia", channels, true)
                .AddField("Valencia", channels, true)
                .AddField("Velia", channels, true)
                .AddField("Grana", smallchannels, true)
                .AddField("Arsha", "```\n--------------\n1:```", true)
                .AddField("Znaleziony?", "**NIE**", true)
                .WithCurrentTimestamp();
            await ReplyAsync(embed: em.Build());
        }
        [Command("editname")]
        public async Task editname(string name)
        {
            var chnl = Context.Guild.GetVoiceChannel(831134028492963850);
            await chnl.ModifyAsync(c => c.Name = name);
        }
        [Command("Sss")]
        public async Task Sss([Remainder][Optional] string timezone)
        {
            var currdate = DateTime.Now;
            var tz = TimeZoneInfo.Local;
            string s = "";
            var gottz = TimeZoneInfo.GetSystemTimeZones();
            TimeZoneInfo fromarg = TimeZoneInfo.Local;

            foreach (var item in gottz)
            {
                s += item.Id + "\n";
            }
            if (timezone != null)
            {
                fromarg = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            }
            s = s.Substring(0, 1500);
            await ReplyAsync(fromarg + "\n" + fromarg.BaseUtcOffset.ToString());
            await ReplyAsync(s);

        }
        [Command("topmsg", RunMode = RunMode.Async)]
        public async Task TopMsgAsync([Optional] string arg)
        {
            try
            {
                var channels = Context.Guild.TextChannels.ToList();
                var done = false;
                var top = new List<UserRanking>();
                var currdate = DateTime.Now.Day;
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
                Log.Info($"Connected in {sw.Elapsed.TotalSeconds:F2}s");
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





//{
//    "title": "Title",
//  "description": "%user.fullusername%",
//  "author": {
//        "name": "AuthorName",
//    "icon_url": "https://cdn.discordapp.com/emojis/527982797080494080.png?v=1"
//  },
//    "fields": {
//        {
//            "name": "sdsds"
//            "value": "sdsds"
//            "inline": "true"
//        },
//        {
//            "name": "sdsds"
//            "value": "sdsds"
//            "inline": "true"
//        }
//    }
//  "footer": {
//        "text": "%guild.name%",
//    "icon_url": "%guild.iconurl%"
//  },
//  "thumbnail": "https://cdn.discordapp.com/emojis/764910077085220874.png?v=1",
//  "image": "https://cdn.discordapp.com/emojis/764910077085220874.png?v=1"
//}