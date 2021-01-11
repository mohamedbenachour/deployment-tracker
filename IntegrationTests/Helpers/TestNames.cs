using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests.Helpers {
    public static class TestNames {
        public static string SiteName => $"Site-{UniqueId}";
        public static string BranchName => $"test/{UniqueId}";
        public static string Environment => $"Env-{UniqueId}";
        public static string HostName => $"{UniqueId}.test";

        public static string TypeName => $"Type-{UniqueId}";

        public static string PublicUrl => $"https://{UniqueId}.test.com";

        public static string UserName => $"user-{UniqueId}";

        private static string UniqueId => Guid.NewGuid().ToString();
    }
}