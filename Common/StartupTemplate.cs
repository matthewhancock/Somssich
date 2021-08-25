using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Common.Util.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Routing;

namespace Common {
    public abstract class StartupTemplate<T> where T : Application, new() {
        //private List<Site.Page> _pages;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection Services) {
            Services.AddRouting();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        private static string[] _cacheHeader = new string[] { Header.Values.Cache.MaxAge_OneYear };
        public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory log) {
            // Find pages
            var pages = RegisterPages();

            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapGet("/css/{**parameters}",
                    async context => await ProcessContent(context, Header.Values.ContentType.Css, Css)); ;
                endpoints.MapGet("/js/{**parameters}",
                    async context => await ProcessContent(context, Header.Values.ContentType.Javascript, Javascript));

                endpoints.MapPost("/{path}", async context => await ProcessPostAsync(context, context.GetRouteValue("path")?.ToString()));

                foreach (var p in pages) {
                    // json needs to go first to capture empty paths
                    endpoints.MapGet($"/json/{p.Key}/{{**parameters}}", async context => {
                        var parameters = context.GetRouteValue("parameters")?.ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                        await Json(context.Response, p, parameters);
                    });

                    var path = p.Path == string.Empty ? string.Empty : $"{p.Path}/{{**parameters}}";
                    endpoints.MapGet($"/{path}", async context => {
                        context.Response.ContentType = Header.Values.ContentType.Html;
                        var parameters = context.GetRouteValue("parameters")?.ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                        try {
                            await Page(context.Response, p, parameters);
                        } catch (Exceptions.RedirectException rex) {
                            context.Response.Redirect(rex.Path, rex.Permanent);
                        } catch (Exceptions.HttpException hex) {
                            await Error.Handle(context.Response, hex.StatusCode, hex.Message);
                        } catch {
                            await Error.FileNotFound(context.Response);
                        }
                    });
                }
            });


            //var application = new T();
            //application.RootLoadFromEnvironment(env);

            //// load config
            //var _config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            //// load local config file if no environment variables
            //var envn = _config["env"];
            //if (_config["env"] == null) {
            //    envn = "local";
            //    _config["env"] = envn;
            //}
            //if (envn == "local") {
            //    _config = new ConfigurationBuilder().AddJsonFile(env.ContentRootPath + "/config.local.json").Build();
            //}
            ////application.RootLoadFromConfig(_config);

            //app.ForceHttps();

            Configure(app);

            app.Run(ProcessRequestAsync);
            app.Run(async context => await Error.FileNotFound(context.Response));
        }
        public abstract Task ProcessPostAsync(HttpContext Context, string Action);

        private static async Task ProcessContent(HttpContext Context, string ContentType, Func<IEnumerable<string>, Task<string>> ContentDelegate) {
            Context.Response.Headers.Add(Header.Cache, _cacheHeader);
            Context.Response.ContentType = ContentType;
            var content = await ContentDelegate(Context.Request.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            await Context.Response.WriteAsync(content);
        }

        public async Task ProcessRequestAsync(HttpContext Context) {
            var rq = Context.Request;
            var path = rq.Path.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var pathc = path.Length;

            if (rq.Method == "POST") {

            } else {
                if (pathc > 0) {
                    switch (path[0]) {
                        case "image":
                            Context.Response.Headers.Add(Header.Cache, _cacheHeader);
                            var file = await Image(path.Skip(1));
                            if (file != null) {
                                var filename = path[1];
                                var fileextension = filename.Substring(filename.IndexOf("."));
                                if (fileextension == "png") {
                                    Context.Response.ContentType = Header.Values.ContentType.Png;
                                } else {
                                    Context.Response.ContentType = Header.Values.ContentType.Jpg;
                                }

                                await Context.Response.Body.WriteAsync(file, 0, file.Length);
                            } else {
                                await Error.FileNotFound(Context.Response);
                            }
                            break;
                        case "file":
                            Context.Response.Headers.Add(Header.Cache, _cacheHeader);
                            var f = await File(path.Skip(1).ToArray());
                            if (f != null) {
                                switch (f.Value.FileType) {
                                    case Util.File.TypeEnum.Pdf:
                                        Context.Response.ContentType = Header.Values.ContentType.Pdf;
                                        Context.Response.Headers.Add(Header.ContentDisposition, new string[] { "attachment" });
                                        break;
                                }
                                await Context.Response.Body.WriteAsync(f.Value.File, 0, f.Value.File.Length);
                            } else {
                                await Error.FileNotFound(Context.Response);
                            }
                            break;
                    }
                }
            }
        }


        protected virtual void Configure(IApplicationBuilder app) { } // Allow inheriting applications to do some configuration

        public abstract List<Site.Page> RegisterPages();

        public abstract Task<string> Css(IEnumerable<string> Path);
        public abstract Task<string> Javascript(IEnumerable<string> Path);
        public abstract Task<byte[]> Image(IEnumerable<string> Path);
        public abstract Task<(byte[] File, Util.File.TypeEnum FileType)?> File(string[] Path);
        protected async Task Json(HttpResponse Response, Site.Page Page, string[] Parameters = null) {
            Response.ContentType = "text/javascript";
            var p = await Page.GenerateContent(Parameters);
            var s = $"{{\"title\":\"{Util.Json.Fix(p.Title)}\",\"description\":\"{Util.Json.Fix(p.Description)}\",\"header\":\"{Util.Json.Fix(p.Header)}\",\"key\":\"{Page.Key}\",\"content\":\"{Util.Json.Fix(p.Content)}\"}}";
            await Response.WriteAsync(s);
        }

        protected abstract Task Page(HttpResponse Response, Site.Page Page, IEnumerable<string> Parameters = null);

        private static class Error {
            public static async Task Handle(HttpResponse Response, int StatusCode, string Message = null) {
                Response.Clear();
                Response.StatusCode = StatusCode;
                if (Message != null) {
                    await Response.WriteAsync(Message);
                }
            }
            public static async Task FileNotFound(HttpResponse Response) {
                await Handle(Response, StatusCode.NotFound);
            }
        }
    }
}
