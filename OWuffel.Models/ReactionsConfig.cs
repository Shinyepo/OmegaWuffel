using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class ReactionsConfig
    {
        [Key]
        public int Id { get; set; }

        public ulong GuildId { get; set; }
        public GuildInformation? GuildInformation { get; set; }

        public ulong ReactServerChannel { get; set; }
        public ulong ReactServerMessage { get; set; }
        public ulong ReactServerParent { get; set; }
    }
}
