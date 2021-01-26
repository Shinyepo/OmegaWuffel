using Discord.WebSocket;
using NLog;
using OWuffel.Extensions.Database;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.events
{
    public class GuildEvents
    {
        private readonly WuffelDBContext _db;
        private Settings WuffelDB;
        private Logger _logger;

        public GuildEvents(WuffelDBContext db)
        {
            _db = db;
            _logger = LogManager.GetCurrentClassLogger();
        }
        public async Task JoinedGuild(SocketGuild arg)
        {
            WuffelDB = await _db.Settings.SingleOrDefaultAsync(s => s.guild_id == arg.Id);
            if (WuffelDB == null)
            {
                await DefaultDbSettings.SetDefaultSettings(_db, arg.Id);
                _logger.Info($"Joined {arg.Name}({arg.Id}) with {arg.MemberCount} users total.");
            }
            else
            {
                WuffelDB.botActive = 1;
                await _db.SaveChangesAsync();
                _logger.Info($"Joined {arg.Name}({arg.Id}) with {arg.MemberCount} users total.");
            }
        }

        public async Task LeftGuild(SocketGuild arg)
        {
            WuffelDB = await _db.Settings.SingleOrDefaultAsync(s => s.guild_id == arg.Id);
            if (WuffelDB != null)
            {
                WuffelDB.botActive = 0;
                await _db.SaveChangesAsync();
                _logger.Info($"Left {arg.Name}({arg.Id}) with {arg.MemberCount} users total.");
            } else
            {
                return;
            }
        }

        public async Task GuildUpdated(SocketGuild arg1, SocketGuild arg2)
        {
            var a = arg1;
            var b = arg2;
            Console.Write("!");
        }
    }
}
