using Discord;
using Discord.WebSocket;
using OWuffel.Extensions.Database;
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
        private Settings Settings;
        private Suggestions Suggestions;

        public DatabaseUtilities(WuffelDBContext db)
        {
            _db = db;
        }

        public async Task<Settings> GetGuildSettingsAsync(ulong guild_id)
        {
            try
            {
                using (var context = new WuffelDBContext())
                {
                    Settings = await context.Settings.SingleOrDefaultAsync(s => s.guild_id == guild_id);
                    Log.Info(guild_id+" "+context.ContextId.ToString());

                    if (Settings == null)
                    {
                        Settings = await SetDefaultSettingsAsync(guild_id);
                    }
                    await context.DisposeAsync();
                    return Settings;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new Settings();
            }
            
        }

        

        internal Task CreateChannelCheck(ChannelCheckModel channelcheck)
        {
            throw new NotImplementedException();
        }

        public async Task<Settings> SetDefaultSettingsAsync(ulong guild_id)
        {
            try
            {
                using (var context = new WuffelDBContext())
                {
                    var Setting = new Settings();
                    Setting.guild_id = guild_id;
                    Setting.botPrefix = "+";
                    Setting.botDisabledCommands = "[{}]";
                    Setting.botActive = 1;
                    await context.Settings.AddAsync(Setting);
                    await context.SaveChangesAsync();
                    await context.DisposeAsync();
                    return Setting;
                }
            }
            catch
            {
                //dont care
            }
            return null;
        }

        public async Task<IAsyncResult> SetSettingsValueAsync(ulong guild, string key, ulong value)
        {
            using (var context = new WuffelDBContext())
            {
                Settings = await context.Settings.SingleOrDefaultAsync(s => s.guild_id == guild);

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
        public async Task<IAsyncResult> SetSettingsValueAsync(ulong guild, string key, string value)
        {
            using (var context = new WuffelDBContext())
            {
                Settings = await context.Settings.SingleOrDefaultAsync(s => s.guild_id == guild);

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
                Suggestions = await context.Suggestions.SingleOrDefaultAsync(s => s.Id == id);
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
                Suggestions = await context.Suggestions.SingleOrDefaultAsync(s => s.Id == id);
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
                Suggestions = await context.Suggestions.FirstOrDefaultAsync(s => s.Id == id);
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
                Suggestions = await context.Suggestions.SingleOrDefaultAsync(s => s.Id == id && s.GuildId == guild);
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
                Suggestions = await context.Suggestions.SingleOrDefaultAsync(s => s.Id == suggestion.Id);
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
                Suggestions = await context.Suggestions.SingleOrDefaultAsync(s => s.Id == id);
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
                Suggestions = await context.Suggestions.SingleOrDefaultAsync(s => s.GuildId == guild && s.MessageId == messageid && s.Status == 1);
                await context.DisposeAsync();
                sw.Stop();
                Log.Info($"Connected in {sw.Elapsed.TotalSeconds:F2}s passed loop");
                return Suggestions;
            }
        }



        //-----------------------CHANNEL CHECKS BDO ---------------------------//
        public async Task<ChannelCheckModel> CreateChannelCheckAsync(ChannelCheckModel channelcheck)
        {
            using (var context = new WuffelDBContext())
            {
                var embeds = context.ChannelChecks.AsQueryable().Where(f => f.GuildId == channelcheck.GuildId).ToList();
                embeds.ForEach(a => a.Status = 0);
                await context.SaveChangesAsync();
                await context.ChannelChecks.AddAsync(channelcheck);
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return channelcheck;
            }
        }

        public async Task<ChannelCheckModel> ChannelCheckUpdateMessageIdAsync(ChannelCheckModel channelcheck)
        {
            using (var context = new WuffelDBContext())
            {                
                var ChannelCheck = await context.ChannelChecks.SingleOrDefaultAsync(ch => ch.Id == channelcheck.Id && ch.Status == 1);
                ChannelCheck.MessageId = channelcheck.MessageId;
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return ChannelCheck;
            }
        }

        public async Task<ChannelCheckModel> GetChannelCheckAsync(int id)
        {
            using (var context = new WuffelDBContext())
            {
                var ChannelCheck = await context.ChannelChecks.SingleOrDefaultAsync(ch => ch.Id == id && ch.Status == 1);
                await context.DisposeAsync();
                return ChannelCheck;
            }
        }
        public async Task<ChannelCheckModel> UpdateCheckChannelsAsync(ChannelCheckModel channelcheck)
        {
            using (var context = new WuffelDBContext())
            {
                var ChannelCheck = await context.ChannelChecks.SingleOrDefaultAsync(ch => ch.Id == channelcheck.Id && ch.Status == 1);
                if (ChannelCheck == null) return ChannelCheck;
                ChannelCheck.Channels = channelcheck.Channels;
                ChannelCheck.Status = channelcheck.Status;
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return ChannelCheck;
            }
        }

        public async Task<ChannelCheckModel> GetLastEmbedAsync(ulong guild_id)
        {
            using (var context = new WuffelDBContext())
            {
                try
                {
                    var channelcheck = await context.ChannelChecks.LastAsync(c => c.GuildId == guild_id && c.Status == 1);
                    await context.DisposeAsync();
                    if (channelcheck == null) return new ChannelCheckModel();
                    return channelcheck;
                }
                catch (Exception)
                {
                    return new ChannelCheckModel();
                }
                
            }
        }


        //-----------------------CHANNEL CHECKS BDO ---------------------------//
        //----------------------- SUPPORT MODULE ------------------------------//

        public async Task<SupportConfiguration> LookForConfigurationAsync(ulong guild_id)
        {
            using (var context = new WuffelDBContext())
            {
                var config = await context.SupportConfiguration.SingleOrDefaultAsync(c => c.GuildId == guild_id);
                await context.DisposeAsync();
                return config;

            }
        }

        public async Task<SupportConfiguration> CreateSupportConfigurationAsync(SupportConfiguration supportconfig)
        {
            using (var context = new WuffelDBContext())
            {
                var isit = await LookForConfigurationAsync(supportconfig.GuildId);
                if (isit != null)
                {
                    context.Remove(isit);
                }
                await context.SupportConfiguration.AddAsync(supportconfig);
                await context.SaveChangesAsync();
                await context.DisposeAsync();
                return supportconfig;
            }
        }
        public async Task<SupportConfiguration> ChangeTicketMessageAsync(ulong guild_id, string message)
        {
            using (var context = new WuffelDBContext())
            {
                var config = await context.SupportConfiguration.SingleOrDefaultAsync(c => c.GuildId == guild_id);
                if (config == null)
                {
                    return new SupportConfiguration();
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
        public async Task<DailyRanking> CreateDailyRankingConfigAsync(DailyRanking daily)
        {
            using (var context = new WuffelDBContext())
            {
                var existing = await context.DailyRankings.SingleOrDefaultAsync(d => d.GuildId == daily.GuildId);
                if (existing != null)
                {
                    existing.ChannelId = daily.ChannelId;
                    await context.SaveChangesAsync();
                    await context.DisposeAsync();
                    return existing;
                } else
                {
                    await context.DailyRankings.AddAsync(daily);
                    await context.SaveChangesAsync();
                    await context.DisposeAsync();
                    return daily;
                }
            }
        }
        public async Task<List<DailyRanking>> GetAllDailyRankingsConfigruations()
        {
            using (var context = new WuffelDBContext())
            {
                var list = context.DailyRankings.ToList();
                return list;
            }
        }

    }
}
