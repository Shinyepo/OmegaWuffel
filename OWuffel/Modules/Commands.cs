using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using OWuffel.Extensions.Database;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWuffel.Modules
{
    public class Commands : ModuleBase<Cipska>
    {
        public Settings WuffelDB { get; set; }
        private WuffelDBContext _db;
        private Logger _logger;

        public Commands(IServiceProvider services)
        {
            _db = services.GetRequiredService<WuffelDBContext>();
            _logger = LogManager.GetCurrentClassLogger();
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
            
            int asd = 1;
            _logger.Info(asd);
            _logger.Info(asd.ToString());
            _logger.Info(Context.Settings.guild_id.ToString());
            _logger.Info(Context.Settings.id.ToString());
            
        }

        [Command("purge")]
        [RequireOwner]
        public async Task Purge(int arg = 0)
        {
            if (arg == 0) await ReplyAsync("Wrong argument.");
            else
            {
                IEnumerable<IMessage> messages = await Context.Channel.GetMessagesAsync(arg + 1).FlattenAsync();
                await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
                const int delay = 3000;
                IUserMessage m = await ReplyAsync($"I have deleted {arg} messages.");
                await Task.Delay(delay);
                await m.DeleteAsync();
            }
        }

        [Command("eval")]
        [RequireOwner]
        public async Task Eval(string arg)
        {
            await Eval(arg);
        }

        [Command("logs")]
        [Alias("logevents")]
        [RequireOwner]
        public Task logs()
        {
            var _ = Task.Run(async () =>
            {
                try
                {
                    var setting = Context.Settings
                        .GetType()
                        .GetProperties()
                        .Where(content => content.Name.Contains("log"))
                        .Select(pi => new
                        {
                            Name = pi.Name,
                            Value = pi.GetGetMethod().Invoke(Context.Settings, null)
                        });
                    foreach (var item in setting)
                    {
                        Console.WriteLine("Name: {0}", item.Name);
                        Console.WriteLine("Value: {0}", item.Value);
                    }
                }
                catch
                {
                    //dont care
                }
            });
            return Task.CompletedTask;

        }
    }
}
