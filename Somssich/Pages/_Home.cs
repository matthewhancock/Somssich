using Common.Site;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace somssich.Pages {
    public class Home : Common.Site.Page {
        public async static Task<string> GenerateContent() {
            var sb = new StringBuilder();
            var content = await Common.Util.File.LoadToString("wwwroot\\Content\\_Home.html");
            sb.Append(content);

            return sb.ToString();
        }

        private PageContent _content;
        public override async Task<PageContent> GenerateContent(IEnumerable<string> Parameters = null) {
            if (_content == null) {
                var c = await GenerateContent();
                _content = new PageContent(Title, c, Description, Header);
            }
            return _content;
        }

        public override string Key => "home";
        public override string Path => string.Empty;
        public override string Header => "Home";
        public override string Title => Application.Title;
    }
}
