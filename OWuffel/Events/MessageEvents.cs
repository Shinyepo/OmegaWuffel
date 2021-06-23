using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OWuffel.util;
using OWuffel.Util;
using Serilog;

namespace OWuffel.events
{
    class MessageEvents
    {
        private readonly DatabaseUtilities _db;
        private readonly IServiceProvider _services;

        public MessageEvents(DatabaseUtilities db, IServiceProvider services)
        {
            _db = db;
            _services = services;

        }
        public async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var message = await before.GetOrDownloadAsync();
            if (message.Embeds.Count != 0) return;
            if (after.Embeds.Count != 0) return;
            if (message.Author.IsBot) return;

            var guild = GetThings.getGuildFromChannel(channel);
            var Settings = await _db.GetGuildSettingsAsync(guild);
            if (Settings.logMessageUpdated == 0) return;
            if (Settings.logIgnoreMessageUpdated != null)
            {
                if (Settings.logIgnoreMessageUpdated.Contains(channel.Id.ToString()) || Settings.logIgnoreMessageUpdated.Contains(after.Author.Id.ToString()))
                {
                    return;
                }
            }

            string beforereply;
            if (message.Content == after.Content) beforereply = "A message was edited but i could not get its content before change.";
            else beforereply = message.Content;

            

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithAuthor(message.Author)
                .WithDescription($"[Edited message](https://discordapp.com/channels/{ guild.Id }/{ channel.Id }/{ message.Id })\n**Message edited by { message.Author } in <#{ channel.Id }>.**")
                .WithColor(Color.Blue)
                .AddField("Before:", beforereply)
                .AddField("After:", after)
                .WithFooter($"• { guild.Name }")
                .WithCurrentTimestamp();
            var ch = guild.GetTextChannel(Settings.logMessageUpdated);
            
            await ch.SendMessageAsync("", false, embed.Build());
        }

        public async Task MessageDelete(Cacheable<IMessage, ulong> msg, ISocketMessageChannel channel)
        {
            
            var message = await msg.GetOrDownloadAsync();
            if (message == null) return;
            if (message.Author.IsBot) return;

            var guild = GetThings.getGuildFromChannel(channel);
            var Settings = await _db.GetGuildSettingsAsync(guild);
            if (Settings.logMessageDeleted == 0) return;
            if (Settings.logIgnoreMessageDeleted != null)
            {
                if (Settings.logIgnoreMessageDeleted.Contains(channel.Id.ToString()) || Settings.logIgnoreMessageDeleted.Contains(message.Author.Id.ToString()))
                {
                    return;
                }
            }
            string content;
            if (message.Content == null) content = "A message was deleted but i could not get its content.";
            else content = message.Content;
            if (message.Embeds.Count > 0 || message.Attachments.Count > 0) content = "A message was deleted but i could not get its content(embed).";


            EmbedBuilder embed = new EmbedBuilder();
            embed.WithAuthor(message.Author)
                .WithDescription($"**Message deleted in <#{ channel.Id }>.**")
                .WithColor(Color.Red)
                .AddField("Content:", content)
                .WithFooter($"• { guild.Name }")
                .WithCurrentTimestamp();

            var ch = guild.GetTextChannel(Settings.logMessageDeleted);

            await ch.SendMessageAsync("", false, embed.Build());
        }

        public async Task MessageBulkDelete(IReadOnlyCollection<Cacheable<IMessage, ulong>> arg1, ISocketMessageChannel arg2)
        {
            var guild = GetThings.getGuildFromChannel(arg2);
            var Settings = await _db.GetGuildSettingsAsync(guild);
            if (Settings.logMessageDeleted == 0) return;
            Console.WriteLine("Event start");
            Console.WriteLine(arg1.Count.ToString());
            Console.WriteLine(arg2);
            Console.WriteLine("Event end");
        }
    }
}