using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class BotSettings
    {
        [Key]
        public int Id { get; set; }

        public GuildInformation? GuildInformation { get; set; }
        public ulong GuildId { get; set; }

        public bool BotActive { get; set; }
        public string? BotPrefix { get; set; }
        public ulong BotModRole { get; set; }
        public ulong BotAdminRole { get; set; }
        public string? BotDisabledCommands { get; set; }
        public bool BotSystemNotice { get; set; }
        public bool BotClearMessage { get; set; }
        public bool BotClearMessagesForBot { get; set; }
        public ulong BotMuteRole { get; set; }
        public string? BotLanguage { get; set; }

    }
}
