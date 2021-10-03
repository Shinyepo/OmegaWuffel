using System.ComponentModel.DataAnnotations;

namespace OWuffel.Models
{
    public class LogsConfig
    {

        [Key]
        public int Id { get; set; }

        public ulong GuildId { get; set; }
        public GuildInformation? GuildInformation { get; set; }

        //UserVoiceStateUpdated
        public bool LogVoicePresencePowerSwitch { get; set; }
        public ulong LogVoiceStateUpdated { get; set; }
        public string? LogIgnoreVoiceStateUpdated { get; set; }

        //UserEvents
        public bool LogUserMovementPowerSwitch { get; set; }
        public ulong LogUserMovement { get; set; }
        public bool LogUserUpdatedPowerSwitch { get; set; }
        public ulong LogUserUpdated { get; set; }

        //RoleEvents
        public bool LogRolePowerSwtich { get; set; }
        public ulong LogRoleEvents { get; set; }

        //MessageEvents
        public bool LogMessageEventsPowerSwitch { get; set; }
        public ulong LogMessageEvents { get; set; }
        public string? LogIgnoreMessageEvents { get; set; }

        public bool LogEmotePowerSwitch { get; set; }
        public ulong LogEmoteEvents { get; set; }

        public ulong LogGuildUpdated { get; set; }
        public bool LogChannelPowerSwitch { get; set; }
        public ulong LogChannelEvents { get; set; }
    }
}
