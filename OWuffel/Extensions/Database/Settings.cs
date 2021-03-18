using System.ComponentModel.DataAnnotations;

namespace OWuffel.Extensions.Database
{
    public class Settings
    {
        
        [Key]
        public int id { get; set; }

        public ulong guild_id { get; set; }

        public int botActive { get; set; }
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
        public ulong logVoiceStateUpdated { get; set; }
        public string logIgnoreVoiceStateUpdated { get; set; }

        //UserEvents
        public ulong logUserJoined { get; set; }
        public ulong logUserLeft { get; set; }
        public ulong logUserBanned { get; set; }
        public ulong logUserUnbanned { get; set; }
        public ulong logUserUpdated { get; set; }

        //RoleEvents
        

        //MessageEvents
        public ulong logMessageDeleted { get; set; }
        public ulong logMessageUpdated { get; set; }
        public string logIgnoreMessageUpdated { get; set; }
        public string logIgnoreMessageDeleted { get; set; }

        //GuildEvents
        public ulong logEmoteCreated { get; set; }
        public ulong logEmoteUpdated { get; set; }
        public ulong logEmoteDeleted { get; set; }
        public ulong logRoleCreated { get; set; }
        public ulong logRoleUpdated { get; set; }
        public ulong logRoleDeleted { get; set; }
        public ulong logGuildUpdated { get; set; }


        //Suggestionconfig
        public ulong suggestionChannel { get; set; }
        public ulong suggestionLogChannel { get; set; }


        public ulong reactServerChannel { get; set; }
        public ulong reactServerMessage { get; set; }
        public ulong reactServerParent { get; set; }


    }
}
