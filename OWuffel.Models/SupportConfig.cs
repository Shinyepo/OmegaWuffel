using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class SupportConfig
    {
        [Key]
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public GuildInformation? GuildInformation { get; set; }
        public ulong ParentId { get; set; }
        public ulong ActiveTicketsId { get; set; }
        public ulong TicketInfoId { get; set; }
        public ulong MessageId { get; set; }
        public int Notify { get; set; }
        public string? TicketMessage { get; set; }
        public int PremiumStatus { get; set; }
    }
}
