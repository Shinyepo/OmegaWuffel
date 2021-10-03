using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using OWuffel.Extensions.Database;
using OWuffel.Models;
using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Modules.DatabaseCommands
{
    //public class SetDB : ModuleBase<SocketCommandContext>
    //{
    //    public BotSettings WuffelDB { get; set; }
    //    private WuffelDBContext _db;

    //    public SetDB(IServiceProvider services)
    //    {
    //        _db = services.GetRequiredService<WuffelDBContext>();
    //    }

    //    [Command("setdb")]
    //    [RequireOwner]
    //    public async Task setdbAsync(string key, string value)
    //    {
    //        if (Context.Settings == null) return;
    //        if (key == "" || value == "") return;
    //        var getvalue = Context.Settings.GetType().GetProperty(key).PropertyType;

    //        if (getvalue == typeof(UInt64))
    //        {
    //            var convertedvalue = Convert.ToUInt64(value);
    //            Context.Settings.GetType().GetProperty(key).SetValue(Context.Settings, convertedvalue);
    //            await _db.SaveChangesAsync();
    //            await ReplyAsync($"Changed {key} to {value}");

    //        }
    //        else if (getvalue == typeof(String))
    //        {
    //            Context.Settings.GetType().GetProperty(key).SetValue(Context.Settings, value);
    //            await _db.SaveChangesAsync();
    //            await ReplyAsync($"Changed {key} to {value}");
    //        }



    //    }
    //}
}
