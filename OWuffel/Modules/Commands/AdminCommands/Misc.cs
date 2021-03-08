using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OWuffel.Modules.Commands.Preconditions;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Discord.Addons.Interactive;

namespace OWuffel.Modules.Commands.AdminCommands
{
    public class Misc : ModuleBase<Cipska>
    {
        public InteractiveService inbase { get; set; }
        public Misc(InteractiveService sv)
        {
            inbase = sv;
        }

        [Command("profile")]
        [Alias("userinfo")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ProfileInfo([Optional] SocketGuildUser user)
        {
            if (user == null)
            {
                user = Context.User as SocketGuildUser;
            }
            try
            {
                EmbedBuilder em = new EmbedBuilder()
                    .WithAuthor(user)
                    .WithTitle($"Detailed information about {user.Username}")
                    .WithColor(Color.Blue)
                    .WithThumbnailUrl(user.GetAvatarUrl() == null ? user.GetDefaultAvatarUrl() : user.GetAvatarUrl());
                if (user.Nickname != null)
                {
                    em.AddField("Nickname: ", user.Nickname == null ? "*not set*" : user.Nickname, true);
                }

                em.AddField("Username: ", user.Username, true)
                .AddField("Discriminator: ", "#" + user.Discriminator, true)
                .AddField("Joined: ", user.JoinedAt.Value.ToLocalTime().ToString("dd/MM/yyyy"), true)
                .AddField("Created: ", user.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy"), true)
                .WithCurrentTimestamp();


                if (user.Roles.Count > 0)
                {
                    string roles = "";
                    foreach (var item in user.Roles)
                    {
                        if (!item.IsEveryone) roles += item.Name + "\n";
                    }
                    em.AddField("Roles: ", roles, false);
                }
                em.AddField("Unique ID: ", user.Id, true);
                if (user.PremiumSince != null)
                {
                    em.AddField("Boosting server since: ", user.PremiumSince.Value.ToString("dd/MM/yyyy"), true);
                }
                await ReplyAsync(embed: em.Build());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        [Command("purge", RunMode = RunMode.Async)]
        [Alias("prune")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        public async Task Prune(SocketGuildChannel channel)
        {
            await ReplyAsync("By using this command i will delete and create again provided channel. Are you sure you want me to do this?");
            var response = await inbase.NextMessageAsync(Context);
            var confirmation = false;
            if (response != null)
            {
                if (response.Content == "y" || response.Content == "yes")
                {
                    confirmation = true;
                }
            }
            else
            {
                await ReplyAsync("You did not reply before timeout");
                return;
            }
            if (confirmation == false) return;
            ITextChannel oldchannel = channel as ITextChannel;
            await channel.DeleteAsync();
            var chan = await Context.Guild.CreateTextChannelAsync(oldchannel.Name) as ITextChannel;
            await chan.ModifyAsync(opt =>
            {
                opt.CategoryId = oldchannel.CategoryId;
                opt.Position = oldchannel.Position;
                opt.IsNsfw = oldchannel.IsNsfw;
                opt.SlowModeInterval = oldchannel.SlowModeInterval;
                opt.Topic = oldchannel.Topic;
            });
            foreach (var item in oldchannel.PermissionOverwrites)
            {
                if (item.TargetType == PermissionTarget.Role)
                {
                    var role = Context.Guild.GetRole(item.TargetId);
                    await chan.AddPermissionOverwriteAsync(role, item.Permissions);
                }
                else if (item.TargetType == PermissionTarget.User)
                {
                    var user = Context.Guild.GetUser(item.TargetId);
                    await chan.AddPermissionOverwriteAsync(user, item.Permissions);
                }
            }

        }

        [Command("purge", RunMode = RunMode.Async)]
        [Alias("prune")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task Prune(string amount, [Optional] SocketGuildUser user)
        {
            var converted = Convert.ToInt16(amount);

            if (converted < 1 && converted > 100)
            {
                await ReplyAsync("Provided number is not valid. Amount of messages to delete must be between 1 and 100.");
                return;
            }
            //download 100 messages, loop through them to compare user to author, if matched -> append to Messages. repeat step 2 -> 3 untill number > converted
            IEnumerable<IMessage> Messages = Enumerable.Empty<IMessage>();
            List<IMessage> list = new List<IMessage>();
            if (user != null)
            {
                var a = await Context.Channel.GetMessagesAsync(100).FlattenAsync();
                var b = 0;

                while (b <= converted)
                {
                    var last = a.Last();
                    Console.WriteLine("before for");
                    foreach (var item in a)
                    {
                        if (b >= converted) break;
                        if (item.Author == user)
                        {
                            list.Add(item);
                            Console.WriteLine(item.Id);
                            Console.WriteLine(b);
                            b++;
                        }
                    }
                    Console.WriteLine("fater for");
                    if (b >= converted) break;
                    a = await Context.Channel.GetMessagesAsync(last, Direction.Before, 100).FlattenAsync();
                }
                Messages = list;
                await ((ITextChannel)Context.Channel).DeleteMessagesAsync(Messages);
                var name = user.Nickname == null ? user.Username : user.Nickname;
                IUserMessage m = await ReplyAsync($"I have deleted {converted} {name}'s messages.");
                await Task.Delay(3000);
                await m.DeleteAsync();

            }
            else
            {
                Messages = await Context.Channel.GetMessagesAsync(converted + 1).FlattenAsync();
                await ((ITextChannel)Context.Channel).DeleteMessagesAsync(Messages);
                IUserMessage m = await ReplyAsync($"I have deleted {converted} messages.");
                await Task.Delay(3000);
                await m.DeleteAsync();
            }


        }
    }
}
