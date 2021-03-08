using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Models
{
    public class EmbedModel
    {
        public string title { get; set; }
        public string description { get; set; }

        public JObject author { get; set; }

        public Int32 color { get; set; }

        public JObject footer { get; set; }

        public string thumbnail { get; set; }

        public string image { get; set; }

        public JArray fields { get; set; }
    }
}
