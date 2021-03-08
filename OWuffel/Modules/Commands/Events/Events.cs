using Discord;
using Discord.Commands;
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

namespace OWuffel.Modules.Commands.Events
{
    public class Events : ModuleBase<Cipska>
    {
        private DatabaseUtilities _db { get; set; }

        public Events(DatabaseUtilities db)
        {
            _db = db;

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
                        await _db.SetSettingsValueAsync(Context.Guild.Id, found.Name, chnl.Id);
                        await ReplyAsync($"{found.Name.Replace("log", "")} related logs will be sent to {chnl.Mention}.");
                    }
                    else
                    {
                        await _db.SetSettingsValueAsync(Context.Guild.Id, found.Name, 0);
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
