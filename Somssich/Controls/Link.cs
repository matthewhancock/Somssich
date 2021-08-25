using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace somssich.Controls {
    public class Link {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime Date { get; set; }
        public string[] Tags { get; set; }
    }
}
