using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Green_Economy
{
    public class DatoAmbientale
    {
        public string Provincia { get; set; }
        public DateTime DataOra { get; set; }
        public double Temperatura { get; set; }
        public int Inquinamento { get; set; }
        public int Umidita { get; set; }
        public double Vento { get; set; }
    }
}
