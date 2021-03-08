using Discord.WebSocket;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Util
{
    public class DatabaseUtilities
    {
        private readonly WuffelDBContext _db;
        private Settings Settings;

        public DatabaseUtilities(WuffelDBContext db)
        {
            _db = db;
        }

        public async Task<Settings> GetGuildSettingsAsync(ulong guild_id)
        {
            
            using (var context = new WuffelDBContext())
            {
                Settings = await context.Settings.SingleOrDefaultAsync(s => s.guild_id == guild_id);
                Log.Info(context.ContextId.ToString());

                if (Settings == null)
                {
                    Settings = await SetDefaultSettingsAsync(guild_id);
                }
                await context.DisposeAsync();
                return Settings;
            }
        }

        public async Task<Settings> SetDefaultSettingsAsync(ulong guild_id)
        {
            try
            {
                var Setting = new Settings();
                Setting.guild_id = guild_id;
                Setting.botPrefix = "+";
                Setting.botDisabledCommands = "[{}]";
                Setting.botActive = 1;
                await _db.Settings.AddAsync(Setting);
                await _db.SaveChangesAsync();
                return Setting;
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
    }
}
