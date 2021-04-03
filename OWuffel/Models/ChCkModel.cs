using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWuffel.Models
{
    public class CheckModel
    {
        public Dictionary<string, string> balenos { get; set; }
        public Dictionary<string, string> calpheon { get; set; }
        public Dictionary<string, string> mediah { get; set; }
        public Dictionary<string, string> serendia { get; set; }
        public Dictionary<string, string> valencia { get; set; }
        public Dictionary<string, string> velia { get; set; }
        public Dictionary<string, string> grana { get; set; }
        public Dictionary<string, string> arsha { get; set; }

        public CheckModel()
        {
            balenos = new Dictionary<string, string>();
            calpheon = new Dictionary<string, string>();
            mediah = new Dictionary<string, string>();
            serendia = new Dictionary<string, string>();
            valencia = new Dictionary<string, string>();
            velia = new Dictionary<string, string>();
            grana = new Dictionary<string, string>();
            arsha = new Dictionary<string, string>();

            balenos.Add("1", "");
            balenos.Add("2", "");
            balenos.Add("3", "");
            balenos.Add("4", "");
            balenos.Add("5", "");
            balenos.Add("6", "");

            calpheon.Add("1", "");
            calpheon.Add("2", "");
            calpheon.Add("3", "");
            calpheon.Add("4", "");
            calpheon.Add("5", "");
            calpheon.Add("6", "");

            mediah.Add("1", "");
            mediah.Add("2", "");
            mediah.Add("3", "");
            mediah.Add("4", "");
            mediah.Add("5", "");
            mediah.Add("6", "");

            serendia.Add("1", "");
            serendia.Add("2", "");
            serendia.Add("3", "");
            serendia.Add("4", "");
            serendia.Add("5", "");
            serendia.Add("6", "");

            valencia.Add("1", "");
            valencia.Add("2", "");
            valencia.Add("3", "");
            valencia.Add("4", "");
            valencia.Add("5", "");
            valencia.Add("6", "");

            velia.Add("1", "");
            velia.Add("2", "");
            velia.Add("3", "");
            velia.Add("4", "");
            velia.Add("5", "");
            velia.Add("6", "");

            grana.Add("1", "");
            grana.Add("2", "");
            grana.Add("3", "");
            grana.Add("4", "");

            arsha.Add("1", "");
        }
    }
    public class ChCkModel
    {       

        public JArray WholeString { get; set; }

        public ChCkModel()
        {
            var model = new CheckModel();
            model.balenos = new Dictionary<string, string>();
            model.calpheon = new Dictionary<string, string>();
            model.mediah = new Dictionary<string, string>();
            model.serendia = new Dictionary<string, string>();
            model.valencia = new Dictionary<string, string>();
            model.velia = new Dictionary<string, string>();
            model.grana = new Dictionary<string, string>();
            model.arsha = new Dictionary<string, string>();

            model.balenos.Add("1", "");
            model.balenos.Add("2", "");
            model.balenos.Add("3", "");
            model.balenos.Add("4", "");
            model.balenos.Add("5", "");
            model.balenos.Add("6", "");

            model.calpheon.Add("6", "");
            model.calpheon.Add("6", "");
            model.calpheon.Add("6", "");
            model.calpheon.Add("6", "");
            model.calpheon.Add("6", "");
            model.calpheon.Add("6", "");

            model.mediah.Add("6", "");
            model.mediah.Add("6", "");
            model.mediah.Add("6", "");
            model.mediah.Add("6", "");
            model.mediah.Add("6", "");
            model.mediah.Add("6", "");

            model.serendia.Add("6", "");
            model.serendia.Add("6", "");
            model.serendia.Add("6", "");
            model.serendia.Add("6", "");
            model.serendia.Add("6", "");
            model.serendia.Add("6", "");

            model.valencia.Add("6", "");
            model.valencia.Add("6", "");
            model.valencia.Add("6", "");
            model.valencia.Add("6", "");
            model.valencia.Add("6", "");
            model.valencia.Add("6", "");

            model.velia.Add("6", "");
            model.velia.Add("6", "");
            model.velia.Add("6", "");
            model.velia.Add("6", "");
            model.velia.Add("6", "");
            model.velia.Add("6", "");

            model.grana.Add("6", "");
            model.grana.Add("6", "");
            model.grana.Add("6", "");
            model.grana.Add("6", "");

            model.arsha.Add("6", "");

            WholeString.Add(model.balenos);
            WholeString.Add(model.calpheon);
            WholeString.Add(model.mediah);
            WholeString.Add(model.serendia);
            WholeString.Add(model.valencia);
            WholeString.Add(model.velia);
            WholeString.Add(model.grana);
            WholeString.Add(model.arsha);
        }
    }
    
}
