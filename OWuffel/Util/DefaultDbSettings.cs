using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OWuffel.Extensions.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Util
{
    public class DefaultDbSettings
    {
        public static async Task<Settings> SetDefaultSettings(WuffelDBContext _db, UInt64 guild)
        {
            try
            {
                var Setting = new Settings();
                Setting.guild_id = guild;
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
