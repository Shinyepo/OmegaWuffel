using OWuffel.Models;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Services
{
    public class EventsService : IEventsService
    {
        private readonly DatabaseUtilities DatabaseUtilities;

        public EventsService(DatabaseUtilities databaseUtilities)
        {
            DatabaseUtilities = databaseUtilities;
        }

        public async Task<LogsConfig> GetLogsConfigsAsync(ulong guildId)
        {
            var result = await DatabaseUtilities.GetLogsConfigAsync(guildId);
            return result;
        }
    }
}
