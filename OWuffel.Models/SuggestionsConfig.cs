using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class SuggestionsConfig
    {
        [Key]
        public int Id { get; set; }

        public ulong GuildId { get; set; }
        public GuildInformation? GuildInformation { get; set; }

        public ulong SuggestionChannel { get; set; }
        public ulong SuggestionLogChannel { get; set; }
    }
}
