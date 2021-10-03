using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class GuildInformation
    {
        [Key]
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public string? GuildName { get; set; }
        public string? GuildAvatar { get; set; }
        public ulong GuildOwnerId { get; set; }
        public string? GuildOwnerName { get; set; }


        public BotSettings? BotSettings { get; set; }
        public DailyRankingConfig? DailyRankingConfig { get; set; }
        public LogsConfig? LogsConfig { get; set; }
        public ReactionsConfig? ReactionsConfig { get; set; }
        public SuggestionsConfig? SuggestionsConfig { get; set; }
        public Suggestions? Suggestions { get; set; }
        public SupportConfig? SupportConfig { get; set; }
        public Tickets? Tickets { get; set; }
        public WelcomeMessageConfig? WelcomeMessageConfig { get; set; }
    }
}
