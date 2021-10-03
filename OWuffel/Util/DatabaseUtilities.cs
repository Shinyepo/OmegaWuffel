using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using OWuffel.Extensions;
using OWuffel.Extensions.Database;
using OWuffel.Models;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Util
{
    public class DatabaseUtilities
    {
        private readonly WuffelDBContext _db;
        private BotSettings Settings;
        private Suggestions Suggestions;

        public DatabaseUtilities(WuffelDBContext db)
        {
            _db = db;
        }

        public async Task<BotSettings> GetGuildSettingsAsync(SocketGuild guild)
        {
            try
            {
                using (var context = new WuffelDBContext())
                {
                    Settings = await context.BotSettings.Include(x=>x.GuildInformation).SingleOrDefaultAsync(s => s.GuildId == guild.Id);
                    Log.Info(guild.Name+"("+guild.Id+") " + context.ContextId.ToString());

                    if (Settings == null)
                    {
                        Settings = await SetDefaultSettingsAsync(guild);
                    }
                    await context.DisposeAsync();
                    return Settings;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new BotSettings();
            }

        }

        public async Task<LogsConfig> GetLogsConfigAsync(ulong guildId)
        {
            try
            {
                using (var context = new WuffelDBContext())
                {
                    var result = await context.LogsConfigs.AsQueryable().SingleOrDefaultAsync(s => s.GuildId == guildId);
                    await context.DisposeAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return null;
            }
        }
        public async Task<BotSettings> SetDefaultSettingsAsync(SocketGuild guild)
        {
            try
            {
                using (var context = new WuffelDBContext())
                {
                    var GuildInformation = new GuildInformation();
                    GuildInformation.GuildName = guild.Name;
                    GuildInformation.GuildAvatar = guild.IconUrl;
                    GuildInformation.GuildOwnerId = guild.Owner.Id;
                    GuildInformation.GuildOwnerName = guild.Owner.Username + "#" + guild.Owner.Discriminator;
                    GuildInformation.GuildId = guild.Id;
                    await context.GuildInformations.AddAsync(GuildInformation);                    
                    await context.SaveChangesAsync();
                    var Setting = new BotSettings();
                    Setting.GuildId = guild.Id;
                    Setting.BotPrefix = "+";
                    Setting.BotDisabledCommands = "[{}]";
                    Setting.BotActive = true;
                    await context.BotSettings.AddAsync(Setting);
                    var dailyRankingConfig = new DailyRankingConfig();
                    dailyRankingConfig.GuildId = guild.Id;
                    await context.DailyRankingConfigs.AddAsync(dailyRankingConfig);
                    var logsConfig = new LogsConfig();
                    logsConfig.GuildId = guild.Id;
                    await context.LogsConfigs.AddAsync(logsConfig);
                    var reactionsConfig = new ReactionsConfig();
                    reactionsConfig.GuildId = guild.Id;
                    await context.ReactionsConfigs.AddAsync(reactionsConfig);
                    var suggestionsConfig = new SuggestionsConfig();
                    suggestionsConfig.GuildId = guild.Id;
                    await context.SuggestionsConfigs.AddAsync(suggestionsConfig);
                    var supportConfig = new SupportConfig();
                    supportConfig.GuildId = guild.Id;
                    await context.SupportConfigs.AddAsync(supportConfig);
                    var welcomeMessageConfig = new WelcomeMessageConfig();
                    welcomeMessageConfig.GuildId = guild.Id;
                    await context.WelcomeMessageConfigs.AddAsync(welcomeMessageConfig);

                    await context.SaveChangesAsync();
                    await context.DisposeAsync();
                    return Setting;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            return null;
        }

        public async Task<IAsyncResult> SetSettingsValueAsync(SocketGuild guild, string key, ulong value)
        {
            using (var context = new WuffelDBContext())
            {
                Settings = await context.BotSettings.AsQueryable().SingleOrDefaultAsync(s => s.GuildId == guild.Id);

                if (Settings == null)
                {
                    Settings = await SetDefaultSettingsAsync(guild);
                }
                Settings.GetType().GetProperty(key).SetValue(Settings, value);
                await context.SaveChangesAsync();

                await context.DisposeAsync();
                return Task.CompletedTask;
            }
        }
        public async Task<IAsyncResult> SetSettingsValueAsync(SocketGuild guild, string key, string value)
        {
            using (var context = new WuffelDBContext())
            {
                Settings = await context.BotSettings.AsQueryable().SingleOrDefaultAsync(s => s.GuildId == guild.Id);

                if (Settings == null)
                {
                    Settings = await SetDefaultSettingsAsync(guild);
                }
                Settings.GetType().GetProperty(key).SetValue(Settings, value);
                await context.SaveChangesAsync();

                await context.DisposeAsync();
                return Task.CompletedTask;
            }
        }



        //------------------------------------------> SUGGESTIONS <------------------------------------------//
        //0 = closed
        //1 = active
        //2 = not happening
        //3 = maybe
        public async Task<Suggestions> GetSuggestionAsync(ulong guild, int id)
        {
            using (var context = new WuffelDBContext())
            {
                Suggestions = await context.Suggestions.AsQueryable().SingleOrDefaultAsync(s => s.Id == id);
                if (Suggestions == null) return new Suggestions();
                if (Suggestions.GuildId != guild) return new Suggestions();
                await context.DisposeAsync();
                return Suggestions;
            }
        }

        public async Task<Suggestions> CreateSuggestionAsync(Suggestions suggestion)
        {
            using (var context = new WuffelDBContext())
            {
                await context.Suggestions.AddAsync(suggestion);
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return suggestion;
            }
        }

        public async Task<Suggestions> AddCommentAsync(ulong guild, int id, string comment)
        {
            using (var context = new WuffelDBContext())
            {
                Suggestions = await context.Suggestions.AsQueryable().SingleOrDefaultAsync(s => s.Id == id);
                if (Suggestions == null) return new Suggestions();
                if (Suggestions.GuildId != guild) return new Suggestions();
                Suggestions.Comment = comment;
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return Suggestions;
            }
        }
        public async Task<Suggestions> SetSuggestionMessageId(ulong guild, int id, IUserMessage statchnlus)
        {
            using (var context = new WuffelDBContext())
            {
                var a = context.Suggestions.Count();
                Suggestions = await context.Suggestions.AsQueryable().FirstOrDefaultAsync(s => s.Id == id);
                if (Suggestions == null) return new Suggestions();
                if (Suggestions.GuildId != guild) return new Suggestions();
                Suggestions.MessageId = statchnlus.Id;
                Suggestions.ChannelId = statchnlus.Channel.Id;
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return Suggestions;
            }
        }
        public async Task<Suggestions> ChangeStatusAsync(ulong guild, int id, int status)
        {
            using (var context = new WuffelDBContext())
            {
                Suggestions = await context.Suggestions.AsQueryable().SingleOrDefaultAsync(s => s.Id == id && s.GuildId == guild);
                if (Suggestions == null) return new Suggestions();
                Suggestions.Status = status;
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return Suggestions;
            }
        }
        public async Task<Suggestions> ChangeVoteAsync(Suggestions suggestion, int vote, string type)
        {
            using (var context = new WuffelDBContext())
            {
                Suggestions = await context.Suggestions.AsQueryable().SingleOrDefaultAsync(s => s.Id == suggestion.Id);
                if (Suggestions == null) return new Suggestions();
                switch (type)
                {
                    case "like":
                        Suggestions.VoteLike += vote;
                        if (Suggestions.VoteLike < 0) Suggestions.VoteLike = 0;
                        break;
                    case "dislike":
                        Suggestions.VoteDislike += vote;
                        if (Suggestions.VoteDislike < 0) Suggestions.VoteDislike = 0;
                        break;
                    default:
                        break;
                }
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return Suggestions;
            }
        }
        public async Task<bool> DeleteSuggestionAsync(ulong guild, int id)
        {
            using (var context = new WuffelDBContext())
            {
                Suggestions = await context.Suggestions.AsQueryable().SingleOrDefaultAsync(s => s.Id == id);
                if (Suggestions == null) return false;
                if (Suggestions.GuildId != guild) return false;
                context.Remove(Suggestions);
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return true;
            }
        }

        public async Task<Suggestions> LoopSuggestions(ulong guild, ulong messageid)
        {
            using (var context = new WuffelDBContext())
            {
                var sw = Stopwatch.StartNew();
                Console.WriteLine(sw + "loop start");
                Suggestions = await context.Suggestions.AsQueryable().SingleOrDefaultAsync(s => s.GuildId == guild && s.MessageId == messageid && s.Status == 1);
                await context.DisposeAsync();
                sw.Stop();
                Log.Info($"Connected in {sw.Elapsed.TotalSeconds:F2}s passed loop");
                return Suggestions;
            }
        }
        
        //----------------------- SUPPORT MODULE ------------------------------//

        public async Task<SupportConfig> LookForConfigurationAsync(ulong guild_id)
        {
            using (var context = new WuffelDBContext())
            {
                var config = await context.SupportConfigs.AsQueryable().SingleOrDefaultAsync(c => c.GuildId == guild_id);
                await context.DisposeAsync();
                return config;

            }
        }

        public async Task<SupportConfig> CreateSupportConfigurationAsync(SupportConfig supportconfig)
        {
            using (var context = new WuffelDBContext())
            {
                var isit = await LookForConfigurationAsync(supportconfig.GuildId);
                if (isit != null)
                {
                    context.Remove(isit);
                }
                await context.SupportConfigs.AddAsync(supportconfig);
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return supportconfig;
            }
        }
        public async Task<SupportConfig> ChangeTicketMessageAsync(ulong guild_id, string message)
        {
            using (var context = new WuffelDBContext())
            {
                var config = await context.SupportConfigs.AsQueryable().SingleOrDefaultAsync(c => c.GuildId == guild_id);
                if (config == null)
                {
                    return new SupportConfig();
                }
                config.TicketMessage = message;
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return config;
            }
        }
        public async Task<int> NumberOfActiveTickets(ulong guild_id)
        {
            using (var context = new WuffelDBContext())
            {
                var number = context.Tickets.Count(t => t.GuildId == guild_id && t.Status == 1);
                await context.DisposeAsync();
                return number;
            }
        }
        public async Task<List<int>> TicketStatsAsync(ulong guild_id)
        {
            using (var context = new WuffelDBContext())
            {
                var active = context.Tickets.Count(t => t.GuildId == guild_id && t.Status == 1);
                var closed = context.Tickets.Count(t => t.GuildId == guild_id && t.Status == 0);
                var total = context.Tickets.Count(t => t.GuildId == guild_id);
                var list = new List<int>();
                list.Add(active);
                list.Add(closed);
                list.Add(total);
                await context.DisposeAsync();
                return list;
            }
        }
        public async Task<Tickets> CreateNewTicket(Tickets ticket)
        {
            using (var context = new WuffelDBContext())
            {
                var activeticket = context.Tickets.AsQueryable().Where(t => t.UserId == ticket.UserId && t.GuildId == ticket.GuildId).ToList();
                int activecount = 0;
                foreach (var item in activeticket)
                {
                    if (item.Status == 1)
                    {
                        activecount++;
                    }
                }
                if (activecount > 2)
                {
                    return new Tickets();
                }
                context.Add(ticket);
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return ticket;
            }
        }

        //---------------------------------------Daily Rankings----------------------------------------//
        public async Task<DailyRankingConfig> CreateDailyRankingConfigAsync(DailyRankingConfig daily)
        {
            using (var context = new WuffelDBContext())
            {
                var existing = await context.DailyRankingConfigs.AsQueryable().SingleOrDefaultAsync(d => d.GuildId == daily.GuildId);
                if (existing != null)
                {
                    context.Remove(existing);
                }
                await context.DailyRankingConfigs.AddAsync(daily);
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return daily;
            }
        }
        public async Task<List<DailyRankingConfig>> GetAllDailyRankingsConfigruations()
        {
            using (var context = new WuffelDBContext())
            {
                var list = context.DailyRankingConfigs.ToList();
                await context.DisposeAsync();
                return list;
            }
        }
        public async Task<DailyRankingConfig> GetGuildDailyRankingConfigAsync(ulong guild_id)
        {
            using (var context = new WuffelDBContext())
            {
                var config = await _db.DailyRankingConfigs.AsQueryable().SingleOrDefaultAsync(c => c.GuildId == guild_id);
                await context.DisposeAsync();
                return config;
            }
        }

    }
}
