using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Util {
    public static class Json {
        public static string Fix(string s) {
            if (string.IsNullOrEmpty(s)) {
                return string.Empty;
            } else {
                return s.Replace(System.Environment.NewLine, String.Empty).Replace(@"\", @"\\").Replace(@"""", @"\""");
            }
        }
    }
}
