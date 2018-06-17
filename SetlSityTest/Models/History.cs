using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetlSityTest.Models
{
    public class History
    {
        public int Id { get; set; }
        public string NameTask { get; set; }
        public string InputData { get; set; }
        public string OutputData { get; set; }
        public string UserName { get; set; }
    }
}