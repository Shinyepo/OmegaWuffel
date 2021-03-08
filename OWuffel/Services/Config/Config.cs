using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Services.Config
{
    public class MainConfig : IConfigModel
    {
        private IConfiguration _config { get; set; }

        public string Token { get; private set; }
        public string Prefix { get; private set; }
        public int TotalShards { get; private set; }
        public string DefaultConnectionString { get; private set; }
        public string Type { get; private set; }
        public string SQLiteConnectionString { get; private set; }
        public ProcessConfig ProcessConfigWindows { get; private set; }
        public ProcessConfig ProcessConfigLinux { get; private set; }

        public MainConfig()
        {
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");
            _config = _builder.Build();

            Token = _config.GetValue<string>("Token");
            Prefix = _config.GetValue<string>("Prefix");
            TotalShards = _config.GetValue<int>("TotalShards");
            DefaultConnectionString = _config.GetConnectionString("DefaultConnection");
            SQLiteConnectionString = _config.GetConnectionString("SQLiteConnectionString");
            ProcessConfigLinux = new ProcessConfig();
            ProcessConfigWindows = new ProcessConfig();
            ProcessConfigWindows.FileName = _config.GetSection("ProcessConfig").GetSection("Windows").GetValue<string>("FileName");
            ProcessConfigWindows.Arguments = _config.GetSection("ProcessConfig").GetSection("Windows").GetValue<string>("Arguments");
            ProcessConfigLinux.FileName = _config.GetSection("ProcessConfig").GetSection("Linux").GetValue<string>("FileName");
            ProcessConfigLinux.Arguments = _config.GetSection("ProcessConfig").GetSection("Linux").GetValue<string>("Arguments");
            
        }

        public class ProcessConfig : IConfigModel.IOperatingSystem
        {
            public string FileName { get; set; }

            public string Arguments { get; set; }
        }
    }
}

