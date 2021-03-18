using Discord;
using Discord.WebSocket;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Events
{
    public class ReactionEvents
    {
        private DatabaseUtilities _db;
        public ReactionEvents(DatabaseUtilities db)
        {
            _db = db;
        }

        private async Task VoteMainTaskAsync(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction, Suggestions suggestion, string type)
        {
            var message = await msg.DownloadAsync();

            var votelike = new Emoji("👍");
            var votedislike = new Emoji("👎");
            if (reaction.Emote.Name == votelike.Name)
            {
                var likera = message.Reactions.SingleOrDefault(e => e.Key.Name == votelike.Name).Value.ReactionCount;
                var amount = 0;
                //if (likera < 2 && type == "added")
                //{
                //    await message.AddReactionAsync(votelike);
                //}
                amount = (likera - 1) - suggestion.VoteLike;
                await VoteChangeAsync(msg, suggestion, amount, "like");

            }
            else if (reaction.Emote.Name == votedislike.Name)
            {
                var dislikera = message.Reactions.SingleOrDefault(e => e.Key.Name == votedislike.Name).Value.ReactionCount;
                var amount = 0;
                //if (dislikera < 2 && type == "added")
                //{
                //    await message.AddReactionAsync(votelike);
                //}
                amount = (dislikera - 1) - suggestion.VoteDislike;
                await VoteChangeAsync(msg, suggestion, amount, "dislike");
            }
            return;
        }

        private async Task VoteChangeAsync(Cacheable<IUserMessage, ulong> msg, Suggestions suggestion, int amount, string action)
        {
            var votelike = new Emoji("👍");
            var votedislike = new Emoji("👎");

            var message = await msg.GetOrDownloadAsync();

            var sug = await _db.ChangeVoteAsync(suggestion, amount, action);
            var embed = message.Embeds.First().ToEmbedBuilder();
            embed.Fields.First().Value = "👍 " + sug.VoteLike + "\n\n👎 " + sug.VoteDislike;
            await message.ModifyAsync(m => m.Embed = embed.Build());
        }

        public Task ReactionAdded(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var message = await msg.DownloadAsync();
                    var chnl = channel as SocketGuildChannel;
                    var suggestion = await _db.LoopSuggestions(chnl.Guild.Id, msg.Id);
                    if (suggestion != null)
                    {
                        await VoteMainTaskAsync(msg, channel, reaction, suggestion, "added");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            });
            return Task.CompletedTask;
        }        

        public Task ReactionRemoved(Cacheable<IUserMessage, ulong> msg, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var message = await msg.DownloadAsync();
                    var chnl = channel as SocketGuildChannel;
                    var suggestion = await _db.LoopSuggestions(chnl.Guild.Id, msg.Id);
                    if (suggestion != null)
                    {
                        await VoteMainTaskAsync(msg, channel, reaction, suggestion, "added");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            });
            return Task.CompletedTask;
        }

        internal Task ReactionCleared(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            throw new NotImplementedException();
        }
    }

    public class ReactionLoop
    {

    }
}
