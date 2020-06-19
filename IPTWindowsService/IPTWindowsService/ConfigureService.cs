using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Topshelf;

namespace IPTWindowsService
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            var rc =  HostFactory.Run(configure =>
            {
                configure.Service<WindowsService>(service =>
                {
                    service.ConstructUsing(s => new WindowsService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });
                //Setup Account that window service use to run.  
                configure.RunAsLocalSystem();
                configure.SetServiceName(ConfigurationManager.AppSettings["ServiceName"]);
                configure.SetDisplayName(ConfigurationManager.AppSettings["DisplayName"]);
                configure.SetDescription(ConfigurationManager.AppSettings["Description"]);
            });

            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode;
        }

    }
}
