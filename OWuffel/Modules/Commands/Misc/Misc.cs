using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Modules.Commands
{
    public class Misc : ModuleBase<Cipska>
    {
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
            em.WithAuthor(Context.Client.CurrentUser)
                .WithDescription("Statistics for the shard this guild is on.")
                .AddField("Shard's ID:", bot.ShardId, false)
                .AddField("Number of guilds:", bot.Guilds.Count, true)
                .AddField("Number of users:", sum.ToString(), true)
                .AddField("Number of guilds:", bot.Guilds.Count, true)
                .AddField("Number of guilds:", bot.Guilds.Count, true)
                .AddField("Number of guilds:", bot.Guilds.Count, true)
                .AddField("Number of guilds:", bot.Guilds.Count, true)
                .AddField("Number of guilds:", bot.Guilds.Count, true)
                .AddField("Number of guilds:", bot.Guilds.Count, true);

            await ReplyAsync(embed: em.Build());

        }

    }
}
