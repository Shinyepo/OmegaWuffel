using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Modules.Commands.OwnerCommands
{
    public class Activity : ModuleBase<SocketCommandContext>
    {
        [Command("botActivity")]
        [RequireOwner]
        public async Task ChangeBotActivityAsync([Remainder]string newActivity)
        {
            await Context.Client.SetActivityAsync(new Game(newActivity));
            //await Context.Client.SetGameAsync(newActivity, null, Discord.ActivityType.Playing);
        }

    }
}
