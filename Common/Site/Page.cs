using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Site {
    public abstract class PageBase {
        protected PageBase() { }
        public abstract string Title { get; }
        public abstract string Header { get; }
        public virtual string Description => string.Empty;

    }
    public class PageContent : PageBase {
        public PageContent(string Title, string Content, string Description, string Header) {
            this.Title = Title;
            this.Content = Content;
            this.Description = Description;
            this.Header = Header;
        }
        public override string Title { get; }
        public override string Description { get; }
        public override string Header { get; }
        public string Content { get; }
    }
    public abstract class Page : PageBase {
        public abstract string Key { get; }
        public abstract string Path { get; }
        public virtual string TitleNav => Header;
        public abstract Task<PageContent> GenerateContent(IEnumerable<string> Parameters = null);
        public string NavLink => $"<a id=\"link-{this.Key}\" href=\"/{this.Path}\" data-page=\"{this.Key}\" onclick=\"return link(this)\">{this.TitleNav}</a>";
    }
}
