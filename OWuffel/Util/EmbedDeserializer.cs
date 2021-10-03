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
                var d = deserialized.Fields;
                em.Title = deserialized.Title != null ? deserialized.Title : "";
                em.Description = deserialized.Description != null ? deserialized.Description : "";
                em.Color = deserialized.Color != 0 ? (Color)System.Drawing.Color.FromArgb(deserialized.Color) : Color.Blue;
                if (deserialized.Fields != null)
                {
                    if (deserialized.Fields.Count > 0)
                    {
                        foreach (var item in deserialized.Fields)
                        {
                            em.AddField(item["name"].ToString(), item["value"], (bool)item["inline"]);
                        }
                    }
                }
                if (deserialized.Footer != null)
                {
                    if (deserialized.Footer.Count > 0)
                    {
                        em.WithFooter(deserialized.Footer["text"].ToString(), deserialized.Footer["icon_url"].ToString());
                    }
                }
                if (deserialized.Author != null)
                {
                    if (deserialized.Author.Count > 0 && deserialized.Author != null)
                    {
                        em.WithAuthor(deserialized.Author["name"].ToString(), deserialized.Author["icon_url"].ToString());
                    }
                }
                em.ImageUrl = deserialized.Image != null ? deserialized.Image : "";
                em.ThumbnailUrl = deserialized.Thumbnail != null ? deserialized.Thumbnail : "";

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
                a.Fields = new JArray("name", "value" , "");
                em.Title = model.Title != null ? model.Title : "";
                em.Description = model.Description != null ? model.Description : "";
                em.Color = model.Color != null ? (Color)System.Drawing.Color.FromArgb(model.Color) : Color.Blue;
                if (model.Fields != null)
                {
                    if (model.Fields.Count > 0)
                    {
                        foreach (var item in model.Fields)
                        {
                            em.AddField(item["name"].ToString(), item["value"], (bool)item["inline"]);
                        }
                    }
                }
                if (model.Footer != null)
                {
                    if (model.Footer.Count > 0)
                    {
                        em.WithFooter(model.Footer["text"].ToString(), model.Footer["icon_url"].ToString());
                    }
                }
                if (model.Author != null)
                {
                    if (model.Author.Count > 0 && model.Author != null)
                    {
                        em.WithAuthor(model.Author["name"].ToString(), model.Author["icon_url"].ToString());
                    }
                }
                em.ImageUrl = model.Image != null ? model.Image : "";
                em.ThumbnailUrl = model.Thumbnail != null ? model.Thumbnail : "";

                await channel.SendMessageAsync(embed: em.Build());
            }
            catch
            {
                //Console.WriteLine(ex);
            }
        }
    }
}
