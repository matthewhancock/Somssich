using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace somssich {
    public class Application : Common.Application {
        public override void LoadFromConfig(IConfiguration Configuration) {
        }

        public const string Title = "Peter Somssich";
    }
}
