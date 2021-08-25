using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Site;

namespace somssich.Pages {
    public class Contribute : Page {
        private PageContent _content;
        public override async Task<PageContent> GenerateContent(IEnumerable<string> Parameters = null) {
            if (_content == null) {
                var c = $"<h1>{Header}</h1><hr />" + await GenerateContent();
                _content = new PageContent(Title, c, Description, Header);
            }
            return _content;
        }

        public static async Task<string> GenerateContent() {
            var content = await Common.Util.File.LoadToString("wwwroot\\Content\\HelpElect.txt");
            content = content.Replace("\r\n", "<br />");
            return content;
        }

        public override string Key => "contribute";
        public override string Path => "Contribute";
        public override string Header => "Contribute/Volunteer";
        public override string Title => Application.Title + " - " + Header;
    }
}
