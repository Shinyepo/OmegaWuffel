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
        public int botClearBotMessagesDelay { get; set; }
        public ulong botMuteRole { get; set; }
        public string botLanguage { get; set; }


        public bool welcomeEnabled { get; set; }
        public ulong welcomeChannel { get; set; }
        public bool welcomeMessageAutoDelete { get; set; }

        public ulong logVoiceStateUpdate { get; set; }
        //UserEvents
        public ulong logUserJoins { get; set; }
        public ulong logUserUpdates { get; set; }

        public ulong logGuildEvents { get; set; }

        public ulong logChannelMessageModule { get; set; }
        public ulong logChannelBanModule { get; set; }
        public ulong logChannelMemberRemoveModule { get; set; }
        public ulong logBlockedByUser { get; set; }
        public ulong logChannelMessageIngored { get; set; }
        public ulong logChannelReact { get; set; }


        public ulong reactServerChannel { get; set; }
        public ulong reactServerMessage { get; set; }
        public ulong reactServerParent { get; set; }


    }
}
