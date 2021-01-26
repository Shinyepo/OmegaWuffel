using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OWuffel.util;

namespace OWuffel.events
{
    class MessageEvents
    {
        public async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            var message = await before.GetOrDownloadAsync();
            string beforereply;

            if (message.Embeds.Count != 0) return;
            if (after.Embeds.Count != 0) return;
            if (message.Author.IsBot) return;

            if (message.Content == after.Content) beforereply = "A message was edited but i could not get its content before change.";
            else beforereply = message.Content;

            var guild = GetThings.getGuildFromChannel(channel);

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithAuthor(message.Author)
                .WithDescription($"[Message update](https://discordapp.com/channels/{ guild.Id }/{ channel.Id }/{ message.Id })\n**Message edited by { message.Author } in <#{ channel.Id }>.**")
                .WithColor(Color.Blue)
                .AddField("Before:", beforereply)
                .AddField("After:", after)
                .WithFooter($"• { guild.Name }")
                .WithCurrentTimestamp();
            var ch = guild.GetTextChannel(793988656074588210);
            
            await ch.SendMessageAsync("", false, embed.Build());
        }

        public async Task MessageDelete(Cacheable<IMessage, ulong> msg, ISocketMessageChannel channel)
        {
            var message = await msg.GetOrDownloadAsync();
            if (message == null) return;
            if (message.Author.IsBot) return;


            string content;
            if (message.Content == null) content = "A message was deleted but i could not get its content.";
            else content = message.Content;
            if (message.Embeds.Count > 0 || message.Attachments.Count > 0) content = "A message was deleted but i could not get its content(embed).";

            var guild = GetThings.getGuildFromChannel(channel);

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithAuthor(message.Author)
                .WithDescription($"[Message deleted](https://discordapp.com/channels/{ guild.Id }/{ channel.Id })\n**Message deleted by { message.Author } in <#{ channel.Id }>.**")
                .WithColor(Color.Red)
                .AddField("Content:", content)
                .WithFooter($"• { guild.Name }")
                .WithCurrentTimestamp();

            var ch = guild.GetTextChannel(793988656074588210);

            await ch.SendMessageAsync("", false, embed.Build());
        }

        public async Task MessageBulkDelete(IReadOnlyCollection<Cacheable<IMessage, ulong>> arg1, ISocketMessageChannel arg2)
        {
            Console.WriteLine("Event start");
            Console.WriteLine(arg1.Count.ToString());
            Console.WriteLine(arg2);
            Console.WriteLine("Event end");
        }
    }
}