using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWuffel.Modules.Commands
{
    public class Commands : ModuleBase<Cipska>
    {
        public Settings WuffelDB { get; set; }
        private WuffelDBContext _db;

        public Commands(IServiceProvider services)
        {
            _db = services.GetRequiredService<WuffelDBContext>();
        }

        [Command("ark")]
        [RequireOwner]
        public async Task Ark(string arg = null)
        {
            if (arg == null) await ReplyAsync("Nie podałeś nazwy dinozaura do przejęcia");
            else {
                await ReplyAsync($"Zaczynam poszukiwania -> {arg}");
                await Task.Delay(5 * 60 * 1000);
                await ReplyAsync($"Znalazlem {arg}!");
                await Task.Delay(3 * 60 * 1000);
                await ReplyAsync($"Powalilem {arg}! Zaczynam karmienie!");
                await Task.Delay(10 * 60 * 1000);
                await ReplyAsync($"Udało się! Przejąłem {(string)arg}. Zaczynam powrót do bazy!");
                
            }
        }

        [Command("s")]
        [RequireOwner]
        public async Task S()
        {
            Log.Info(Context.Client.Guilds.Count.ToString());
            Log.Info("Dupa");
            /*var avatar = await DownloadUploadImage.DiscordAvatarMagicAsync(new Uri(Context.User.GetAvatarUrl()));
            EmbedBuilder embed = new EmbedBuilder()
                .WithImageUrl(avatar);

            await ReplyAsync(null, false, embed.Build());*/
        }

        [Command("Detonacja")]
        [RequireOwner]
        public async Task Detonacja(ulong id)
        {
            await Context.Client.GetGuild(id).LeaveAsync();
            return;
        }

        [Command("eval")]
        [RequireOwner]
        public async Task Eval([Remainder]string arg)
        {
            await Eval(arg);
        }
    }
}
