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

        [Command("say")]
        public async Task say([Remainder]string message)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(message);
        }
        [Command("say")]
        public async Task saychnl(SocketGuildChannel channel, [Remainder]string message)
        {
            await Context.Message.DeleteAsync();
            var chnl = channel as ITextChannel;
            await chnl.SendMessageAsync(message);
        }
        [Command("say")]
        public async Task saydm(SocketUser user, [Remainder]string message)
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
                .AddField("<:rmemory:762736008210546708> Ram usage:", thisProcess.WorkingSet64/1024/1024 + " MB", true)
                .AddField("🏓 Latency:", bot.Latency, true)
                .AddField("🕥 Uptime:", formateddate, true);
            

            await ReplyAsync(embed: em.Build());

        }
        [Command("feedback")]
        public async Task feedback([Remainder]string message)
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

    }
}
