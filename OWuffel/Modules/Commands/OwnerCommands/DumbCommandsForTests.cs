using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;
using OWuffel.Models;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Modules.Commands.OwnerCommands
{
    [RequireOwner]
    public class DumbCommandsForTests: ModuleBase<Cipska>
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