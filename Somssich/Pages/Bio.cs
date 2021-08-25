﻿using Common.Site;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace somssich.Pages {
    public class Bio : Page {

        private static async Task<string> GenerateContent() {
            var sb = new StringBuilder();
            var content = await Common.Util.File.LoadToString("wwwroot\\Content\\Bio.txt");

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

        public override string Key => "bio";
        public override string Path => "Biography";
        public override string Header => "Biography";
        public override string Title => Application.Title + " - Biography";
    }
}
