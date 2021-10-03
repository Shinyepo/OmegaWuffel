using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class DailyRankingConfig
    {
        [Key]
        public int Id { get; set; }
        public GuildInformation? GuildInformation { get; set; }
        public ulong GuildId { get; set; }

        public ulong ChannelId { get; set; }
        public string? IgnoreId { get; set; }
    }
}
