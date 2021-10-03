using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class Suggestions
    {
        [Key]
        public int Id { get; set; }
        public ulong GuildId { get; set; }

        public GuildInformation? GuildInformation { get; set; }

        public string? Author { get; set; }
        public ulong AuthorId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong MessageId { get; set; }

        public string? Content { get; set; }
        public string? Comment { get; set; }
        public int VoteLike { get; set; }
        public int VoteDislike { get; set; }

        public int Status { get; set; }

        public string? Moderator { get; set; }
        public ulong ModeratorId { get; set; }

        public string? Timestamp { get; set; }
    }
}
