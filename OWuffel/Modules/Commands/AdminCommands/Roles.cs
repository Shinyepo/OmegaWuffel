using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Modules.Commands.AdminCommands
{
    public class Roles: ModuleBase<SocketCommandContext>
    {
        public async Task<bool> RemoveRole(SocketGuildUser user, SocketRole role)
        {
            await user.RemoveRoleAsync(role);
            return false;
        }

        public async Task<bool> AddRole(SocketGuildUser user, SocketRole role)
        {
            await user.RemoveRoleAsync(role);
            return false;
        }

        [Command("role")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task setrole(string action, SocketGuildUser user, [Remainder]SocketRole role)
        {
            var AddAliases = new List<string>{ "+", "add", "grant" };
            var RemoveAliases = new List<string>{ "-", "remove", "revoke", "delete" };
            var em = new EmbedBuilder()
                .WithCurrentTimestamp()
                .WithColor(Color.Green);

            if (RemoveAliases.Contains(action))
            {
                await AddRole(user, role);
                em.WithTitle("Role removed Successfuly.")
                    .WithDescription(role.Name + " was removed from " + user.Username + "'s roles.");
                await ReplyAsync(embed: em.Build());
                return;
            } else if (AddAliases.Contains(action))
            {
                await RemoveRole(user, role);
                em.WithTitle("Role added Successfuly.")
                    .WithDescription(role.Name + " was added to " + user.Username + "'s roles.");
                await ReplyAsync(embed: em.Build());

            }
            else
            {
                em.WithTitle("Role action error.")
                    .WithDescription("Invalid action.\nTo add a role please use \"+\", \"add\", \"grant\"\nTo remove role please use \"-\", \"remove\", \"revoke\"");
                await ReplyAsync(embed: em.Build());
                return;
            }

        }
    }
}
