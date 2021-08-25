using Common.Site;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace somssich.Pages {
    public class Links : Common.Site.Page {
        private PageContent _content;
        public override async Task<PageContent> GenerateContent(IEnumerable<string> Parameters = null) {
            if (_content == null) {
                var c = $"<h1>{Header}</h1><hr />" + await GenerateContent();
                _content = new PageContent(Title, c, Description, Header);
            }
            return _content;
        }
        public static async Task<string> GenerateContent() {
            var sb = new StringBuilder();
            var links = JsonConvert.DeserializeObject<List<Controls.Link>>(await Common.Util.File.LoadToString("wwwroot\\Links\\LettersToEditor.json"));
            var content = await Common.Util.File.LoadToString("wwwroot\\Content\\LettersToEditor.txt");

            links.Sort((a, b) => b.Date.CompareTo(a.Date));

            sb.Append("<b id=\"local\">Local Topics:</b><br /><br />");
            foreach (var i in links.FindAll(q => q.Tags.Any(t => t == "local"))) {
                sb.Append($"<a href=\"{i.Url}\" target=\"_blank\">{i.Title}</a><br />{i.Date.ToString("MMMM dd, yyyy")}");
                if (i.Description != null) {
                    sb.Append("<br />" + i.Description);
                }
                sb.Append("<br /><br />");
            }

            sb.Append("<b id=\"state\">State Topics:</b><br /><br />");
            foreach (var i in links.FindAll(q => q.Tags.Any(t => t == "state"))) {
                sb.Append($"<a href=\"{i.Url}\" target=\"_blank\">{i.Title}</a><br />{i.Date.ToString("MMMM dd, yyyy")}");
                if (i.Description != null) {
                    sb.Append("<br />" + i.Description);
                }
                sb.Append("<br /><br />");
            }

            sb.Append("<b id=\"national\">National and Other Topics:</b><br /><br />");
            foreach (var i in links.FindAll(q => q.Tags.Any(t => t == "national"))) {
                sb.Append($"<a href=\"{i.Url}\" target=\"_blank\">{i.Title}</a><br />{i.Date.ToString("MMMM dd, yyyy")}");
                if (i.Description != null) {
                    sb.Append("<br />" + i.Description);
                }
                sb.Append("<br /><br />");
            }
            sb.Append("<hr />");
            sb.Append(content.Replace("\r\n", "<br />"));

            return sb.ToString();
        }

        public override string Key => "links";
        public override string Path => "Links";
        public override string Header => "Links to Documents";
        public override string Title => Application.Title + " - " + Header;
    }
}
