using Discord;
using Discord.Rest;
using Discord.WebSocket;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Events
{
    public class VoiceChannelEvents
    {
        private readonly DatabaseUtilities _db;
        private readonly IServiceProvider _services;

        public VoiceChannelEvents(DatabaseUtilities db, IServiceProvider services)
        {
            _db = db;
            _services = services;

        }
        public Task UserVoiceStateUpdate(SocketUser user, SocketVoiceState previousstate, SocketVoiceState newstate)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    if (!(user is IGuildUser usr) || usr.IsBot)
                        return;

                    var beforeVch = previousstate.VoiceChannel;
                    var afterVch = newstate.VoiceChannel;

                    if (beforeVch == afterVch)
                        return;
                    var Settings = await _db.GetGuildSettingsAsync(usr.Guild.Id);
                    if (Settings == null || Settings.logVoiceStateUpdate == 0) return;
                    ITextChannel logChannel;
                    var member = user as SocketGuildUser;
                    var guild = member.Guild;

                    var ch = guild.GetTextChannel(Settings.logVoiceStateUpdate);
                    logChannel = ch;

                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithAuthor(user)
                        .WithDescription("")
                        .WithColor(Color.Blue)
                        //.WithFooter($"• { guild.Name }")
                        .WithCurrentTimestamp();

                    if (beforeVch?.Guild == afterVch?.Guild)
                    {
                        embed.Description = $"🎙️ **{user.Username} moved to another channel.**";

                        embed.AddField("Previous channel:", previousstate.VoiceChannel.Name)
                             .AddField("New channel:", newstate.VoiceChannel.Name);

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (beforeVch == null)
                    {
                        embed.Description = $"🎙 **{user.Username} joined voice channel.**";
                        embed.AddField("Channel:", newstate.VoiceChannel.Name);

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (afterVch == null)
                    {
                        embed.Description = $"🎙 **{user.Username} left voice channel.**";
                        embed.AddField("Last channel:", previousstate.VoiceChannel.Name);

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    return;
                }
                catch
                {
                    // ignored
                }
            });
            return Task.CompletedTask;
        }
    }
}