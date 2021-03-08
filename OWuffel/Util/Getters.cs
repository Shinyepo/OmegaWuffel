using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace OWuffel.util
{
    class GetThings
    {
        public static SocketGuild getGuildFromChannel(IMessageChannel channel)
        {
            var chnl = channel as SocketGuildChannel;
            var guild = chnl.Guild;
            
            return guild;
        }

        public static int GetAllUserCount(DiscordSocketClient arg)
        {
            var guildcoll = arg.Guilds;
            var count = 0; ;
            foreach (var guild in guildcoll)
            {
                count += guild.MemberCount;
            }
            return count;
        }
    }

}
