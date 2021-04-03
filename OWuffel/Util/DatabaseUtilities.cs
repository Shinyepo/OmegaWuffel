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
                await _db.ChannelChecks.AddAsync(channelcheck);
                await _db.SaveChangesAsync();
                await context.DisposeAsync();
                return channelcheck;
            }
        }

        public async Task<ChannelCheckModel> ChannelCheckUpdateMessageIdAsync(ChannelCheckModel channelcheck)
        {
            using (var context = new WuffelDBContext())
            {
                var ChannelCheck = await _db.ChannelChecks.SingleOrDefaultAsync(ch => ch.Id == channelcheck.Id && ch.Status == 1);
                ChannelCheck.MessageId = channelcheck.MessageId;
                await _db.SaveChangesAsync();
                await context.DisposeAsync();
                return ChannelCheck;
            }
        }

        public async Task<ChannelCheckModel> GetChannelCheckAsync(int id)
        {
            using (var context = new WuffelDBContext())
            {
                var ChannelCheck = await _db.ChannelChecks.SingleOrDefaultAsync(ch => ch.Id == id && ch.Status == 1);
                await context.DisposeAsync();
                return ChannelCheck;
            }
        }
        public async Task<ChannelCheckModel> UpdateCheckChannelsAsync(ChannelCheckModel channelcheck)
        {
            using (var context = new WuffelDBContext())
            {
                var ChannelCheck = await _db.ChannelChecks.SingleOrDefaultAsync(ch => ch.Id == channelcheck.Id && ch.Status == 1);
                if (ChannelCheck == null) return ChannelCheck;
                ChannelCheck.Channels = channelcheck.Channels;
                ChannelCheck.Status = channelcheck.Status;
                await _db.SaveChangesAsync();
                await context.DisposeAsync();
                return ChannelCheck;
            }
        }


        //-----------------------CHANNEL CHECKS BDO ---------------------------//


    }
}
