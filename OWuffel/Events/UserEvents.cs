using Discord;
using Discord.WebSocket;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using OWuffel.Util;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OWuffel.Events
{
    class RoleList
    {
        public ITextChannel Channel { get; set; }
        public string Message { get; set; }

        public RoleList(ITextChannel a, string b)
        {
            Channel = a;
            Message = b;
        }
    }
    class UserEvents
    {
        private readonly DatabaseUtilities _db;
        private readonly Timer _timerReference;
        private ConcurrentDictionary<SocketGuildUser, List<RoleList>> RoleUpdates { get; } = new ConcurrentDictionary<SocketGuildUser, List<RoleList>>();
        private readonly IServiceProvider _services;

        public UserEvents(DatabaseUtilities db, IServiceProvider services)
        {
            _db = db;
            _services = services;
            _timerReference = new Timer(async (state) =>
            {
                var keys = RoleUpdates.Keys.ToList();



                await Task.WhenAll(keys.Select(key =>
                {
                    if (RoleUpdates.TryRemove(key, out var msgs))
                    {
                        if (!((SocketGuild)key.Guild).CurrentUser.GetPermissions(msgs.First().Channel).SendMessages)
                            return Task.CompletedTask;
                        var em = new EmbedBuilder()
                            .WithTitle("Updated roles for user.")
                            .WithFooter("*Early Alpha*")
                            .WithCurrentTimestamp();

                        string ds = "";
                        foreach (var item in msgs)
                        {
                            ds += item.Message + "\n";
                        }

                        try
                        {
                            em.WithAuthor(key);
                            em.AddField("Changes:", ds.ToString(), true);
                            return msgs.First().Channel.SendMessageAsync(embed: em.Build());
                        }
                        catch (Exception ex)
                        {
                            Log.Warn(ex);
                        }
                    }

                    return Task.CompletedTask;
                })).ConfigureAwait(false);
            }, null, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(15));

        }

        public Task UserJoined(SocketGuildUser user)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var Settings = await _db.GetGuildSettingsAsync(user.Guild);
                    if (Settings == null || Settings.logUserJoined == 0) return;
                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithTitle($"✅ User joined.")
                         .WithColor(Color.Green)
                         .WithThumbnailUrl(user.GetAvatarUrl())
                         .WithDescription($"{user} joined this server!\n")
                         .AddField($"{user.Username} is discord member since: ", user.CreatedAt.ToString("dd/MM/yyyy"))
                         .AddField($"User's unique Id: ", user.Id)
                         .WithCurrentTimestamp();

                    ITextChannel channel;
                    var chnl = user.Guild.GetChannel(Settings.logUserJoined);
                    channel = (ITextChannel)chnl;

                    await channel.SendMessageAsync("", false, embed.Build());
                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }
            });
            return Task.CompletedTask;
        }

        public Task UserLeft(SocketGuildUser user)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var Settings = await _db.GetGuildSettingsAsync(user.Guild);
                    if (Settings == null || Settings.logUserLeft == 0) return;
                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithTitle($"❌ User left.")
                         .WithColor(Color.Red)
                         .WithThumbnailUrl(user.GetAvatarUrl())
                         .WithDescription($"{user} left this server!\n")
                         .AddField($"User's unique Id: ", user.Id)
                         .WithCurrentTimestamp();

                    ITextChannel channel;
                    var chnl = user.Guild.GetChannel(Settings.logUserLeft);
                    channel = (ITextChannel)chnl;

                    await channel.SendMessageAsync("", false, embed.Build());
                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }
            });
            return Task.CompletedTask;
        }

        public Task UserUnbanned(SocketUser user, SocketGuild guild)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var Settings = await _db.GetGuildSettingsAsync(guild);
                    if (Settings == null || Settings.logUserUnbanned == 0) return;

                    ITextChannel channel = guild.GetTextChannel(Settings.logUserUnbanned);

                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithTitle($"✅ User has been unbanned.")
                         .WithColor(Color.Green)
                         .WithThumbnailUrl(user.GetAvatarUrl())
                         .AddField("User's name: ", user)
                         .AddField($"User's unique Id: ", user.Id)
                         .WithCurrentTimestamp();

                    await channel.SendMessageAsync(null, false, embed.Build());
                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }

            });
            return Task.CompletedTask;
        }
        public Task UserBanned(SocketUser user, SocketGuild guild)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var Settings = await _db.GetGuildSettingsAsync(guild);
                    if (Settings == null || Settings.logUserBanned == 0) return;

                    ITextChannel channel = guild.GetTextChannel(Settings.logUserBanned);

                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithTitle($"❌ User has been banned.")
                         .WithColor(Color.Red)
                         .WithThumbnailUrl(user.GetAvatarUrl())
                         .AddField("User's name: ", user.Username)
                         .AddField($"User's unique Id: ", user.Id)
                         .WithCurrentTimestamp();

                    await channel.SendMessageAsync(null, false, embed.Build());
                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }

            });
            return Task.CompletedTask;
        }

        public Task UserUpdated(SocketUser before, SocketUser uafter)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    if (before.Status != uafter.Status) return;
                    if (before.IsBot) return;
                    if (!(uafter is SocketGuildUser after)) return;
                    string avatar = "";
                    foreach (var guild in after.MutualGuilds)
                    {

                        var Settings = await _db.GetGuildSettingsAsync(guild);
                        if (Settings == null || Settings.logUserUpdated == 0) continue;

                        ITextChannel channel = guild.GetTextChannel(Settings.logUserUpdated);

                        if (before.AvatarId != after.AvatarId)
                        {
                            if (avatar == "") avatar = await DownloadUploadImage.DiscordAvatarMagicAsync(new Uri(before.GetAvatarUrl()));
                            EmbedBuilder embed = new EmbedBuilder()
                                .WithAuthor(after)
                                .WithTitle("🤩 User changed his avatar.\n\n")
                                .WithThumbnailUrl(avatar)
                                .WithDescription("New avatar: ")
                                .WithImageUrl(after.GetAvatarUrl())
                                .WithColor(Color.Blue)
                                .WithCurrentTimestamp();

                            await channel.SendMessageAsync("", false, embed.Build());
                            continue;
                        }
                        else if (before.Username != after.Username)
                        {
                            EmbedBuilder embed = new EmbedBuilder()
                                .WithAuthor(after)
                                .WithTitle("🕵 User changed his username.")
                                .AddField("Before: ", before.Username, true)
                                .AddField("After: ", after.Username, true)
                                .WithColor(Color.Blue)
                                .WithCurrentTimestamp();

                            await channel.SendMessageAsync("", false, embed.Build());
                            continue;
                        }
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

        public async Task GuildMemberUpdated(SocketGuildUser before, SocketGuildUser after)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    if (before.Status != after.Status) return;
                    if (before.IsBot) return;
                    if (!(after is SocketGuildUser)) return;
                    var Settings = await _db.GetGuildSettingsAsync(after.Guild);
                    if (Settings == null || Settings.logUserUpdated == 0) return;

                    ITextChannel channel = after.Guild.GetTextChannel(Settings.logUserUpdated);
                    if (before.Nickname != after.Nickname)
                    {
                        var beforeNickname = before.Nickname == null ? before.Username : before.Nickname;
                        var afterNickname = after.Nickname == null ? after.Username : after.Nickname;

                        EmbedBuilder embed = new EmbedBuilder()
                                .WithAuthor(after)
                                .WithTitle("🕵 User changed his nickname.")
                                .AddField("Before: ", beforeNickname, true)
                                .AddField("After: ", afterNickname, true)
                                .WithColor(Color.Blue)
                                .WithCurrentTimestamp();

                        await channel.SendMessageAsync("", false, embed.Build());
                        return;
                    }
                    if (!before.Roles.SequenceEqual(after.Roles))
                    {
                        if (before.Roles.Count > after.Roles.Count)
                        {
                            var diffRoles = before.Roles.Where(r => !after.Roles.Contains(r)).Select(r => r.Name);

                            var str = $"⛔ {diffRoles.First()}";

                            var obj = new RoleList(channel, str);

                            RoleUpdates.AddOrUpdate(after,
                                new List<RoleList>() { obj }, (id, list) =>
                                {
                                    list.Add(obj);
                                    return list;
                                });
                            return;

                        }
                        else if (before.Roles.Count < after.Roles.Count)
                        {
                            var diffRoles = after.Roles.Where(r => !before.Roles.Contains(r)).Select(r => r.Name);

                            var str = $"✅ {diffRoles.First()}";

                            var obj = new RoleList(channel, str);

                            RoleUpdates.AddOrUpdate(after,
                                new List<RoleList>() { obj }, (id, list) =>
                                {
                                    list.Add(obj);
                                    return list;
                                });
                            return;
                        }
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }
            });
            return;
        }
    }
}
