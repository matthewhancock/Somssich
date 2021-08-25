using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Util.Http {
    public static class Header {
        public const string Cache = "Cache-Control";
        public const string ContentDisposition = "Content-Disposition";
        public const string Referrer = "Referer";
        public const string UserAgent = "User-Agent";
        public class Values {
            public class Cache {
                public const string Public = "public";
                public const string Private = "private";
                public const string MaxAge_Zero = "max-age=0";
                public const string MaxAge_OneYear = "max-age=31557600"; // 365.25 * 24 * 60 * 60 - one year in seconds
            }
            public class ContentType {
                public const string Css = "text/css; charset=UTF-8";
                public const string Html = "text/html; charset=UTF-8";
                public const string Javascript = "text/javascript; charset=UTF-8";
                public const string Json = "application/json";
                //-- File extensions...
                public const string Eot = "application/vnd.ms-fontobject";
                public const string Jpg = "image/jpeg";
                public const string Png = "image/png";
                public const string Pdf = "application/pdf";
            }
        }
    }

    public static class Method {
        public const string Get = "GET";
        public const string Post = "POST";
    }
    public static class Scheme {
        public const string Http = "http";
        public const string Https = "https";
    }

    public static class Exceptions {
        public class HttpException : Exception {
            public HttpException(int StatusCode, string Message) :base(Message) {
                this.StatusCode = StatusCode;
            }
            public int StatusCode { get; }
        }
        public class RedirectException : HttpException {
            public RedirectException(string Path, bool Permanent = false)
                : base(Permanent ? Http.StatusCode.RedirectPermanent : Http.StatusCode.Redirect, null) {
                this.Path = Path;
                this.Permanent = Permanent;
            }
            public string Path { get; }
            public bool Permanent { get; }
        }
    }

    // converted from System.Net.HttpStatusCode enum
    public static class StatusCode {
        public const int Continue = 100;
        public const int SwitchingProtocols = 101;
        public const int OK = 200;
        public const int Created = 201;
        public const int Accepted = 202;
        public const int NonAuthoritativeInformation = 203;
        public const int NoContent = 204;
        public const int ResetContent = 205;
        public const int PartialContent = 206;
        public const int MultipleChoices = 300;
        public const int Ambiguous = 300;
        //public const int MovedPermanently = 301;
        //public const int Moved = 301;
        //public const int Found = 302;
        public const int RedirectPermanent = 301; // added for clearer label
        public const int Redirect = 302;
        public const int SeeOther = 303;
        public const int RedirectMethod = 303;
        public const int NotModified = 304;
        public const int UseProxy = 305;
        public const int Unused = 306;
        public const int TemporaryRedirect = 307;
        public const int RedirectKeepVerb = 307;
        public const int BadRequest = 400;
        public const int Unauthorized = 401;
        public const int PaymentRequired = 402;
        public const int Forbidden = 403;
        public const int NotFound = 404;
        public const int MethodNotAllowed = 405;
        public const int NotAcceptable = 406;
        public const int ProxyAuthenticationRequired = 407;
        public const int RequestTimeout = 408;
        public const int Conflict = 409;
        public const int Gone = 410;
        public const int LengthRequired = 411;
        public const int PreconditionFailed = 412;
        public const int RequestEntityTooLarge = 413;
        public const int RequestUriTooLong = 414;
        public const int UnsupportedMediaType = 415;
        public const int RequestedRangeNotSatisfiable = 416;
        public const int ExpectationFailed = 417;
        public const int UpgradeRequired = 426;
        public const int InternalServerError = 500;
        public const int NotImplemented = 501;
        public const int BadGateway = 502;
        public const int ServiceUnavailable = 503;
        public const int GatewayTimeout = 504;
        public const int HttpVersionNotSupported = 505;
        // New codes
        public const int Censorship = 451;
    }
}