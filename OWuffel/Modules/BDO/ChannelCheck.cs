using Discord;
using Discord.Commands;
using Interactivity;
using Newtonsoft.Json;
using OWuffel.Extensions.Database;
using OWuffel.Models;
using OWuffel.Services;
using OWuffel.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Modules.BDO
{
    public class ChannelCheck : ModuleBase<Cipska>
    {
        private DatabaseUtilities _db;
        public InteractivityService _intserv { get; set; }

        public ChannelCheck(DatabaseUtilities db, InteractivityService intserv)
        {
            _db = db;
            _intserv = intserv;
        }

        [Command("find")]
        [Alias("f", "channelcheck", "frajer")]
        public async Task findfrajer(string nick)
        {
            await Context.Message.DeleteAsync();
            var channelcheck = new ChannelCheckModel();
            channelcheck.AuthorId = Context.User.Id;
            channelcheck.Target = nick;
            channelcheck.ChannelId = Context.Message.Channel.Id;
            channelcheck.Timestamp = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            channelcheck.GuildId = Context.Guild.Id;
            channelcheck.Status = 1;
            var model = new CheckModel();
            var ser = JsonConvert.SerializeObject(model);
            channelcheck.Channels = ser.ToString();


            var insChannelCheck = await _db.CreateChannelCheckAsync(channelcheck);

            var channels = "```\n--------------\n1:\n2:\n3:\n4:\n5:\n6:```";
            var elviachannels4 = "```\n--------------\n1:\n2:\n3*:\n4*:\n5*:\n6*:```";
            var elviachannels3 = "```\n--------------\n1:\n2:\n3:\n4*:\n5*:\n6*:```";
            var smallchannels = "```\n--------------\n1:\n2:\n3:\n4:```";
            var em = new EmbedBuilder()
                .WithAuthor(Context.User)
                .WithColor(Color.Blue)
                .WithDescription($"Poszukiwania frajera o nicku: **{nick}**.")
                .AddField("Balenos", channels, true)
                .AddField("Calpheon", elviachannels4, true)
                .AddField("Mediah", elviachannels4, true)
                .AddField("Serendia", elviachannels4, true)
                .AddField("Valencia", elviachannels3, true)
                .AddField("Velia", channels, true)
                .AddField("Grana", smallchannels, true)
                .AddField("Arsha", "```\n--------------\n1*:```", true)
                .AddField("Znaleziony?", "**NIE**", true)
                .AddField("Legenda:", "* - oznaczenie serwera typu Elvia", false)
                .WithFooter($"Id embeda: {insChannelCheck.Id}")
                .WithCurrentTimestamp();


            var msg = await ReplyAsync(embed: em.Build());
            insChannelCheck.MessageId = msg.Id;
            await _db.ChannelCheckUpdateMessageIdAsync(insChannelCheck);
        }
        [Command("channel")]
        [Alias("chck", "check")]
        public async Task ChannelCheckAsync(int id, string channel, [Remainder]string status)
        {
            await Context.Message.DeleteAsync();
            var channelcheck = await _db.GetChannelCheckAsync(id);
            if (channelcheck == null)
            {
                await ReplyAsync("Nie ma aktywnego eventu z takim id.");
                return;
            }
            var balenosalias = new List<string> { "b", "bal", "balenos" };
            var calpheonalias = new List<string> { "c", "cal", "calpheon", "kapelon", "ciapelon" };
            var mediahalias = new List<string> { "m", "med", "mediah", "medijah" };
            var serendiaalias = new List<string> { "s", "ser", "serendia", "serenada", "serendija" };
            var valenciaalias = new List<string> { "val", "valencia", "pustynia", "desert" };
            var veliaalias = new List<string> { "vel", "velia", "veliia" };
            var granaalias = new List<string> { "k", "kama", "kamasylvia", "grana", "g", "gra" };
            var arsha = new List<string> { "a", "arsha", "pvp" };

            var msg = await Context.Guild.GetTextChannel(channelcheck.ChannelId).GetMessageAsync(channelcheck.MessageId) as IUserMessage;
            if (msg == null) return;

            var em = msg.Embeds.First().ToEmbedBuilder();
            em.Fields.Clear();
            CheckModel model = JsonConvert.DeserializeObject<CheckModel>(channelcheck.Channels);
            var channelnumber = channel[channel.Length - 1].ToString();
            var channelalias = channel.ToLower().Remove(channel.Length - 1, 1);
            if (channelalias == "")
            {
                await ReplyAsync("Nie ma takiego kanalu głupku");
                return;
            }
            int number = Convert.ToInt32(channelnumber);
            if (number < 1 || number > 6)
            {
                await ReplyAsync("Nie ma takiego kanalu głupku");
                return;
            }
            if (balenosalias.Contains(channelalias))
            {
                model.balenos[number.ToString()] = status;
            }
            else if (calpheonalias.Contains(channelalias))
            {
                model.calpheon[number.ToString()] = status;
            }
            else if (mediahalias.Contains(channelalias))
            {
                model.mediah[number.ToString()] = status;
            }
            else if (serendiaalias.Contains(channelalias))
            {
                model.serendia[number.ToString()] = status;
            }
            else if (valenciaalias.Contains(channelalias))
            {
                model.valencia[number.ToString()] = status;
            }
            else if (veliaalias.Contains(channelalias))
            {
                model.velia[number.ToString()] = status;
            }
            else if (granaalias.Contains(channelalias))
            {
                if (number > 4) return;
                model.grana[number.ToString()] = status;
            }
            else if (arsha.Contains(channelalias))
            {
                if (number > 1) return;
                model.arsha[number.ToString()] = status;
            }
            else
            {
                await ReplyAsync("jestes glupi, nie ma takiego kanalu.");
            }

            var balenosfield = "```\n--------------\n";
            int i = 1;
            foreach (var item in model.balenos)
            {
                balenosfield += i + ": " + item.Value + "\n";
                i++;
            }
            balenosfield += "```";

            var calpheonfield = "```\n--------------\n";
            i = 1;
            foreach (var item in model.calpheon)
            {
                if (i > 2)
                {
                    calpheonfield += i + "*: " + item.Value + "\n";
                    i++;
                    continue;
                }
                calpheonfield += i + ": " + item.Value + "\n";
                i++;
            }
            calpheonfield += "```";

            var mediahfield = "```\n--------------\n";
            i = 1;
            foreach (var item in model.mediah)
            {
                if (i > 2)
                {
                    mediahfield += i + "*: " + item.Value + "\n";
                    i++;
                    continue;
                }
                mediahfield += i + ": " + item.Value + "\n";
                i++;
            }
            mediahfield += "```";

            var serendiafield = "```\n--------------\n";
            i = 1;
            foreach (var item in model.serendia)
            {
                if (i > 2)
                {
                    serendiafield += i + "*: " + item.Value + "\n";
                    i++;
                    continue;
                }
                serendiafield += i + ": " + item.Value + "\n";
                i++;
            }
            serendiafield += "```";

            var valenciafield = "```\n--------------\n";
            i = 1;
            foreach (var item in model.valencia)
            {
                if (i > 3)
                {
                    valenciafield += i + "*: " + item.Value + "\n";
                    i++;
                    continue;
                }
                valenciafield += i + ": " + item.Value + "\n";
                i++;
            }
            valenciafield += "```";

            var veliafield = "```\n--------------\n";
            i = 1;
            foreach (var item in model.velia)
            {
                veliafield += i + ": " + item.Value + "\n";
                i++;
            }
            veliafield += "```";

            var granafield = "```\n--------------\n";
            i = 1;
            foreach (var item in model.grana)
            {
                granafield += i + ": " + item.Value + "\n";
                i++;
            }
            granafield += "```";

            var arshafield = "```\n--------------\n";
            i = 1;
            foreach (var item in model.arsha)
            {
                arshafield += i + "*: " + item.Value + "\n";
                i++;
            }
            arshafield += "```";

            em.AddField("Balenos", balenosfield, true);
            em.AddField("Calpheon", calpheonfield, true);
            em.AddField("Mediah", mediahfield, true);
            em.AddField("Serendia", serendiafield, true);
            em.AddField("Valencia", valenciafield, true);
            em.AddField("Velia", veliafield, true);
            em.AddField("Grana", granafield, true);
            em.AddField("Arsha", arshafield, true);
            
            if (status.ToLower() == "bingo" || status.ToLower() == "tak" || status.ToLower() == "znaleziony")
            {
                em.AddField("Znaleziony?", "Znaleziony na **"+channel+"**", true);
                channelcheck.Status = 0;
            } else
            {
                em.AddField("Znaleziony?", "**NIE**", true);
            }
            em.AddField("Legenda:", "* - oznaczenie serwera typu Elvia", false);
            await msg.ModifyAsync(m => m.Embed = em.Build());
            channelcheck.Channels = JsonConvert.SerializeObject(model);
            await _db.UpdateCheckChannelsAsync(channelcheck);
        }
    }
}
