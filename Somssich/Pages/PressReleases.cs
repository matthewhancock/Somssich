using Common.Site;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace somssich.Pages {
    public class PressReleases : Common.Site.Page {
        private PageContent _content;
        private List<Controls.PressRelease> _blogs = null;
        private Dictionary<string, PageContent> _blogsOutput = new Dictionary<string, PageContent>();
        public override async Task<PageContent> GenerateContent(IEnumerable<string> Parameters = null) {
            if (Parameters == null) {
                if (_content == null) {
                    if (_blogs is null) {
                        _blogs = await LoadBlogs();
                    }
                    var c = await GenerateContent(_blogs);
                    _content = new PageContent(Title, c, Description, Header);
                }
                return _content;
            } else {
                var path = Parameters.First();

                if (_blogsOutput.ContainsKey(path)) {
                    return _blogsOutput[path];
                } else {
                    if (_blogs is null) {
                        _blogs = await LoadBlogs();
                    }
                    var blog = _blogs.Find(q => string.Equals(q.Path, path, StringComparison.OrdinalIgnoreCase));

                    if (blog != null) {
                        var blogOutput = $"<aside class=\"gray_aside\">&lt; <a href=\"/{Path}\" data-page=\"{Key}\" onclick=\"return link(this)\">All Press Releases</a></aside>" +
                            $"<h1>{blog.Title}</h1>{blog.Date.ToString("MMMM dd, yyyy")}" +
                            $"<hr />{blog.Content.Replace("\r\n", "<br />")}" +
                            $"<hr /><aside class=\"gray_aside\">&lt; <a href=\"/{Path}\" data-page=\"{Key}\" onclick=\"return link(this)\">All Press Releases</a></aside>";

                        var content = new PageContent($"{blog.Title} - {Title}", blogOutput, blog.Description, Header);

                        _blogsOutput.Add(path, content);

                        return content;
                    } else {
                        throw new Common.Util.Http.Exceptions.RedirectException($"/{Path}");
                    }
                }
            }
        }

        private static async Task<List<Controls.PressRelease>> LoadBlogs() {
            var files = System.IO.Directory.GetFiles("wwwroot\\Shares\\PressReleases");

            var blogs = new List<Controls.PressRelease>();
            foreach (var f in files) {
                var blog = JsonConvert.DeserializeObject<Controls.PressRelease>(await Common.Util.File.LoadToString(f));
                blogs.Add(blog);
            }
            return blogs;
        }

        private async Task<string> GenerateContent(List<Controls.PressRelease> PressReleases) {
            var sb = new System.Text.StringBuilder();
            sb.Append("Below are my recent press releases.");
            sb.Append("<hr />");

            PressReleases.Sort((a, b) => { return b.Date.CompareTo(a.Date); }); // Descending Sort
            foreach (var pr in PressReleases) {
                if (pr.Link) {
                    sb.Append($"<a href=\"{pr.Url}\" target=\"_blank\">{pr.Title}{(pr.LinkType == "pdf" ? " (pdf)" : "")}</a><br />{pr.Date:MMMM dd, yyyy}");
                }
                else {
                    sb.Append($"<a href=\"/{Path}/{pr.Path}\" data-page=\"{Key}/{pr.Path}\" onclick=\"return link(this)\" target=\"_blank\">{pr.Title}</a><br />{pr.Date:MMMM dd, yyyy}");
                }

                if (pr.Description != null) {
                    sb.Append("<br />" + pr.Description);
                }
                sb.Append("<br /><br />");
            }


            var content = await Common.Util.File.LoadToString("wwwroot\\Content\\PressReleases.txt");
            sb.Append("<hr />");
            sb.Append(content);

            return $"<h1>{Header}</h1><hr />" + sb.ToString();
        }

        public override string Header => "Press Releases";
        public override string Key => "pr";
        public override string Path => "Press-Releases";
        public override string Title => Application.Title + " - " + Header;

    }
}
