using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserSampleApi.Model
{  
    public class RandomUser
    {
        public Result[] results { get; set; }
        public Info info { get; set; }
    }

    public class Info
    {
        public string seed { get; set; }
        public int results { get; set; }
        public int page { get; set; }
        public string version { get; set; }
    }

    public class Result
    {
        public Name name { get; set; }
        public Dob dob { get; set; }
        public Id id { get; set; }
    }

    public class Name
    {
        public string title { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Dob
    {
        public DateTime date { get; set; }
        public int age { get; set; }
    }

    public class Id
    {
        public string name { get; set; }
        public object value { get; set; }
    }

}
