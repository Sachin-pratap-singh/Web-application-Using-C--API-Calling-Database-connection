using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecapi.Models
{
    public class category
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

        public class Base_Datum
        {
           
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public int sequence { get; set; }
        }

        public class Base
        {
            public string status { get; set; }
            public Info info { get; set; }
            public List<Base_Datum> data { get; set; }
        }
    }
}
