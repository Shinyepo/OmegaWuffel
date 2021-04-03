using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Extensions.Database
{
    public class ChannelCheckModel
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong AuthorId { get; set; }
        public ulong MessageId { get; set; }
        public ulong ChannelId { get; set; }

        public string Target { get; set; }

        public string Channels { get; set; }

        //0 - disabled, 1 - enabled
        public int Status { get; set; }
        public string Found { get; set; }

        public string Timestamp { get; set; }
    }
}
