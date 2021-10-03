//using Discord;
//using Discord.Addons.Interactive;
//using Discord.Commands;
//using Interactivity;
//using Interactivity.Pagination;
//using Newtonsoft.Json;
//using OWuffel.Services;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace OWuffel.Modules.BDO
//{
//    class Members
//    {
//        public string Name { get; set; }
//        public string ProfileUrl { get; set; }
//    }
//    class Characters
//    {
//        public string Name { get; set; }
//        public string ClassName { get; set; }
//        public string Level { get; set; }
//        public string Contribution { get; set; }
//        public List<string> LifeSkills { get; set; }
//    }

//    public class WebsiteAPI : ModuleBase<SocketCommandContext>
//    {

//        public InteractiveService inbase { get; set; }
//        public InteractivityService intserv { get; set; }
//        public Dictionary<string, string> ClassIcon = new Dictionary<string, string>();

//        public WebsiteAPI(InteractiveService sv, InteractivityService inserv)
//        {
//            inbase = sv;
//            intserv = inserv;
//            ClassIcon.Add("Berserker", "https://cdn.discordapp.com/attachments/750774776188108941/817482511146811434/tIFPCUQ.png");
//            ClassIcon.Add("Ranger", "https://cdn.discordapp.com/attachments/750774776188108941/817482587710685194/uXL4qWS.png");
//            ClassIcon.Add("Sorceress", "https://cdn.discordapp.com/attachments/750774776188108941/817482665162571866/zkh3UYm.png");
//            ClassIcon.Add("Tamer", "https://cdn.discordapp.com/attachments/750774776188108941/817482719482347560/QfNM7sR.png");
//            ClassIcon.Add("Valkyrie", "https://cdn.discordapp.com/attachments/750774776188108941/817482770611437588/gnq2xoQ.png");
//            ClassIcon.Add("Warrior", "https://cdn.discordapp.com/attachments/750774776188108941/817482840135565382/faN7IEq.png");
//            ClassIcon.Add("Witch", "https://cdn.discordapp.com/attachments/750774776188108941/817482889519693834/wrGUwr8.png");
//            ClassIcon.Add("Wizard", "https://cdn.discordapp.com/attachments/750774776188108941/817482934423519322/5Akw6al.png");
//            ClassIcon.Add("Musa", "https://cdn.discordapp.com/attachments/750774776188108941/817483060299300924/WaPWiUu.png");
//            ClassIcon.Add("Maehwa", "https://cdn.discordapp.com/attachments/750774776188108941/817483110471172144/LUreDoO.png");
//            ClassIcon.Add("Ninja", "https://cdn.discordapp.com/attachments/750774776188108941/817483168524795965/edoK0qE.png");
//            ClassIcon.Add("Kunoichi", "https://cdn.discordapp.com/attachments/750774776188108941/817483223549345982/oeDzRyl.png");
//            ClassIcon.Add("Dark Knight", "https://cdn.discordapp.com/attachments/750774776188108941/817483275849302089/volHSTx.png");
//            ClassIcon.Add("Striker", "https://cdn.discordapp.com/attachments/750774776188108941/817483322132922448/x6XgpbV.png");
//            ClassIcon.Add("Mystic", "https://cdn.discordapp.com/attachments/750774776188108941/817483378437128252/LrcrxLy.png");
//            ClassIcon.Add("Lahn", "https://cdn.discordapp.com/attachments/750774776188108941/817483458372304956/G480zoA.png");
//            ClassIcon.Add("Archer", "https://cdn.discordapp.com/attachments/750774776188108941/817483519483183104/IMco5O1.png");
//            ClassIcon.Add("Shai", "https://cdn.discordapp.com/attachments/750774776188108941/817483573266481272/ApLRlFp.png");
//            ClassIcon.Add("Guardian", "https://cdn.discordapp.com/attachments/750774776188108941/817483620604182646/7dR36tD.png");
//            ClassIcon.Add("Hashashin", "https://cdn.discordapp.com/attachments/750774776188108941/817483667378274344/AYDj4Bx.png");
//            ClassIcon.Add("Nova", "https://cdn.discordapp.com/attachments/750774776188108941/817483714094170172/GZUjWIP.png");
//            ClassIcon.Add("Sage", "https://cdn.discordapp.com/attachments/812328100988977166/822220102493274162/sage.png");
//            ClassIcon.Add("Corsair", "https://cdn.discordapp.com/attachments/812328100988977166/857366915261136957/oeDzRyl.png");
//        }

//        [Command("familyname", RunMode = RunMode.Async)]
//        [Alias("fn")]
//        public async Task FindFamilyName(string name, [Optional] string region)
//        {
//            try
//            {
//                var urlAddress = $"https://www.naeu.playblackdesert.com/en-US/Adventure?region=EU&searchType=2&searchKeyword={name}";
//                if (region != null)
//                {
//                    if (region.ToLower() == "eu") urlAddress = $"https://www.naeu.playblackdesert.com/en-US/Adventure?region=EU&searchType=2&searchKeyword={name}";
//                    if (region.ToLower() == "na") urlAddress = $"https://www.naeu.playblackdesert.com/en-US/Adventure?region=NA&searchType=2&searchKeyword={name}";
//                }
//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
//                WebHeaderCollection myWebHeaderCollection = request.Headers;
//                myWebHeaderCollection.Add("Content-Type", "text/plain");
//                myWebHeaderCollection.Add("User-Agent", "Discord");
//                myWebHeaderCollection.Add("Host", "www.naeu.playblackdesert.com");
//                var tempmsg = await ReplyAsync("Fetching profile data...");
//                var respons = await request.GetResponseAsync();
//                HttpWebResponse response = (HttpWebResponse)respons;
//                if (response.StatusCode == HttpStatusCode.OK)
//                {
//                    await tempmsg.DeleteAsync();
//                    Stream receiveStream = response.GetResponseStream();
//                    StreamReader readStream = null;
//                    if (response.CharacterSet == null)
//                        readStream = new StreamReader(receiveStream);
//                    else
//                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
//                    string data = readStream.ReadToEnd();

//                    var first = data.Split("<div class=\"box_list_area\">")[1].Split("</ul>");
//                    var sec = first[0];
//                    if (sec.Contains("<li class=\"no_result\">"))
//                    {
//                        await ReplyAsync("No search results.");
//                        return;
//                    }
//                    var regiongot = sec.Split("<span class=\"region_info")[1].Split(">")[1].Split("<")[0];
//                    var href = sec.Split("<a href=\"")[1].Split("\">");
//                    var profilelink = href[0];
//                    var familyname = href[1].Split("</a>")[0];
//                    var lvlstart = href[7].Split("</span>")[0];
//                    var lvl = lvlstart.Replace(" ", "").Trim();
//                    var charactername = href[8].Split("</span>")[0];
//                    var classame = href[11].Split("</span>")[0];
//                    var guildinfo = sec.Split("<div class=\"state\">")[1];
//                    var guildlink = guildinfo.Split("<a href=\"")[1].Split("\">")[0];
//                    var guildname = guildinfo.Split("\">")[1].Split("</a>")[0];

//                    response.Close();
//                    readStream.Close();


//                    HttpWebRequest profilerequest = (HttpWebRequest)WebRequest.Create(profilelink);
//                    WebHeaderCollection HeaderCollection = profilerequest.Headers;
//                    //HeaderCollection.Add("Content-Type", "text/html");
//                    HeaderCollection.Add("User-Agent", "DiscordProfile");
//                    HeaderCollection.Add("Host", "www.naeu.playblackdesert.com");

//                    var responsprofile = await profilerequest.GetResponseAsync();
//                    HttpWebResponse responseprofile = (HttpWebResponse)responsprofile;
//                    if (responseprofile.StatusCode == HttpStatusCode.OK)
//                    {
//                        Stream receiveStreamprofile = responseprofile.GetResponseStream();
//                        StreamReader readStreamprofile = null;
//                        if (responseprofile.CharacterSet == null)
//                            readStreamprofile = new StreamReader(receiveStreamprofile);
//                        else
//                            readStreamprofile = new StreamReader(receiveStreamprofile, Encoding.GetEncoding(responseprofile.CharacterSet));
//                        string profiledata = readStreamprofile.ReadToEnd();

//                        var characterlist = profiledata.Split("<ul class=\"character_list\">")[1].Split("</article>")[0];
//                        var count = characterlist.Split("<li>");
//                        bool countcheck = false;
//                        countcheck = count[2].Split("<span class=\"spec_level\">")[1].Split("</em>")[0].Contains("Private");
//                        var countreal = 0;
//                        var priv = false;
//                        if (countcheck)
//                        {
//                            countreal = count.Count() - 1;
//                            priv = true;
//                        }
//                        else
//                        {
//                            countreal = ((count.Count() - 12) - 1) / 12 + 1;
//                        }
//                        var Characters = new List<Characters>();
//                        string[] splitchar = null;
//                        for (int i = 1; i <= countreal; i++)
//                        {

//                            if (i != 1 && priv == false)
//                            {
//                                splitchar = count[1 + (i - 1) * 12].Split("<div class=\"character_desc_area\">");
//                            }
//                            else
//                            {
//                                splitchar = count[i].Split("<div class=\"character_desc_area\">");
//                            }

//                            var charname = splitchar[1].Split("<p class=\"character_name\">")[1].Split("</p>")[0].Replace(" ", "").Trim();
//                            var charclass = splitchar[1].Split("<em>")[1].Split("</em>")[0];
//                            var charlevel = splitchar[1].Split("<span>")[1].Split("</span>")[0];
//                            if (charlevel.Contains("Private")) charlevel = "Private";
//                            else
//                            {
//                                charlevel = charlevel.Split("<em>")[1].Split("</em>")[0];
//                            }
//                            var contribution = splitchar[1].Split("<span>")[2].Split("</span>")[0];
//                            if (contribution.Contains("Private")) contribution = "Private";
//                            else
//                            {
//                                contribution = contribution.Split("<em>")[1].Split("</em>")[0];
//                            }
//                            bool start = false;
//                            var startt = splitchar[1].Split("<span class=\"spec_level\">");
//                            if (startt.Count() > 1) start = startt[1].Contains("Private");
//                            var LifeSkills = new List<string>();
//                            if (start == false)
//                            {
//                                var b = i;
//                                if (b != 1) b = 1 + (i - 1) * 12;
//                                for (int a = b; a < b + 11; a++)
//                                {
//                                    var lif = count[a + 1].Split("<span class=\"spec_level\">")[1].Split("</em>")[0].Replace("<em>", " ");
//                                    LifeSkills.Add(lif);
//                                }
//                            }
//                            else
//                            {
//                                LifeSkills = null;
//                            }

//                            Characters.Add(new Characters { Name = charname, Level = charlevel, ClassName = charclass, Contribution = contribution, LifeSkills = LifeSkills });


//                        }

//                        var pages = new PageBuilder[countreal];
//                        var descrip = "";
//                        if (guildname != "Private" && guildlink != "javascript:void(0)")
//                        {
//                            var reg = "EU";
//                            if (region != null)
//                            {
//                                if (region.ToLower() == "na") reg = "NA";
//                            }
//                            descrip = $"This player is a member of [{guildname}]({guildlink + $"&region={reg}"}) guild.";
//                        }
//                        else
//                        {
//                            descrip = "This player's guild is private.";
//                        }

//                        for (int i = 0; i < countreal; i++)
//                        {
//                            if (Characters[i].LifeSkills != null)
//                            {
//                                string chname = Characters[i].Name;
//                                if (Characters[i].Name.Contains("<spanclass=\"selected_label\">"))
//                                {
//                                    chname = Characters[i].Name.Split("<spanclass=\"selected_label\">")[0] + " - Main character";
//                                }
//                                pages[i] = new PageBuilder().AddField("Character name: ", chname, false)
//                                    .WithDescription(descrip)
//                                    .WithThumbnailUrl(ClassIcon[Characters[i].ClassName])
//                                            .AddField("Class: ", Characters[i].ClassName, true)
//                                            .AddField("Level: ", Characters[i].Level, true)
//                                            .AddField("Contribution: ", Characters[i].Contribution, true)
//                                            .AddField("Gathering: ", Characters[i].LifeSkills[0], true)
//                                            .AddField("Fishing: ", Characters[i].LifeSkills[1], true)
//                                            .AddField("Hunting: ", Characters[i].LifeSkills[2], true)
//                                            .AddField("Cooking: ", Characters[i].LifeSkills[3], true)
//                                            .AddField("Alchemy: ", Characters[i].LifeSkills[4], true)
//                                            .AddField("Processing: ", Characters[i].LifeSkills[5], true)
//                                            .AddField("Training: ", Characters[i].LifeSkills[6], true)
//                                            .AddField("Trading: ", Characters[i].LifeSkills[7], true)
//                                            .AddField("Farming: ", Characters[i].LifeSkills[8], true)
//                                            .AddField("Sailing: ", Characters[i].LifeSkills[9], true)
//                                            .AddField("Barter: ", Characters[i].LifeSkills[10], true)
//                                            .WithColor(Color.Blue)
//                                            .WithAuthor("Family Name: " + familyname, url: profilelink);
//                            }
//                            else
//                            {
//                                string chname = Characters[i].Name;
//                                if (Characters[i].Name.Contains("<spanclass=\"selected_label\">"))
//                                {
//                                    chname = Characters[i].Name.Split("<spanclass=\"selected_label\">")[0] + " - Main character";
//                                }
//                                string priviet = "Private";
//                                pages[i] = new PageBuilder().AddField("Character name: ", chname, false)
//                                    .WithThumbnailUrl(ClassIcon[Characters[i].ClassName])
//                                    .WithDescription(descrip)
//                                            .AddField("Class: ", Characters[i].ClassName, true)
//                                            .AddField("Level: ", Characters[i].Level, true)
//                                            .AddField("Contribution: ", Characters[i].Contribution, true)
//                                            .AddField("Gathering: ", priviet, true)
//                                            .AddField("Fishing: ", priviet, true)
//                                            .AddField("Hunting: ", priviet, true)
//                                            .AddField("Cooking: ", priviet, true)
//                                            .AddField("Alchemy: ", priviet, true)
//                                            .AddField("Processing: ", priviet, true)
//                                            .AddField("Training: ", priviet, true)
//                                            .AddField("Trading: ", priviet, true)
//                                            .AddField("Farming: ", priviet, true)
//                                            .AddField("Sailing: ", priviet, true)
//                                            .AddField("Barter: ", priviet, true)
//                                            .WithColor(Color.Blue)
//                                            .WithAuthor("Family Name: " + familyname, url: profilelink);
//                            }
//                        }


//                        var paginator = new StaticPaginatorBuilder()
//                            .WithPages(pages)
//                            .WithFooter(PaginatorFooter.PageNumber)
//                            .WithDefaultEmotes()
//                            .Build();

//                        responseprofile.Close();
//                        readStreamprofile.Close();
//                        await intserv.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(10));
//                    }


//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//            }
//        }
//        [Command("charactername", RunMode = RunMode.Async)]
//        [Alias("ch")]
//        public async Task FindCharacterAsync(string name, [Optional] string region)
//        {
//            try
//            {


//                var urlAddress = $"https://www.naeu.playblackdesert.com/en-US/Adventure?region=EU&searchType=1&searchKeyword={name}";
//                if (region != null)
//                {
//                    if (region.ToLower() == "eu") urlAddress = $"https://www.naeu.playblackdesert.com/en-US/Adventure?region=EU&searchType=2&searchKeyword={name}";
//                    if (region.ToLower() == "na") urlAddress = $"https://www.naeu.playblackdesert.com/en-US/Adventure?region=NA&searchType=2&searchKeyword={name}";
//                }
//                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
//                WebHeaderCollection myWebHeaderCollection = request.Headers;
//                myWebHeaderCollection.Add("Content-Type", "text/plain");
//                myWebHeaderCollection.Add("User-Agent", "Discord");
//                myWebHeaderCollection.Add("Host", "www.naeu.playblackdesert.com");
//                var tempmsg = await ReplyAsync("Fetching profile data...");
//                var respons = await request.GetResponseAsync();
//                HttpWebResponse response = (HttpWebResponse)respons;
//                if (response.StatusCode == HttpStatusCode.OK)
//                {
//                    await tempmsg.DeleteAsync();
//                    Stream receiveStream = response.GetResponseStream();
//                    StreamReader readStream = null;
//                    if (response.CharacterSet == null)
//                        readStream = new StreamReader(receiveStream);
//                    else
//                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
//                    string data = readStream.ReadToEnd();

//                    var first = data.Split("<div class=\"box_list_area\">")[1].Split("</ul>");
//                    var sec = first[0];
//                    if (sec.Contains("<li class=\"no_result\">"))
//                    {
//                        await ReplyAsync("No search results.");
//                        return;
//                    }
//                    var regiongot = sec.Split("<span class=\"region_info")[1].Split(">")[1].Split("<")[0];
//                    var href = sec.Split("<a href=\"")[1].Split("\">");
//                    var profilelink = href[0];
//                    var familyname = href[1].Split("</a>")[0];
//                    var lvlstart = href[7].Split("</span>")[0];
//                    var lvl = lvlstart.Replace(" ", "").Trim();
//                    var charactername = href[8].Split("</span>")[0];
//                    var classame = href[11].Split("</span>")[0];
//                    var guildinfo = sec.Split("<div class=\"state\">")[1];
//                    var guildlink = guildinfo.Split("<a href=\"")[1].Split("\">")[0];
//                    var guildname = guildinfo.Split("\">")[1].Split("</a>")[0];

//                    response.Close();
//                    readStream.Close();


//                    HttpWebRequest profilerequest = (HttpWebRequest)WebRequest.Create(profilelink);
//                    WebHeaderCollection HeaderCollection = profilerequest.Headers;
//                    //HeaderCollection.Add("Content-Type", "text/html");
//                    HeaderCollection.Add("User-Agent", "DiscordProfile");
//                    HeaderCollection.Add("Host", "www.naeu.playblackdesert.com");

//                    var responsprofile = await profilerequest.GetResponseAsync();
//                    HttpWebResponse responseprofile = (HttpWebResponse)responsprofile;
//                    if (responseprofile.StatusCode == HttpStatusCode.OK)
//                    {
//                        Stream receiveStreamprofile = responseprofile.GetResponseStream();
//                        StreamReader readStreamprofile = null;
//                        if (responseprofile.CharacterSet == null)
//                            readStreamprofile = new StreamReader(receiveStreamprofile);
//                        else
//                            readStreamprofile = new StreamReader(receiveStreamprofile, Encoding.GetEncoding(responseprofile.CharacterSet));
//                        string profiledata = readStreamprofile.ReadToEnd();

//                        var characterlist = profiledata.Split("<ul class=\"character_list\">")[1].Split("</article>")[0];
//                        var count = characterlist.Split("<li>");
//                        bool countcheck = false;
//                        countcheck = count[2].Split("<span class=\"spec_level\">")[1].Split("</em>")[0].Contains("Private");
//                        var countreal = 0;
//                        var priv = false;
//                        if (countcheck)
//                        {
//                            countreal = count.Count() - 1;
//                            priv = true;
//                        }
//                        else
//                        {
//                            countreal = ((count.Count() - 12) - 1) / 12 + 1;
//                        }
//                        var Characters = new List<Characters>();
//                        string[] splitchar = null;
//                        for (int i = 1; i <= countreal; i++)
//                        {

//                            if (i != 1 && priv == false)
//                            {
//                                splitchar = count[1 + (i - 1) * 12].Split("<div class=\"character_desc_area\">");
//                            }
//                            else
//                            {
//                                splitchar = count[i].Split("<div class=\"character_desc_area\">");
//                            }

//                            var charname = splitchar[1].Split("<p class=\"character_name\">")[1].Split("</p>")[0].Replace(" ", "").Trim();
//                            var charclass = splitchar[1].Split("<em>")[1].Split("</em>")[0];
//                            var charlevel = splitchar[1].Split("<span>")[1].Split("</span>")[0];
//                            if (charlevel.Contains("Private")) charlevel = "Private";
//                            else
//                            {
//                                charlevel = charlevel.Split("<em>")[1].Split("</em>")[0];
//                            }
//                            var contribution = splitchar[1].Split("<span>")[2].Split("</span>")[0];
//                            if (contribution.Contains("Private")) contribution = "Private";
//                            else
//                            {
//                                contribution = contribution.Split("<em>")[1].Split("</em>")[0];
//                            }
//                            bool start = false;
//                            var startt = splitchar[1].Split("<span class=\"spec_level\">");
//                            if (startt.Count() > 1) start = startt[1].Contains("Private");
//                            var LifeSkills = new List<string>();
//                            if (start == false)
//                            {
//                                var b = i;
//                                if (b != 1) b = 1 + (i - 1) * 12;
//                                for (int a = b; a < b + 11; a++)
//                                {
//                                    var lif = count[a + 1].Split("<span class=\"spec_level\">")[1].Split("</em>")[0].Replace("<em>", " ");
//                                    LifeSkills.Add(lif);
//                                }
//                            }
//                            else
//                            {
//                                LifeSkills = null;
//                            }

//                            Characters.Add(new Characters { Name = charname, Level = charlevel, ClassName = charclass, Contribution = contribution, LifeSkills = LifeSkills });


//                        }
//                        var pages = new PageBuilder[countreal];
//                        var descrip = "";
//                        if (guildname != "Private" && guildlink != "javascript:void(0)")
//                        {
//                            var reg = "EU";
//                            if (region != null)
//                            {
//                                if (region.ToLower() == "na") reg = "NA";
//                            }
//                            descrip = $"This player is a member of [{guildname}]({guildlink + $"&region={reg}"}) guild.";
//                        }
//                        else
//                        {//hello team
//                            descrip = "This player's guild is private.";
//                        }
//                        for (int i = 0; i < countreal; i++)
//                        {
//                            if (Characters[i].LifeSkills != null)
//                            {
//                                string chname = Characters[i].Name;
//                                if (Characters[i].Name.Contains("<spanclass=\"selected_label\">"))
//                                {
//                                    chname = Characters[i].Name.Split("<spanclass=\"selected_label\">")[0] + " - Main character";
//                                }
//                                pages[i] = new PageBuilder().AddField("Character name: ", chname, false)
//                                    .WithDescription(descrip)
//                                    .WithThumbnailUrl(ClassIcon[Characters[i].ClassName])
//                                            .AddField("Class: ", Characters[i].ClassName, true)
//                                            .AddField("Level: ", Characters[i].Level, true)
//                                            .AddField("Contribution: ", Characters[i].Contribution, true)
//                                            .AddField("Gathering: ", Characters[i].LifeSkills[0], true)
//                                            .AddField("Fishing: ", Characters[i].LifeSkills[1], true)
//                                            .AddField("Hunting: ", Characters[i].LifeSkills[2], true)
//                                            .AddField("Cooking: ", Characters[i].LifeSkills[3], true)
//                                            .AddField("Alchemy: ", Characters[i].LifeSkills[4], true)
//                                            .AddField("Processing: ", Characters[i].LifeSkills[5], true)
//                                            .AddField("Training: ", Characters[i].LifeSkills[6], true)
//                                            .AddField("Trading: ", Characters[i].LifeSkills[7], true)
//                                            .AddField("Farming: ", Characters[i].LifeSkills[8], true)
//                                            .AddField("Sailing: ", Characters[i].LifeSkills[9], true)
//                                            .AddField("Barter: ", Characters[i].LifeSkills[10], true)
//                                            .WithColor(Color.Blue)
//                                            .WithAuthor("Family Name: " + familyname, url: profilelink);
//                            }
//                            else
//                            {
//                                string chname = Characters[i].Name;
//                                if (Characters[i].Name.Contains("<spanclass=\"selected_label\">"))
//                                {
//                                    chname = Characters[i].Name.Split("<spanclass=\"selected_label\">")[0] + " - Main character";
//                                }
//                                string priviet = "Private";
//                                pages[i] = new PageBuilder().AddField("Character name: ", chname, false)
//                                    .WithDescription(descrip)
//                                    .WithThumbnailUrl(ClassIcon[Characters[i].ClassName])
//                                            .AddField("Class: ", Characters[i].ClassName, true)
//                                            .AddField("Level: ", Characters[i].Level, true)
//                                            .AddField("Contribution: ", Characters[i].Contribution, true)
//                                            .AddField("Gathering: ", priviet, true)
//                                            .AddField("Fishing: ", priviet, true)
//                                            .AddField("Hunting: ", priviet, true)
//                                            .AddField("Cooking: ", priviet, true)
//                                            .AddField("Alchemy: ", priviet, true)
//                                            .AddField("Processing: ", priviet, true)
//                                            .AddField("Training: ", priviet, true)
//                                            .AddField("Trading: ", priviet, true)
//                                            .AddField("Farming: ", priviet, true)
//                                            .AddField("Sailing: ", priviet, true)
//                                            .AddField("Barter: ", priviet, true)
//                                            .WithColor(Color.Blue)
//                                            .WithAuthor("Family Name: " + familyname, url: profilelink);
//                            }
//                        }


//                        var paginator = new StaticPaginatorBuilder()
//                            .WithPages(pages)
//                            .WithFooter(PaginatorFooter.PageNumber)
//                            .WithDefaultEmotes()
//                            .Build();

//                        responseprofile.Close();
//                        readStreamprofile.Close();
//                        await intserv.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(10));
//                    }


//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//            }
//        }
//        [Command("findguild", RunMode = RunMode.Async)]
//        [Alias("guild")]
//        public async Task FindGuild(string name, [Optional] string region)
//        {

//            string urlAddress = $"https://www.naeu.playblackdesert.com/en-US/Adventure/Guild/GuildProfile?guildName={name}&region=EU";
//            if (region != null)
//            {
//                if (region.ToLower() == "eu") urlAddress = $"https://www.naeu.playblackdesert.com/en-US/Adventure/Guild/GuildProfile?guildName={name}&region=EU";
//                if (region.ToLower() == "na") urlAddress = $"https://www.naeu.playblackdesert.com/en-US/Adventure/Guild/GuildProfile?guildName={name}&region=NA";
//            }
//            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
//            WebHeaderCollection myWebHeaderCollection = request.Headers;
//            /*request.CookieContainer = new CookieContainer();
//            var cookie = new Cookie("__RequestVerificationToken", "YGCcIeX0ZBPyz9Qfid2ouAhiPPCK6MryfUYeUzK2-hrGGfz3Yc40W_ejVHXQNaBjhvI2r2igcNsCcmlx3gXi6ONoFflYpWxAa376AUJajzk1");
//            cookie.Domain = "naeu.playblackdesert.com";
//            cookie.Path = @"/";
//            cookie.HttpOnly = true;
//            request.CookieContainer.Add(cookie);*/
//            myWebHeaderCollection.Add("Content-Type", "text/plain");
//            myWebHeaderCollection.Add("User-Agent", "Discord");
//            myWebHeaderCollection.Add("Host", "www.naeu.playblackdesert.com");

//            var respons = await request.GetResponseAsync();
//            HttpWebResponse response = (HttpWebResponse)respons;
//            if (response.StatusCode == HttpStatusCode.OK)
//            {
//                Stream receiveStream = response.GetResponseStream();
//                StreamReader readStream = null;
//                if (response.CharacterSet == null)
//                    readStream = new StreamReader(receiveStream);
//                else
//                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
//                string data = readStream.ReadToEnd();
//                var first = data.Split("<article class=\"content\">")[1].Split("</article>");
//                var sec = first[0];
//                var startregion = sec.Split("<span class=\"region_info")[1].Split("</span>");
//                var guildName = startregion[1].Split("<p>")[1].Split("</p>")[0];
//                var createdAt = startregion[2].Split("<span class=\"desc\">")[1].Split("<span>")[1];
//                var regiongot = startregion[0].Split(">")[1];
//                var line_list = sec.Split("<ul class=\"line_list\">")[1];
//                var membercount = line_list.Split("<em>")[1].Split("</em>")[0];
//                var ocuppying = line_list.Split("<span class=\"title\">Occupying</span>")[1].Split("<span class=\"desc\">")[1].Split("</span>")[0];
//                var guildMaster = sec.Split("Guild Master")[1].Split("<a href=")[1].Split("\">");//.Split(" </a>")[0];                
//                var guildMasterLink = guildMaster[0].Replace("\"", "");
//                var guildMasterName = guildMaster[1].Split("</a>")[0];
//                var MembersList = new List<Members>();
//                var lista = sec.Split("<ul class=\"adventure_list_table\">")[2].Split("</ul>")[0];
//                var count = lista.Split("<li>");
//                var GuildMasterWhole = $"[{guildMasterName}]({guildMasterLink})";
//                EmbedAuthorBuilder author = new EmbedAuthorBuilder();
//                if (regiongot == "EU")
//                {
//                    author.Name = guildName;
//                    author.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/twitter/259/flag-european-union_1f1ea-1f1fa.png";
//                    author.Url = urlAddress;
//                }
//                if (regiongot == "NA")
//                {
//                    author.Name = guildName;
//                    author.IconUrl = "https://emojipedia-us.s3.dualstack.us-west-1.amazonaws.com/thumbs/120/twitter/259/flag-united-states_1f1fa-1f1f8.png";
//                    author.Url = urlAddress;
//                }

//                for (int i = 1; i < count.Count(); i++)
//                {
//                    var splitmem = count[i].Split("<div class=\"guild_name\">");

//                    if (splitmem.Count() == 3)
//                    {
//                        var firstmember = splitmem[1].Split("<a href=\"")[1].Split("\">");
//                        var link1 = firstmember[0];
//                        var name1 = firstmember[1].Split("</a>")[0];
//                        if (name1 == guildMasterName) name1 = "👑 " + name1;
//                        MembersList.Add(new Members { Name = name1, ProfileUrl = link1 });

//                        var secmember = splitmem[2].Split("<a href=\"")[1].Split("\">");
//                        var link2 = secmember[0];
//                        var name2 = secmember[1].Split("</a>")[0];
//                        if (name2 == guildMasterName) name1 = "👑 " + name1;
//                        MembersList.Add(new Members { Name = name2, ProfileUrl = link2 });
//                    }
//                    else if (splitmem.Count() == 2)
//                    {
//                        var firstmember = splitmem[1].Split("<a href=\"")[1].Split("\">");
//                        var link1 = firstmember[0];
//                        var name1 = firstmember[1].Split("</a>")[0];
//                        if (name1 == guildMasterName) name1 = "👑 " + name1;
//                        MembersList.Add(new Members { Name = name1, ProfileUrl = link1 });
//                    }
//                }
//                var toembed1 = "";
//                var toembed2 = "";
//                var toembed3 = "";
//                var toembed4 = "";
//                int b = 1;
//                foreach (var item in MembersList)
//                {
//                    if (b < 26) toembed1 += $"{item.Name}\n";
//                    if (b > 25 && b < 51) toembed2 += $"{item.Name}\n";
//                    if (b > 50 && b < 76) toembed3 += $"{item.Name}\n";
//                    if (b > 75) toembed4 += $"{item.Name}\n";
//                    b++;
//                }
//                var a = new PaginatedMessage();
//                var starting = "```Family Name";
//                var pages = new PageBuilder[] {
//                    new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed1}```"),
//                };
//                if (toembed2.Length > 0)
//                {
//                    pages = new PageBuilder[] {
//                        new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed1}```"),

//                        new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed2}```"),
//                    };
//                }
//                if (toembed3.Length > 0)
//                {
//                    pages = new PageBuilder[] {
//                        new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed1}```"),
//                        new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed2}```"),
//                        new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed3}```"),
//                    };
//                }
//                if (toembed4.Length > 0)
//                {
//                    pages = new PageBuilder[] {
//                        new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed1}```"),
//                        new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed2}```"),
//                        new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed3}```"),
//                        new PageBuilder().AddField("Guild Master : ", GuildMasterWhole, true)
//                            .AddField("Region: ", regiongot, true)
//                            .AddField("Created on: ", createdAt, true)
//                            .AddField("Member count: ", membercount)
//                            .WithColor(Color.Blue)
//                            .WithAuthor(author)
//                            .AddField("Member list: ",starting.PadRight(30)+$"\n\n{toembed4}```"),
//                    };
//                }

//                var paginator = new StaticPaginatorBuilder()
//                    .WithPages(pages)
//                    .WithFooter(PaginatorFooter.PageNumber)
//                    .WithDefaultEmotes()
//                    .Build();

//                response.Close();
//                readStream.Close();
//                await intserv.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(10));
//            }
//        }
//    }
//}