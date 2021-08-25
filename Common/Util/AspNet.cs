using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace Common.Util.AspNet {
    public static class Middleware {
        public static IApplicationBuilder ForceHttps(this IApplicationBuilder App) {
            return App.Use(async (Context, Next) => {
                if (Context.Request.IsHttps) {
                    await Next();
                } else {
                    if (Context.Request.Method == Http.Method.Get) {
                        Context.Response.Redirect("https://" + Context.Request.Host + Context.Request.Path, true);
                    } else {
                        Context.Response.Clear();
                        Context.Response.StatusCode = Http.StatusCode.BadRequest;
                    }
                }
            });
        }
    }
}
