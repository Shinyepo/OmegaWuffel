using Discord;
using Discord.WebSocket;
using OWuffel.Extensions.Database;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Events
{
    class UserEvents
    {
        private readonly DatabaseUtilities _db;
        private readonly IServiceProvider _services;

        public UserEvents(DatabaseUtilities db, IServiceProvider services)
        {
            _db = db;
            _services = services;

        }

        public Task UserJoined(SocketGuildUser user)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var Settings = await _db.GetGuildSettingsAsync(user.Guild.Id);
                    if (Settings == null || Settings.logUserJoins == 0) return;
                    var _ = Task.Run(async () =>
                    {
                        EmbedBuilder embed = new EmbedBuilder();
                        embed.WithTitle($"✅ User joined.")
                             .WithColor(Color.Green)
                             .WithThumbnailUrl(user.GetAvatarUrl())
                             .WithDescription($"{user} joined this server!\n")
                             .AddField($"{user.Username} is discord member since: ", user.CreatedAt)
                             .AddField($"User's unique Id: ", user.Id)
                             //.WithFooter($"• { user.Guild.Name }")
                             .WithCurrentTimestamp();

                        ITextChannel channel;
                        var chnl = user.Guild.GetChannel(Settings.logUserJoins);
                        channel = (ITextChannel)chnl;

                        await channel.SendMessageAsync("", false, embed.Build());
                    });
                }
                catch
                {
                    //dont care
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
                    var Settings = await _db.GetGuildSettingsAsync(user.Guild.Id);
                    if (Settings == null || Settings.logUserJoins == 0) return;
                    var _ = Task.Run(async () =>
                    {
                        EmbedBuilder embed = new EmbedBuilder();
                        embed.WithTitle($"❌ User left.")
                             .WithColor(Color.Red)
                             .WithThumbnailUrl(user.GetAvatarUrl())
                             .WithDescription($"{user} left this server!\n")
                             .AddField($"User's unique Id: ", user.Id)
                             //.WithFooter($"• { user.Guild.Name }")
                             .WithCurrentTimestamp();

                        ITextChannel channel;
                        var chnl = user.Guild.GetChannel(Settings.logUserJoins);
                        channel = (ITextChannel)chnl;

                        await channel.SendMessageAsync("", false, embed.Build());
                    });
                }
                catch
                {
                    //dont care
                }
            });            
            return Task.CompletedTask;
        }

        public Task UserUnbanned(SocketUser arg1, SocketGuild arg2)
        {
            var _ = Task.Run(async () =>
            {

            });
            return Task.CompletedTask;
        }

        public Task UserUpdated(SocketUser before, SocketUser after)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    if (before.IsBot) return;
                    if (!(after is SocketGuildUser)) return;
                    var memberbefore = before as SocketGuildUser;
                    var memberafter = after as SocketGuildUser;
                    var Settings = await _db.GetGuildSettingsAsync(memberafter.Guild.Id);
                    if (Settings == null || Settings.logUserUpdates == 0) return;

                    ITextChannel channel = memberbefore.Guild.GetTextChannel(Settings.logUserUpdates);

                    if (before.AvatarId != after.AvatarId)
                    {

                        EmbedBuilder embed = new EmbedBuilder()
                            .WithAuthor(memberafter)
                            .WithThumbnailUrl(after.GetAvatarUrl())
                            .WithDescription($"{memberafter.Username} changed avatar.")
                            .WithColor(Color.Blue)
                            //.WithFooter($"• { memberafter.Guild.Name }")
                            .WithCurrentTimestamp();

                        await channel.SendMessageAsync("", false, embed.Build());
                        return;

                    }
                    else if (before.Username != after.Username)
                    {
                        EmbedBuilder embed = new EmbedBuilder()
                            .WithAuthor(before)
                            .WithThumbnailUrl(after.GetAvatarUrl())
                            .WithDescription($"{before.Username} changed username.")
                            .AddField("Before: ", before.Username)
                            .AddField("After: ", after.Username)
                            .WithColor(Color.Blue)
                            //.WithFooter($"• { memberafter.Guild.Name }")
                            .WithCurrentTimestamp();

                        await channel.SendMessageAsync("", false, embed.Build());
                        return;

                    }
                    return;
                }
                catch
                {
                    //dont care
                }
            });
            return Task.CompletedTask;
        }
    }
}
