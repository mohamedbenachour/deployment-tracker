using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace deployment_tracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
        var isService = !(Debugger.IsAttached || args.Contains("--console"));

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                Directory.SetCurrentDirectory(pathToContentRoot);
            }

            var builder = CreateWebHostBuilder(
                args.Where(arg => arg != "--console").ToArray());

            var host = builder.Build();

            if (isService && IsWindows())
            {
                // To run the app without the CustomWebHostService change the
                // next line to host.RunAsService();
                host.RunAsService();
            }
            else
            {
                host.Run();
            }
        }

        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    if (IsWindows()) {
                        logging.AddEventLog();
                    }
                })
                .UseStartup<Startup>();
    }
}
