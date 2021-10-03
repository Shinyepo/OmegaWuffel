//using Discord;
//using Discord.Commands;
//using OWuffel.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using OWuffel.Extensions.Database;
//using OWuffel.Util;
//using Discord.WebSocket;
//using Interactivity;
//using OWuffel.Models;

//namespace OWuffel.Modules.Commands.Suggestions
//{
//    public class SuggestionCommands : ModuleBase<SocketCommandContext>
//    {
//        private DatabaseUtilities _db;
//        public InteractivityService _intserv { get; set; }

//        public SuggestionCommands(DatabaseUtilities db, InteractivityService intserv)
//        {
//            _db = db;
//            _intserv = intserv;
//        }

//        [Command("Suggestionchannel")]
//        [Alias("sugchannel", "schannel")]
//        [RequireUserPermission(GuildPermission.Administrator)]
//        public async Task SuggestionChannel(SocketGuildChannel channel)
//        {
//            await Context.Message.DeleteAsync();
//            if (channel is SocketVoiceChannel)
//            {
//                await ReplyAsync("cipa");
//                return;
//            }

//            await _db.SetSettingsValueAsync(Context.Guild, "suggestionChannel", channel.Id);
//            var em = new EmbedBuilder()
//                .WithTitle("Suggestion channel configuration")
//                .WithColor(Color.Green)
//                .WithCurrentTimestamp()
//                .WithDescription("Successfuly set suggestion channel to **" + channel + "**");

//            await ReplyAsync(embed: em.Build());
//        }

//        [Command("suggest")]
//        public async Task CreateSuggestion([Remainder] string content)
//        {
//            await Context.Message.DeleteAsync();
//            if (Context.Settings.suggestionChannel == 0)
//            {
//                var e = new EmbedBuilder()
//                    .WithCurrentTimestamp()
//                    .WithColor(Color.Red)
//                    .WithTitle("Error while creating suggestion!")
//                    .WithDescription($"There is no channel configured for suggestions.\nPlease use `{Context.Settings.botPrefix}SuggestionChannel #channel` to setup the channel.");
//                await ReplyAsync(embed: e.Build());
//                return;
//            }

//            var Suggestion = new Models.Suggestions();
//            Suggestion.GuildId = Context.Guild.Id;
//            Suggestion.Author = Context.User.Username;
//            Suggestion.AuthorId = Context.User.Id;
//            Suggestion.Content = content;
//            Suggestion.VoteLike = 0;
//            Suggestion.VoteDislike = 0;
//            Suggestion.Status = 1;
//            Suggestion.Timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

//            var result = await _db.CreateSuggestionAsync(Suggestion);

//            var em = new EmbedBuilder()
//                .WithColor(Color.Blue)
//                .WithFooter($"ID: {result.Id} ")
//                .WithCurrentTimestamp()
//                .WithAuthor(Context.User)
//                .WithDescription(result.Content)
//                .AddField("Votes:", "👍 " + result.VoteLike + "\n\n👎 " + result.VoteDislike, false)
//                .AddField("Status", "Waiting for votes", false);

//            var msg = await ReplyAsync(embed: em.Build());
//            var votelike = new Emoji("👍");
//            var votedislike = new Emoji("👎");
//            await msg.AddReactionAsync(votelike);
//            await msg.AddReactionAsync(votedislike);
//            await _db.SetSuggestionMessageId(result.GuildId, result.Id, msg);
//        }
//        [Command("DeleteSuggestion", RunMode = RunMode.Async)]
//        [Alias("removesuggestion", "rsuggestion", "dsuggestion")]
//        [RequireUserPermission(GuildPermission.Administrator)]
//        public async Task GetSuggestion(string id)
//        {
//            await Context.Message.DeleteAsync();
//            var a = Convert.ToInt32(id);
//            var suggestion = await _db.GetSuggestionAsync(Context.Guild.Id, a);
//            if (suggestion == null)
//            {
//                await ReplyAsync("Error while deleting suggestion. Please make sure the id is correct.");
//                return;
//            }

//            var res = await _db.DeleteSuggestionAsync(Context.Guild.Id, a);
//            if (res)
//            {

//                var msg = await ReplyAsync("Successfuly deleted suggestion");
//                _intserv.DelayedDeleteMessageAsync(msg, TimeSpan.FromMinutes(5));
//                var mesg = await Context.Guild.GetTextChannel(suggestion.ChannelId).GetMessageAsync(suggestion.MessageId);
//                await mesg.DeleteAsync();
//            }
//            else
//            {
//                await ReplyAsync("Error while deleting suggestion. Please make sure the id is correct.");
//            }
//        }
//        [Command("addcomment")]
//        [RequireUserPermission(GuildPermission.Administrator)]
//        public async Task AddComment(string id, [Remainder] string comment = "")
//        {
//            await Context.Message.DeleteAsync();
//            var a = Convert.ToInt32(id);
//            if (a == null || comment == "")
//            {
//                await ReplyAsync("wrong id or huj ci w dupe");
//                return;
//            }
//            var suggestion = await _db.GetSuggestionAsync(Context.Guild.Id, a);
//            var msg = await Context.Guild.GetTextChannel(suggestion.ChannelId).GetMessageAsync(suggestion.MessageId) as IUserMessage;
//            var embed = msg.Embeds.First().ToEmbedBuilder();
//            embed.AddField($"{Context.User.Username}'s comment", comment, false);

//            await msg.ModifyAsync(m => m.Embed = embed.Build());
//        }
//        [Command("suggestionstatus")]
//        [Alias("changestatus", "sstatus", "status")]
//        [RequireUserPermission(GuildPermission.Administrator)]
//        public async Task ChangeStatus(string id, string status)
//        {
//            await Context.Message.DeleteAsync();
//            var a = Convert.ToInt32(id);
//            var b = Convert.ToInt32(status);
//            var suggestion = await _db.GetSuggestionAsync(Context.Guild.Id, a);
//            if (suggestion != null)
//            {
//                var result = await _db.ChangeStatusAsync(Context.Guild.Id, a, b);
//                if (result != null)
//                {
//                    var msg = await Context.Guild.GetTextChannel(suggestion.ChannelId).GetMessageAsync(suggestion.MessageId) as IUserMessage;
//                    var response = "";
//                    var color = Color.Blue;
//                    if (b == 1)
//                    {
//                        response = "Waiting for votes";
//                        color = Color.Blue;
//                    }
//                    if (b == 2)
//                    {
//                        response = "Implemented";
//                        color = Color.Green;
//                    }
//                    if (b == 3)
//                    {
//                        response = "Rejected";
//                        color = Color.Red;
//                    }
//                    if (b == 0)
//                    {
//                        response = "Closed";
//                        color = Color.DarkGrey;
//                    }
//                    var embed = msg.Embeds.First().ToEmbedBuilder();
//                    embed.Color = color;
//                    embed.Fields.SingleOrDefault(f => f.Name.Contains("Status")).Value = response;
//                    await msg.ModifyAsync(m => m.Embed = embed.Build());
//                }
//            }
//        }
//    }
//}
