using Discord;
using Discord.Commands;
using Interactivity;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Modules.Tickets
{
    public class Support: ModuleBase<Cipska>
    {
        public InteractivityService intserv { get; set; }
        public DatabaseUtilities db;

        public Support(InteractivityService interactivityservice, DatabaseUtilities debe)
        {
            intserv = interactivityservice;
            db = debe;
        }

        [Command("ConfigureSupport", RunMode = RunMode.Async)]
        [Alias("cs")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task ConfigureSupportAsync()
        {
            var config = await db.LookForConfigurationAsync(Context.Guild.Id);
            if (config != null)
            {
                var me = await ReplyAsync("Support module configuration is already present in the database. If you want to override whole configuration please react with ✅.");
                var yes = new Emoji("✅");
                await me.AddReactionAsync(yes);
                var res = await intserv.NextReactionAsync();
                if (res.Value.Emote.Name != yes.Name)
                {
                    return;
                }
            }


            var msg = await ReplyAsync("What type of configuration do you want to proceed with? React with 🇦 for Automatic or 🇲 for Manual.");
            var a = new Emoji("🇦");
            var m = new Emoji("🇲");
            await msg.AddReactionAsync(a);
            await msg.AddReactionAsync(m);
            var response = await intserv.NextReactionAsync();
            if (response.Value.Emote.Name == a.Name)
            {
                if (Context.Guild.CategoryChannels.SingleOrDefault(cat => cat.Name.ToLower() == "support") != null)
                {
                    await ReplyAsync("There already is category called \"Support\", please rename/delete existing category or run the manual configuration.");
                    return;
                }
                var conf = new SupportConfiguration();
                var cat = await Context.Guild.CreateCategoryChannelAsync("Support");
                var perm = new OverwritePermissions(sendMessages: PermValue.Deny);
                await cat.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, permissions: perm);
                var chnl = await Context.Guild.CreateTextChannelAsync("Contact-Support", c => c.CategoryId = cat.Id);
                //await chnl.ModifyAsync(c => c.CategoryId = cat.Id);
                var em = new EmbedBuilder()
                    .WithTitle(Context.Guild.Name + " Support")
                    .WithColor(Color.Green)
                    .WithDescription($"Welcome to **{Context.Guild.Name}'s** support center.\n\nIn order to create new ticket simply click on reaction(📩) below this message.");
                var envelope = new Emoji("📩");
                var mainmsg = await chnl.SendMessageAsync(embed: em.Build());
                await mainmsg.AddReactionAsync(envelope);

                conf.ParentId = cat.Id;
                conf.TicketInfoId = chnl.Id;
                conf.MessageId = mainmsg.Id;
                conf.GuildId = Context.Guild.Id;
                conf.Notify = 1;
                conf.PremiumStatus = 0;
                conf.TicketMessage = "";
                var createdconfig = await db.CreateSupportConfigurationAsync(conf);                
                return;
            }
                

        }
    }
}
