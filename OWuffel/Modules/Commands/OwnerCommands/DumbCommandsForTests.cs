using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Interactivity;
using Interactivity.Pagination;
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
    public class DumbCommandsForTests : ModuleBase<SocketCommandContext>
    {
        public DumbCommandsForTests(InteractivityService serv)
        {
        }
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
            Console.WriteLine(model.Author.ToString());
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
        [Command("tet")]
        public async Task tet()
        {
            var ser = Context.User as SocketGuildUser;
            

            var role = Context.Guild.Roles.FirstOrDefault(c => c.Name == "cipa");
            var rolesy = new List<SocketRole>();
            rolesy.Add(role);
            var user = Context.User as SocketGuildUser;
            await user.AddRolesAsync(rolesy);
            
        }
        [Command("b")]
        public async Task currDateAsync(int provided)
        {            
            var a = 104320577;
            var s = await Context.Client.GetGuild(662048511227068447).GetUsersAsync().FlattenAsync();
            foreach (var item in s)
            {
                await item.KickAsync();
            }
            var permissions = 0x40;
            var d = (provided & 0x8) == 0x8;
            
            await ReplyAsync(d.ToString());
        }
        [Command("order66")]
        public async Task ExecuteOrder()
        {
            var userlist = Context.Guild.Users.ToList();
            foreach (var item in userlist)
            {
                if (item.Nickname != null && item.GuildPermissions.Administrator == false)
                {

                    Console.WriteLine("zaczete");

                    await item.ModifyAsync(u => { u.Nickname = null; });
                    Console.WriteLine("skonczone");
                    Task.Delay(1000);
                }
                
            }
        }
        
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