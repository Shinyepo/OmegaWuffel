using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OWuffel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Util
{
    public static class EmbedDeserializer
    {
        public static async Task<string> PlaceholderReplacer(string str, SocketGuildUser user)
        {
            string result = str;
            result = result.Replace("%user.username%", user.Username.ToString());
            result = result.Replace("%user.fullusername%", user.ToString());
            result = result.Replace("%user.mention%", "<@" + user.Id.ToString() + ">");
            result = result.Replace("%user.createddate%", user.CreatedAt.ToString());
            result = result.Replace("%user.createdtime%", user.CreatedAt.ToLocalTime().ToString());
            result = result.Replace("%user.avatarurl%", user.GetAvatarUrl() != null ? user.GetAvatarUrl().ToString() : user.GetDefaultAvatarUrl().ToString());
            result = result.Replace("%user.id%", user.Id.ToString());
            result = result.Replace("%user.joineddate%", user.JoinedAt.Value.ToString());
            result = result.Replace("%user.joinedtime%", user.JoinedAt.Value.ToLocalTime().ToString());
            result = result.Replace("%user.nickname%", user.Nickname != null ? user.Nickname.ToString() : user.Username.ToString());
            return result;
        }
        public static async Task<string> PlaceholderReplacer(string str, SocketGuild guild)
        {
            string result = str;
            result = result.Replace("%guild.name%", guild.Name.ToString());
            result = result.Replace("%guild.id%", guild.Id.ToString());
            result = result.Replace("%guild.createddate%", guild.CreatedAt.ToString());
            result = result.Replace("%guild.createdtime%", guild.CreatedAt.ToLocalTime().ToString());
            result = result.Replace("%guild.membercount%", guild.MemberCount.ToString());
            result = result.Replace("%guild.owner%", guild.Owner.ToString());
            result = result.Replace("%guild.iconurl%", guild.IconUrl != null ? guild.IconUrl.ToString() : guild.CurrentUser.GetAvatarUrl().ToString());
            result = result.Replace("%guild.ownermention%", "<@" + guild.Owner.Id.ToString() + ">");
            return result;
        }

        public static async Task<EmbedBuilder> PrepareEmbed(string json)
        {
            string prepared = json;
            var builtEmbed = await DeserializeEmbedFromJson(prepared);
            return builtEmbed;
        }

        public static async Task<EmbedBuilder> PrepareEmbed(string json, SocketGuildUser user)
        {
            string prepared = json;
            if (json.Contains("%")) prepared = await PlaceholderReplacer(json, user);

            var builtEmbed = await DeserializeEmbedFromJson(prepared);
            return builtEmbed;
        }

        public static async Task<EmbedBuilder> PrepareEmbed(string json, SocketGuild guild)
        {
            string prepared = json;
            if (json.Contains("%")) prepared = await PlaceholderReplacer(json, guild);

            var builtEmbed = await DeserializeEmbedFromJson(prepared);
            return builtEmbed;
        }
        public static async Task<EmbedBuilder> PrepareEmbed(string json, SocketGuild guild, SocketGuildUser user)
        {
            string prepared = json;
            if (json.Contains("%"))
            {
                prepared = await PlaceholderReplacer(json, guild);
                prepared = await PlaceholderReplacer(prepared, user);
            }

            var builtEmbed = await DeserializeEmbedFromJson(prepared);
            return builtEmbed;
        }

        public static async Task<EmbedBuilder> DeserializeEmbedFromJson(string json)
        {
            //var path = Path.Combine("EmbedJsons", "502565740491046912.json");
            EmbedBuilder em = new EmbedBuilder();
            try
            {
                //var deserialized = JsonConvert.DeserializeObject<EmbedModel>(File.ReadAllText(path));
                var deserialized = JsonConvert.DeserializeObject<EmbedModel>(json);

                em.Title = deserialized.title != null ? deserialized.title : "";
                em.Description = deserialized.description != null ? deserialized.description : "";
                em.Color = deserialized.color != null ? (Color)System.Drawing.Color.FromArgb(deserialized.color) : Color.Blue;
                if (deserialized.fields != null)
                {
                    if (deserialized.fields.Count > 0)
                    {
                        foreach (var item in deserialized.fields)
                        {
                            em.AddField(item["name"].ToString(), item["value"], (bool)item["inline"]);
                        }
                    }
                }
                if (deserialized.footer != null)
                {
                    if (deserialized.footer.Count > 0)
                    {
                        em.WithFooter(deserialized.footer["text"].ToString(), deserialized.footer["icon_url"].ToString());
                    }
                }
                if (deserialized.author != null)
                {
                    if (deserialized.author.Count > 0 && deserialized.author != null)
                    {
                        em.WithAuthor(deserialized.author["name"].ToString(), deserialized.author["icon_url"].ToString());
                    }
                }
                em.ImageUrl = deserialized.image != null ? deserialized.image : "";
                em.ThumbnailUrl = deserialized.thumbnail != null ? deserialized.thumbnail : "";

                return em;
            }
            catch
            {
                //Console.WriteLine(ex);
            }
            return em;
        }
        /*
        public async Task<JArray> JArrayHelper(List<JArrayHelper> list)
        {

        }
        public async Task<JObject> JObjectHelper(List<JObjectHelper> list)
        {
            return list;
        }*/
        public static async Task SendEmbedFromModelAsync(EmbedModel model, ITextChannel channel)
        {
            try
            {
                EmbedBuilder em = new EmbedBuilder();
                var a = new EmbedModel();
                a.fields = new JArray("name", "value" , "");
                em.Title = model.title != null ? model.title : "";
                em.Description = model.description != null ? model.description : "";
                em.Color = model.color != null ? (Color)System.Drawing.Color.FromArgb(model.color) : Color.Blue;
                if (model.fields != null)
                {
                    if (model.fields.Count > 0)
                    {
                        foreach (var item in model.fields)
                        {
                            em.AddField(item["name"].ToString(), item["value"], (bool)item["inline"]);
                        }
                    }
                }
                if (model.footer != null)
                {
                    if (model.footer.Count > 0)
                    {
                        em.WithFooter(model.footer["text"].ToString(), model.footer["icon_url"].ToString());
                    }
                }
                if (model.author != null)
                {
                    if (model.author.Count > 0 && model.author != null)
                    {
                        em.WithAuthor(model.author["name"].ToString(), model.author["icon_url"].ToString());
                    }
                }
                em.ImageUrl = model.image != null ? model.image : "";
                em.ThumbnailUrl = model.thumbnail != null ? model.thumbnail : "";

                await channel.SendMessageAsync(embed: em.Build());
            }
            catch
            {
                //Console.WriteLine(ex);
            }
        }
    }
}
