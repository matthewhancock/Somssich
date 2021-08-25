using System;

namespace somssich.Controls {
    public class PressRelease : Link {
        public Guid ID { get; set; }
        public string Path { get; set; }
        public string Content { get; set; }
        public bool Link { get; set; }
        public string LinkType { get; set; }
    }
}
