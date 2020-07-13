using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntegrationTests.Helpers
{
    public static class TestNames
    {
        public static string SiteName => $"Site-{Guid.NewGuid()}";
        public static string BranchName => $"test/{Guid.NewGuid()}";
        public static string Environment => $"Env-{Guid.NewGuid()}";
        public static string HostName => $"{Guid.NewGuid()}.test";
    }
}
