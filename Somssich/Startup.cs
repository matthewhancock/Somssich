using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Site;
using Common.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace somssich {
    public class Startup : Common.StartupTemplate<Application> {

        protected override void Configure(IApplicationBuilder app) {
        }


        public override async Task<string> Css(IEnumerable<string> Path)
            => await Common.Util.File.LoadToString("wwwroot\\Files\\css\\this.min.css");

        private static Dictionary<string, byte[]> _images = new Dictionary<string, byte[]>();
        public override async Task<byte[]> Image(IEnumerable<string> Path) {
            byte[] file = null;
            var pathl = Path.Count();
            var filename = Path.First();
            if (!_images.ContainsKey(filename)) {
                try {
                    file = await Common.Util.File.LoadToBuffer($"wwwroot\\Files\\images\\{filename}");
                    _images.Add(filename, file);
                } catch { }
            } else {
                file = _images[filename];
            }
            return file;
        }

        public override async Task<string> Javascript(IEnumerable<string> Path) {
            return await Common.Util.File.LoadToString("wwwroot\\Files\\js\\this.min.js");
        }

        public override Task ProcessPostAsync(HttpContext Context, string Action) {
            throw new NotImplementedException();
        }

        public override List<Page> RegisterPages() => new List<Page>() {
                Pages.Home, Pages.Bio, Pages.HelpElect, Pages.LocalIssues, Pages.StateIssues,  Pages.Contact, Pages.PressReleases, Pages.Links, Pages.Election
        };

        private static class Pages {
            public static Page Home = new somssich.Pages.Home();
            public static Page Bio = new somssich.Pages.Bio();
            public static Page HelpElect = new somssich.Pages.Contribute();
            public static Page LocalIssues = new somssich.Pages.LocalIssues();
            public static Page StateIssues = new somssich.Pages.StateIssues();
            public static Page Contact = new somssich.Pages.Contact();
            public static Page PressReleases = new somssich.Pages.PressReleases();
            public static Page Links = new somssich.Pages.Links();
            public static Page Election = new somssich.Pages.Election();
        }
        private static string tags = (new Html.Head.Tag("link", new Dictionary<string, string> { { "rel", "stylesheet" }, { "type", "text/css" }, { "href", "/css?v3" } })).Output() +
            (new Html.Head.Tag("link", new Dictionary<string, string> { { "rel", "stylesheet" }, { "type", "text/css" }, { "href", "//cloud.typography.com/607958/780628/css/fonts.css" } })).Output() +
            (new Html.Head.Tag.Javascript("/js")).Output() + (new Html.Head.Tag.Javascript("//platform.twitter.com/widgets.js")).Output() + (new Html.Head.Tag.Javascript("//connect.facebook.net/en_US/sdk.js#xfbml=1&appId=175259985884771&version=v2.0")).Output();
        private static string body_start = $"<header><div id=\"h\"><aside id=\"logo\"><div id=\"logo_description\">Portsmouth<br />State Representative<br />Ward 3</div><div>Peter</div><div>Somssich</div></aside><hr /><nav id=\"n\" data-key=\"";
        private static string body_mid = "\">" + Pages.Home.NavLink + Pages.Bio.NavLink + Pages.LocalIssues.NavLink + Pages.StateIssues.NavLink + 
            Pages.Links.NavLink + Pages.PressReleases.NavLink + Pages.Election.NavLink + Pages.Contact.NavLink + Pages.HelpElect.NavLink + 
            $"</nav><hr /><footer id=\"h-f\">" /* + "<b>General Election</b><br />November 3, 2020<br /><br /><br />" */ + $"<aside class=\"fs08\"><b>Last Updated</b><br />{LastUpdated.ToString("MMMM dd, yyy h:mm tt")}</aside></footer></div></header><main id=\"m\"><section id=\"content\">";
        private static string body_end = $"</section></main>";

        protected override async Task Page(HttpResponse Response, Page Page, IEnumerable<string> Parameters = null) {
            try {
                var c = await Page.GenerateContent(Parameters);

                await Html.WriteOutput(Response, c.Title, tags +
                    "<meta name=\"description\" content=\"" + (c.Description ?? Page.Description) + "\">",
                    body_start + Page.Key + body_mid +
                    c.Content +
                    body_end);
            } catch (Common.Util.Http.Exceptions.RedirectException rex) {
                Response.Redirect(rex.Path);
            } catch (Exception ex) {
                await Response.WriteAsync(ex.Message);
            }
        }

        public override Task<(byte[] File, File.TypeEnum FileType)?> File(string[] Path) {
            throw new NotImplementedException();
        }

        private static DateTime LastUpdated => new DateTime(2021, 8, 24, 9, 17, 0);
    }
}
