using Discord;
using Discord.Rest;
using Discord.WebSocket;
using OWuffel.Services;
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
                    var guildtodb = usr.Guild as SocketGuild;

                    var Settings = await _db.GetGuildSettingsAsync(guildtodb);
                    if (Settings == null || Settings.logVoiceStateUpdated == 0) return;
                    if (Settings.logIgnoreVoiceStateUpdated != null)
                    {
                        if (beforeVch == null)
                        {
                            if (Settings.logIgnoreVoiceStateUpdated.Contains(user.Id.ToString()) || Settings.logIgnoreVoiceStateUpdated.Contains(afterVch.Id.ToString()))
                            {
                                return;
                            }
                        }
                        else if (afterVch == null)
                        {

                            if (Settings.logIgnoreVoiceStateUpdated.Contains(beforeVch.Id.ToString()) || Settings.logIgnoreVoiceStateUpdated.Contains(user.Id.ToString()))
                            {
                                return;
                            }
                        }
                    }
                    ITextChannel logChannel;
                    var member = user as SocketGuildUser;
                    var guild = member.Guild;
                    var ch = guild.GetTextChannel(Settings.logVoiceStateUpdated);
                    logChannel = ch;

                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithAuthor(user)
                        .WithColor(Color.Blue)
                        .WithCurrentTimestamp();

                    if (beforeVch?.Guild == afterVch?.Guild && beforeVch.Id != afterVch.Id)
                    {
                        embed.WithDescription($"🎙️ **{user.Username} moved to another channel.**");

                        embed.AddField("Previous channel:", previousstate.VoiceChannel.Name)
                             .AddField("New channel:", newstate.VoiceChannel.Name);

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (beforeVch == null)
                    {
                        embed.WithDescription($"🎙 **{user.Username} joined voice channel.**");
                        embed.AddField("Channel:", newstate.VoiceChannel.Name);

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (afterVch == null)
                    {
                        embed.WithDescription($"🎙 **{user.Username} left voice channel.**");
                        embed.AddField("Last channel:", previousstate.VoiceChannel.Name);

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (!previousstate.IsDeafened && newstate.IsDeafened)
                    {
                        embed.WithTitle("❌ 🎧 User has been deafened");

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (previousstate.IsDeafened && !newstate.IsDeafened)
                    {
                        embed.WithTitle("✅ 🎧 User is no longer deafened");

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (!previousstate.IsMuted && newstate.IsMuted)
                    {
                        embed.WithTitle("❌ 🎙️ User has been muted");

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (previousstate.IsMuted && !newstate.IsMuted)
                    {
                        embed.WithTitle("✅ 🎙️ User is no longer muted");

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (!previousstate.IsStreaming && newstate.IsStreaming)
                    {
                        embed.WithTitle("✅ 🎦 User is now streaming!")
                            .AddField("Channel: ", member.VoiceChannel.Name, false);

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    else if (previousstate.IsStreaming && !newstate.IsStreaming)
                    {
                        embed.WithTitle("❌ 🎦 User is no longer streaming!")
                            .AddField("Channel: ", member.VoiceChannel.Name, false);

                        await ch.SendMessageAsync("", false, embed.Build());
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }
            });
            return Task.CompletedTask;
        }
    }
}