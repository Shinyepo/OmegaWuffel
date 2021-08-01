using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Interactivity;
using Interactivity.Pagination;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OWuffel.Modules.Commands.Events
{
    public class Events : ModuleBase<Cipska>
    {
        private DatabaseUtilities _db { get; set; }
        public InteractivityService intserv { get; set; }


        public Events(DatabaseUtilities db, InteractivityService serv)
        {
            _db = db;
            intserv = serv;

        }

        [Command("Ignorelist", RunMode = RunMode.Async)]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task IgnoreList()
        {
            var ignoreVoice = Context.Settings.logIgnoreVoiceStateUpdated;
            var ignoreUpdate = Context.Settings.logIgnoreMessageEvents;
            var ignoreDelete = Context.Settings.logIgnoreMessageEvents;

            var deleteList = "";
            var updateList = "";
            var voiceList = "";

            if (ignoreDelete != null)
            {
                var split = ignoreDelete.Split(",");
                
                foreach (var item in split)
                {
                    var id = Convert.ToUInt64(item);
                    var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                    var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                    if (channel != null)
                    {
                        deleteList += "<#" + channel.Id + ">\n";
                    }
                    else if (user != null)
                    {
                        deleteList += user.Mention + "\n";
                    }
                }
            }
            if (ignoreUpdate != null)
            {
                var split = ignoreUpdate.Split(",");

                foreach (var item in split)
                {
                    var id = Convert.ToUInt64(item);
                    var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                    var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                    if (channel != null)
                    {
                        updateList += "<#" + channel.Id + ">\n";
                    }
                    else if (user != null)
                    {
                        updateList += user.Mention + "\n";
                    }
                }
            }
            if (ignoreVoice != null)
            {
                var split = ignoreVoice.Split(",");

                foreach (var item in split)
                {
                    var id = Convert.ToUInt64(item);
                    var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                    var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                    if (channel != null)
                    {
                        voiceList += "<#" + channel.Id + ">\n";
                    }
                    else if (user != null)
                    {
                        voiceList += user.Mention + "\n";
                    }
                }
            }

            var pages = new PageBuilder[3];
            if (voiceList == "") voiceList = "There are no channels or users ignored for this event.";
            if (deleteList == "") deleteList = "There are no channels or users ignored for this event.";
            if (updateList == "") updateList = "There are no channels or users ignored for this event.";
            pages[0] = new PageBuilder().WithTitle("Log ignore list")
                .WithColor(Color.Blue)
                .WithDescription("List of ignored channels and user for VoicePresence logs.\n\n"+ voiceList);
            pages[1] = new PageBuilder().WithTitle("Log ignore list")
                .WithColor(Color.Blue)
                .WithDescription("List of ignored channels and user for MessageDeleted logs.\n\n"+ deleteList);
            pages[2] = new PageBuilder().WithTitle("Log ignore list")
                .WithColor(Color.Blue)
                .WithDescription("List of ignored channels and user for MessageUpdated logs.\n\n"+ updateList);



            var paginator = new StaticPaginatorBuilder()
                            .WithPages(pages)
                            .WithFooter(PaginatorFooter.PageNumber)
                            .WithDefaultEmotes()
                            .Build();

            await intserv.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(10));
            return;
        }

        [Command("Unignore")]
        [Alias("UnIgnoreLogs", "UnIgnoreChannel", "LogsUnIgnore")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task UnIgnoreLogs(string log = "", [Remainder] string values = "")
        {
            try
            {
                EmbedBuilder em = new EmbedBuilder()
                    .WithCurrentTimestamp();
                var EventNames = new List<string> { "messageevents", "messagedeleted", "messageupdated", "voicepresence" };
                if (log == "" || values == "" || !EventNames.Contains(log.ToLower()))
                {
                    em.WithColor(Color.Red)
                        .WithTitle("Log unignore help.")
                        .WithDescription($"**Wrong syntax**\nTo remove channels or users from the ignore list please use\n `{Context.Settings.botPrefix}Unignore EventName #Channel #AnotherChannel @user`\nor\n`{Context.Settings.botPrefix}Unignore EventName all` - to cleare the ignore list for event.\n\nAvailable **EventName**'s:\n MessageEvents, MessageDeleted, MessageUpdated, VoicePresence");

                    await ReplyAsync(embed: em.Build());
                    return;
                }
                var toignore = new List<string>();
                var channellist = new List<SocketGuildChannel>();
                var userlist = new List<SocketGuildUser>();
                var wronglist = new List<string>();
                if (values.ToLower() != "all")
                {
                    var SplitValues = values.Split(" ");

                    foreach (var item in SplitValues)
                    {
                        var regex = new Regex(@"\d+");
                        var result = regex.Match(item).Value;
                        if (result != "")
                        {
                            var id = Convert.ToUInt64(result);
                            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                            var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                            if (channel == null && user == null)
                            {
                                wronglist.Add(item);
                            }
                            else if (channel != null)
                            {
                                channellist.Add(channel);
                                toignore.Add(result);
                            }
                            else if (user != null)
                            {
                                userlist.Add(user);
                                toignore.Add(result);
                            }

                        }
                        else
                        {
                            wronglist.Add(item);
                        }
                    }
                }
                if (log.ToLower() == "voicepresence")
                {
                    if (toignore.Count == 0 && values.ToLower() == "all")
                    {
                        await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreVoiceStateUpdated", null);
                        em.WithTitle("Log unignore success")
                            .WithColor(Color.Green)
                            .WithDescription("Successfuly removed all entries from the ignore list for VoicePresence event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var fromcontext = Context.Settings.logIgnoreVoiceStateUpdated == null ? "" : Context.Settings.logIgnoreVoiceStateUpdated;
                    var ignoredvoice = fromcontext == "" ? new List<string>() : fromcontext.Split(",").ToList();
                    int removed = 0;
                    var cantremove = new List<string>();
                    foreach (var item in toignore)
                    {
                        if (ignoredvoice.Contains(item))
                        {
                            ignoredvoice.Remove(item);
                            removed++;
                        }
                        else
                        {
                            var id = Convert.ToUInt64(item);
                            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                            var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                            if (user != null)
                            {
                                cantremove.Add(user.Mention);
                            }
                            else if (channel != null)
                            {
                                cantremove.Add(channel.Name);

                            }
                        }
                    }
                    if (removed == 0)
                    {
                        em.WithTitle("Log unignore error.")
                        .WithColor(Color.Red)
                        .WithDescription($"The arguments you provided are not valid or are not on the ignore list for this event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var todb = String.Join(",", ignoredvoice);
                    if (todb == "") todb = null;
                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreVoiceStateUpdated", todb);
                    var toembed = "";
                    if (userlist.Count > 0) toembed = toembed + String.Join(", ", userlist);
                    if (channellist.Count > 0) toembed = toembed + " " + String.Join(", ", channellist);
                    em.WithTitle("Log unignore success.")
                        .WithColor(Color.Green)
                        .WithDescription($"Successfuly removed **{toembed}** from the VoicePresence ignore list.");
                    if (cantremove.Count > 0)
                    {
                        var quickerstring = String.Join(", ", cantremove);
                        em.Description += "\n\n\nThese arguments are not on the ignore list for this event: **" + quickerstring + "**";
                    }
                    await ReplyAsync(embed: em.Build());
                }
                else if (log.ToLower() == "messageupdated")
                {
                    if (toignore.Count == 0 && values.ToLower() == "all")
                    {
                        await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageUpdated", null);
                        em.WithTitle("Log unignore success")
                            .WithColor(Color.Green)
                            .WithDescription("Successfuly removed all entries from the ignore list for MessageUpdated event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var fromcontext = Context.Settings.logIgnoreMessageEvents == null ? "" : Context.Settings.logIgnoreMessageEvents;
                    var ignoredupdate = fromcontext == "" ? new List<string>() : fromcontext.Split(",").ToList();
                    int removed = 0;
                    var cantremove = new List<string>();
                    foreach (var item in toignore)
                    {
                        if (ignoredupdate.Contains(item))
                        {
                            ignoredupdate.Remove(item);
                            removed++;
                        }
                        else
                        {
                            var id = Convert.ToUInt64(item);
                            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                            var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                            if (user != null)
                            {
                                cantremove.Add(user.Mention);
                            }
                            else if (channel != null)
                            {
                                cantremove.Add(channel.Name);

                            }
                        }
                    }
                    if (removed == 0)
                    {
                        em.WithTitle("Log unignore error.")
                        .WithColor(Color.Red)
                        .WithDescription($"The arguments you provided are not valid or are not on the ignore list for this event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var todb = String.Join(",", ignoredupdate);
                    if (todb == "") todb = null;
                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageUpdated", todb);
                    var toembed = "";
                    if (userlist.Count > 0) toembed = toembed + String.Join(", ", userlist);
                    if (channellist.Count > 0) toembed = toembed + " " + String.Join(", ", channellist);
                    em.WithTitle("Log unignore success.")
                        .WithColor(Color.Green)
                        .WithDescription($"Successfuly removed **{toembed}** from the MessageUpdated ignore list.");
                    if (cantremove.Count > 0)
                    {
                        var quickerstring = String.Join(", ", cantremove);
                        em.Description += "\n\n\nThese arguments are not on the ignore list for this event: **" + quickerstring + "**";
                    }
                    await ReplyAsync(embed: em.Build());
                }
                else if (log.ToLower() == "messagedeleted")
                {
                    if (toignore.Count == 0 && values.ToLower() == "all")
                    {
                        await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageDeleted", null);
                        em.WithTitle("Log unignore success")
                            .WithColor(Color.Green)
                            .WithDescription("Successfuly removed all entries from the ignore list for MessageDeleted event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var fromcontext = Context.Settings.logIgnoreMessageEvents == null ? "" : Context.Settings.logIgnoreMessageEvents;
                    var ignoreddelete = fromcontext == "" ? new List<string>() : fromcontext.Split(",").ToList();
                    int removed = 0;
                    var cantremove = new List<string>();
                    foreach (var item in toignore)
                    {
                        if (ignoreddelete.Contains(item))
                        {
                            ignoreddelete.Remove(item);
                            removed++;
                        }
                        else
                        {
                            var id = Convert.ToUInt64(item);
                            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                            var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                            if (user != null)
                            {
                                cantremove.Add(user.Mention);
                            }
                            else if (channel != null)
                            {
                                cantremove.Add(channel.Name);

                            }
                        }
                    }
                    if (removed == 0)
                    {
                        em.WithTitle("Log unignore error.")
                        .WithColor(Color.Red)
                        .WithDescription($"The arguments you provided are not valid or are not on the ignore list for this event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var todb = String.Join(",", ignoreddelete);
                    if (todb == "") todb = null;
                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageDeleted", todb);
                    var toembed = "";
                    if (userlist.Count > 0) toembed = toembed + String.Join(", ", userlist);
                    if (channellist.Count > 0) toembed = toembed + " " + String.Join(", ", channellist);
                    em.WithTitle("Log unignore success.")
                        .WithColor(Color.Green)
                        .WithDescription($"Successfuly removed **{toembed}** from the MessageDeleted ignore list.");
                    if (cantremove.Count > 0)
                    {
                        var quickerstring = String.Join(", ", cantremove);
                        em.Description += "\n\n\nThese arguments are not on the ignore list for this event: **" + quickerstring + "**";
                    }
                    await ReplyAsync(embed: em.Build());
                }
                else if (log.ToLower() == "messageevents")
                {
                    if (toignore.Count == 0 && values.ToLower() == "all")
                    {
                        await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageDeleted", null);
                        await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageUpdated", null);
                        em.WithTitle("Log unignore success")
                            .WithColor(Color.Green)
                            .WithDescription("Successfuly removed all entries from the ignore list for MessageEvents event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var fromcontext = Context.Settings.logIgnoreMessageEvents == null ? "" : Context.Settings.logIgnoreMessageEvents;
                    var ignoreddelete = fromcontext == "" ? new List<string>() : fromcontext.Split(",").ToList();
                    int removed = 0;
                    foreach (var item in toignore)
                    {
                        if (ignoreddelete.Contains(item))
                        {
                            ignoreddelete.Remove(item);
                            removed++;
                        }
                    }
                    var todbdelete = String.Join(",", ignoreddelete);

                    var fromcontextupdated = Context.Settings.logIgnoreMessageEvents == null ? "" : Context.Settings.logIgnoreMessageEvents;
                    var ignoredupdate = fromcontextupdated == "" ? new List<string>() : fromcontextupdated.Split(",").ToList();
                    foreach (var item in toignore)
                    {
                        if (ignoredupdate.Contains(item))
                        {
                            ignoredupdate.Remove(item);
                            removed++;
                        }
                    }
                    if (removed == 0)
                    {
                        em.WithTitle("Log unignore error.")
                        .WithColor(Color.Red)
                        .WithDescription($"The arguments you provided are not valid or are not on the ignore list for this event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var todb = String.Join(",", ignoreddelete);
                    if (todb == "") todb = null;
                    if (todbdelete == "") todbdelete = null;
                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageDeleted", todbdelete);
                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageUpdated", todb);
                    var toembed = "";
                    if (userlist.Count > 0) toembed = toembed + String.Join(", ", userlist);
                    if (channellist.Count > 0) toembed = toembed + " " + String.Join(", ", channellist);
                    em.WithTitle("Log unignore success.")
                        .WithColor(Color.Green)
                        .WithDescription($"Successfuly removed **{toembed}** from the MessageEvents ignore list.");
                    await ReplyAsync(embed: em.Build());
                }
                if (wronglist.Count > 0)
                {
                    var quickstring = String.Join(", ", wronglist);
                    em.WithTitle("Log unignore error")
                        .WithColor(Color.Red)
                        .WithDescription($"I didnt recognize these arguments as a channel or user: **{quickstring}**");
                    await ReplyAsync(embed: em.Build());
                }
                return;

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return;
            }
        }

        [Command("Ignore")]
        [Alias("IgnoreLogs", "IgnoreChannel", "LogsIgnore")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task IgnoreLogs(string log = "", [Remainder] string values = "")
        {
            try
            {
                EmbedBuilder em = new EmbedBuilder()
                    .WithCurrentTimestamp();
                var EventNames = new List<string> { "messageevents", "messagedeleted", "messageupdated", "voicepresence" };
                if (log == "" || values == "" || !EventNames.Contains(log.ToLower()))
                {
                    em.WithColor(Color.Red)
                        .WithTitle("Log ignore help.")
                        .WithDescription($"**Wrong syntax**\nTo exclude certain channels or users from creating logs please use\n `{Context.Settings.botPrefix}Ignore EventName #Channel #AnotherChannel @user`\nAvailable **EventName**'s:\n\n MessageEvents, MessageDeleted, MessageUpdated, VoicePresence");

                    await ReplyAsync(embed: em.Build());
                    return;
                }
                var SplitValues = values.Split(" ");
                var toignore = new List<string>();
                var channellist = new List<SocketGuildChannel>();
                var userlist = new List<SocketGuildUser>();
                var wronglist = new List<string>();
                foreach (var item in SplitValues)
                {
                    var regex = new Regex(@"\d+");
                    var result = regex.Match(item).Value;
                    if (result != "")
                    {
                        var id = Convert.ToUInt64(result);
                        var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                        var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                        if (channel == null && user == null)
                        {
                            wronglist.Add(item);
                        }
                        else if (channel != null)
                        {
                            channellist.Add(channel);
                            toignore.Add(result);
                        }
                        else if (user != null)
                        {
                            userlist.Add(user);
                            toignore.Add(result);
                        }

                    }
                    else
                    {
                        wronglist.Add(item);
                    }
                }

                if (log.ToLower() == "voicepresence")
                {
                    var fromcontext = Context.Settings.logIgnoreVoiceStateUpdated == null ? "" : Context.Settings.logIgnoreVoiceStateUpdated;
                    var ignoredvoice = fromcontext == "" ? new List<string>() : fromcontext.Split(",").ToList();
                    int added = 0;
                    var duplicates = new List<string>();
                    foreach (var item in toignore)
                    {
                        if (!ignoredvoice.Contains(item))
                        {
                            ignoredvoice.Add(item);
                            added++;
                        }
                        else
                        {
                            var id = Convert.ToUInt64(item);
                            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                            var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                            if (user != null)
                            {
                                userlist.Remove(user);
                                duplicates.Add(user.Mention);
                            }
                            else if (channel != null)
                            {
                                channellist.Remove(channel);
                                duplicates.Add(channel.Name);

                            }
                        }
                    }
                    if (added == 0)
                    {
                        em.WithTitle("Log ignore error.")
                        .WithColor(Color.Red)
                        .WithDescription($"The arguments you provided are not valid or are already on the ignore list for this event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var todb = String.Join(",", ignoredvoice);
                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreVoiceStateUpdated", todb);
                    var toembed = "";
                    if (userlist.Count > 0) toembed = toembed + String.Join(", ", userlist);
                    if (channellist.Count > 0) toembed = toembed + " " + String.Join(", ", channellist);
                    em.WithTitle("Log ignore success.")
                        .WithColor(Color.Green)
                        .WithDescription($"Successfuly added **{toembed}**  to the VoicePresence ignore list.");
                    if (duplicates.Count > 0)
                    {
                        var quickerstring = String.Join(", ", duplicates);
                        em.Description += "\n\n\nThese arguments are already on the ignore list for this event: " + quickerstring;
                    }
                    await ReplyAsync(embed: em.Build());
                }
                else if (log.ToLower() == "messageupdated")
                {
                    var fromcontext = Context.Settings.logIgnoreMessageEvents == null ? "" : Context.Settings.logIgnoreMessageEvents;
                    var ignoredmessageupdate = fromcontext == "" ? new List<string>() : fromcontext.Split(",").ToList();
                    int added = 0;
                    var duplicates = new List<string>();
                    foreach (var item in toignore)
                    {
                        if (!ignoredmessageupdate.Contains(item))
                        {
                            ignoredmessageupdate.Add(item);
                            added++;
                        }
                        else
                        {
                            var id = Convert.ToUInt64(item);
                            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                            var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                            if (user != null)
                            {
                                userlist.Remove(user);
                                duplicates.Add(user.Mention);
                            }
                            else if (channel != null)
                            {
                                channellist.Remove(channel);
                                duplicates.Add(channel.Name);

                            }
                        }
                    }
                    if (added == 0)
                    {
                        em.WithTitle("Log ignore error.")
                        .WithColor(Color.Red)
                        .WithDescription($"The arguments you provided are not valid or are already on the ignore list for this event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var todb = String.Join(",", ignoredmessageupdate);
                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageUpdated", todb);
                    var toembed = "";
                    if (userlist.Count > 0) toembed = toembed + String.Join(", ", userlist);
                    if (channellist.Count > 0) toembed = toembed + " " + String.Join(", ", channellist);
                    em.WithTitle("Log ignore success.")
                        .WithColor(Color.Green)
                        .WithDescription($"Successfuly added **{toembed}**  to the MessageUpdated ignore list.");
                    if (duplicates.Count > 0)
                    {
                        var quickerstring = String.Join(", ", duplicates);
                        em.Description += "\n\n\nThese arguments are already on the ignore list for this event: " + quickerstring;
                    }
                    await ReplyAsync(embed: em.Build());
                }
                else if (log.ToLower() == "messagedeleted")
                {
                    var fromcontext = Context.Settings.logIgnoreMessageEvents == null ? "" : Context.Settings.logIgnoreMessageEvents;
                    var ignoredmessagedeleted = fromcontext == "" ? new List<string>() : fromcontext.Split(",").ToList();
                    int added = 0;
                    var duplicates = new List<string>();
                    foreach (var item in toignore)
                    {
                        if (!ignoredmessagedeleted.Contains(item))
                        {
                            ignoredmessagedeleted.Add(item);
                            added++;
                        }
                        else
                        {
                            var id = Convert.ToUInt64(item);
                            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                            var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                            if (user != null)
                            {
                                userlist.Remove(user);
                                duplicates.Add(user.Mention);
                            }
                            else if (channel != null)
                            {
                                channellist.Remove(channel);
                                duplicates.Add(channel.Name);

                            }
                        }
                    }
                    if (added == 0)
                    {
                        em.WithTitle("Log ignore error.")
                        .WithColor(Color.Red)
                        .WithDescription($"The arguments you provided are not valid or are already on the ignore list for this event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var todb = String.Join(",", ignoredmessagedeleted);
                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageDeleted", todb);
                    var toembed = "";
                    if (userlist.Count > 0) toembed = toembed + String.Join(", ", userlist);
                    if (channellist.Count > 0) toembed = toembed + " " + String.Join(", ", channellist);
                    em.WithTitle("Log ignore success.")
                        .WithColor(Color.Green)
                        .WithDescription($"Successfuly added **{toembed}**  to the MessageDeleted ignore list.");
                    if (duplicates.Count > 0)
                    {
                        var quickerstring = String.Join(", ", duplicates);
                        em.Description += "\n\n\nThese arguments are already on the ignore list for this event: " + quickerstring;
                    }
                    await ReplyAsync(embed: em.Build());
                }
                else if (log.ToLower() == "messageevents")
                {
                    var deletefromcontext = Context.Settings.logIgnoreMessageEvents == null ? "" : Context.Settings.logIgnoreMessageEvents;
                    var ignoredmessagedeleted = deletefromcontext == "" ? new List<string>() : deletefromcontext.Split(",").ToList();
                    int added = 0;
                    var duplicates = new List<string>();
                    foreach (var item in toignore)
                    {
                        if (!ignoredmessagedeleted.Contains(item))
                        {
                            ignoredmessagedeleted.Add(item);
                            added++;
                        }
                        else
                        {
                            var id = Convert.ToUInt64(item);
                            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                            var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                            if (user != null)
                            {
                                userlist.Remove(user);
                                duplicates.Add(user.Mention);
                            }
                            else if (channel != null)
                            {
                                channellist.Remove(channel);
                                duplicates.Add(channel.Name);

                            }
                        }
                    }
                    var todbdelete = String.Join(",", ignoredmessagedeleted);

                    var updatefromcontext = Context.Settings.logIgnoreMessageEvents == null ? "" : Context.Settings.logIgnoreMessageEvents;
                    var ignoredmessageupdate = updatefromcontext == "" ? new List<string>() : updatefromcontext.Split(",").ToList();
                    foreach (var item in toignore)
                    {
                        if (!ignoredmessageupdate.Contains(item))
                        {
                            ignoredmessageupdate.Add(item);
                            added++;
                        }
                        else
                        {
                            var id = Convert.ToUInt64(item);
                            var channel = Context.Guild.Channels.FirstOrDefault(c => c.Id == id);
                            var user = Context.Guild.Users.FirstOrDefault(c => c.Id == id);
                            if (user != null)
                            {
                                userlist.Remove(user);
                                duplicates.Add(user.Mention);
                            }
                            else if (channel != null)
                            {
                                channellist.Remove(channel);
                                duplicates.Add(channel.Name);

                            }
                        }
                    }
                    if (added == 0)
                    {
                        em.WithTitle("Log ignore error.")
                        .WithColor(Color.Red)
                        .WithDescription($"The arguments you provided are not valid or are already on the ignore list for this event.");
                        await ReplyAsync(embed: em.Build());
                        return;
                    }
                    var todbupdate = String.Join(",", ignoredmessageupdate);

                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageUpdated", todbupdate);
                    await _db.SetSettingsValueAsync(Context.Guild, "logIgnoreMessageDeleted", todbdelete);
                    var toembed = "";
                    if (userlist.Count > 0) toembed = toembed + String.Join(", ", userlist);
                    if (channellist.Count > 0) toembed = toembed + " " + String.Join(", ", channellist);
                    em.WithTitle("Log ignore success.")
                        .WithColor(Color.Green)
                        .WithDescription($"Successfuly added **{toembed}**  to the MessageEvents ignore list.");
                    if (duplicates.Count > 0)
                    {
                        var quickerstring = String.Join(", ", duplicates);
                        em.Description += "\n\n\nThese arguments are already on the ignore list for this event: " + quickerstring;
                    }
                    await ReplyAsync(embed: em.Build());
                }
                if (wronglist.Count > 0)
                {
                    var quickstring = String.Join(", ", wronglist);
                    em.WithTitle("Log ignore error")
                        .WithColor(Color.Red)
                        .WithDescription($"I didnt recognize these arguments as a channel or user: **{quickstring}**");
                    await ReplyAsync(embed: em.Build());
                }
                return;

            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return;
            }
        }



        [Command("Logs")]
        [Alias("events")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task LogsAsync(string key = "", string value = "")
        {
            try
            {
                var setting = Context.Settings
                        .GetType()
                        .GetProperties()
                        .Where(content => content.Name.Contains("log"))
                        .Where(content => !content.Name.Contains("Ignore"))
                        .Select(pi => new
                        {
                            Name = pi.Name,
                            Value = pi.GetGetMethod().Invoke(Context.Settings, null)
                        });
                if (key == "")
                {
                    EmbedBuilder embed = new EmbedBuilder()
                        .WithTitle("Available events: ")
                        .WithDescription("")
                        .WithColor(Color.Blue)
                        .WithCurrentTimestamp();

                    foreach (var item in setting)
                    {
                        var toembed = item.Value.ToString() == "0" ? "" : $"<#{item.Value}>";
                        embed.Description += string.Format("{0}: {1}\n", item.Name.Replace("log", ""), toembed);
                    }
                    await ReplyAsync(embed: embed.Build());
                }
                else
                {
                    var properkey = key.Insert(0, "log");

                    var found = setting.FirstOrDefault(s => s.Name.ToLower() == properkey.ToLower());

                    if (found == null)
                    {
                        await ReplyAsync("There is no such event");
                        return;
                    }
                    if (value != "")
                    {
                        var reg = new Regex("\\A<#\\w+>");
                        var foundregex = reg.IsMatch(value);
                        var cutchannel = foundregex ? value.Replace("<#", "").Replace(">", "") : value;
                        ulong channelid = Convert.ToUInt64(cutchannel);
                        ITextChannel chnl = Context.Guild.GetTextChannel(channelid);

                        if (chnl == null)
                        {
                            await ReplyAsync("Provided value is not a existing channel");
                            return;
                        }
                        await _db.SetSettingsValueAsync(Context.Guild, found.Name, chnl.Id);
                        await ReplyAsync($"{found.Name.Replace("log", "")} related logs will be sent to {chnl.Mention}.");
                    }
                    else
                    {
                        await _db.SetSettingsValueAsync(Context.Guild, found.Name, 0);
                        await ReplyAsync($"Disabled logs for {found.Name.Replace("log", "")}.");
                    }






                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
