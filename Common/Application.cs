using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Common {
    public abstract class Application {
        private static string _path, _env;
        //private static IConfiguration _config;

        //private static EnvironmentConfiguration _env;
        //private static Configuration _config;

        internal void RootLoadFromEnvironment(IHostEnvironment env) {
            _path = env.ContentRootPath;
            //LoadFromEnvironment(env);
        }
        //public abstract EnvironmentConfiguration LoadFromEnvironment(IHostingEnvironment env);
        internal void RootLoadFromConfig(IConfiguration Configuration) {
            _env = Configuration["env"] ?? Environment.Constants.Local;
            /*_config =*/
            LoadFromConfig(Configuration);
        }
        public abstract void LoadFromConfig(IConfiguration Configuration);

        //public class EnvironmentConfiguration {
        //    public string ApplicationBasePath { get; internal set; }
        //}
        public static class Environment {
            public static class Constants {
                public const string Local = "local";
                public const string Development = "dev";
                public const string Test = "test";
                public const string Performance = "perf";
                public const string Staging = "stage";
                public const string Production = "prod";
            }
            public static string Name => _env;
            public static bool IsLocal => _env == Constants.Local;
            public static bool IsDevelopment => _env == Constants.Development;
            public static bool IsTest => _env == Constants.Test;
            public static bool IsPerformance => _env == Constants.Performance;
            public static bool IsStaging => _env == Constants.Staging;
            public static bool IsProduction => _env == Constants.Production;

            public static string ApplicationBasePath { get { return _path; } }
            //    public static EnvironmentConfiguration Configuration { get { return _env; } }
        }
    }
}
