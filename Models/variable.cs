using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecapi.Models
{
    public class variable
    {
        public class Timing
        {
            public int executing { get; set; }
            public string unit { get; set; }
        }

        public class Total
        {
            public int record_count { get; set; }
        }

        public class Info
        {
            public Timing timing { get; set; }
            public string result_coverage { get; set; }
            public Total total { get; set; }
        }

        public class Base_aDatum
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public int sequence { get; set; }
            public bool is_invalid { get; set; }
            public int? group { get; set; }
            public int? level { get; set; }
            public string report { get; set; }
            public string unit { get; set; }
        }

        public class Base_a
        {
            public string status { get; set; }
            public Info info { get; set; }
            public List<Base_aDatum> data { get; set; }
        }


    }
}
