using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class WelcomeMessageConfig
    {
        [Key]
        public int Id { get; set; }

        public ulong GuildId { get; set; }
        public GuildInformation? GuildInformation { get; set; }

        public ulong WelcomeChannel { get; set; }
        public bool WelcomeMessageAutoDelete { get; set; }
    }
}
