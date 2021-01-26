using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Services.Config
{
    public interface IConfigModel
    {
        public string Token { get; }
        public string Prefix { get; }

        public int TotalShards { get; }

        public string DefaultConnectionString { get; }

        public string Type { get; }
        public string SQLiteConnectionString { get; }
    }
}
