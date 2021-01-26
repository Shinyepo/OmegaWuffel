using Discord.WebSocket;
using OWuffel.Extensions.Database;
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
            Settings = await _db.Settings.SingleOrDefaultAsync(s => s.guild_id == guild_id);
            if (Settings == null)
            {
                Settings = await SetDefaultSettingsAsync(guild_id);
            }
            return Settings;
        }

        public async Task<Settings> SetDefaultSettingsAsync(ulong guild_id)
        {
            try
            {
                var Setting = new Settings();
                Setting.guild_id = guild_id;
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
    }
}
