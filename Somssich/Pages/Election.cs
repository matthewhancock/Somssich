using Common.Site;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace somssich.Pages {
    public class Election : Page {

        private static async Task<string> GenerateContent() {
            var sb = new StringBuilder();
            var content = await Common.Util.File.LoadToString("wwwroot\\Content\\Election.txt");

            sb.Append(content.Replace("\r\n", "<br />"));

            return sb.ToString();
        }

        private PageContent _content;
        public override async Task<PageContent> GenerateContent(IEnumerable<string> Parameters = null) {
            if (_content == null) {
                var c = $"<h1>{Header}</h1><hr />" + await GenerateContent();
                _content = new PageContent(Title, c, Description, Header);
            }
            return _content;
        }

        public override string Key => "election";
        public override string Path => "Election";
        public override string Header => "Elections";
        public override string Title => Application.Title + " - " + Header;
    }
}
