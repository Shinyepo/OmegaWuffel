//using Discord;
//using Discord.WebSocket;
//using OWuffel.Extensions;
//using OWuffel.Extensions.Database;
//using OWuffel.Models;
//using OWuffel.Services;
//using OWuffel.Util;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OWuffel.Events
//{
//    public class ReactionEvents
//    {
//        private DatabaseUtilities _db;
//        public ReactionEvents(DatabaseUtilities db)
//        {
//            _db = db;
//        }

//        private async Task CreateNewTicketAsync(SupportConfig config, IUserMessage message, SocketGuildUser user)
//        {
//            try
//            {
//                var channel = message.Channel as SocketGuildChannel;
//                var guild = channel.Guild;

//                var cat = guild.CategoryChannels.SingleOrDefault(c => c.Id == config.ParentId);

//                var ticket = new Tickets();
//                ticket.GuildId = guild.Id;
//                ticket.Status = 1;
//                ticket.UserId = user.Id;
//                ticket.Timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

//                var newticket = await _db.CreateNewTicket(ticket);
//                if (newticket.Id == 0)
//                {
//                    await user.SendMessageAsync("You already have 3 active tickets. Wait for them to be resolved before creating new tickets.");
//                    return;
//                }

//                var chnl = await guild.CreateTextChannelAsync(ticket.Id + "-" + user.Username, c => c.CategoryId = config.ParentId);
//                await chnl.SyncPermissionsAsync();
//                var useroverride = new OverwritePermissions(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow);
//                await chnl.AddPermissionOverwriteAsync(user, permissions: useroverride);
//                string desc = user.Mention + ", this channel was created for your ticket. Please explain your matter as detailed as possible to make it easier for moderators to resolve your ticket.";
//                if (config.TicketMessage != "")
//                {
//                    desc = config.TicketMessage;
//                }
//                var em = new EmbedBuilder()
//                    .WithTitle("Ticket information")
//                    .WithColor(Color.Green)
//                    .WithDescription(desc);
//                await chnl.SendMessageAsync(embed: em.Build());
//                var count = await _db.NumberOfActiveTickets(guild.Id);
//                var activetickets = guild.GetVoiceChannel(config.ActiveTicketsId);
//                var name = $"Active Tickets: {count}";
//                await activetickets.ModifyAsync(c => c.Name = name);
//            }
//            catch (Exception ex)
//            {
//                Log.Error(ex);
//            }
//        }


//        private async Task VoteMainTaskAsync(Cacheable<IUserMessage, ulong> msg, SocketGuildChannel channel, SocketReaction reaction, Suggestions suggestion, string type)
//        {
//            var sw = Stopwatch.StartNew();
//            Console.WriteLine("maintask start");
//            var message = await msg.DownloadAsync();

//            var votelike = new Emoji("👍");
//            var votedislike = new Emoji("👎");
//            if (reaction.Emote.Name == votelike.Name)
//            {
//                var likera = message.Reactions.SingleOrDefault(e => e.Key.Name == votelike.Name).Value.ReactionCount;
//                var amount = 0;
//                amount = (likera - 1) - suggestion.VoteLike;
//                await VoteChangeAsync(msg, suggestion, amount, "like");

//            }
//            else if (reaction.Emote.Name == votedislike.Name)
//            {
//                var dislikera = message.Reactions.SingleOrDefault(e => e.Key.Name == votedislike.Name).Value.ReactionCount;
//                var amount = 0;
//                amount = (dislikera - 1) - suggestion.VoteDislike;
//                await VoteChangeAsync(msg, suggestion, amount, "dislike");
//            }
//            sw.Stop();
//            Log.Info($"maintask finished in {sw.Elapsed.TotalSeconds:F2}s");
//            return;
//        }

//        private async Task VoteChangeAsync(Cacheable<IUserMessage, ulong> msg, Suggestions suggestion, int amount, string action)
//        {
//            var sw = Stopwatch.StartNew();
//            Console.WriteLine("votechange start");
//            var votelike = new Emoji("👍");
//            var votedislike = new Emoji("👎");

//            var message = await msg.GetOrDownloadAsync();

//            var sug = await _db.ChangeVoteAsync(suggestion, amount, action);
//            var embed = message.Embeds.First().ToEmbedBuilder();
//            embed.Fields.First().Value = "👍 " + sug.VoteLike + "\n\n👎 " + sug.VoteDislike;
//            await message.ModifyAsync(m => m.Embed = embed.Build());
//            sw.Stop();
//            Log.Info($"votechange finished in {sw.Elapsed.TotalSeconds:F2}s ");
//        }

//        public Task ReactionAdded(Cacheable<IUserMessage, ulong> msg, Cacheable<IMessageChannel, ulong> channel)
//        {
//            var _ = Task.Run(async () =>
//            {
//                try
//                {

//                    //var sw = Stopwatch.StartNew();
//                    //Console.WriteLine(sw + " start reactionadded");
//                    var message = await msg.DownloadAsync();
//                    var list = message.Reactions.Where(x => x.Key.Name != reaction.Emote.Name).Select(x => x.Key).ToArray();
//                    await message.RemoveReactionsAsync(reaction.User.GetValueOrDefault(), list);






//                    var emotelist = new List<IEmote>();
//                    foreach (var item in message.Reactions)
//                    {
//                        if (item.Key.Name != reaction.Emote.Name)
//                        {
//                            var limit = message.Reactions.SingleOrDefault(x => x.Key == item.Key).Value.ReactionCount;
//                            var b = await message.GetReactionUsersAsync(item.Key, limit).FlattenAsync();
//                            b = b.Where(x => x.Id == reaction.UserId);
//                            if (b.Count() > 0)
//                            {
//                                emotelist.Add(item.Key);
//                            }
//                        }
//                    }
//                    if (emotelist.Count() > 0)
//                    {
//                        var user = reaction.User.GetValueOrDefault();
//                        await message.RemoveReactionsAsync(user, emotelist.ToArray());
//                    }

//                    var chnl = (await channel.GetOrDownloadAsync()) as SocketGuildChannel;
//                    var suggestion = await _db.LoopSuggestions(chnl.Guild.Id, msg.Id);
//                    if (suggestion != null)
//                    {
//                        if (suggestion.Status == 1)
//                        {
//                            await VoteMainTaskAsync(msg, chnl, reaction, suggestion, "added");
//                            //sw.Stop();
//                            //Log.Info($"Connected in {sw.Elapsed.TotalSeconds:F2}s passed");
//                            return;
//                        }
//                        //sw.Stop();
//                        //Log.Info($"Connected in {sw.Elapsed.TotalSeconds:F2}s not 1");
//                        return;
//                    }

//                    //support
//                    var support = await _db.LookForConfigurationAsync(chnl.Guild.Id);
//                    if (support != null)
//                    {
//                        if (support.MessageId == message.Id)
//                        {
//                            var emoji = new Emoji("📩");
//                            if (reaction.Emote.Name == emoji.Name)
//                            {
//                                var user = reaction.User.Value as SocketGuildUser;
//                                await CreateNewTicketAsync(support, message, user);
//                                await message.RemoveReactionAsync(emoji, reaction.UserId);
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Log.Error(ex);
//                }
//            });
//            return Task.CompletedTask;
//        }
//        public Task ReactionRemoved(Cacheable<IUserMessage, ulong> msg, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction)
//        {
//            var _ = Task.Run(async () =>
//            {
//                try
//                {
//                    var message = await msg.DownloadAsync();
//                    var chnl = (await channel.GetOrDownloadAsync()) as SocketGuildChannel;
//                    var suggestion = await _db.LoopSuggestions(chnl.Guild.Id, msg.Id);
//                    if (suggestion != null)
//                    {
//                        await VoteMainTaskAsync(msg, chnl, reaction, suggestion, "added");
//                        return;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    Log.Error(ex);
//                }
//            });
//            return Task.CompletedTask;
//        }

//        internal Task ReactionCleared(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class ReactionLoop
//    {

//    }
//}
