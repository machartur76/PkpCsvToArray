using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkpCsvToArray
{
    public class Point
    {
        public decimal Lon { get; set; }
        public decimal Lat { get; set; }
        public int Index { get; set; }
        public int Lp { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
    }
}
