using OWuffel.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OWuffel.Util
{
    public static class DownloadUploadImage
    {
        public static Task<string> DiscordAvatarMagicAsync(Uri avatar)
        {
            var _ = Task.Run(async () =>
            {

                using (var w = new WebClient())
                {
                    var clientID = "26661837a041fa5";
                    w.Headers.Add("Authorization", "Client-ID " + clientID);
                    var values = new NameValueCollection
                    {
                        { "image", $"{avatar.AbsoluteUri}" },
                        { "type", "url" }
                    };

                    byte[] response = w.UploadValues("https://api.imgur.com/3/upload.xml", values);
                    var result = XDocument.Load(new MemoryStream(response));
                    var status = result.Root.Attribute("status").Value;
                    var final = status == "200" ? result.Root.Element("link").Value : "";

                    return final;
                }
            });
            return _;
        }
    }
}
