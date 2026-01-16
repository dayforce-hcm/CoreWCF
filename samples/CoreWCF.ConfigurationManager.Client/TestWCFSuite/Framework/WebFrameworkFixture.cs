using System;
using System.Collections.Generic;
using System.IO;
using Xunit.Sdk;

namespace TestWCFSuite.Framework
{
    public class WebFrameworkFixture : IDisposable
    {
        private readonly MessageSinkTestOutputHelper m_output;
        private readonly List<ProcessConfig> m_pocesses = new List<ProcessConfig>();
        private readonly List<ProcessInstance> m_instances = new List<ProcessInstance>();

        public WebFrameworkFixture(IMessageSink messageSink) 
        {
            m_output = new MessageSinkTestOutputHelper(messageSink);
            var solutionDir = Utility.FindSolutionDirectory(m_output);
            //dotnet run --no-build --urls "https://localhost:7055;http://localhost:5151" --project C:\Dayforce\dev\NetCore-Migration\sample\TestWCFService\TestWCFService.csproj 
            var fullProjectPath = Path.Combine(solutionDir, "samples\\CoreWCF.ConfigurationManager.Client", "TestWCFService");

            m_output.WriteLine($"Starting server instances...");
            var processConfig = new ProcessConfig
            {
                IsNetCore = true,
                ProjectPath = fullProjectPath,
                LaunchUrl = "/WeatherService.svc",
                ApplicationUrls = "https://localhost:7055;http://localhost:5151"
            };
            m_pocesses.Add(processConfig);
            m_instances.Add(new ProcessInstance(processConfig, m_output));
        }

        public bool IsReady()
        {
            var result = true;
            foreach (var process in m_instances)
            {
                result &= process.IsRunning();
            }
            return result;
        }

        public void Dispose()
        {
            foreach(var pi in  m_instances)
            {
                pi.Dispose();
            }
        }
    }
}
