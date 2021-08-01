using System;
using System.ComponentModel.DataAnnotations;

namespace OWuffel.Extensions.Database
{
    public class Settings
    {
        
        [Key]
        public int id { get; set; }

        public ulong guild_id { get; set; }
        public string guild_name { get; set; }
        public string guild_icon_hash { get; set; }

        public bool botActive { get; set; }
        public string botPrefix { get; set; }
        public ulong botModRole { get; set; }
        public ulong botAdminRole { get; set; }
        public string botDisabledCommands { get; set; }
        public bool botSystemNotice { get; set; }
        public bool botClearMessage { get; set; }
        public bool botClearMessagesForBot { get; set; }
        public ulong botMuteRole { get; set; }
        public string botLanguage { get; set; }


        public ulong welcomeChannel { get; set; }
        public bool welcomeMessageAutoDelete { get; set; }

        //UserVoiceStateUpdated
        public bool logVoicePresencePowerSwitch { get; set; }
        public ulong logVoiceStateUpdated { get; set; }
        public string logIgnoreVoiceStateUpdated { get; set; }

        //UserEvents
        public bool logUserMovementPowerSwitch { get; set; }
        public ulong logUserMovement { get; set; }
        public bool logUserUpdatedPowerSwitch { get; set; }
        public ulong logUserUpdated { get; set; }

        //RoleEvents
        public bool logRolePowerSwtich { get; set; }
        public ulong logRoleEvents { get; set; }

        //MessageEvents
        public bool logMessageEventsPowerSwitch { get; set; }
        public ulong logMessageEvents { get; set; }
        public string logIgnoreMessageEvents { get; set; }

        public bool logEmotePowerSwitch { get; set; }
        public ulong logEmoteEvents { get; set; }

        public ulong logGuildUpdated { get; set; }
        public bool logChannelPowerSwitch { get; set; }
        public ulong logChannelEvents { get; set; }


        //Suggestionconfig
        public ulong suggestionChannel { get; set; }
        public ulong suggestionLogChannel { get; set; }


        public ulong reactServerChannel { get; set; }
        public ulong reactServerMessage { get; set; }
        public ulong reactServerParent { get; set; }


    }
}
