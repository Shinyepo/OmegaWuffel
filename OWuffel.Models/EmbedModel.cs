using Newtonsoft.Json.Linq;
using System;

namespace OWuffel.Models
{
    public class EmbedModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }

        public JObject? Author { get; set; }

        public Int32 Color { get; set; }

        public JObject? Footer { get; set; }

        public string? Thumbnail { get; set; }

        public string? Image { get; set; }

        public JArray? Fields { get; set; }
    }
}
