using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Extensions.Database
{
    public class SupportConfiguration
    {
        [Key]
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public ulong ParentId { get; set; }
        public ulong TicketInfoId { get; set; }
        public ulong MessageId { get; set; }
        public int Notify { get; set; }
        public string TicketMessage { get; set; }
        public int PremiumStatus { get; set; }
    }
}
