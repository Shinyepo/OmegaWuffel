using Discord;
using Discord.WebSocket;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OWuffel.events
{
    public class GuildEvents
    {
        private readonly DatabaseUtilities _db;
        private readonly WuffelDBContext _debe;

        public GuildEvents(DatabaseUtilities db, WuffelDBContext debe)
        {
            _db = db;
            _debe = debe;
        }

        public async Task JoinedGuild(SocketGuild arg)
        {

            var Settings = await _db.GetGuildSettingsAsync(arg);
            if (Settings == null)
            {
                await _db.SetDefaultSettingsAsync(arg);
                Log.Info($"Joined {arg.Name}({arg.Id}) with {arg.MemberCount} users total.");
            }
            else
            {
                Settings.botActive = true;
                await _debe.SaveChangesAsync();
                Log.Info($"Joined {arg.Name}({arg.Id}) with {arg.MemberCount} users total.");
            }
        }

        public async Task LeftGuild(SocketGuild arg)
        {
            var Settings = await _db.GetGuildSettingsAsync(arg);
            if (Settings != null)
            {
                Settings.botActive = false;
                await _debe.SaveChangesAsync();
                Log.Info($"Left {arg.Name}({arg.Id}) with {arg.MemberCount} users total.");
            }
            else
            {
                return;
            }
        }

        public Task GuildUpdated(SocketGuild guild1, SocketGuild guild2)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var settings = await _db.GetGuildSettingsAsync(guild2);

                    if (guild1.IconUrl != guild2.IconUrl)
                    {
                        if (settings.logGuildUpdated == 0) return;
                        ITextChannel logChannel = guild2.GetTextChannel(settings.logGuildUpdated);
                        if (logChannel == null) return;
                        string icon = "";
                        if (guild1.IconUrl != null)
                        {
                            Uri ur = new Uri(guild1.IconUrl);
                            icon = await DownloadUploadImage.DiscordAvatarMagicAsync(ur);
                            Console.WriteLine(icon);
                        }
                        var readyIcon = guild2.IconUrl + "?size=1024";
                        EmbedBuilder em = new EmbedBuilder()
                               .WithAuthor(guild2.Name, icon)
                               .WithTitle("Server icon changed.")
                               .WithImageUrl(readyIcon)
                               .WithColor(Color.Blue)
                               .WithCurrentTimestamp();

                        await logChannel.SendMessageAsync(embed: em.Build());
                    }
                    if (guild1.Emotes.Count != guild2.Emotes.Count)
                    {
                        EmbedBuilder em = new EmbedBuilder()
                               .WithAuthor(guild2.Name, guild2.IconUrl)
                               .WithCurrentTimestamp();

                        if (guild1.Emotes.Count > guild2.Emotes.Count)
                        {
                            if (settings.logEmoteDeleted == 0) return;
                            ITextChannel logChannel = guild2.GetTextChannel(settings.logEmoteDeleted);
                            if (logChannel == null) return;
                            var diffRoles = guild1.Emotes.Where(r => !guild2.Emotes.Contains(r)).Select(r => r.Name);
                            var emote = guild1.Emotes.FirstOrDefault(e => e.Name == diffRoles.First());
                            em.WithTitle("❌ Emote removed")
                                .WithColor(Color.Red)
                                .WithDescription($"[:{emote.Name}:]({emote.Url})");

                            await logChannel.SendMessageAsync(embed: em.Build());
                        }
                        else if (guild1.Emotes.Count < guild2.Emotes.Count)
                        {
                            if (settings.logEmoteCreated == 0) return;
                            ITextChannel logChannel = guild2.GetTextChannel(settings.logEmoteCreated);
                            if (logChannel == null) return;
                            var diffRoles = guild2.Emotes.Where(r => !guild1.Emotes.Contains(r)).Select(r => r.Name);
                            var emote = guild2.Emotes.FirstOrDefault(e => e.Name == diffRoles.First());
                            em.WithTitle("✅ Emote added")
                                .WithColor(Color.Green)
                                .WithDescription($"<:{emote.Name}:{emote.Id}> [:{emote.Name}:]({emote.Url})");

                            await logChannel.SendMessageAsync(embed: em.Build());

                        }
                    }
                    if (guild1.Emotes != guild2.Emotes)
                    {
                        if (settings.logEmoteUpdated == 0) return;
                        ITextChannel logChannel = guild2.GetTextChannel(settings.logEmoteUpdated);
                        if (logChannel == null) return;

                        EmbedBuilder em = new EmbedBuilder()
                            .WithAuthor(guild2.Name, guild2.IconUrl)
                            .WithTitle("Emote updated")
                            .WithColor(Color.Blue)
                            .WithCurrentTimestamp();

                        var oldEmotes = guild1.Emotes.ToList();
                        var newEmotes = guild2.Emotes.ToList();
                        string oldName = "";
                        string newName = "";
                        Emote emote = null;
                        for (int i = 0; i < oldEmotes.Count; i++)
                        {
                            if (oldEmotes[i].Name != newEmotes[i].Name)
                            {
                                oldName = oldEmotes[i].Name;
                                newName = newEmotes[i].Name;
                                emote = newEmotes[i];
                            }
                        }
                        var value = emote + " " + oldName + " -> " + newName;
                        em.AddField("Name change:", value, true);

                        await logChannel.SendMessageAsync(embed: em.Build());

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            });
            return Task.CompletedTask;
        }

        internal Task ChannelUpdated(SocketChannel channel1, SocketChannel channel2)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var chnl1 = channel1 as SocketGuildChannel;
                    var chnl2 = channel2 as SocketGuildChannel;
                    var guild = chnl1.Guild;
                    var Settings = await _db.GetGuildSettingsAsync(guild);
                    if (Settings.logChannelUpdated == 0) return;
                    ITextChannel channel = guild.GetTextChannel(Settings.logChannelUpdated);

                    //if (chnl.Name != chnl2.Name)
                    //{
                    //    EmbedBuilder embed = new EmbedBuilder();
                    //    embed.WithTitle($"Channel updated.")
                    //             .WithColor(Color.Blue)
                    //             .WithDescription($"Cipa huj na szybko: {chnl.Name} -> {chnl2.Name}.")
                    //             .WithCurrentTimestamp();

                    //    await channel.SendMessageAsync(embed: embed.Build());
                    //}
                    /////////////////////////////////////////////////////////////////////////////////////////////////////
                    var list1 = chnl1.PermissionOverwrites.ToList();
                    var list2 = chnl2.PermissionOverwrites.ToList();
                    if (!list1.SequenceEqual(list2))
                    {
                        var staralista = list1.Where(p => !list2.Contains(p)).ToList();
                        var nowalista = list2.Where(p => !list1.Contains(p)).ToList();

                        if (staralista.Count == 0)
                        {
                            var dodane = "";
                        }

                        if (nowalista.Count == 0)
                        {
                            var usuniete = "";
                        }
                        if (staralista.Count > 0 && nowalista.Count > 0)
                        {
                            var listtt1 = staralista[0].Permissions.ToAllowList();
                            var listtt2 = nowalista[0].Permissions.ToAllowList();
                            var minus = listtt1.Where(p => !listtt2.Contains(p)).ToList();
                            var plus = listtt2.Where(p => !listtt1.Contains(p)).ToList();

                            var stringdodany = "";
                            var stringusuniety = "";
                            var nazwacelu = "";
                            if (staralista[0].TargetType == PermissionTarget.Role)
                            {
                                nazwacelu = guild.GetRole(staralista[0].TargetId).Name;
                            } else if (staralista[0].TargetType == PermissionTarget.User)
                            {
                                nazwacelu = guild.GetUser(staralista[0].TargetId).Username;
                            }
                            foreach (var item1 in plus)
                            {
                                stringdodany += item1.ToString() + "\n";
                            }

                            foreach (var item2 in minus)
                            {
                                stringusuniety += item2.ToString() + "\n";
                            }
                            var embed = new EmbedBuilder()
                            .WithTitle($"Update roli {nazwacelu}")
                            .WithColor(Color.Green);
                            if (stringdodany != "")
                            {
                                embed.AddField("dodane: ", stringdodany, true);
                            }
                            if (stringusuniety != "")
                            {
                                embed.AddField("usuniete: ", stringusuniety, true);
                            }

                            await channel.SendMessageAsync(embed: embed.Build());

                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }
                return;
            });
            return Task.CompletedTask;
        }

        public Task RoleCreated(SocketRole role)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var Settings = await _db.GetGuildSettingsAsync(role.Guild);
                    if (Settings.logRoleCreated == 0) return;
                    ITextChannel channel = role.Guild.GetTextChannel(Settings.logRoleCreated);

                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithTitle($"✅ Role created.")
                             .WithColor(Color.Green)
                             .WithDescription($"New role has been created: {role.Name}.")
                             .WithCurrentTimestamp();

                    await channel.SendMessageAsync(embed: embed.Build());

                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }
                return;
            });
            return Task.CompletedTask;
        }

        public Task RoleUpdated(SocketRole role1, SocketRole role2)
        {

            var _ = Task.Run(async () =>
            {
                try
                {
                    var Settings = await _db.GetGuildSettingsAsync(role2.Guild);
                    if (Settings.logRoleUpdated == 0) return;
                    ITextChannel channel = role2.Guild.GetTextChannel(Settings.logRoleUpdated);

                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithTitle($"🗒 Role update.")
                             .WithDescription($"'{role1.Name}' has been updated")
                             .WithColor(Color.Green)
                             .WithCurrentTimestamp();

                    if (role1.Name != role2.Name)
                    {
                        embed.Description = $"Updated name.";
                        embed.AddField("Before: ", role1.Name, true)
                             .AddField("After: ", role2.Name, true);
                        await channel.SendMessageAsync("", false, embed.Build());
                        return;
                    }
                    else if (!role1.Permissions.ToList().SequenceEqual(role2.Permissions.ToList()))
                    {

                        var list1 = role1.Permissions.ToList();
                        var list2 = role2.Permissions.ToList();

                        //revoked
                        var revokedPermissions = list1.Where(r => !list2.Contains(r)).ToList();
                        //granted
                        var grantedPermissions = list2.Where(r => !list1.Contains(r)).ToList();

                        if (revokedPermissions.Count > 0)
                        {
                            StringBuilder revoked = new StringBuilder();
                            foreach (var item in revokedPermissions)
                            {
                                string[] split = Regex.Split(item.ToString(), @"(?<!^)(?=[A-Z])");
                                revoked.Append(string.Join(" ", split) + "\n");
                            }
                            embed.AddField("❌ Revoked permission:", revoked, true);
                        }
                        if (grantedPermissions.Count > 0)
                        {
                            StringBuilder granted = new StringBuilder();
                            foreach (var item in grantedPermissions)
                            {
                                string[] split = Regex.Split(item.ToString(), @"(?<!^)(?=[A-Z])");
                                granted.Append(string.Join(" ", split) + "\n");
                            }
                            embed.AddField("✅ Granted permission:", granted, true);

                        }

                        await channel.SendMessageAsync(embed: embed.Build());
                        return;



                    }

                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }
                return;
            });
            return Task.CompletedTask;
        }

        public Task RoleDeleted(SocketRole role)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var Settings = await _db.GetGuildSettingsAsync(role.Guild);
                    if (Settings.logRoleDeleted == 0) return;
                    ITextChannel channel = role.Guild.GetTextChannel(Settings.logRoleDeleted);

                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithTitle($"❌ Role deleted.")
                             .WithColor(Color.Red)
                             .WithDescription($"'{role.Name}' Role has been deleted.")
                             .WithCurrentTimestamp();

                    await channel.SendMessageAsync(embed: embed.Build());
                }
                catch (Exception ex)
                {
                    Log.Warn(ex);
                }
                return;
            });
            return Task.CompletedTask;
        }
    }
}
