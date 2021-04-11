using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Extensions.Database
{
    public class Tickets
    {
        [Key]
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong UserId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong ModeratorId { get; set; }
        public int Status { get; set; }
        public string Comment { get; set; }
        public ulong WhoClosedTicket { get; set; }
        public string Timestamp { get; set; }

    }
}
