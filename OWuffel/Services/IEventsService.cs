using OWuffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Services
{
    public interface IEventsService
    {
        Task<LogsConfig> GetLogsConfigsAsync(ulong guildId);
    }
}
