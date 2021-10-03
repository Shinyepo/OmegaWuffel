using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Models
{
    public class StreamStatistics
    {
        [Key]
        public int Id { get; set; }

        public GuildInformation GuildInformation {  get; set; }
        public ulong GuildId { get; set; }


        public ulong UserId {  get; set; }
        public string UserName {  get; set; }
        public string Discriminator { get; set; }
        public ulong StreamTime { get; set; }

    }
}
