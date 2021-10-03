//using Discord;
//using Discord.Commands;
//using Interactivity;
//using OWuffel.Extensions.Database;
//using OWuffel.Models;
//using OWuffel.Services;
//using OWuffel.Util;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace OWuffel.Modules.Tickets
//{
//    public class Support : ModuleBase<SocketCommandContext>
//    {
//        public InteractivityService intserv { get; set; }
//        public DatabaseUtilities db;

//        public Support(InteractivityService interactivityservice, DatabaseUtilities debe)
//        {
//            intserv = interactivityservice;
//            db = debe;
//        }
//        //private Task ActiveTicketsLoop()
//        //{
//        //    var _ = Task.Run(async () =>
//        //    {
//        //        while (true)
//        //        {

//        //        }
//        //        return Task.CompletedTask;
//        //    });
//        //    return Task.CompletedTask;

//        //}

//        [Command("ConfigureSupport", RunMode = RunMode.Async)]
//        [Alias("cs")]
//        [RequireUserPermission(GuildPermission.Administrator)]
//        public async Task ConfigureSupportAsync()
//        {
//            var config = await db.LookForConfigurationAsync(Context.Guild.Id);
//            if (config != null)
//            {
//                var me = await ReplyAsync("Support module configuration is already present in the database. If you want to override whole configuration please react with ✅. React");
//                var yes = new Emoji("✅");
//                await me.AddReactionAsync(yes);
//                var res = await intserv.NextReactionAsync(timeout: TimeSpan.FromMinutes(2));
//                if (res.Value == null)
//                {
//                    await ReplyAsync($"{Context.User.Mention}, i grew impatient of waiting for response and i timedout.");
//                    return;
//                }
//                if (res.Value.Emote.Name != yes.Name)
//                {
//                    return;
//                }
//            }


//            var msg = await ReplyAsync("What type of configuration do you want to proceed with? React with 🇦 for Automatic or 🇲 for Manual.");
//            var a = new Emoji("🇦");
//            var m = new Emoji("🇲");
//            await msg.AddReactionAsync(a);
//            await msg.AddReactionAsync(m);
//            var response = await intserv.NextReactionAsync();
//            if (response.Value.Emote.Name == a.Name)
//            {
//                //if (Context.Guild.CategoryChannels.SingleOrDefault(cat => cat.Name.ToLower() == "support") != null)
//                //{
//                //    await ReplyAsync("There already is category called \"Support\", please rename/delete existing category or run the manual configuration.");
//                //    return;
//                //}
//                var conf = new SupportConfiguration();
//                var cat = await Context.Guild.CreateCategoryChannelAsync("Support");
//                var channelperm = new OverwritePermissions(sendMessages: PermValue.Deny);
//                var catperm = new OverwritePermissions(sendMessages: PermValue.Deny, viewChannel: PermValue.Deny);
//                var ticketperm = new OverwritePermissions(connect: PermValue.Deny);
//                await cat.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, permissions: catperm);
//                var activetickets = await Context.Guild.CreateVoiceChannelAsync("Active Tickets: 0", c => c.CategoryId = cat.Id);
//                await activetickets.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, permissions: ticketperm);
//                var chnl = await Context.Guild.CreateTextChannelAsync("Contact-Support", c => c.CategoryId = cat.Id);
//                await chnl.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, permissions: channelperm);
                

//                var em = new EmbedBuilder()
//                    .WithTitle(Context.Guild.Name + " Support")
//                    .WithColor(Color.Green)
//                    .WithDescription($"Welcome to **{Context.Guild.Name}'s** support center.\n\nIn order to create new ticket simply click on reaction(📩) below this message.");
//                var envelope = new Emoji("📩");
//                var mainmsg = await chnl.SendMessageAsync(embed: em.Build());
//                await mainmsg.AddReactionAsync(envelope);

//                conf.ParentId = cat.Id;
//                conf.TicketInfoId = chnl.Id;
//                conf.ActiveTicketsId = activetickets.Id;
//                conf.MessageId = mainmsg.Id;
//                conf.GuildId = Context.Guild.Id;
//                conf.Notify = 1;
//                conf.PremiumStatus = 0;
//                conf.TicketMessage = "";
//                var createdconfig = await db.CreateSupportConfigurationAsync(conf);
//                await ReplyAsync($"**Configuration completed successfully**\n\nI have created category called **{cat.Name}** and set some basic permissions for it.\nIf you want to allow more roles or users to view and reply to tickets please edit category's permissions. Add wanted roles/users, set View Channel and Send Messages permissions to true.\nEvery new ticket created by user will synchronize its channel permissions with category permissions.\n\nAdditionally i created {chnl.Mention} channel. Everyone can see this channel and by reacting with \"📩\" to the message the user will create new ticket. If you want to change the message in this channel please use **\"{Context.Settings.botPrefix}csm message\"**.\nEvery new ticket has a default message. You can change this message by using this command **\"{Context.Settings.botPrefix}tm message\"**");
//                return;
//            }
//        }
//        [Command("contactsupportmessage")]
//        [Alias("csm")]
//        [RequireUserPermission(GuildPermission.Administrator)]
//        public async Task CSMAsync([Remainder] string message)
//        {
//            var config = await db.LookForConfigurationAsync(Context.Guild.Id);
//            var msg = await Context.Guild.GetTextChannel(config.TicketInfoId).GetMessageAsync(config.MessageId) as IUserMessage;
//            var em = msg.Embeds.First().ToEmbedBuilder();
//            em.Description = message;

//            await msg.ModifyAsync(m => m.Embed = em.Build());
//            await ReplyAsync("Message has been changed successfully.");
//        }
//        [Command("ticketmessage")]
//        [Alias("tm")]
//        [RequireUserPermission(GuildPermission.Administrator)]
//        public async Task TMAsync([Remainder] string message)
//        {
//            if (message.ToLower() == "default" || message.ToLower() == "reset") message = "";
//            var config = await db.ChangeTicketMessageAsync(Context.Guild.Id, message);
//            if (config.Id == 0)
//            {
//                await ReplyAsync("There is no support module configuration.");
//                return;
//            }
//            await ReplyAsync("Message for new tickets has been changed successfully.");
//        }
//        [Command("TicketsStats")]
//        [Alias("ts", "ticketstat","ticketstats")]
//        [RequireUserPermission(GuildPermission.Administrator)]
//        public async Task NumberofActiveTicketsAsync()
//        {
//            // 0 - active, 1 - closed, 2 - total
//            var list = await db.TicketStatsAsync(Context.Guild.Id);
//            var embed = new EmbedBuilder()
//                .WithTitle("Statistics for tickets")
//                .WithColor(Color.Green)
//                .WithDescription($"Total number of tickets: **{list[2]}**\nActive tickets: **{list[0]}**\nClosed tickets: **{list[1]}**");
//            await ReplyAsync(embed: embed.Build());
//        }
//    }
//}
