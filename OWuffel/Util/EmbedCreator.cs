using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Util
{
    class EmbedCreator
    {
        private EmbedBuilder Embed;
        public EmbedCreator()
        {
            Embed = new EmbedBuilder();
        }

        public Task CreateAsync(List<string> properties, ITextChannel channel)
        {
            var _ = Task.Run(async () =>
            {
                try
                {


                    await channel.SendMessageAsync("", false, Embed.Build());
                }
                catch
                {
                    //dont care
                }
            });
            return Task.CompletedTask;
        }

    }
}
