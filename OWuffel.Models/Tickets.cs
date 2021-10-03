using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class Tickets
    {
        [Key]
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public GuildInformation? GuildInformation { get; set; }
        public ulong UserId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong ModeratorId { get; set; }
        public int Status { get; set; }
        public string? Comment { get; set; }
        public ulong WhoClosedTicket { get; set; }
        public string? Timestamp { get; set; }

    }
}
